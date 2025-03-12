using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class PracticesAssignStudent
{

    public int Id { get; set; }
    public long? PracticesAssignId { get; set; }
    public int? TeacherId { get; set; }
    public int StudentId { get; set; }
    public int PracticeCode { get; set; }
    public int PracticeTypeId { get; set; }
   
    public bool IsStart { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? AnswerStartDate { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? AnswerEndDate { get; set; }
    public int? QuestionsCount { get; set; }

    [Column(TypeName = "decimal(8,2)")]
    public double? TotalScore { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public double? Score { get; set; }
    public bool? IsSubmited { get; set; } = false;
    public bool IsEditable { get; set; } = false;
    public int? BookId { get; set; }
    public bool IsAutoCheckAnswer { get; set; } = false;
    [Column(TypeName = "datetime")]
    public DateTime? ValidDateFrom { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? ValidDateTo { get; set; }
    public int DurationByMin { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? TranDate { get; set; }
}
