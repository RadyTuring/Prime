using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Dto
{
    public class BlockUserDto
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "enter Your Full Name"), StringLength(60), Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Role")]
        public string UserType { get; set; }
        public string Status { get; set; }
        [Display(Name = "Block type")]
        public string? Blocktype { get; set; }
       
        public int? BlockLevel { get; set; }
    }
     
}
