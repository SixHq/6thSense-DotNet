using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class Encryption_Middleware
{
    private readonly RequestDelegate _next;


    public Encryption_Middleware(RequestDelegate next)
    {
        _next = next;

    }

    public async Task Invoke(HttpContext httpContext)
    {
        //get the api key
        var apiKey = httpContext.Request.Headers["apikey"];

        //clone the request and body 
        var requestBody = await CloneBodyAsync(httpContext.Request.Body);
        var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;

        // Get the private and public keys from Firebase Storage
        var privateKey = await GetTextFromFirebaseStorageAsync("private key url", apiKey);
        var publicKey = await GetTextFromFirebaseStorageAsync("public key url", apiKey);

        var decryptedRequestBody = await PostOrderDecryptAsync(privateKey, requestBody);

        // Replace the request body with the decrypted one
        httpContext.Request.Body = decryptedRequestBody;


        await _next(httpContext);

        responseBody.Seek(0, SeekOrigin.Begin);

        

        // Encrypt the response body using the public key
        var encryptedResponseBody = await PostOrderEncryptAsync(publicKey, responseBody);

        // Replace the response body with the encrypted one
        httpContext.Response.Body = encryptedResponseBody;



    }

    private async Task<Stream> PostOrderEncryptAsync(string publicKey, Stream input)
    {
        // Create a RSACryptoServiceProvider object from the public key
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);

        // Read all bytes from the input stream
        var inputBytes = await ReadAllBytesAsync(input);

        // Encrypt the input bytes using RSA with OAEP padding
        var outputBytes = rsa.Encrypt(inputBytes, true);

        // Create and return a memory stream from the output bytes
        return new MemoryStream(outputBytes);
    }

    private async Task<Stream> PostOrderDecryptAsync(string privateKey, Stream input)
    {
        // Create a RSACryptoServiceProvider object from the private key
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKey);

        // Read all bytes from the input stream
        var inputBytes = await ReadAllBytesAsync(input);

        // Decrypt the input bytes using RSA with OAEP padding
        var outputBytes = rsa.Decrypt(inputBytes, true);

        // Create and return a memory stream from the output bytes
        return new MemoryStream(outputBytes);
    }


    private async Task<byte[]> ReadAllBytesAsync(Stream input)
    {
        // Create a memory stream to copy the input stream
        using var output = new MemoryStream();

        // Copy the input stream to the output stream
        await input.CopyToAsync(output);

        // Return the output stream as an array of bytes
        return output.ToArray();
    }

    private async Task<string> GetTextFromFirebaseStorageAsync(string fileUrl, string apiKey)
    {
        using var client = new HttpClient();
        var response = await client.PostAsync($"{fileUrl}?apikey={apiKey}", null);

        if (response.IsSuccessStatusCode)
        {
            // Read and return the response content as a string
            return await response.Content.ReadAsStringAsync();
        }
        throw new Exception($"Error retrieving text file: {response.ReasonPhrase}");
    }



    private async Task<Stream> CloneBodyAsync(Stream input)
    {
        var output = new MemoryStream();

        await input.CopyToAsync(output);

        input.Seek(0, SeekOrigin.Begin);
        output.Seek(0, SeekOrigin.Begin);


        return output;

    }
}


public static class Encryption_MiddlewareExtensions
{
    public static IApplicationBuilder UseEncryption_Middleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Encryption_Middleware>();

    }

}