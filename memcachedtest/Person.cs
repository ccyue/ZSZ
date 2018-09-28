using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memcachedtest
{
    [Serializable]
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
