using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Entities;

public class Practice
{
    [Key]
    public int Id { get; set; }
    
    public int  PracticeCode { get; set; }
    public int PracticeTypeId { get; set; }
   
    [StringLength(20)]
    public int? TeacherId { get; set; }
    public int? BookId { get; set; }
    public bool? IsShared { get; set; }
    public bool IsGlobal { get; set; }=false;

    [StringLength(100)]
    public string? PracticeTitle { get; set; }

    [StringLength(100)]
    public int? DurationByMin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TranDt { get; set; } = DateTime.Now;
    public bool? IsSuspend { get; set; } = false;
    public bool IsAssigned { get; set; } = false;
}
