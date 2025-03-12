using System.ComponentModel.DataAnnotations;

namespace Dto;
public class UserUpdateRoleDto
{
    [Required(ErrorMessage = "enter User Name"), StringLength(60), Display(Name = "User Name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Choose the Role"), Display(Name = "Role")]
    public int RoleId { get; set; }

}