using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PracticeQuestionsStudent
{
    public long Id { get; set; }

    public long? PracticesAssignId { get; set; }
    public int StudentId { get; set; }

    public int TeacherId { get; set; }
    public int PracticeCode { get; set; }
    public int? BookId { get; set; }

    public long QuestionId { get; set; }

    [StringLength(300)]
    public string? ModelAnswer { get; set; }
   
    [StringLength(300)]
    public string? AnswerMedia { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public double Score { get; set; }
    public string? StudentAnswer { get; set; }
    [StringLength(300)]
    public string? StudentAnswerMedia { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public double? StudentScore { get; set; }
    public int? AcutalStudentDurationByMin { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDateTime { get; set; }
    [StringLength(1)]
    public string? Status { get; set; }
    public bool? IsFinished { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? TranDt { get; set; } = DateTime.Now;

}
