using System.Linq;

namespace SoftmakeAll.SDK.Helpers.String.Extensions
{
  public static class StringExtensions
  {
    #region Methods
    public static System.String RemoveDiacritics(this System.String String)
    {
      if (System.String.IsNullOrEmpty(String))
        return String;

      System.Text.StringBuilder StringBuilder = new System.Text.StringBuilder();

      foreach (System.Char Char in String.Normalize(System.Text.NormalizationForm.FormD))
        if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(Char) != System.Globalization.UnicodeCategory.NonSpacingMark)
          StringBuilder.Append(Char);

      return StringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
    }
    public static System.String ToEmptyIfNull(this System.String String)
    {
      if (String == null)
        return "";

      return String;
    }
    public static System.Boolean IsNullOrEmpty(this System.String String)
    {
      return System.String.IsNullOrEmpty(String);
    }
    public static System.Boolean IsNullOrWhiteSpace(this System.String String)
    {
      return System.String.IsNullOrWhiteSpace(String);
    }
    public static System.Boolean IsNullOrEmpty(params System.String[] Strings)
    {
      if ((Strings == null) || (Strings.Length == 0))
        return false;

      foreach (System.String String in Strings)
        if (String.IsNullOrEmpty())
          return true;

      return false;
    }
    public static System.Boolean IsNullOrWhiteSpace(params System.String[] Strings)
    {
      if ((Strings == null) || (Strings.Length == 0))
        return false;

      foreach (System.String String in Strings)
        if (String.IsNullOrWhiteSpace())
          return true;

      return false;
    }
    public static System.String ReplaceLeftWhiteSpacesUntil(this System.String String, System.Char NewChar) { return String.ReplaceLeftWhiteSpacesUntil(NewChar, null); }
    public static System.String ReplaceLeftWhiteSpacesUntil(this System.String String, System.Char NewChar, System.Char IgnoreChar) { return String.ReplaceLeftWhiteSpacesUntil(NewChar, new System.Char[] { IgnoreChar }); }
    public static System.String ReplaceLeftWhiteSpacesUntil(this System.String String, System.Char NewChar, System.Char[] IgnoreChars)
    {
      if (System.String.IsNullOrEmpty(String))
        return String;

      if (System.String.IsNullOrWhiteSpace(String))
        return String.Replace(' ', NewChar);

      if (IgnoreChars == null)
        IgnoreChars = new System.Char[] { ' ' };
      else
        IgnoreChars = IgnoreChars.Concat(new System.Char[] { ' ' }).ToArray();

      System.Int32 Count = 0;
      while ((Count < String.Length) && (IgnoreChars.Contains(String[Count])))
        Count++;

      String = $"{String.Substring(0, Count).Replace(' ', NewChar)}{String.Substring(Count)}";
      return String;
    }
    public static System.String ReplaceRightWhiteSpacesUntil(this System.String String, System.Char NewChar) { return String.ReplaceRightWhiteSpacesUntil(NewChar, null); }
    public static System.String ReplaceRightWhiteSpacesUntil(this System.String String, System.Char NewChar, System.Char IgnoreChar) { return String.ReplaceRightWhiteSpacesUntil(NewChar, new System.Char[] { IgnoreChar }); }
    public static System.String ReplaceRightWhiteSpacesUntil(this System.String String, System.Char NewChar, System.Char[] IgnoreChars)
    {
      if (System.String.IsNullOrEmpty(String))
        return String;

      if (System.String.IsNullOrWhiteSpace(String))
        return String.Replace(' ', NewChar);

      if (IgnoreChars == null)
        IgnoreChars = new System.Char[] { ' ' };
      else
        IgnoreChars = IgnoreChars.Concat(new System.Char[] { ' ' }).ToArray();

      System.Int32 Count = String.Length - 1;
      while ((Count >= 0) && (IgnoreChars.Contains(String[Count])))
        Count--;

      String = $"{String.Substring(0, Count)}{String.Substring(Count).Replace(' ', NewChar)}";
      return String;
    }
    #endregion
  }
}