// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQSPublisher;

//sqs credenciels here new AWSCredensions
var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
    CustomerId = Guid.NewGuid(),
    Name = "John",
    Email = "Doe",
    GituhUserName = "JohnDoe",
    DateOfBirth = DateTime.Now
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    },
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine($"Message sent to SQS with id {response.MessageId}");
Console.ReadLine();