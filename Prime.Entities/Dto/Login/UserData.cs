using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class UserData
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }
    public string? Country { get; set; }
    public string? CountryUtc { get; set; }
    public Double TimeDif { get; set; }
    public int? RoleId { get; set; }

    public string? ImageFile { get; set; }
}


