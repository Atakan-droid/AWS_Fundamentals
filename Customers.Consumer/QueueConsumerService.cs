using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using MediatR;
using Microsoft.Extensions.Options;
using SQSPublisher;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private IAmazonSQS _sqs;
    private IOptions<QueueSettings> _queueSettings;
    private IMediator _mediator;
    private ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings, IMediator mediator,
        ILogger<QueueConsumerService> logger)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);

        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            AttributeNames = new List<string>() { "All" },
            MessageAttributeNames = new List<string>() { "All" },
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

            foreach (var message in response.Messages)
            {
                var messagetype = message.MessageAttributes["MessageType"].StringValue;
                var type = Type.GetType($"Customers.Consumer.Messages.{messagetype}");

                if (type is null)
                {
                    _logger.LogWarning("Unkown message type : {MessageType}", messagetype);
                }

                object typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;
                try
                {
                    await _mediator.Send(typedMessage!, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError("Unable to process message {MessageType} : {Message}",
                        messagetype, e.Message);
                    continue;
                }

                await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }


            await Task.Delay(1000, stoppingToken);
        }
    }
}