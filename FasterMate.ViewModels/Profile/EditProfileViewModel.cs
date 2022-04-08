namespace FasterMate.ViewModels.Profile
{
    using System.ComponentModel.DataAnnotations;

    using FasterMate.ViewModels.ValidationAttributes;

    using Microsoft.AspNetCore.Http;

    public class EditProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(64, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(64, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string LastName { get; set; }

        [StringLength(256, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Bio { get; set; }

        [Display(Name = "Profile Picture")]
        [ImageValidation]
        public IFormFile Image { get; set; }
    }
}
