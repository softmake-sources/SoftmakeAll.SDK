namespace SoftmakeAll.SDK.Helpers.Regex.Extensions
{
  public static class RegexExtensions
  {
    #region Methods
    public static System.Boolean IsValidEmail(this System.String String)
    {
      if (System.String.IsNullOrWhiteSpace(String)) return false;
      const System.String Pattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
      return System.Text.RegularExpressions.Regex.IsMatch(String, Pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
    public static System.Boolean IdnMappingIsValidEmail(this System.String String)
    {
      if (System.String.IsNullOrWhiteSpace(String))
        return false;

      try
      {
        String = System.Text.RegularExpressions.Regex.Replace(String, @"(@)(.+)$", DomainMapper, System.Text.RegularExpressions.RegexOptions.None, System.TimeSpan.FromMilliseconds(200));
        System.String DomainMapper(System.Text.RegularExpressions.Match Match)
        {
          return System.String.Concat(Match.Groups[1].Value, new System.Globalization.IdnMapping().GetAscii(Match.Groups[2].Value));
        }
      }
      catch (System.Text.RegularExpressions.RegexMatchTimeoutException) { return false; }
      catch (System.ArgumentException) { return false; }

      return SoftmakeAll.SDK.Helpers.Regex.Extensions.RegexExtensions.IsValidEmail(String);
    }
    #endregion
  }
}