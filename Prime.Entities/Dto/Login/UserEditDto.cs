using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace  Dto 
{
    public class UserEditDto
    {
       
        [StringLength(60)]

        public string? UserName { get; set; }

        [StringLength(60)]

        public string? SchoolUserCode { get; set; }

        [StringLength(60)]

        public string? FirstName { get; set; }

        [StringLength(60)]

        public string? LastName { get; set; }

        [StringLength(60)]

        public string? MidleName { get; set; }
        [StringLength(60)]
        public string? FullName { get; set; }

        public IFormFile DefaultImage { get; set; }
        [StringLength(60)]
        [Column(TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        public string? Email { get; set; }
        public int? GenderId { get; set; }
       
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
