namespace FasterMate.ViewModels.Comment
{
    using System.ComponentModel.DataAnnotations;

    public class AddCommentViewModel
    {
        public string PostId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Comment must be at least {1} characters long and no more than {0} characters.")]
        public string Text { get; set; }
    }
}
