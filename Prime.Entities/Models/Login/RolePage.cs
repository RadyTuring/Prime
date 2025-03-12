using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RolePage
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
    }
}
