using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ITI.ElectroDev.Presentation
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(16)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
