using PaymentApplication.Common;
using PaymentApplication.ValueObjects;

namespace PaymentApplication.Controller;

public class HttpRequestController : IHttpRequestController
{
    private readonly HttpClient _client;
    private readonly string _requestBaseUrl;

    public HttpRequestController()
    {
        _client = new HttpClient();
        _requestBaseUrl = ApiStrings.BaseUrl;
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await _client.GetAsync($"{_requestBaseUrl}/{url}");
    }
    
    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        content.Headers.ContentType = new("application/json");
        return await _client.PostAsync($"{_requestBaseUrl}/{url}", content);
    }
}