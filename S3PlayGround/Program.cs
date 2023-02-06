// See https://aka.ms/new-console-template for more information

using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

var s3Client = new AmazonS3Client();

await using var inputStream = new FileStream("./watermark.png", FileMode.Open, FileAccess.Read);

var putObjectRequest = new PutObjectRequest()
{
    BucketName = "atakanawscourse",
    Key = "image/face.jpg",
    ContentType = "image/jpeg",
    InputStream = inputStream
};

await s3Client.PutObjectAsync(putObjectRequest);


var getObjectRequest = new GetObjectRequest()
{
    BucketName = "atakanawscourse",
    Key = "image/watermark.jpg"
};

var response = await s3Client.GetObjectAsync(getObjectRequest);

using var memoryStream = new MemoryStream();
response.ResponseStream.CopyTo(memoryStream);

var text = Encoding.Default.GetString(memoryStream.ToArray());

Console.WriteLine(text);