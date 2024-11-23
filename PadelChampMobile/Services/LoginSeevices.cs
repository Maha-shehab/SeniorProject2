using Newtonsoft.Json;
using System.Text;
using Microsoft.Maui.Storage; // For Preferences API

namespace PadelChampMobile.Services;

public class ErrorInfo
{
    public string status { get; set; }
    public bool isError { get; set; }
    public string message { get; set; }
    public List<string> errors { get; set; }
    public dynamic payload { get; set; }
}

public class LoginSeevices : ILoginRepository
{
    public LoginSeevices()
    {
    }

    public async Task<string> Login(string email, string password)
    {
        try
        {
            using (var client = new HttpClient())
            {
                string baseUrl = $"http://10.0.2.2:5079/api/auth/login/{email}/{password}";
                client.BaseAddress = new Uri(baseUrl);

                // Make the GET request
                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON response
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<ErrorInfo>(jsonResponse);

                    // Store email and name in Preferences
                    Preferences.Set("UserEmail", (string)responseObject.payload.email);
                    Preferences.Set("UserName", (string)responseObject.payload.name);
                    Preferences.Set("UserId",(string)responseObject.payload.userId);
                    return "true"; // Login successful
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(error);

                    return errorInfo.errors.Count > 0 ? errorInfo.errors[0] : "Unknown error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            return $"Network error: {httpEx.Message}";
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }
}
