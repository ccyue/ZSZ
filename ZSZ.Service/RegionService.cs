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
    public class RegionService : IRegionService
    {
        public RegionDTO[] GetAll(long cityId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RegionEntity> cs = new CommonService<RegionEntity>(dbc);
                return cs.GetAll().Include(p => p.City).AsNoTracking().Where(p => p.CityId == cityId)
                    .ToList().Select(p => ToDTO(p))
                    .ToArray();
            }
        }

        public RegionDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RegionEntity> cs = new CommonService<RegionEntity>(dbc);
                var region = cs.GetAll().Include(p => p.City).AsNoTracking().SingleOrDefault(p => p.Id == id);
                    return region == null ? null : ToDTO(region);
            }
        }
        private RegionDTO ToDTO(RegionEntity entity)
        {
            return new RegionDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                CreateDateTime = entity.CreateDateTime,
                CityId = entity.CityId,
                CityName = entity.City.Name
            };
        }
    }
}
