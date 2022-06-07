using System.Linq;

namespace SoftmakeAll.SDK.NetReflector
{
  public class Discovery
  {
    #region Constructor
    public Discovery() : this(true) { }
    public Discovery(System.Boolean LoadStructuresWithTheSameInheritanceLevelOnly) => this.LoadStructuresWithTheSameInheritanceLevelOnly = LoadStructuresWithTheSameInheritanceLevelOnly;
    #endregion

    #region Objects
    private System.Reflection.Assembly _Assembly;
    #endregion

    #region Fields
    private System.Boolean LoadStructuresWithTheSameInheritanceLevelOnly;
    #endregion

    #region Properties
    public System.Reflection.Assembly Assembly => this._Assembly;
    #endregion

    #region Methods
    public System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Namespace> TryLoadAssembly(System.Byte[] RawAssembly) => this.TryLoadAssembly(RawAssembly, out _);
    public System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Namespace> TryLoadAssembly(System.Byte[] RawAssembly, out System.String ErrorMessage)
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Namespace> Result = new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Namespace>();

      try
      {
        this._Assembly = System.Reflection.Assembly.Load(RawAssembly);
        ErrorMessage = "";
      }
      catch (System.Exception ex)
      {
        this._Assembly = null;
        ErrorMessage = ex.Message;
        return Result;
      }

      foreach (SoftmakeAll.SDK.NetReflector.Structures.Namespace Namespace in this.GetExportedNamespaces())
      {
        Result.Add(Namespace);
        Namespace.Objects.AddRange(this.GetExportedObjects(Namespace.Name));
      }

