using System.Net;
using System.Text.Json;

// Build request to acquire managed identities for Azure resources token
var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

try
{
    // Call /token endpoint
    var response = await myClient.GetAsync("http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https://management.azure.com/");
    var streamResponse = await response.Content.ReadAsStreamAsync();

    // Pipe response Stream to a StreamReader, and extract access token
    StreamReader sr = new StreamReader(streamResponse); 
    string stringResponse = sr.ReadToEnd();
    Dictionary<string, string> list = JsonSerializer.Deserialize<Dictionary<string, string>>(stringResponse)!;
    string accessToken = list["access_token"];
    Console.WriteLine(accessToken);
    Console.ReadKey();
}
catch (Exception e)
{
    string errorText = String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : "Acquire token failed");
    Console.WriteLine(errorText);
    Console.ReadKey();
}