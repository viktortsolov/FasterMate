namespace FasterMate.ViewModels.Profile
{
    using System.ComponentModel.DataAnnotations;

    using FasterMate.ViewModels.ValidationAttributes;

    using Microsoft.AspNetCore.Http;

    public class EditProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(256, ErrorMessage = "{0} must be at most {1} characters long.")]
        public string Bio { get; set; }

        [Display(Name = "Profile Picture")]
        [ImageValidation]
        public IFormFile Image { get; set; }
    }
}
