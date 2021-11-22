using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports listing resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class SupportsListing<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T>
  {
    #region Constructor
    /// <summary>
    /// Supports listing resources.
    /// </summary>
    /// <param name="Route">The address of the resource.</param>
    public SupportsListing(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    public System.String CompressStringForURL(System.String UncompressedString)
    {
      if (System.String.IsNullOrWhiteSpace(UncompressedString))
        return UncompressedString;

      try
      {
        using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
        {
          using (System.IO.Compression.DeflateStream DeflateStream = new System.IO.Compression.DeflateStream(MemoryStream, System.IO.Compression.CompressionLevel.Fastest))
          using (System.IO.StreamWriter StreamWriter = new System.IO.StreamWriter(DeflateStream, System.Text.Encoding.UTF8))
            StreamWriter.Write(UncompressedString);

          return System.Web.HttpUtility.UrlEncode(System.Convert.ToBase64String(MemoryStream.ToArray()));
        }
      }
      catch { }

      return null;
    }
    private System.String GenerateListURL(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
    {
      System.String URL = $"{base.Route}?skip={Skip}&take={Take}";

      if ((Parameters != null) && (Parameters.Any())) URL = $"{URL}&{System.String.Join('&', Parameters.Select(p => $"{p.Key}={p.Value}"))}";
      if ((Fields != null) && (Fields.Any())) URL = $"{URL}&fields={System.String.Join(',', Fields)}";
      if ((Filter != null) && (Filter.Any())) URL = $"{URL}&filter=dflt:{this.CompressStringForURL(Filter.ToJsonElement().ToRawText())}";
      if ((Group != null) && (Group.Any())) URL = $"{URL}&group={System.String.Join(',', Group)}";
      if ((Sort != null) && (Sort.Any())) URL = $"{URL}&sort={System.String.Join('&', Sort.Select(p => $"{(p.Value ? '-' : '+')}{p.Key}"))}";

      return URL;
    }

    /// <summary>
    /// Fetch a list of resources.
    /// </summary>
    /// <param name="Parameters">Parameters to send as a query string.</param>
    /// <param name="Fields">List of field names to retrieve.</param>
    /// <param name="Filter">List of filters to apply.</param>
    /// <param name="Group">List of field names to group and perform aggregates if available.</param>
    /// <param name="Sort">List of fields to apply sorting.</param>
    /// <param name="Skip">Number of records to skip.</param>
    /// <param name="Take">Number of records to take.</param>
    /// <returns>The list of the resources.</returns>
    public SoftmakeAll.SDK.Fluent.ResourceList<T> List(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => base.ProcessListOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = this.GenerateListURL(Parameters, Fields, Filter, Group, Sort, Skip, Take) }));

    /// <summary>
    /// Fetch a list of resources.
    /// </summary>
    /// <param name="Parameters">Parameters to send as a query string.</param>
    /// <param name="Fields">List of field names to retrieve.</param>
    /// <param name="Filter">List of filters to apply.</param>
    /// <param name="Group">List of field names to group and perform aggregates if available.</param>
    /// <param name="Sort">List of fields to apply sorting.</param>
    /// <param name="Skip">Number of records to skip.</param>
    /// <param name="Take">Number of records to take.</param>
    /// <returns>The list of the resources.</returns>
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => base.ProcessListOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = this.GenerateListURL(Parameters, Fields, Filter, Group, Sort, Skip, Take) }));

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Byte ID) => this.Show(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int16 ID) => this.Show(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int32 ID) => this.Show(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int64 ID) => this.Show(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Char ID) => this.Show(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.String ID)
    {
      if (!(System.String.IsNullOrWhiteSpace(ID)))
        return base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = $"{base.Route}/{ID}" }), default);

      return default;
    }


    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID) => await this.ShowAsync(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID) => await this.ShowAsync(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID) => await this.ShowAsync(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID) => await this.ShowAsync(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Char ID) => await this.ShowAsync(ID.ToString());

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.String ID)
    {
      if (!(System.String.IsNullOrWhiteSpace(ID)))
        return base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = $"{base.Route}/{ID}" }), default);

      return default;
    }
    #endregion
  }
}