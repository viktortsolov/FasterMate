namespace FasterMate.ViewModels.Group
{
    using System.ComponentModel.DataAnnotations;

    using FasterMate.ViewModels.ValidationAttributes;

    using Microsoft.AspNetCore.Http;

    public class CreateGroupViewModel
    {
        [Required]
        [StringLength(64, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Name { get; set; }

        [ImageValidation]
        [Display(Name = "Group Picture")]
        public IFormFile Image { get; set; }
    }
}
