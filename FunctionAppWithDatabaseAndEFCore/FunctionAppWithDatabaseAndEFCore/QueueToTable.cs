using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppWithDatabaseAndEFCore;

public class QueueToTable
{
    private readonly ILogger<QueueToTable> _logger;

    public QueueToTable(ILogger<QueueToTable> logger)
    {
        _logger = logger;
    }

    [Function(nameof(QueueToTable))]
    [TableOutput("ChatLog", Connection = "MyImportantStorage")]
    public TableEntity Run([QueueTrigger("chat-requests", Connection = "MyImportantStorage")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);

        return new TableEntity("ChatLog", message.MessageId)
        {
            { "MessageText", message.MessageText },
            { "InsertedAt", DateTimeOffset.UtcNow }
        };
    }

    [Function("HttpToTable")]
    public async Task<IActionResult> RunHttp(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var requestBody = await req.ReadFromJsonAsync<ChatMessage>();
        if (requestBody is null || string.IsNullOrEmpty(requestBody.MessageText))
        {
            return new BadRequestObjectResult("Please provide a 'messageText' property in the request body.");
        }

        _logger.LogInformation("HTTP trigger function processed: {messageText}", requestBody.MessageText);

        var connectionString = Environment.GetEnvironmentVariable("MyImportantStorage");
        if (string.IsNullOrEmpty(connectionString))
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var tableClient = new TableClient(connectionString, "ChatLog");
        await tableClient.CreateIfNotExistsAsync();

        var entity = new TableEntity("ChatLog", Guid.NewGuid().ToString())
        {
            { "MessageText", requestBody.MessageText },
            { "InsertedAt", DateTimeOffset.UtcNow }
        };

        await tableClient.AddEntityAsync(entity);

        return new OkObjectResult(entity);
    }
}

public record ChatMessage(string MessageText);