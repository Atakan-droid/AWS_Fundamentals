using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;
using Customers.Api.Database;
using Dapper;

namespace Customers.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly string _tableName = "customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerJson).ToAttributeMap();
        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttributes,
            ConditionExpression = "attribute_not_exists(pk) AND attribute_not_exists(sk)",
        };

        var response = await _dynamoDbClient.PutItemAsync(createItemRequest);

        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetWithTransac()
    {
        var customer = new CustomerDto()
        {
            Email = "Atakan", UpdatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            FullName = "Atakan",
            DateOfBirth = DateTime.MinValue,
            GitHubUsername = "Github atakan"
        };

        var asJson = JsonSerializer.Serialize(customer);
        var attribute = Document.FromJson(asJson).ToAttributeMap();


        var transactionRequest = new TransactWriteItemsRequest
        {
            TransactItems = new List<TransactWriteItem>
            {
                new()
                {
                    Put = new()
                    {
                        TableName = _tableName,
                        Item = attribute,
                    }
                },
                new()
                {
                    Put = new Put()
                    {
                        TableName = _tableName,
                        Item = attribute,
                    }
                }
            }
        };

        var response = await _dynamoDbClient.TransactWriteItemsAsync(transactionRequest);
        return customer;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        var response = await _dynamoDbClient.GetItemAsync(getItemRequest);
        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<CustomerDto>(itemAsDocument);
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        var queryRequest = new QueryRequest
        {
            TableName = _tableName,
            IndexName = "email-id-index",
            KeyConditionExpression = "Email= :v_email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {
                    ":v_Email", new AttributeValue { S = email }
                }
            }
        };
        var response = await _dynamoDbClient.QueryAsync(queryRequest);
        if (response.Items.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Items[0]);
        return JsonSerializer.Deserialize<CustomerDto>(itemAsDocument);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var scanRequest = new ScanRequest
        {
            TableName = _tableName,
        };

        var response = await _dynamoDbClient.ScanAsync(scanRequest);
        return response.Items.Select(x =>
        {
            var json = Document.FromAttributeMap(x).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(json);
        })!;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerJson).ToAttributeMap();
        var updateItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttributes
        };

        var response = await _dynamoDbClient.PutItemAsync(updateItemRequest);

        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        var response = await _dynamoDbClient.DeleteItemAsync(deleteItemRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}