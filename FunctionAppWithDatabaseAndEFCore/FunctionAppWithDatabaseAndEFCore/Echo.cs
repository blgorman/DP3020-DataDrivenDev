using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunctionAppWithDatabaseAndEFCore;

public class Echo
{
    private readonly ILogger<Echo> _logger;

    public Echo(ILogger<Echo> logger)
    {
        _logger = logger;
    }

    [Function("Echo")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        StreamReader reader = new StreamReader(req.Body);
        string requestBody = reader.ReadToEnd();
        response.WriteString(requestBody);

        return response;
    }
}