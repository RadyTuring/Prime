

using System.ComponentModel.DataAnnotations;

namespace Entities;
//TeacherId	TeacherUserName	TeacherFullName		AssignDate	ValidUpToDate	BookId	BookName	PatchNumber	TeacherCountry	AdminCountry
public class AdminTeachersBooksV
{
    public string? TeacherCode { get; set; }  
    public int? AdminId { get; set; }
    public string? AdminFullName { get; set; }
    public string? AdminCode { get; set; }
    public string? AdminUserName { get; set; }
    public int? TeacherId { get; set; }
    public string? TeacherUserName { get; set; }
    public string? TeacherFullName { get; set; }
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? AssignDate { get; set; }
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? ValidUpToDate { get; set; }
   
    public int? BookId { get; set; }
    public string? BookName { get; set; }
    public int? PatchNumber { get; set; }
    public int? TeacherCountry { get; set; }
    public int? AdminCountry { get; set; }
}


