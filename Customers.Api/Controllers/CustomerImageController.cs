using System.Net;
using Amazon.S3;
using Customers.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

public class CustomerImageController
{
    private readonly ICustomerImageService _customerImageService;

    public CustomerImageController(ICustomerImageService customerImageService)
    {
        _customerImageService = customerImageService;
    }

    // [HttpPost("customers/{id:guid}/image")]
    // public async Task<IActionResult> Upload([FromRoute] Guid id, [FromForm(Name = "Data")] IFormFile file)
    // {
    //     var response = await _customerImageService.UploadImage(id, file);
    //
    //     if (response.HttpStatusCode == HttpStatusCode.OK)
    //     {
    //         return Ok(response);
    //     }
    //
    //     return BadRequest(response);
    // }
    //
    // [HttpGet("customers/{id:guid}/image")]
    // public async Task<IActionResult> Get([FromRoute] Guid id)
    // {
    //     try
    //     {
    //         var response = await _customerImageService.GetImage(id);
    //         return File(response.ResponseStream, response.Headers.ContentType);
    //     }
    //     catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.")
    //     {
    //         return NotFound();
    //     }
    // }
    //
    // [HttpDelete("customers/{id:guid}/image")]
    // public async Task<IActionResult> Delete([FromRoute] Guid id)
    // {
    //     var response = await _customerImageService.DeleteImage(id);
    //     return response.HttpStatusCode switch
    //     {
    //         HttpStatusCode.NoContent => Ok(),
    //         HttpStatusCode.NotFound => NotFound(),
    //         _ => BadRequest(response)
    //     };
    // }
}