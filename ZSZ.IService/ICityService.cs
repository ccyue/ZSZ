using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface ICityService:IServiceSupport
    {
        CityDTO[] GetAll();
        CityDTO GetById(long id);
        /// <summary>
        /// Add new city
        /// </summary>
        long Add(string cityName, string initials, bool isHot);
    }
}
