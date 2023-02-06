using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleDynamoDbFunction;

public class Function
{
    public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Beginning to process {dynamoEvent.Records.Count} records...");

        foreach (var record in dynamoEvent.Records)
        {
            context.Logger.LogInformation($"Event ID: {record.EventID}");
            context.Logger.LogInformation($"Event Name: {record.EventName}");

            context.Logger.LogInformation($"Event Body: {record.Dynamodb.NewImage}");

            if (record.Dynamodb.OldImage != null)
            {
                var newDocument = Document.FromAttributeMap(record.Dynamodb.NewImage);
                context.Logger.LogInformation($"New Document: {newDocument.ToJson()}");
            }

            // TODO: Add business logic processing the record.Dynamodb object.
        }

        context.Logger.LogInformation("Stream processing complete.");
    }
}