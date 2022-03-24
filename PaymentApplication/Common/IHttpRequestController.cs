namespace PaymentApplication.Common;

public interface IHttpRequestController
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    //test
}