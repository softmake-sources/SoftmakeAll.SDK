using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using SoftmakeAll.SDK.Helpers.JSON.Extensions;
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
      this.RazorComponents = new System.Collections.Generic.Dictionary<System.String, System.String>();
      this.IndentSize = 2;
      this.AdditionalReferences = new System.Collections.Generic.List<System.String>();
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
    public System.Collections.Generic.Dictionary<System.String, System.String> CodeFiles { get; }
    public System.Collections.Generic.Dictionary<System.String, System.String> RazorComponents { get; }
    public System.Byte IndentSize { get; set; }
    public System.String DefaultReferencesDirectoryPath { get; set; }
    public System.Collections.Generic.List<System.String> AdditionalReferences { get; }
    public System.Boolean OmitCompilationHiddenSeverity { get; set; }
    public System.Boolean OmitCompilationInfoSeverity { get; set; }
    public System.Boolean OmitCompilationWarningSeverity { get; set; }
    public SoftmakeAll.SDK.NetReflector.Compiler.OutputKinds OutputKind { get; set; }
    public SoftmakeAll.SDK.NetReflector.Compiler.Platforms Platform { get; set; }
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> CompileToAsync(System.String DirectoryPath) => await this.CompileToAsync(DirectoryPath, System.Threading.CancellationToken.None);
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> CompileToAsync(System.String DirectoryPath, System.Threading.CancellationToken CancellationToken)
    {
      if (System.String.IsNullOrWhiteSpace(DirectoryPath)) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "directory path"));
      if (!(System.IO.Directory.Exists(DirectoryPath))) throw new System.Exception("The directory path doesn't exists.");

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> CompileResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        CompileResult = await this.CompileToAsync(MemoryStream, CancellationToken);
        if (CompileResult.ExitCode == 0) System.IO.File.WriteAllBytes(System.IO.Path.Combine(DirectoryPath, this.OutputFileName), MemoryStream.ToArray());
      }
      return CompileResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> CompileToAsync(System.IO.Stream Stream) => await this.CompileToAsync(Stream, System.Threading.CancellationToken.None);
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> CompileToAsync(System.IO.Stream Stream, System.Threading.CancellationToken CancellationToken)
    {
      if (Stream == null) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "Stream"));
      if (System.String.IsNullOrWhiteSpace(this.OutputFileName)) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "OutputFileName"));
      if ((this.CodeFiles == null) || (!(this.CodeFiles.Any()))) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "Code"));

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> CompileResults = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();
      System.Collections.Generic.List<System.Text.Json.JsonElement> Diagnostics = new System.Collections.Generic.List<System.Text.Json.JsonElement>();

      if (this.RazorComponents.Any())
        foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> RazorComponent in this.RazorComponents)
        {
          SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ProcessRazorComponentResult = await this.ProcessRazorComponentAsync(new System.IO.FileInfo(RazorComponent.Key).Name, RazorComponent.Value, true, null, CancellationToken);

          if (ProcessRazorComponentResult.ExitCode == 0)
          {
            System.String GeneratedComponentCode = ProcessRazorComponentResult.Data.GetString("GeneratedComponentCode");
            if (!(System.String.IsNullOrWhiteSpace(GeneratedComponentCode)))
              this.CodeFiles.Add($"{RazorComponent.Key}.g.cs", GeneratedComponentCode);
            continue;
          }

          if (ProcessRazorComponentResult.ExitCode == 1)
            Diagnostics.Add(ProcessRazorComponentResult.Data);
          else
            Diagnostics.Add(new { Severity = ProcessRazorComponentResult.ExitCode, Category = 0, ID = 0, ProcessRazorComponentResult.Message, Location = 0 }.ToJsonElement());
        }

      System.Collections.Generic.List<Microsoft.CodeAnalysis.SyntaxTree> SyntaxTrees = new System.Collections.Generic.List<Microsoft.CodeAnalysis.SyntaxTree>();
      foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> CodeFile in this.CodeFiles.Where(c => (!(System.String.IsNullOrWhiteSpace(c.Value)))))
        SyntaxTrees.Add(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(CodeFile.Value.Trim()).GetRoot().NormalizeWhitespace(new System.String(' ', this.IndentSize), System.Environment.NewLine, false).ToFullString(), null, CodeFile.Key));
      if (!(SyntaxTrees.Any())) throw new System.Exception(System.String.Format(SoftmakeAll.SDK.NetReflector.Compiler.NullOrEmptyMessage, "Code"));

      System.Collections.Generic.List<Microsoft.CodeAnalysis.MetadataReference> CurrentReferences = new System.Collections.Generic.List<Microsoft.CodeAnalysis.MetadataReference>();
      if (!(System.String.IsNullOrWhiteSpace(this.DefaultReferencesDirectoryPath)))
      {
        if (!(System.IO.Directory.Exists(this.DefaultReferencesDirectoryPath)))
          throw new System.IO.DirectoryNotFoundException();
        else
          foreach (System.String ReferencedFile in System.IO.Directory.GetFiles(this.DefaultReferencesDirectoryPath, "*.*", System.IO.SearchOption.AllDirectories))
            CurrentReferences.Add(Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(ReferencedFile));
      }

      if ((this.AdditionalReferences != null) && (this.AdditionalReferences.Any()))
        foreach (System.String ReferencedFile in this.AdditionalReferences)
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
          if ((Diagnostic.Location != null) && (!(Diagnostic.Location.SourceSpan.IsEmpty)))
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
        CompileResults.Data = Diagnostics.ToJsonElement();
      }

      CompileResults.ExitCode = System.Convert.ToInt16((!(EmitResult.Success)) ? -1 : CompileResults.Data.IsValid() ? 1 : 0);

      return CompileResults;
    }
    private async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ProcessRazorComponentAsync(System.String ComponentName, System.String FileContents, System.Boolean FindNamespaceInFileContents = true, System.String Namespace = "", System.Threading.CancellationToken CancellationToken = default)
    {
      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>() { ExitCode = 400 };

      if (System.String.IsNullOrWhiteSpace(ComponentName))
      {
        Result.Message = "Parameter ComponentName cannot be null.";
        return Result;
      }

      ComponentName = ComponentName.Replace(".razor", "");
      if (System.String.IsNullOrWhiteSpace(ComponentName))
      {
        Result.Message = "Parameter ComponentName cannot be null.";
        return Result;
      }

      if (System.String.IsNullOrWhiteSpace(FileContents))
      {
        Result.Message = "Parameter FileContents cannot be null.";
        return Result;
      }

      if ((!(FindNamespaceInFileContents)) && (System.String.IsNullOrWhiteSpace(Namespace)))
      {
        Result.Message = "Parameter Namespace cannot be null when Parameter FindNamespaceInFileContents is false.";
        return Result;
      }

      if (FindNamespaceInFileContents)
      {
        const System.String NamespaceTag = "@namespace ";

        Namespace = null;
        using (System.IO.StringReader StringReader = new System.IO.StringReader(FileContents))
          while (StringReader.Peek() > -1)
          {
            System.String Line = (await StringReader.ReadLineAsync(CancellationToken)).Trim();
            if (Line.StartsWith(NamespaceTag))
            {
              Namespace = Line[NamespaceTag.Length..^0];
              break;
            }
          }

        if (System.String.IsNullOrWhiteSpace(Namespace))
        {
          Result.Message = $"Namespace cannot be null. Check the FileContents and add tag {NamespaceTag}on the top of FileContents.";
          return Result;
        }
      }

      Microsoft.AspNetCore.Razor.Language.RazorCSharpDocument RazorCSharpDocument = null;
      try
      {
        Microsoft.AspNetCore.Razor.Language.RazorProjectEngine RazorProjectEngine = Microsoft.AspNetCore.Razor.Language.RazorProjectEngine.Create(Microsoft.AspNetCore.Razor.Language.RazorConfiguration.Default, Microsoft.AspNetCore.Razor.Language.RazorProjectFileSystem.Create(@"."));
        Microsoft.AspNetCore.Razor.Language.RazorSourceDocument RazorSourceDocument = Microsoft.AspNetCore.Razor.Language.RazorSourceDocument.Create(FileContents, ComponentName);
        Microsoft.AspNetCore.Razor.Language.RazorCodeDocument RazorCodeDocument = RazorProjectEngine.Process(RazorSourceDocument, "component", null, null);
        RazorCSharpDocument = RazorCodeDocument.GetCSharpDocument();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = 500;
        Result.Message = ex.Message;
        return Result;
      }

      if (RazorCSharpDocument == null)
      {
        Result.ExitCode = 500;
        Result.Message = "Unknow error on the document.";
        return Result;
      }

      System.Int32 DiagnosticsCount = RazorCSharpDocument.Diagnostics.Count;
      if (DiagnosticsCount > 0)
      {
        Result.ExitCode = 1; // DO NOT REMOVE
        Result.Message = $"{DiagnosticsCount} item{(DiagnosticsCount == 1 ? "" : 's')} were found. See the Data property for more details.";
        Result.Data = RazorCSharpDocument.Diagnostics.Select(d => new { d.Severity, Category = 0, ID = 0, Message = d.ToString(), Location = 0 }).ToArray().ToJsonElement();
        return Result;
      }

      System.String GeneratedComponentCode = RazorCSharpDocument.GeneratedCode;
      if (System.String.IsNullOrWhiteSpace(GeneratedComponentCode))
      {
        Result.Message = "Unknow error on the generated code.";
        return Result;
      }

      System.String Checksum = null;
      using (System.IO.StringReader StringReader = new System.IO.StringReader(GeneratedComponentCode))
        while ((StringReader.Peek() > -1) && (!((Checksum = await StringReader.ReadLineAsync(CancellationToken)).StartsWith("#pragma checksum "))))
          continue;

      if (System.String.IsNullOrWhiteSpace(Checksum))
      {
        Result.Message = "Checksum cannot bet empty.";
        return Result;
      }

      System.String[] SplittedChecksum = Checksum.Split(' ');
      if ((SplittedChecksum == null) || (SplittedChecksum.Length < 5))
      {
        Result.Message = "Checksum was not calculated.";
        return Result;
      }

      Checksum = SplittedChecksum.Last();
      if (System.String.IsNullOrWhiteSpace(Checksum))
      {
        Result.Message = $"Checksum length is invalid.";
        return Result;
      }

      GeneratedComponentCode = GeneratedComponentCode.Replace(" __GeneratedComponent", $" {Namespace}").Replace($"AspNetCore_{Checksum[1..^1]}", ComponentName);

      Result.ExitCode = 0;
      Result.Data = new { GeneratedComponentCode }.ToJsonElement();

      return Result;
    }
    public static async System.Threading.Tasks.Task<System.Object> EvaluateScriptAsync(System.String Code) => await Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.EvaluateAsync(Code);
    public static async System.Threading.Tasks.Task<T> EvaluateScriptAsync<T>(System.String Code) => await Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.EvaluateAsync<T>(Code);
    #endregion
  }
}