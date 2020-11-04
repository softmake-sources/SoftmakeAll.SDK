using System.Linq;

namespace SoftmakeAll.SDK.FileWR.CSV
{
  public class ConverterEngine
  {
    #region Fields
    internal static System.Collections.Generic.List<System.Tuple<System.String, System.String>> AllowedDataTypeNameReplacements = new System.Collections.Generic.List<System.Tuple<System.String, System.String>>()
    {
      new System.Tuple<System.String, System.String>("Boolean", "Int32"),
      new System.Tuple<System.String, System.String>("Boolean", "Int64"),
      new System.Tuple<System.String, System.String>("Boolean", "Single"),
      new System.Tuple<System.String, System.String>("Boolean", "Double"),
      new System.Tuple<System.String, System.String>("Int32", "Int64"),
      new System.Tuple<System.String, System.String>("Int32", "Single"),
      new System.Tuple<System.String, System.String>("Int32", "Double"),
      new System.Tuple<System.String, System.String>("Int64", "Double"),
      new System.Tuple<System.String, System.String>("Single", "Double")
    };
    #endregion

    #region Constructor
    public ConverterEngine()
    {
      this.Encoding = System.Text.Encoding.UTF8;
      this.ContainsHeader = true;
      this.FileColumns = new System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.CSV.FileColumn>();
    }
    #endregion

