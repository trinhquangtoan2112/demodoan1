using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demodoan1.Models.UserDto
{
    public class UserDto
    {
   
        [Required]
        [MaxLength(30)]
        public string MatKhau { get; set; }
        [Required]
        [MaxLength(40)]
        public string Email { get; set; }

       
    }
}
