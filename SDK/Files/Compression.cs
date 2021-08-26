using System.IO.Compression;
using System.Linq;

namespace SoftmakeAll.SDK.Files
{
  public static class Compression
  {
    #region Methods
    public static System.Byte[] CreateZipArchive(System.Collections.Generic.IEnumerable<System.String> Files) => SoftmakeAll.SDK.Files.Compression.CreateZipArchive(Files.ToDictionary(k => k, v => new System.IO.FileInfo(v).Name));
    public static System.Byte[] CreateZipArchive(System.Collections.Generic.Dictionary<System.String, System.String> Contents)
    {
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        SoftmakeAll.SDK.Files.Compression.CreateZipArchive(Contents, MemoryStream);
        return MemoryStream.ToArray();
      }
    }
    public static System.Byte[] CreateZipArchive(System.Collections.Generic.Dictionary<System.String, System.Byte[]> Contents)
    {
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        SoftmakeAll.SDK.Files.Compression.CreateZipArchive(Contents, MemoryStream);
        return MemoryStream.ToArray();
      }
    }
    public static async System.Threading.Tasks.Task<System.Byte[]> CreateZipArchiveAsync(System.Collections.Generic.Dictionary<System.String, System.Byte[]> Contents)
    {
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        await SoftmakeAll.SDK.Files.Compression.CreateZipArchiveAsync(Contents, MemoryStream);
        return MemoryStream.ToArray();
      }
    }

    public static void CreateZipArchive(System.Collections.Generic.IEnumerable<System.String> Files, System.IO.Stream Destination) => SoftmakeAll.SDK.Files.Compression.CreateZipArchive(Files.ToDictionary(k => k, v => new System.IO.FileInfo(v).Name), Destination);
    public static void CreateZipArchive(System.Collections.Generic.Dictionary<System.String, System.String> Contents, System.IO.Stream Destination)
    {
      if ((Contents != null) && (Contents.Count > 0))
        using (System.IO.Compression.ZipArchive ZipArchive = new System.IO.Compression.ZipArchive(Destination, System.IO.Compression.ZipArchiveMode.Create, false))
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> Content in Contents)
            ZipArchive.CreateEntryFromFile(Content.Key, Content.Value);
    }
    public static void CreateZipArchive(System.Collections.Generic.Dictionary<System.String, System.Byte[]> Contents, System.IO.Stream Destination)
    {
      if ((Contents != null) && (Contents.Count > 0))
        using (System.IO.Compression.ZipArchive ZipArchive = new System.IO.Compression.ZipArchive(Destination, System.IO.Compression.ZipArchiveMode.Create, false))
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.Byte[]> Content in Contents)
            using (System.IO.Stream Stream = ZipArchive.CreateEntry(Content.Key).Open())
              Stream.Write(Content.Value, 0, Content.Value.Length);
    }
    public static async System.Threading.Tasks.Task CreateZipArchiveAsync(System.Collections.Generic.Dictionary<System.String, System.Byte[]> Contents, System.IO.Stream Destination)
    {
      if ((Contents != null) && (Contents.Count > 0))
        using (System.IO.Compression.ZipArchive ZipArchive = new System.IO.Compression.ZipArchive(Destination, System.IO.Compression.ZipArchiveMode.Create, false))
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.Byte[]> Content in Contents)
            using (System.IO.Stream Stream = ZipArchive.CreateEntry(Content.Key).Open())
              await Stream.WriteAsync(Content.Value, 0, Content.Value.Length);
    }
    #endregion
  }
}