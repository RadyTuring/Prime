using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class StudentsResultV
{//Id	TeacherId	StudentId	PracticeCode	PracticeTypeId	PracticesAssignId	IsStart	ValidDateFrom	ValidDateTo	Score	TotalScore	QuestionsCount	IsSubmited	UserName	FullName
    public int Id { get; set; }
    public int? TeacherId { get; set; }
    public int StudentId { get; set; }
    public int PracticeCode { get; set; }
    public int PracticeTypeId { get; set; }
    public long PracticesAssignId { get; set; }
    public bool IsStart { get; set; }
    public DateTime? ValidDateFrom { get; set; }
    public DateTime? ValidDateTo { get; set; }
    public int? QuestionsCount { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public double? TotalScore { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public double? Score { get; set; }
    public bool? IsSubmited { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
        
}

