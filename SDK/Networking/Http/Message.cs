using System.Linq;

namespace SoftmakeAll.SDK.Networking.Http
{
  public abstract class Message
  {
    #region Fields
    private readonly System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<System.String>> _Headers;
    private readonly System.Net.CookieCollection _Cookies;
    #endregion

    #region Constructor
    public Message()
    {
      this._Cookies = new System.Net.CookieCollection();
      this._Headers = new System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<System.String>>();
      this._Body = null;
    }
    #endregion

    #region Properties
    #region Read Only
    private System.Byte[] _Body;
    public System.Byte[] Body { get => this._Body; }
    #endregion
    #endregion

    #region Methods
    private System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<System.String>> GetAllHeaders()
    {
      if (!(this._Cookies.Any()))
        return this._Headers;

      System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<System.String>> Result = new System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<System.String>>();
      foreach (var Header in this._Headers)
        Result.Add(Header.Key, Header.Value);

      Result.Add("Cookie", new System.Collections.Generic.List<System.String>() { System.String.Join(';', this._Cookies.Where(c => ((c.Expires == System.DateTime.MinValue) || (c.Expires >= System.DateTime.Now))).Select(c => $"{c.Name}={c.Value}")) });

      return Result;
    }
    public void AddHeader(System.String Key, System.String Value) => this.AddHeader(Key, Value, false);
    public void AddHeader(System.String Key, System.String Value, System.Boolean Overwrite)
    {
      if (System.String.IsNullOrWhiteSpace(Key))
        return;

      if (Key.ToLower() == "cookie")
        throw new System.Exception($"Invalid Key: '{Key}'. Use AddCookie(...) instead.");

      if ((!(this._Headers.ContainsKey(Key))) || (Overwrite))
        this._Headers[Key] = new System.Collections.Generic.List<System.String>() { Value };
      else
        this._Headers[Key].Add(Value);
    }
    public System.Collections.Generic.List<System.String> GetHeader(System.String Key) => ((System.String.IsNullOrWhiteSpace(Key)) || (!(this.GetAllHeaders().ContainsKey(Key)))) ? null : this.GetAllHeaders()[Key];
    public System.String GetHeader(System.String Key, System.String Separator)
    {
      System.Collections.Generic.List<System.String> Headers = this.GetHeader(Key);
      return Headers == null ? null : System.String.Join(Separator, Headers);
    }
    public System.String GetHeaderValue(System.String Key) => this.GetHeader(Key)?.FirstOrDefault();
    public System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<System.String>> GetHeaders() => this.GetAllHeaders();
    public System.Collections.Generic.Dictionary<System.String, System.String> GetHeaders(System.String Separator) => this.GetAllHeaders().ToDictionary(h => h.Key, h => System.String.Join(Separator, h.Value));
    public void ClearHeaders() => this.ClearHeaders(false);
    public void ClearHeaders(System.Boolean KeepCookies)
    {
      this._Headers.Clear();
      if (!(KeepCookies)) this.ClearCookies();
    }
    public void RemoveHeader(System.String Key)
    {
      if (System.String.IsNullOrWhiteSpace(Key))
        return;

      if (Key.ToLower() == "cookie")
        throw new System.Exception($"Invalid Key: '{Key}'. Use RemoveCookie(...) instead.");

      this._Headers.Remove(Key);
    }

    public virtual void AddCookie(System.Net.Cookie Cookie)
    {
      if ((Cookie != null) && (!(System.String.IsNullOrWhiteSpace(Cookie.Name))))
        this._Cookies.Add(Cookie);
    }
    public virtual void AddCookie(System.String Name, System.String Value)
    {
      if (System.String.IsNullOrWhiteSpace(Name))
        return;

      this.AddCookie(new System.Net.Cookie(Name, Value));
    }
    public System.Net.Cookie GetCookie(System.String Name) => System.String.IsNullOrWhiteSpace(Name) ? null : this._Cookies.FirstOrDefault(c => c.Name == Name);
    public System.Net.CookieCollection GetCookies() => this._Cookies;
    public void RemoveCookie(System.String Name)
    {
      System.Net.Cookie Cookie = this.GetCookie(Name);
      if (Cookie != null)
        this._Cookies.Remove(Cookie);
    }
    public void ClearCookies() => this._Cookies.Clear();

    public virtual void SetBody(System.String Body, System.String ContentType)
    {
      if ((System.String.IsNullOrWhiteSpace(Body)) && (System.String.IsNullOrWhiteSpace(ContentType)))
      {
        this.ClearBody();
        return;
      }

      if (System.String.IsNullOrWhiteSpace(Body))
        throw new System.ArgumentNullException("Body");

      if (System.String.IsNullOrWhiteSpace(ContentType))
        throw new System.ArgumentNullException("ContentType");

      this.SetBody(System.Text.Encoding.UTF8.GetBytes(Body), ContentType);
    }
    public virtual void SetBody(System.Byte[] Body, System.String ContentType)
    {
      if ((Body == null) && (System.String.IsNullOrWhiteSpace(ContentType)))
      {
        this.ClearBody();
        return;
      }

      if (Body == null)
        throw new System.ArgumentNullException("Body");

      if (System.String.IsNullOrWhiteSpace(ContentType))
        throw new System.ArgumentNullException("ContentType");

      this._Body = Body;
      this.AddHeader("Content-Type", ContentType, true);
      this.AddHeader("Content-Length", this._Body.Length.ToString(), true);
    }
    public virtual void ClearBody()
    {
      this._Body = null;
      this.RemoveHeader("Content-Type");
      this.RemoveHeader("Content-Length");
    }
    #endregion
  }
}