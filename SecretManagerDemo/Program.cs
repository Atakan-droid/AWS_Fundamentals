// See https://aka.ms/new-console-template for more information

using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var secretsmanagerClient = new AmazonSecretsManagerClient();

var listSecretsVersionRequests = new ListSecretVersionIdsRequest()
{
    SecretId = "ApiKey",
    IncludeDeprecated = true
};

var versionResponse = await secretsmanagerClient.ListSecretVersionIdsAsync(listSecretsVersionRequests);

var getSecretRequest = new GetSecretValueRequest()
{
    SecretId = "ApiKey",
    VersionStage = "AWSCURRENT"
};

var response = await secretsmanagerClient.GetSecretValueAsync(getSecretRequest);

Console.WriteLine(response.SecretString);

var describeSecretRequest = new DescribeSecretRequest()
{
    SecretId = "Apikey"
};

var describeResponse = await secretsmanagerClient.DescribeSecretAsync(describeSecretRequest);

Console.WriteLine(describeResponse.Description);