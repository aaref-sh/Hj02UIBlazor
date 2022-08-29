using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace HjUI01.Pages;

public partial class Login
{

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] 
    public IJSRuntime JsRuntime { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string message = "";
    public string status { get; set; }
    bool loading = false;
    string token;

    string url = "https://localhost:5000/api/Authenticate/login";
    //HttpClient should be instancied once and not be disposed 
    private static readonly HttpClient client = new HttpClient();
    string[] errors = { "","" };
    struct Log { public string PhoneNumber { get; set; } public string Password { get; set; } }
    struct ers
    {
        public string[] PhoneNumber { get; set; } 
        public string[] Password { get; set; } 
    }
    struct state
    {
        public bool success { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string? icon { get; set; }
        public object array { get; set; }
        public ers errors { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string id { get; set; }
        public string expiration { get; set; }
    }

    private async Task login()
    {
        if (loading) return;
        errors[0] = "";
        errors[1] = "";
        loading = true;
        var user = new { PhoneNumber = PhoneNumber, Password = Password };
        var response = await client.PostAsJsonAsync(url, user);
        var content = await response.Content.ReadFromJsonAsync<state>();
        if (content.success)
        {
            await WriteCookies("id", content.id, 30);
            await WriteCookies("token", content.token, 30);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            if (content.status == 400)
            {
                message = content.title;
                status = "warning";
                foreach(var item in content.errors.PhoneNumber) errors[0] += item +"<br>";
                foreach(var item in content.errors.Password) errors[1] += item +"<br>";

            }
            else
            {
                message = content.message;
                status = content.icon;
            }
        }

        Console.WriteLine(content);
        loading = false;
    }
    protected async Task WriteCookies(string name, string value,int days)=>
        await JsRuntime.InvokeAsync<object>("WriteCookie.WriteCookie", name, value, DateTime.Now.AddDays(days));

    
    protected async Task<string> ReadCookies(string name) => 
        await JsRuntime.InvokeAsync<string>("ReadCookie.ReadCookie", name);

    
}
