namespace HjUI01.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

// ...

public partial class NavBar
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    public IJSRuntime JsRuntime { get; set; }
    struct tab { 
        public int id { get; set; } 
        public string path { get; set; } 
        public string text { get; set; } 
        public string icon { get; set; } 
    }
    string BaseUri;
    bool showingNav = false;
    string navtougleicon = "list";
    List<tab> tabs = new();
    string page = "";
    bool next_to_active(int id){
        if(id == 0)return false;
        return tabs[id - 1].path == page;
    }
    bool befor_active(int id){
        if(id >= tabs.Count -1)return false;
        return tabs[id + 1].path == page;
    }
    string tclass (int id){
        if (befor_active(id)) return "befor-active";
        if (next_to_active(id)) return "next-to-active";
        return page == tabs[id].path? "nav-item-active":"";
    }
    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        page = e.Location.Substring(BaseUri.Length - 1);
        //Console.WriteLine("URL of new location: {Location}", e.Location);
        //Console.WriteLine("new route: {route}", page);
        this.StateHasChanged();
    }
    protected void tougleNav()
    {
        showingNav = !showingNav;
        navtougleicon = showingNav? "x":"list";
    }
    protected override void OnInitialized()
    {
        BaseUri = NavigationManager.Uri;
        page = NavigationManager.Uri.Substring(BaseUri.Length - 1);
        NavigationManager.LocationChanged += HandleLocationChanged;
        tabs.Add(new tab { id = 0, text="" , path="", icon = ""});
        tabs.Add(new tab { id = 1, text="الرئيسية" , path="/" ,icon = "house"});
        tabs.Add(new tab { id = 2, text="عداد" , path="/counter" ,icon = "counter"});
        tabs.Add(new tab { id = 3, text="تعليمات" , path="/info" ,icon = "info"});
        tabs.Add(new tab { id = 4, text="" , path="" ,icon = ""});
    }
    public void Dispose()
    {
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
    bool NoId = true;
    async void NoIdAsync() =>
       NoId = await JsRuntime.InvokeAsync<string>("ReadCookie.ReadCookie", "id") == "";

}
