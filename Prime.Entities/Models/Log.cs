using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class Log
{
    [Key]
    public int RecId { get; set; }

    public int UserId { get; set; }
    [Column(TypeName = "nvarchar(60)")]
    public string? Action { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TranDate { get; set; }=DateTime.Now;
 
}
