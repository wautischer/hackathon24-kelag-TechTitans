using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

public class ApiDAO
{
    private readonly HttpClient _httpClient;
    private readonly TicketService _ticketService;

    public ApiDAO(HttpClient httpClient, TicketService ticketService)
    {
        _httpClient = httpClient;
        _ticketService = ticketService;
    }

    public async Task InsertIssueAsync(IssueModel issue)
    {
        const string repoName = "Kelag-Hackathon-2024-Team-4";
        const string url = $"https://apim-forstsee-hackathon.azure-api.net/github/{repoName}/issues";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        string secret = Environment.GetEnvironmentVariable("SUB-KEY");
        request.Headers.Add("Ocp-Apim-Subscription-Key", secret);
        request.Headers.Add("Cache-Control", "no-cache");

        var jsonIssue = new
        {
            issue.title,
            issue.body,
            labels = issue.labels.Split(',').Select(label => label.Trim()).ToArray()
        };

        request.Content = new StringContent(JsonSerializer.Serialize(jsonIssue), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        Console.WriteLine(response.IsSuccessStatusCode ? "Issue successfully inserted." : "Error inserting issue.");
    }

    public async Task InsertIssueAsync(string titleU, string bodyU, string labelsU)
    {
        const string repoName = "Kelag-Hackathon-2024-Team-4";
        const string url = $"https://apim-forstsee-hackathon.azure-api.net/github/{repoName}/issues";
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        string secret = Environment.GetEnvironmentVariable("SUB-KEY");
        request.Headers.Add("Ocp-Apim-Subscription-Key", secret);
        request.Headers.Add("Cache-Control", "no-cache");

        var jsonIssue = new
        {
            title = titleU,
            body = bodyU,
            labels = labelsU.Split(',').Select(label => label.Trim()).ToArray()
        };

        request.Content = new StringContent(JsonSerializer.Serialize(jsonIssue), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        Console.WriteLine(response.IsSuccessStatusCode ? "Issue successfully inserted." : "Error inserting issue.");
    }

    public async Task InsertTicketAsync(string title, string shortDesc, string tags, string longDesc, string priority)
    {
        var ticket = new Ticket
        {
            title = title,
            tags = tags,
            description = shortDesc,
            created_at = DateTime.UtcNow,
            priority = priority,
            descriptionLong = longDesc
        };

        await _ticketService.InsertTicketAsync(ticket);
        Console.WriteLine("Ticket successfully inserted into the database.");
    }
}

public class IssueModel
{
    public string title { get; set; }
    public string body { get; set; }
    public string labels { get; set; }
}

public class RequestModel
{
    public string description { get; set; }
}

public class ChatResponse
{
    public Choice[] choices { get; set; }
}

public class Choice
{
    public Message message { get; set; }
}

public class Message
{
    public string content { get; set; }
}
