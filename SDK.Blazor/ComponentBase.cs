namespace SoftmakeAll.SDK.Blazor.Components
{
  public abstract class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
  {
    #region Constructor
    public ComponentBase() : base() { }
    #endregion

    #region Parameters
    [Microsoft.AspNetCore.Components.Parameter] public System.String ID { get; set; }
    [Microsoft.AspNetCore.Components.Parameter] public Microsoft.AspNetCore.Components.RenderFragment ChildContent { get; set; }
    #endregion

    #region Methods
    protected override void OnInitialized()
    {
      base.OnInitialized();
      this.ID = System.Guid.NewGuid().ToString().Replace("-", "").ToLower();
    }
    #endregion
  }
}