      return Result;
    }
    private System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Namespace> GetExportedNamespaces()
    {
      if (this._Assembly == null)
        return new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Namespace>();

      return this._Assembly.ExportedTypes.Select(t => new SoftmakeAll.SDK.NetReflector.Structures.Namespace() { Name = t.Namespace, Objects = new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Object>() }).DistinctBy(n => n.Name).ToList();
    }
    private System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Object> GetExportedObjects(System.String Namespace)
    {
      if (this._Assembly == null)
        return new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Object>();

      return
      this._Assembly.ExportedTypes
      .Where(t => ((t.FullName == $"{Namespace}.{t.Name}") && ((!(t.IsInterface)) && (((t.IsAbstract) && (t.IsSealed)) || ((!(t.IsAbstract)) || (t.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false)))))))
      .Select(t =>
      {
        System.Byte InheritanceLevel = (System.Byte)(this.GetInheritanceLevel(t) - 1);

        return
        new SoftmakeAll.SDK.NetReflector.Structures.Object()
        {
          Name = t.Name,
          IsStatic = t.IsAbstract && t.IsSealed,
          IsClass = t.IsClass,
          IsStructure = t.IsValueType,
          InheritanceLevel = InheritanceLevel,
          BaseObjectName = this.IsValidType(t.BaseType) ? t.BaseType.FullName : null,
          CustomAttributes = t.CustomAttributes.Select(ca => ca.ToString()).ToList(),
          Properties = this.GetExportedProperties(t, InheritanceLevel),
          Methods = this.GetExportedMethods(t, InheritanceLevel),
        };
      }
      ).OrderBy(o => o.Name).ToList();
    }
    private System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Property> GetExportedProperties(System.Type Type, System.Byte ParentInheritanceLevel)
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Property> Result = new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Property>();
      if (Type == null)
        return Result;


      Result.AddRange(
      Type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
      .Select(p =>
      {
        System.Byte InheritanceLevel = (System.Byte)(this.GetInheritanceLevel(p.DeclaringType) - 1);
        return ((this.LoadStructuresWithTheSameInheritanceLevelOnly) && (InheritanceLevel != ParentInheritanceLevel)) ? default :
        new SoftmakeAll.SDK.NetReflector.Structures.Property()
        {
          Name = p.Name,
          IsStatic = true,
          CanRead = p.CanRead,
          CanWrite = p.CanWrite,
          InheritanceLevel = InheritanceLevel,
          TypeDescription = p.PropertyType.ToString(),
          CustomAttributes = p.CustomAttributes.Select(ca => ca.ToString()).ToList()
        };
      })
      .Where(m => (!(System.String.IsNullOrWhiteSpace(m.Name)))));


      Result.AddRange(
      Type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
      .Select(p =>
      {
        System.Byte InheritanceLevel = (System.Byte)(this.GetInheritanceLevel(p.DeclaringType) - 1);
        return ((this.LoadStructuresWithTheSameInheritanceLevelOnly) && (InheritanceLevel != ParentInheritanceLevel)) ? default :
        new SoftmakeAll.SDK.NetReflector.Structures.Property()
        {
          Name = p.Name,
          IsStatic = false,
          CanRead = p.CanRead,
          CanWrite = p.CanWrite,
          InheritanceLevel = InheritanceLevel,
          TypeDescription = p.PropertyType.ToString(),
          CustomAttributes = p.CustomAttributes.Select(ca => ca.ToString()).ToList()
        };
      })
      .Where(m => (!(System.String.IsNullOrWhiteSpace(m.Name)))));


      return Result.OrderBy(p => p.Name).ToList();
    }
    private System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Method> GetExportedMethods(System.Type Type, System.Byte ParentInheritanceLevel)
    {
      if (Type == null)
        return new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Method>();

      return
      Type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance)
        .Where(m => (!(m.Attributes.HasFlag(System.Reflection.MethodAttributes.SpecialName))))
        .Where(m => (!(typeof(System.Object).GetMethods().Select(om => om.Name).Contains(m.Name))))
        .Select(m =>
        {
          System.Byte InheritanceLevel = (System.Byte)(this.GetInheritanceLevel(m.DeclaringType) - 1);
          return ((this.LoadStructuresWithTheSameInheritanceLevelOnly) && (InheritanceLevel != ParentInheritanceLevel)) ? default :
          new SoftmakeAll.SDK.NetReflector.Structures.Method()
          {
            Name = m.Name,
            IsConstructor = m.IsConstructor,
            IsStatic = m.IsStatic,
            IsAsync = m.IsDefined(typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute), false),
            InheritanceLevel = InheritanceLevel,
            TypeDescription = m.ReturnType.ToString(),
            Parameters = this.GetMethodParameters(m),
            CustomAttributes = m.CustomAttributes.Select(ca => ca.ToString()).Where(ca => (!(ca.Contains(".AsyncStateMachineAttribute"))) && (!(ca.Contains(".DebuggerStepThroughAttribute")))).ToList()
          };
        })
        .Where(m => (!(System.String.IsNullOrWhiteSpace(m.Name))))
        .OrderBy(m => m.Name).ToList();
    }
    private System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Parameter> GetMethodParameters(System.Reflection.MethodInfo MethodInfo)
    {
      if (MethodInfo == null)
        return new System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Parameter>();

      return
        MethodInfo.GetParameters()
        .Select(p =>
          new SoftmakeAll.SDK.NetReflector.Structures.Parameter()
          {
            Name = p.Name,
            IsOutput = p.IsOut,
            IsReference = ((!(p.IsOut)) && (p.ParameterType.ToString().EndsWith('&'))),
            IsParamArray = p.IsDefined(typeof(System.ParamArrayAttribute), false),
            IsOptional = p.IsOptional,
            Position = p.Position,
            TypeDescription = p.ParameterType.ToString(),
            DefaultValue = p.DefaultValue,
            CustomAttributes = p.CustomAttributes.Select(ca => ca.ToString()).ToList()
          }
        ).OrderBy(p => p.Position).ToList();
    }

    #region Helpers
    private System.Byte GetInheritanceLevel(System.Type Type) => this.GetInheritanceLevel(Type, 0);
    private System.Byte GetInheritanceLevel(System.Type Type, System.Byte CurrentLevel) => this.IsValidType(Type) ? this.GetInheritanceLevel(Type.BaseType, ++CurrentLevel) : CurrentLevel;
    private System.Boolean IsValidType(System.Type Type) => Type != null && Type.FullName != "System.Object";
    #endregion
    #endregion
  }
}