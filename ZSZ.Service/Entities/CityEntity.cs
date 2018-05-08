using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
    public class CityEntity :BaseEntity
    {
        public string Name { get; set; }
        public string Initials { get; set; }
        public bool IsHot { get; set; }
    }
}
