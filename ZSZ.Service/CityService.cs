using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class CityService : ICityService
    {
        public long Add(string cityName)
        {
            using(ZSZDbContext ctx = new ZSZDbContext())
            {
                CommonService<CityEntity> bs = new CommonService<CityEntity>(ctx);
                // Any > where+count
                bool exists = bs.GetAll().Any(p => p.Name == cityName);
                if(exists)
                {
                    throw new ArgumentException("The city has exists!");
                }
                CityEntity city = new CityEntity(){ Name= cityName, CreateDateTime =DateTime.Now};
                ctx.Cities.Add(city);
                ctx.SaveChanges();
                return city.Id;
            }
        }
    }
}
