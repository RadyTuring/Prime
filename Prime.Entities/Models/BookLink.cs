using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class BookLink
{
    public int Id { get; set; }

    public int PlatformOsId { get; set; }
    public int BookServiceId { get; set; }
    public int BookId { get; set; }

    [StringLength(1000), DataType(DataType.Url, ErrorMessage = "Invalid Url"), Column(TypeName = "varchar(1000)")]
    public string? Links { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TranDate { get; set; }=DateTime.Now;
 
}
