using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
public class BookPrint
{
    public int Id { get; set; }
    public string BkCode { get; set; }
    public int? UserId { get; set; }
    public DateTime? AssignDate { get; set;}
    public int BookId { get; set; }
    public string?  BookServices { get; set; }
    public bool IncludeEkit { get; set; }
    public int? PatchNumber { get; set; }
    public DateTime? ValidUpToDate { get; set; }
    public bool IsBlocked { get; set; }=false;
}

