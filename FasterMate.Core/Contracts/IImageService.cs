namespace FasterMate.Core.Contracts
{
    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        Task<string> CreateAsync(IFormFile image, string path);
    }
}
