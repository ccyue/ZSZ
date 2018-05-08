using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class CityService : ICityService
    {
        public long Add(string cityName,string initials,bool isHot)
        {
            using(ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<CityEntity> cs = new CommonService<CityEntity>(dbc);
                // Any > where+count
                bool exists = cs.GetAll().Any(p => p.Name == cityName);
                if(exists)
                {
                    throw new ArgumentException(string.Format("{0} has already exist.", cityName));
                }
                CityEntity city = new CityEntity(){
                    Name = cityName,
                    Initials = initials,
                    IsHot = isHot,
                    CreateDateTime =DateTime.Now};
                dbc.Cities.Add(city);
                dbc.SaveChanges();
                return city.Id;
            }
        }

        public CityDTO[] GetAll()
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<CityEntity> cs = new CommonService<CityEntity>(dbc);
                return cs.GetAll().AsNoTracking().ToList().Select(p => ToDTO(p)).ToArray();
            }
        }

        public CityDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<CityEntity> cs = new CommonService<CityEntity>(dbc);
                var city = cs.GetById(id);
                return city == null ? null : ToDTO(city);
            }
        }
        private CityDTO ToDTO(CityEntity city)
        {
            return new CityDTO()
            {
                Id = city.Id,
                Name = city.Name,
                Initials = city.Initials,
                IsHot = city.IsHot,
                CreateDateTime = city.CreateDateTime
            };
        }
    }
}
