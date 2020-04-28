using SoftmakeAll.SDK.Helpers.LINQ.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.NETReflector
{
  public class Discovery
  {
    #region Constructor
    public Discovery() { }
    #endregion

    #region Objects
    private System.Reflection.Assembly _Assembly;
    #endregion

    #region Properties
    public System.Reflection.Assembly Assembly => this._Assembly;
    #endregion

    #region Methods
    public System.Boolean TryLoadAssembly(System.Byte[] RawAssembly)
    {
      System.String ErrorMessage = "";
      return this.TryLoadAssembly(RawAssembly, out ErrorMessage);
    }
    public System.Boolean TryLoadAssembly(System.Byte[] RawAssembly, out System.String ErrorMessage)
    {
      try
      {
        this._Assembly = System.Reflection.Assembly.Load(RawAssembly);
        ErrorMessage = "";
      }
      catch (System.Exception ex)
      {
        this._Assembly = null;
        ErrorMessage = ex.Message;
        return false;
      }

      return true;
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Namespace> Discover()
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Namespace> Namespaces = this.GetExportedNamespaces();

      foreach (SoftmakeAll.SDK.NETReflector.Structures.Namespace Namespace in Namespaces)
        foreach (SoftmakeAll.SDK.NETReflector.Structures.Object Object in this.GetExportedObjects(Namespace.Name))
        {
          System.String FullName = Object.Name;
          if (!(System.String.IsNullOrWhiteSpace(Namespace.Name)))
            FullName = System.String.Concat(Namespace.Name, ".", FullName);

          Object.Properties.AddRange(this.GetExportedProperties(FullName));
          Object.Methods.AddRange(this.GetExportedMethods(FullName));
          Namespace.Objects.Add(Object);
        }

      return Namespaces;
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Namespace> GetExportedNamespaces()
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Namespace> Result = new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Namespace>();

      if (this._Assembly == null)
        return new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Namespace>();

      Result.AddRange(
        this._Assembly.ExportedTypes
        .Select(t =>
          new SoftmakeAll.SDK.NETReflector.Structures.Namespace()
          {
            Name = t.Namespace,
            Objects = new System.Collections.Generic.List<Structures.Object>()
          }
        ).DistinctBy(n => n.Name));

      return Result;
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Object> GetExportedObjects(System.String Namespace)
    {
      if (this._Assembly == null)
        return new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Object>();

      if (!(System.String.IsNullOrWhiteSpace(Namespace)))
        Namespace = System.String.Concat(Namespace, ".");

      return
        this._Assembly.ExportedTypes
        .Where(t => (t.FullName == System.String.Concat(Namespace, t.Name)))
        .Where(t => ((!(t.IsInterface)) && (((t.IsAbstract) && (t.IsSealed)) || ((!(t.IsAbstract)) || (t.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))))))
        .Select(t =>
          new SoftmakeAll.SDK.NETReflector.Structures.Object()
          {
            IsStatic = t.IsAbstract && t.IsSealed,
            IsClass = t.IsClass,
            IsStructure = t.IsValueType,
            Name = t.Name,
            Properties = new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Property>(),
            Methods = new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Method>()
          }
        ).OrderBy(o => o.Name).ToList();
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Property> GetExportedProperties(System.String FullName)
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Property> Result = new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Property>();

      if ((this._Assembly == null) || (System.String.IsNullOrWhiteSpace(FullName)))
        return Result;

      System.Type Type = this._Assembly.ExportedTypes.Where(t => t.FullName == FullName).FirstOrDefault();
      if (Type == null)
        return Result;

      Result.AddRange(
        Type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
        .Select(p =>
          new SoftmakeAll.SDK.NETReflector.Structures.Property()
          {
            IsStatic = true,
            TypeDescription = p.PropertyType.ToString(),
            Name = p.Name,
            CanRead = p.CanRead,
            CanWrite = p.CanWrite
          }
        ));

      Result.AddRange(
        Type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
        .Select(p =>
          new SoftmakeAll.SDK.NETReflector.Structures.Property()
          {
            IsStatic = false,
            TypeDescription = p.PropertyType.ToString(),
            Name = p.Name,
            CanRead = p.CanRead,
            CanWrite = p.CanWrite
          }
        ));

      return Result.OrderBy(p => p.Name).ToList();
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Method> GetExportedMethods(System.String FullName)
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Method> Result = new System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Method>();

      if ((this._Assembly == null) || (System.String.IsNullOrWhiteSpace(FullName)))
        return Result;

      System.Type Type = this._Assembly.ExportedTypes.Where(t => t.FullName == FullName).FirstOrDefault();
      if (Type == null)
        return Result;

      Result.AddRange(
        Type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance)
        .Where(m => (!(m.Attributes.HasFlag(System.Reflection.MethodAttributes.SpecialName))))
        .Where(m => (!(typeof(System.Object).GetMethods().Select(om => om.Name).Contains(m.Name))))
        .Select(m =>
          new SoftmakeAll.SDK.NETReflector.Structures.Method()
          {
            IsConstructor = m.IsConstructor,
            IsStatic = m.IsStatic,
            IsAsync = m.IsDefined(typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute), false),
            TypeDescription = m.ReturnType.ToString(),
            Name = m.Name,
            Parameters = this.GetMethodParameters(m)
          }
        ));

      return Result.OrderBy(m => m.Name).ToList();
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Parameter> GetMethodParameters(System.Reflection.MethodInfo MethodInfo)
    {
      return
        MethodInfo.GetParameters()
        .Select(p =>
          new SoftmakeAll.SDK.NETReflector.Structures.Parameter()
          {
            Position = p.Position,
            TypeDescription = p.ParameterType.ToString(),
            Name = p.Name,
            IsOutput = p.IsOut,
            IsReference = ((!(p.IsOut)) && (p.ParameterType.ToString().EndsWith('&'))),
            IsParamArray = p.IsDefined(typeof(System.ParamArrayAttribute), false),
            IsOptional = p.IsOptional
          }
        ).OrderBy(p => p.Position).ToList();
    }
    #endregion
  }
}