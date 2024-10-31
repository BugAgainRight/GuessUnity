using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using UnityEngine;

public static class Server
{
    public static readonly RestClient Client = new ("http://localhost:8633/");

    public static async Task<T> Get<T>(string url, params (string name, string value)[] query)
    {
        var request = new RestRequest(url, Method.Get);
        foreach (var q in query)
        {
            request.AddQueryParameter(q.name, q.value);
        }
        var response = await Client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public static async Task<T> Post<T>(string url, object body)
    {
        var request = new RestRequest(url, Method.Post);
        request.AddBody(JsonConvert.SerializeObject(body), ContentType.Json);
        var response = await Client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<T>(response.Content);
    }
    
    public static async Task<T> Post<T>(string url, object body, params (string name, string value)[] query)
    {
        var request = new RestRequest(url, Method.Post);
        foreach (var q in query)
        {
            request.AddQueryParameter(q.name, q.value);
        }
        request.AddBody(JsonConvert.SerializeObject(body), ContentType.Json);
        var response = await Client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<T>(response.Content);
    }
}
