// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppWithDatabaseAndEFCore;

public class ProcessEvent
{
    private readonly ILogger<ProcessEvent> _logger;

    public ProcessEvent(ILogger<ProcessEvent> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ProcessEvent))]
    public void Run([EventGridTrigger] EventGridEvent cloudEvent)
    {
        _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.EventType, cloudEvent.Subject);
    }
}