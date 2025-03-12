using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dto;

public  class QuestionTypeDto
{
    public string TypeName { get; set; }
    public string QuestHeader { get; set; }
    public string? QuestionDesc { get; set; }


}
