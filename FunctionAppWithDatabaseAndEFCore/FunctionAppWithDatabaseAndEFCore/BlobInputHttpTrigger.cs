using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppWithDatabaseAndEFCore;

public class BlobInputHttpTrigger
{
    private readonly ILogger<BlobInputHttpTrigger> _logger;

    public BlobInputHttpTrigger(ILogger<BlobInputHttpTrigger> logger)
    {
        _logger = logger;
    }

    [Function("BlobInputHttpTrigger")]
    public async Task<IActionResult> Binding(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "blobs/{container}")] HttpRequest req,
        [BlobInput("{container}", Connection = "MyImportantStorage")] BlobContainerClient containerClient)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var blobList = await GetBlobDetailsAsync(containerClient);

        return new OkObjectResult(blobList);
    }

    [Function("BlobListHttpTrigger")]
    public async Task<IActionResult> NoBinding(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request (manual client).");

        var containerName = req.Query["container"].ToString();
        if (string.IsNullOrEmpty(containerName))
        {
            return new BadRequestObjectResult("Please provide a 'container' query parameter.");
        }

        var blobServiceUri = Environment.GetEnvironmentVariable("MyImportantStorage__blobServiceUri");
        if (string.IsNullOrEmpty(blobServiceUri))
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var blobServiceClient = new BlobServiceClient(new Uri(blobServiceUri), new DefaultAzureCredential());
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var blobList = await GetBlobDetailsAsync(containerClient);

        return new OkObjectResult(blobList);
    }

    private async Task<List<object>> GetBlobDetailsAsync(BlobContainerClient containerClient)
    {
        List<object> blobList = [];
        await foreach (BlobItem blob in containerClient.GetBlobsAsync())
        {
            blobList.Add(new
            {
                blob.Name,
                ContentType = blob.Properties.ContentType,
                ContentLength = blob.Properties.ContentLength,
                LastModified = blob.Properties.LastModified,
                CreatedOn = blob.Properties.CreatedOn,
                BlobType = blob.Properties.BlobType?.ToString(),
                AccessTier = blob.Properties.AccessTier?.ToString()
            });
        }

        _logger.LogInformation("Found {BlobCount} blobs in container.", blobList.Count);

        return blobList;
    }
}