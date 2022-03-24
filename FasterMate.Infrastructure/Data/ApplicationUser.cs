namespace FasterMate.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
        }

        [Required]
        public DateTime CreatedOn { get; set; }


        [Required]
        [ForeignKey(nameof(Profile))]
        public string? ProfileId { get; set; }
        [Required]
        public virtual Profile? Profile { get; set; }
    }
}
