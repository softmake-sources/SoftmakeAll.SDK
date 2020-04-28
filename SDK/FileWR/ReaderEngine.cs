using System.Linq;

namespace SoftmakeAll.SDK.FileWR
{
  public class ReaderEngine : System.IDisposable
  {
    #region Constructor
    public ReaderEngine(SoftmakeAll.SDK.FileWR.FileMap FileMap, System.String Path, System.Int32 BufferSize)
    {
      if ((FileMap == null) || (!(FileMap.Builded)))
        throw new System.Exception("The map needs to be build. Invoke the BuildMap() method from the FileMap class.");

      System.Boolean HasValidationErrors = true;
      using (System.IO.FileStream DiscoverNewLineFileStream = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
      {
        if (DiscoverNewLineFileStream.Length == FileMap.FileRegistersLength + 1)
        {
          System.Int32 Factor = FileMap.FileRegistersLength + 1;
          System.Byte[] Buffer = new System.Byte[Factor];
          DiscoverNewLineFileStream.Read(Buffer, 0, Factor);
          if (Buffer[Factor - 1] == 10)
          {
            this.NewLineCharsCount = 1;
            HasValidationErrors = false;
          }
        }
        else
        {
          System.Int32 Factor = FileMap.FileRegistersLength + 2;
          System.Byte[] Buffer = new System.Byte[Factor];
          DiscoverNewLineFileStream.Read(Buffer, 0, Factor);
          if (((Buffer[Factor - 2] == 13) && (Buffer[Factor - 1] == 10)) || ((Buffer[Factor - 2] == 10) && (Buffer[Factor - 1] == 13)))
          {
            this.NewLineCharsCount = 2;
            HasValidationErrors = false;
          }
          else if (Buffer[Factor - 2] == 10)
          {
            this.NewLineCharsCount = 1;
            HasValidationErrors = false;
          }
        }
      }

      if (HasValidationErrors)
        throw new System.Exception("The file must end with a line break.");

      this.FileStream = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);

      this.FileMap = FileMap;
      this.Path = Path;

      this.FileRegistersLengthWithNewLine = FileMap.FileRegistersLength + this.NewLineCharsCount;

      if (System.Environment.Is64BitOperatingSystem)
        BufferSize = System.Math.Min(BufferSize, 2147483591);
      else
        BufferSize = System.Math.Min(BufferSize, 1073741794);

      BufferSize = System.Convert.ToInt32(System.Math.Min(System.Convert.ToInt64(BufferSize), this.FileStream.Length));
      BufferSize = System.Math.Max(BufferSize, FileMap.FileRegistersLength);
      BufferSize = System.Math.Max(BufferSize, FileRegistersLengthWithNewLine);
      this.BufferSize = (BufferSize / FileRegistersLengthWithNewLine) * FileRegistersLengthWithNewLine;

      this.Data = new System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.Data>();
    }
    #endregion

    #region Fields
    private System.IO.FileStream FileStream;
    private System.Byte NewLineCharsCount;
    private System.Int32 FileRegistersLengthWithNewLine;
    #endregion

    #region Properties
    public SoftmakeAll.SDK.FileWR.FileMap FileMap { get; }
    public System.String Path { get; }
    public System.Int32 BufferSize { get; }
    public System.Int64 TotalBytesReaded = 0;
    public System.Int64 TotalBytesToRead => this.FileStream.Length;
    public System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.Data> Data { get; }
    #endregion

    #region Methods
    public System.Boolean EndOfStream()
    {
      return this.TotalBytesReaded >= this.TotalBytesToRead;
    }
    public void ReadRegisters() { this.ReadRegisters(true); }
    public void ReadRegisters(System.Boolean ClearDataBeforeRead)
    {
      if (ClearDataBeforeRead)
        this.Data.Clear();

      System.Byte[] Buffer = new System.Byte[this.BufferSize];
      this.FileStream.Read(Buffer, 0, Buffer.Length);
      this.TotalBytesReaded += Buffer.Length;

      System.Int32 BytesReaded = 0;

      while (BytesReaded < Buffer.Length)
      {
        System.Byte[] Register = Buffer.Skip(BytesReaded).Take(this.FileRegistersLengthWithNewLine).ToArray();
        BytesReaded += this.FileRegistersLengthWithNewLine;
        if (Register[0] == 0)
          continue;

        SoftmakeAll.SDK.FileWR.FileRegister FileRegister = this.FileMap.FileRegisters.FirstOrDefault(fr => fr.Type == Register[fr.TypePosition]);

        SoftmakeAll.SDK.FileWR.Data RegisterData = new SoftmakeAll.SDK.FileWR.Data();
        RegisterData.RegisterType = FileRegister.Type;
        RegisterData.ColumnValues = new System.Collections.Generic.List<System.Byte[]>();
        foreach (SoftmakeAll.SDK.FileWR.FileRegisterColumn FileRegisterColumn in FileRegister.FileRegisterColumns)
          if (!(FileRegisterColumn.IgnoreValues))
            RegisterData.ColumnValues.Add(Register.Skip(FileRegisterColumn.StartPosition).Take(FileRegisterColumn.ContentLength).ToArray());
        this.Data.Add(RegisterData);
      }
    }
    public async System.Threading.Tasks.Task ReadRegistersAsync() { await this.ReadRegistersAsync(true); }
    public async System.Threading.Tasks.Task ReadRegistersAsync(System.Boolean ClearDataBeforeRead)
    {
      if (ClearDataBeforeRead)
        this.Data.Clear();

      System.Byte[] Buffer = new System.Byte[this.BufferSize];
      await this.FileStream.ReadAsync(Buffer, 0, Buffer.Length);
      this.TotalBytesReaded += Buffer.Length;

      System.Int32 BytesReaded = 0;

      while (BytesReaded < Buffer.Length)
      {
        System.Byte[] Register = Buffer.Skip(BytesReaded).Take(this.FileRegistersLengthWithNewLine).ToArray();
        BytesReaded += this.FileRegistersLengthWithNewLine;
        if (Register[0] == 0)
          continue;

        SoftmakeAll.SDK.FileWR.FileRegister FileRegister = this.FileMap.FileRegisters.FirstOrDefault(fr => fr.Type == Register[fr.TypePosition]);
        if (FileRegister == null)
          continue;

        SoftmakeAll.SDK.FileWR.Data RegisterData = new SoftmakeAll.SDK.FileWR.Data();
        RegisterData.RegisterType = FileRegister.Type;
        RegisterData.ColumnValues = new System.Collections.Generic.List<System.Byte[]>();
        foreach (SoftmakeAll.SDK.FileWR.FileRegisterColumn FileRegisterColumn in FileRegister.FileRegisterColumns)
          if (!(FileRegisterColumn.IgnoreValues))
            RegisterData.ColumnValues.Add(Register.Skip(FileRegisterColumn.StartPosition).Take(FileRegisterColumn.ContentLength).ToArray());
        this.Data.Add(RegisterData);
      }
    }
    public void Close()
    {
      FileStream.Close();
    }
    public void Dispose()
    {
      FileStream.Close();
      FileStream.Dispose();
    }
    #endregion
  }
}