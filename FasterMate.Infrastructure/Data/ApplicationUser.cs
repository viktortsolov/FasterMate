namespace FasterMate.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApplicationUser : IdentityUser
    {
        //public ApplicationUser()
        //{
        //    this.Roles = new HashSet<IdentityUserRole<string>>();
        //    this.Claims = new HashSet<IdentityUserClaim<string>>();
        //    this.Logins = new HashSet<IdentityUserLogin<string>>();
        //}

        [Required]
        public DateTime CreatedOn { get; set; }

        //public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
        //public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        //public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public Guid ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
    }
}
