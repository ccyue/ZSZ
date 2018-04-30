using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IIdNameService:IServiceSupport
    {
        long Add(string typeName, string name);
        IdNameDTO GetById(long Id);
        IdNameDTO[] GetAll(string typeName);
    }
}
