using Amazon.S3.Model;

namespace Customers.Api.Services;

public interface ICustomerImageService
{
    Task<PutObjectResponse> UploadImage(Guid id, IFormFile file);
    Task<DeleteObjectResponse> DeleteImage(Guid id);
    Task<GetObjectResponse> GetImage(Guid id);
}