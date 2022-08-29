using System.Net;
using System.Text.Json;
using System.Text;

namespace HjUI01.Pages;

public partial class Register
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    string message = "";
    string status = "" ;
    string[] errors = { "","","","",""};
    string url = "https://localhost:5000/api/Authenticate/register";
    //HttpClient should be instancied once and not be disposed 
    private static readonly HttpClient client = new HttpClient();

    protected async void register()
    {
        var url = "https://localhost:5000/api/Authenticate/register";


        var data = new Dictionary<string, string>
        {
            {"FirstName", FirstName},
            {"LastName", LastName},
            {"PhoneNumber", PhoneNumber},
            {"Password", Password},
            {"ConfirmPassword", ConfirmPassword}
        };

        var res = await Post(data);
    }

    public async Task<string> Post( Dictionary<string, string> data )
    {
        var res = await client.PostAsync(url, new FormUrlEncodedContent(data));

        return await res.Content.ReadAsStringAsync();
    }
}
