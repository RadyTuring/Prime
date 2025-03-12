using System.ComponentModel.DataAnnotations;

namespace  ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Please Enter the User Name") ,Display(Name ="User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter the Password"),DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
