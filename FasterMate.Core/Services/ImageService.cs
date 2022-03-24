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

        public async Task<string> CreateAsync(IFormFile image, string path)
        {
            Directory.CreateDirectory($"{path}\\");
            var extension = Path.GetExtension(image.FileName).TrimStart('.');

            var imageObj = new Image
            {
                Extension = extension,
            };

            await this.imgRepo.AddAsync(imageObj);
            await this.imgRepo.SaveChangesAsync();

            var physicalPath = $"{path}\\{imageObj.Id}.{extension}";
            using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return imageObj.Id;
        }
    }
}
