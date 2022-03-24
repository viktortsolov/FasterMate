namespace FasterMate.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class ImageValidationAttribute : ValidationAttribute
    {
        public ImageValidationAttribute()
        {
            ErrorMessage = "Invalid image type. Please use only .png/.jpg/.jpeg";
        }

        public string[] Extensions => new string[] { ".png", ".jpg", ".jpeg" };

        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;
            bool isValid = true;

            if (file != null && file.Length > 0)
            {
                var fileName = file.FileName;

                isValid = this.Extensions.Any(x => fileName.EndsWith(x));
            }

            return isValid;
        }
    }
}
