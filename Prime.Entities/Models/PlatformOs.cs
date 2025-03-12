using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PlatformOs
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(60)")]
    public string OsName { get; set; }
 
}
