using Newtonsoft.Json;

namespace PadelChampMobile.Services;

public class RegisterServices : IregisterRepository
{
    public class ErrorInfo
    {
        public string status { get; set; }
        public bool isError { get; set; }
        public string message { get; set; }
        public List<string> errors { get; set; }
        public object payload { get; set; }
    }
    public async Task<string> Register(string UserName, string Email, string Password, string PhoneNumber, string FirstName, string LastName, string Gender, DateTime DateOfBirth)
    {
        try
        {
           // Resource<object> resource = new Resource<object>(); 
            var d = DateOfBirth.Date;
            // var dateTimeString = DateOfBirth.Date.ToString();
            // var dateOfBirth = dateTimeString.Split(' ');
             // string formattedDate = "4-4-2024";// dateOfBirth[0];
            string formattedDate = DateOfBirth.ToString("d-M-yyyy");

           
            var client = new HttpClient();
            string baseUrl = "http://10.0.2.2:5079/api/auth/register/" + UserName + "/" + Email + "/" + Password + "/" + PhoneNumber + "/" + FirstName + "/" + LastName + "/" + Gender + "/" + formattedDate;
            client.BaseAddress = new Uri(baseUrl);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return "true";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(error);

                string errorMessage = errorInfo.errors[0];



                return errorMessage;
            }
        }
        catch (Exception ex)
        {

            // Handle exception
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "false";
        }
    }
}