    #region Properties
    public System.Text.Encoding Encoding { get; set; }
    public System.Boolean ContainsHeader { get; set; }
    public System.Globalization.CultureInfo CultureInfo { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.CSV.FileColumn> FileColumns { get; }
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task<System.Data.DataTable> ConvertToDataTableAsync(System.String FileName)
    {
      if (!(System.IO.File.Exists(FileName)))
        throw new System.Exception("File not found.");

      System.Data.DataTable Result = null;
      using (System.IO.FileStream FileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        Result = await this.ConvertToDataTableAsync(FileStream);
      return Result;
    }
    public async System.Threading.Tasks.Task<System.Data.DataTable> ConvertToDataTableAsync(System.IO.Stream Stream)
    {
      System.Data.DataTable Result = new System.Data.DataTable();
      const System.String RegexSplit = "(?:,|\\n|^)(\"(?:(?:\"\")*[^\"]*)*\"|[^\",\\n]*|(?:\\n|$))";

      if (FileColumns.Count == 0)
      {
        this.FileColumns.Clear();
        using (System.IO.StreamReader AnalyserStreamReader = new System.IO.StreamReader(Stream, this.Encoding, false, 4096, true))
        {
          System.String CurrentLine = null;
          while (AnalyserStreamReader.Peek() > -1)
          {
            CurrentLine = await AnalyserStreamReader.ReadLineAsync();
            if (!(System.String.IsNullOrWhiteSpace(CurrentLine)))
              break;
          }
          if (System.String.IsNullOrWhiteSpace(CurrentLine)) return null;

          System.String[] DataTableColumnNames = System.Text.RegularExpressions.Regex.Split(CurrentLine, RegexSplit).Where((c, i) => i % 2 == 1).ToArray();
          for (System.Int32 i = 0; i < DataTableColumnNames.Length; i++)
            FileColumns.Add(new SoftmakeAll.SDK.FileWR.CSV.FileColumn() { Name = ((!(this.ContainsHeader)) ? $"column{i:00000}" : DataTableColumnNames[i]), Index = i });
          DataTableColumnNames = null;

          while (AnalyserStreamReader.Peek() > -1)
          {
            CurrentLine = await AnalyserStreamReader.ReadLineAsync();
            if (System.String.IsNullOrWhiteSpace(CurrentLine))
              continue;

            if (CurrentLine.Count(c => c == '"') % 2 != 0)
              continue;

            System.String[] Columns = System.Text.RegularExpressions.Regex.Split(CurrentLine, RegexSplit).Where((c, i) => i % 2 == 1).ToArray();
            if (FileColumns.Count != Columns.Length)
              continue;

            for (System.Int32 i = 0; i < Columns.Length; i++)
            {
              if ((Columns[i].StartsWith('"')) && (Columns[i].EndsWith('"')))
                Columns[i] = Columns[i][1..^1];

              FileColumns[i].AllowNulls = System.String.IsNullOrEmpty(Columns[i]);
              if (!(System.String.IsNullOrEmpty(Columns[i])))
                FileColumns[i].MaxLength = Columns[i].Length;
              else
                continue;

              System.Globalization.CultureInfo ConverterCultureInfo = this.FileColumns[i].CultureInfo ?? this.CultureInfo;
              if (ConverterCultureInfo == null)
                continue;

              this.DefineFileColumnProperties(FileColumns[i], Columns[i], ConverterCultureInfo);
            }
          }
        }
      }

      foreach (SoftmakeAll.SDK.FileWR.CSV.FileColumn FileColumn in FileColumns)
      {
        if (System.String.IsNullOrWhiteSpace(FileColumn.DataTypeName))
          FileColumn.DataTypeName = "String";
        Result.Columns.Add(new System.Data.DataColumn(FileColumn.Name, System.Type.GetType($"System.{FileColumn.DataTypeName}", true, false)) { MaxLength = FileColumn.MaxLength == 0 ? -1 : FileColumn.MaxLength });
      }

      Stream.Position = 0;
      using (System.IO.StreamReader AnalyserStreamReader = new System.IO.StreamReader(Stream, this.Encoding))
      {
        if (this.ContainsHeader) await AnalyserStreamReader.ReadLineAsync();
        System.String CurrentLine = null;
        while (AnalyserStreamReader.Peek() > -1)
        {
          CurrentLine = await AnalyserStreamReader.ReadLineAsync();
          if (System.String.IsNullOrWhiteSpace(CurrentLine))
            continue;

          if (CurrentLine.Count(c => c == '"') % 2 != 0)
            continue;

          System.String[] Columns = System.Text.RegularExpressions.Regex.Split(CurrentLine, RegexSplit).Where((c, i) => i % 2 == 1).ToArray();
          if (FileColumns.Count > Columns.Length)
            continue;

          System.Data.DataRow DataRow = Result.Rows.Add();

          for (System.Int32 i = 0; i < this.FileColumns.Count; i++)
          {
            System.Int32 j = this.FileColumns[i].Index;

            if (System.String.IsNullOrEmpty(Columns[j]))
              DataRow[i] = System.Convert.DBNull;
            else
            {
              if (Columns[j].StartsWith('"'))
                Columns[j] = Columns[j][1..^0];
              if (Columns[j].EndsWith('"'))
                Columns[j] = Columns[j][0..^1];

              System.Globalization.CultureInfo ConverterCultureInfo = this.FileColumns[i].CultureInfo ?? this.CultureInfo;
              if (ConverterCultureInfo == null)
              {
                DataRow[i] = System.Net.WebUtility.HtmlDecode(Columns[j]);
                continue;
              }

              System.String NumberGroupSeparator = ConverterCultureInfo.NumberFormat.NumberGroupSeparator;

              switch (this.FileColumns[i].DataTypeName)
              {
                case "Boolean":
                  DataRow[i] = System.Int32.Parse(Columns[j].Replace("true", "1").Replace("false", "0"));
                  break;
                case "DateTime":
                  DataRow[i] = System.Convert.ToDateTime(Columns[j], ConverterCultureInfo);
                  break;
                case "DateTimeOffset":
                  DataRow[i] = System.DateTimeOffset.Parse(Columns[j], ConverterCultureInfo);
                  break;
                case "Int32":
                  DataRow[i] = System.Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(Columns[j], @$"[\-\+\{NumberGroupSeparator}]", ""), ConverterCultureInfo);
                  break;
                case "Int64":
                  DataRow[i] = System.Convert.ToInt64(System.Text.RegularExpressions.Regex.Replace(Columns[j], @$"[\-\+\{NumberGroupSeparator}]", ""), ConverterCultureInfo);
                  break;
                case "Single":
                  DataRow[i] = System.Convert.ToSingle(Columns[j], ConverterCultureInfo);
                  break;
                case "Double":
                  DataRow[i] = System.Convert.ToDouble(Columns[j], ConverterCultureInfo);
                  break;
                default:
                  DataRow[i] = System.Net.WebUtility.HtmlDecode(Columns[j]);
                  break;
              }
            }
          }
        }
      }

      for (System.Int32 i = 0; i < Result.Columns.Count; i++)
        Result.Columns[i].AllowDBNull = FileColumns[i].AllowNulls;

      return Result;
    }
    private void DefineFileColumnProperties(SoftmakeAll.SDK.FileWR.CSV.FileColumn FileColumn, System.String Value, System.Globalization.CultureInfo CultureInfo)
    {
      Value = Value.ToLower();

      if ((Value == "0") || (Value == "1") || (Value == "false") || (Value == "true"))
      {
        FileColumn.DataTypeName = "Boolean";
        return;
      }

      if ((Value.Length == 10) && (Value.Count(c => ((c == '/') || (c == '-'))) == 2))
      {
        System.DateTime Parsed = new System.DateTime();
        if (!(System.DateTime.TryParse(Value, CultureInfo, System.Globalization.DateTimeStyles.None, out Parsed)))
        {
          FileColumn.DataTypeName = "String";
          return;
        }
        FileColumn.DataTypeName = "DateTime";
        return;
      }

      if ((Value.Any(c => System.Char.IsLetter(c))) || ((System.String.IsNullOrWhiteSpace(Value)) && (Value != "")))
      {
        FileColumn.DataTypeName = "String";
        return;
      }

      System.String NumberGroupSeparator = CultureInfo.NumberFormat.NumberGroupSeparator;
      if (System.Text.RegularExpressions.Regex.Replace(Value, @$"[\-\+\{NumberGroupSeparator}]", "").All(c => System.Char.IsNumber(c)))
      {
        System.Int64 Parsed = 0;
        if (!(System.Int64.TryParse(Value, System.Globalization.NumberStyles.Any, CultureInfo, out Parsed)))
        {
          FileColumn.DataTypeName = "String";
          return;
        }
        if (Parsed > System.Int32.MaxValue)
          FileColumn.DataTypeName = "Int64";
        else
          FileColumn.DataTypeName = "Int32";
        return;
      }

      if ((System.Text.RegularExpressions.Regex.Replace(Value, @"[\-\+\.\,]", "").All(c => System.Char.IsNumber(c))))
      {
        System.Double Parsed = 0;
        if (!(System.Double.TryParse(Value, System.Globalization.NumberStyles.Any, CultureInfo, out Parsed)))
        {
          FileColumn.DataTypeName = "String";
          return;
        }
        if (Parsed > System.Single.MaxValue)
          FileColumn.DataTypeName = "Double";
        else
          FileColumn.DataTypeName = "Single";
        return;
      }

      FileColumn.DataTypeName = "String";
    }
    #endregion
  }
}