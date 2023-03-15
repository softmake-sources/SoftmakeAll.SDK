using System.Linq;

namespace SoftmakeAll.SDK.Helpers.String.Extensions
{
    public static class StringExtensions
    {
        // DO NOT CHANGE THIS STRING! DO NOT EXECUTE TRIM IN THIS STRING!
        public const System.String UnicodeChars = "                                 !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~                                  ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ";

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
        public static System.String ReplaceLeftWhiteSpacesUntil(this System.String String, System.Char NewChar) => String.ReplaceLeftWhiteSpacesUntil(NewChar, null);
        public static System.String ReplaceLeftWhiteSpacesUntil(this System.String String, System.Char NewChar, System.Char IgnoreChar) => String.ReplaceLeftWhiteSpacesUntil(NewChar, new System.Char[] { IgnoreChar });
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
        public static System.String ReplaceRightWhiteSpacesUntil(this System.String String, System.Char NewChar) => String.ReplaceRightWhiteSpacesUntil(NewChar, null);
        public static System.String ReplaceRightWhiteSpacesUntil(this System.String String, System.Char NewChar, System.Char IgnoreChar) => String.ReplaceRightWhiteSpacesUntil(NewChar, new System.Char[] { IgnoreChar });
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
        public static System.String Stuff(this System.String String, System.Int32 StartIndex, System.Int32 Count, System.String Insert) => String.Remove(StartIndex, Count).Insert(StartIndex, Insert);
        public static System.String Stuff(this System.String String, System.Int32 StartIndex, System.Int32 Count, System.String Insert, System.Boolean ThrowOnError)
        {
            if (ThrowOnError)
                return String.Stuff(StartIndex, Count, Insert);

            if (System.String.IsNullOrEmpty(String))
                return String;

            System.Int32 StringLength = String.Length;

            StartIndex = System.Math.Max(0, StartIndex);
            if (StartIndex >= StringLength)
                return null;

            Count = System.Math.Max(0, Count);
            if (Count <= 0)
                return String;

            return String.Stuff(StartIndex, System.Math.Min(Count, StringLength - StartIndex), Insert);
        }
        public static System.String TreatUnicodeChars(this System.String String)
        {
            if (System.String.IsNullOrWhiteSpace(String))
                return String;

            System.Text.RegularExpressions.MatchCollection MatchCollection = System.Text.RegularExpressions.Regex.Matches(String, @"\\u\w\w(\w\w)", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if ((MatchCollection == null) || (MatchCollection.Count == 0))
                return String;

            System.Text.StringBuilder StringBuilder = new System.Text.StringBuilder(String);

            System.Int32 Factor = -5;
            MatchCollection
              .Select(m => m.Groups[1])
              .Select(g => (System.Byte.Parse(g.Value, System.Globalization.NumberStyles.HexNumber), g.Index - 4)).ToList()
              .ForEach(e => StringBuilder.Stuff(e.Item2 - (Factor += 5), 6, SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.UnicodeChars[e.Item1]));

            return StringBuilder.ToString();
        }

        public static void Stuff(this System.Text.StringBuilder StringBuilder, System.Int32 StartIndex, System.Int32 Count, System.String Insert) => StringBuilder.Remove(StartIndex, Count).Insert(StartIndex, Insert);
        public static void Stuff(this System.Text.StringBuilder StringBuilder, System.Int32 StartIndex, System.Int32 Count, System.String Insert, System.Boolean ThrowOnError)
        {
            if (ThrowOnError)
            {
                StringBuilder.Stuff(StartIndex, Count, Insert);
                return;
            }

            if (StringBuilder == null)
                return;

            System.Int32 StringBuilderLength = StringBuilder.Length;
            if (StringBuilderLength == 0)
                return;

            StartIndex = System.Math.Max(0, StartIndex);
            if (StartIndex >= StringBuilderLength)
                return;

            Count = System.Math.Max(0, Count);
            if (Count <= 0)
                return;

            StringBuilder.Stuff(StartIndex, System.Math.Min(Count, StringBuilderLength - StartIndex), Insert);
        }
        public static void Stuff(this System.Text.StringBuilder StringBuilder, System.Int32 StartIndex, System.Int32 Count, System.Char Insert) => StringBuilder.Remove(StartIndex, Count).Insert(StartIndex, Insert);
        public static void Stuff(this System.Text.StringBuilder StringBuilder, System.Int32 StartIndex, System.Int32 Count, System.Char Insert, System.Boolean ThrowOnError)
        {
            if (ThrowOnError)
            {
                StringBuilder.Stuff(StartIndex, Count, Insert);
                return;
            }

            if (StringBuilder == null)
                return;

            System.Int32 StringBuilderLength = StringBuilder.Length;
            if (StringBuilderLength == 0)
                return;

            StartIndex = System.Math.Max(0, StartIndex);
            if (StartIndex >= StringBuilderLength)
                return;

            Count = System.Math.Max(0, Count);
            if (Count <= 0)
                return;

            StringBuilder.Stuff(StartIndex, System.Math.Min(Count, StringBuilderLength - StartIndex), Insert);
        }
        #endregion
    }
}