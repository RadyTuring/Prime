using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;
public class StringUpdateDto
{
    public string StringValue { get; set; }
}
public class StringIdUpdateDto
{
    public int Id { get; set; }
    public string StringValue { get; set; }
}
public class IntIdUpdateDto
{
    public int Id { get; set; }
    public int? IntValue { get; set; }
}
