using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class QuestionType
{
    [Key]
    public int TypeId { get; set; }
    [StringLength(50)]
    public string TypeName { get; set; }
    [StringLength(50)]
    public string QuestHeader { get; set; }
    [StringLength(100)]
    public string? QuestionDesc { get; set; }

   
}
