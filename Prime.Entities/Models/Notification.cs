using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class Notification
{

    public int Id { get; set; }

    [StringLength(50)]

    public string  Category { get; set; }

    public string Message { get; set; }
    [ForeignKey("User")]
    public int FromUserId { get; set; }
    public User? User { get; set; }
    public int  ToUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NoteDate{ get; set; }=DateTime.Now;

    public bool IsRead { get; set; } = false;

    public bool? IsDone { get; set; }
    public long? DocNo { get; set; }
}