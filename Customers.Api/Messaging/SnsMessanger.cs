using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SnsMessanger : ISnsMessanger
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private string _topicArn;
    private IOptions<TopicSettings> _topicSettings;

    public SnsMessanger(IAmazonSimpleNotificationService snsClient, string topicArn,
        IOptions<TopicSettings> topicSettings)
    {
        _snsClient = snsClient;
        _topicArn = topicArn;
        _topicSettings = topicSettings;
    }

    public async Task<PublishResponse> PublishMessageAsync<T>(T message)
    {
        var topicArn = await GetTopicArn();
        var messageJson = JsonSerializer.Serialize(message);
        var request = new PublishRequest
        {
            TopicArn = topicArn,
            Message = messageJson,
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };

        return await _snsClient.PublishAsync(request);
    }

    private async ValueTask<string> GetTopicArn()
    {
        if (_topicArn is not null)
        {
            return _topicArn;
        }

        var topicName = await _snsClient.FindTopicAsync(_topicSettings.Value.Name);
        _topicArn = topicName.TopicArn;
        return _topicArn;
    }
}