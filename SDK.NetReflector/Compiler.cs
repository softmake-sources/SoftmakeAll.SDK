using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace SoftmakeAll.SDK.NetReflector
{
  public class Compiler
  {
    #region Constructor
    public Compiler()
    {
      this.OutputFileName = "DynamicallyLinkedLibrary";
      this.CodeFiles = new System.Collections.Generic.Dictionary<System.String, System.String>();
      this.IndentSize = 2;
      this.ReferencedFiles = new System.Collections.Generic.List<System.String>();
      this.LoadDefaultReferences = true;
      this.OutputKind = SoftmakeAll.SDK.NetReflector.Compiler.OutputKinds.DynamicallyLinkedLibrary;
      this.Platform = SoftmakeAll.SDK.NetReflector.Compiler.Platforms.AnyCpu;
    }
    #endregion

    #region Enums
    public enum OutputKinds
    {
      ConsoleApplication = 0,
      WindowsApplication = 1,
      DynamicallyLinkedLibrary = 2,
      NetModule = 3,
      WindowsRuntimeMetadata = 4,
      WindowsRuntimeApplication = 5
    }
    public enum Platforms
    {
      AnyCpu = 0,
      X86 = 1,
      X64 = 2,
      Itanium = 3,
      AnyCpu32BitPreferred = 4,
      Arm = 5,
      Arm64 = 6
    }
    #endregion

    #region Constants
    private const System.String NullOrEmptyMessage = "The {0} cannot be null or empty.";
    #endregion

    #region Properties
    public System.String OutputFileName { get; set; }
    public System.Collections.Generic.Dictionary<System.String, System.String> CodeFiles { get; set; }
    public System.Byte IndentSize { get; set; }
    public System.Collections.Generic.List<System.String> ReferencedFiles { get; set; }
    public System.Boolean LoadDefaultReferences { get; set; }
    public System.Boolean OmitCompilationHiddenSeverity { get; set; }
    public System.Boolean OmitCompilationInfoSeverity { get; set; }
    public System.Boolean OmitCompilationWarningSeverity { get; set; }
    public SoftmakeAll.SDK.NetReflector.Compiler.OutputKinds OutputKind { get; set; }
    public SoftmakeAll.SDK.NetReflector.Compiler.Platforms Platform { get; set; }
    #endregion

    #region Methods
    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> CompileTo(System.String DirectoryPath)
    {
      if (System.String.IsNullOrWhiteSpace(DirectoryPath)) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "directory path"));
      if (!(System.IO.Directory.Exists(DirectoryPath))) throw new System.Exception("The directory path doesn't exists.");

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> CompileResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>(true);
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        CompileResult = this.CompileTo(MemoryStream);
        if (CompileResult.ExitCode == 0) System.IO.File.WriteAllBytes(System.IO.Path.Combine(DirectoryPath, this.OutputFileName), MemoryStream.ToArray());
      }
      return CompileResult;
    }
    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> CompileTo(System.IO.Stream Stream)
    {
      if (Stream == null) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "Stream"));
      if (System.String.IsNullOrWhiteSpace(this.OutputFileName)) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "OutputFileName"));
      if ((this.CodeFiles == null) || (!(this.CodeFiles.Any()))) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "Code"));

      System.Collections.Generic.List<Microsoft.CodeAnalysis.SyntaxTree> SyntaxTrees = new System.Collections.Generic.List<Microsoft.CodeAnalysis.SyntaxTree>();
      foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> CodeFile in this.CodeFiles.Where(c => (!(System.String.IsNullOrWhiteSpace(c.Value)))))
        SyntaxTrees.Add(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(CodeFile.Value.Trim()).GetRoot().NormalizeWhitespace(new System.String(' ', this.IndentSize), System.Environment.NewLine, false).ToFullString(), null, CodeFile.Key));
      if (!(SyntaxTrees.Any())) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "Code"));

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> CompileResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>(true);

      System.Collections.Generic.List<Microsoft.CodeAnalysis.MetadataReference> CurrentReferences = new System.Collections.Generic.List<Microsoft.CodeAnalysis.MetadataReference>();
      if ((this.ReferencedFiles != null) && (this.ReferencedFiles.Any()))
        foreach (System.String ReferencedFile in this.ReferencedFiles)
          CurrentReferences.Add(Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(ReferencedFile));

      if (LoadDefaultReferences)
        foreach (System.String ReferencedFile in System.IO.Directory.GetFiles(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "DefaultAssemblies")))
          CurrentReferences.Add(Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(ReferencedFile));

      Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions CSharpCompilationOptions = new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions
        (
        optimizationLevel: Microsoft.CodeAnalysis.OptimizationLevel.Release,
        outputKind: (Microsoft.CodeAnalysis.OutputKind)this.OutputKind,
        platform: (Microsoft.CodeAnalysis.Platform)this.Platform
        );

      System.IO.FileInfo OutputFileNameInfo = new System.IO.FileInfo(this.OutputFileName);
      System.String AssemblyName = OutputFileNameInfo.Name.Substring(0, OutputFileNameInfo.Name.Length - OutputFileNameInfo.Extension.Length);
      Microsoft.CodeAnalysis.CSharp.CSharpCompilation CSharpCompilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create(AssemblyName, SyntaxTrees, CurrentReferences, CSharpCompilationOptions);


      Microsoft.CodeAnalysis.Emit.EmitResult EmitResult = CSharpCompilation.Emit(Stream);
      if (EmitResult.Diagnostics.Length > 0)
      {
        System.Collections.Generic.List<System.Text.Json.JsonElement> Diagnostics = new System.Collections.Generic.List<System.Text.Json.JsonElement>();
        foreach (Microsoft.CodeAnalysis.Diagnostic Diagnostic in EmitResult.Diagnostics)
        {
          if ((this.OmitCompilationHiddenSeverity) && (Diagnostic.Descriptor.DefaultSeverity == Microsoft.CodeAnalysis.DiagnosticSeverity.Hidden)) continue;
          if ((this.OmitCompilationInfoSeverity) && (Diagnostic.Descriptor.DefaultSeverity == Microsoft.CodeAnalysis.DiagnosticSeverity.Info)) continue;
          if ((this.OmitCompilationWarningSeverity) && (Diagnostic.Descriptor.DefaultSeverity == Microsoft.CodeAnalysis.DiagnosticSeverity.Warning)) continue;

          System.String ID = Diagnostic.Descriptor.Id;
          if (ID == "CS8019") // CS8019 = using directives
            continue;

          System.String Severity = Diagnostic.Descriptor.DefaultSeverity.ToString();
          System.String Category = Diagnostic.Descriptor.Category;
          System.String Message = Diagnostic.GetMessage();
          System.Text.Json.JsonElement Location = new System.Text.Json.JsonElement();
          if ((Diagnostic.Location != null) && (Diagnostic.Location.SourceSpan != null) && (!(Diagnostic.Location.SourceSpan.IsEmpty)))
          {
            Microsoft.CodeAnalysis.FileLinePositionSpan FileLinePositionSpan = Diagnostic.Location.GetMappedLineSpan();
            System.Nullable<System.Int32> Line = null;
            System.Nullable<System.Int32> Character = null;
            if (FileLinePositionSpan.IsValid)
            {
              Line = FileLinePositionSpan.StartLinePosition.Line;
              Character = FileLinePositionSpan.StartLinePosition.Character;
            }
            Location = new { Diagnostic.Location.SourceTree.FilePath, Line, Character, Code = Diagnostic.Location.SourceTree.ToString().Substring(Diagnostic.Location.SourceSpan.Start, Diagnostic.Location.SourceSpan.End - Diagnostic.Location.SourceSpan.Start) }.ToJsonElement();
          }
          Diagnostics.Add(new { Severity, Category, ID, Message, Location }.ToJsonElement());
        }
        CompileResult.Data = Diagnostics.ToJsonElement();
      }

      CompileResult.ExitCode = System.Convert.ToInt16((!(EmitResult.Success)) ? -1 : CompileResult.Data.IsValid() ? 1 : 0);

      return CompileResult;
    }
    #endregion
  }
}