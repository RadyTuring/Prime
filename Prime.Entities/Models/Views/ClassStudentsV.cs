using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
public class ClassStudentsV
{
    [Key, Column(Order = 0)]
    public int UserId { get; set; }
    public string? ClassCode { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? ImageFile { get; set; }
    public int? CountryId { get; set; }
    public string? Notes { get; set; }
   
    public int? BookId { get; set; }
    public DateTime? AssignDate { get; set; }
    public bool IsOnline { get; set; }
    public int? RoleId { get; set; }
    public int TeacherId { get; set; }
    public bool IsActiveUser { get; set; }
    public int Id { get; set; } //class Id
}

