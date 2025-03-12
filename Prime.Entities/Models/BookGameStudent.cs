using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class BookGameStudent
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int BookId { get; set; }
    [Column(TypeName = "decimal(5, 2)")]
    public decimal Score { get; set; }
}

