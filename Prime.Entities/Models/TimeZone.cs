using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities; 

public class TbTimeZone
{
    public int Id { get; set; }
    public string Utcoffset { get; set; }
    public string Location { get; set; }
    public double TimeDiff  { get; set; }
}
