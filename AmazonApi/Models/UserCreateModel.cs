using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ITI.ElectroDev.Presentation
{
    public class UserCreateModel
    {
       
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required, MinLength(6), MaxLength(16)]
        [Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        [Display(Name = "Confirm Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
