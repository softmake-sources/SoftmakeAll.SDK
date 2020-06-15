namespace SoftmakeAll.SDK.Helpers.LINQ.Extensions
{
  public static class LINQExtensions
  {
    #region Methods
    public static System.Collections.Generic.IEnumerable<TSource> DistinctBy<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> Source, System.Func<TSource, TKey> KeySelector)
    {
      System.Collections.Generic.HashSet<TKey> HashSet = new System.Collections.Generic.HashSet<TKey>();
      foreach (TSource Element in Source)
        if (HashSet.Add(KeySelector(Element)))
          yield return Element;
    }
    #endregion
  }
}