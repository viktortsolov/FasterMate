namespace FasterMate.ViewModels.Post
{
    using System.ComponentModel.DataAnnotations;

    using FasterMate.ViewModels.ValidationAttributes;

    using Microsoft.AspNetCore.Http;

    public class CreatePostViewModel
    {
        public string ReturnId { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Picture")]
        [ImageValidation]
        public IFormFile Image { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Location { get; set; }
    }
}
