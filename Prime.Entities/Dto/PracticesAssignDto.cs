using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Dto;

public class PracticesAssignStudentsDto
{
    public int PracticeCode { get; set; }
    public List<int> StudentsId  { get; set; }
    public bool IsAutoCheckAnswer { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int DurationByMin { get; set; }
    public bool IsEditable { get; set; } = false;
}
public class PracticesAssignClassDto
{
    public int PracticeCode { get; set; }
    public string ClassCode  { get; set; }
    public bool IsAutoCheckAnswer { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int DurationByMin { get; set; }
    public bool IsEditable { get; set; } = false;
}

public class PracticesUnAssignStudentsDto
{
    public int PracticeCode { get; set; }
    public List<int> StudentsId { get; set; }
}
public class PracticesUnAssignClassDto
{
    public int PracticeCode { get; set; }
    public string ClassCode { get; set; }
}