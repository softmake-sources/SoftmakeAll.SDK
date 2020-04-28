﻿using System.IO.Compression;
using System.Linq;

namespace SoftmakeAll.SDK.Files
{
  public static class Compression
  {
    #region Methods
    public static System.Byte[] CreateZipArchive(System.Collections.Generic.IEnumerable<System.String> Files)
    {
      if ((Files == null) || (!(Files.Any())))
        return null;

      System.Collections.Generic.Dictionary<System.String, System.String> FilesDictionary = new System.Collections.Generic.Dictionary<System.String, System.String>();
      foreach (System.String File in Files)
        FilesDictionary.Add(File, File);

      return SoftmakeAll.SDK.Files.Compression.CreateZipArchive(FilesDictionary);
    }
    public static System.Byte[] CreateZipArchive(System.Collections.Generic.Dictionary<System.String, System.String> Files)
    {
      if ((Files == null) || (Files.Count == 0))
        return null;

      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        using (System.IO.Compression.ZipArchive ZipArchive = new System.IO.Compression.ZipArchive(MemoryStream, System.IO.Compression.ZipArchiveMode.Create, false))
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> File in Files)
            ZipArchive.CreateEntryFromFile(File.Key, File.Value);
        return MemoryStream.ToArray();
      }
    }
    #endregion
  }
}