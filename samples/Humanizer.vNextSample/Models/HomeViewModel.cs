using System.ComponentModel.DataAnnotations;

namespace Humanizer.vNextSample.Models
{
    public class HomeViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Phone]        
        public string PhoneNumber { get; set; }

        [Display(Name = "Remember Me?")]
        public bool BrowserRemembered { get; set; }

    }


}
