namespace FasterMate.Core.Services
{
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;

    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly IRepository<Image> imgRepo;

        public ImageService(IRepository<Image> _imgRepo)
        {
            imgRepo = _imgRepo;
        }

        public async Task<string> CreateAsync(IFormFile img, string path)
        {
            Directory.CreateDirectory($"{path}\\");
            var extension = Path.GetExtension(img.FileName).TrimStart('.');

            var imgObj = new Image
            {
                Extension = extension,
            };

            await imgRepo.AddAsync(imgObj);
            await imgRepo.SaveChangesAsync();

            var physicalPath = $"{path}\\{imgObj.Id}.{extension}";
            using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }

            return imgObj.Id;
        }
    }
}
