using System.ComponentModel.DataAnnotations;

namespace XsollaTestTask1.Models
{
    public class UserInfo
    {
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
