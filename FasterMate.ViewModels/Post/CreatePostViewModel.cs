namespace FasterMate.ViewModels.Post
{
    using FasterMate.ViewModels.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class CreatePostViewModel
    {
        public string ReturnId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Picture")]
        [ImageValidation]
        public IFormFile Image { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Location { get; set; }
    }
}
