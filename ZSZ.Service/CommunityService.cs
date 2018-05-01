using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service
{
    public class CommunityService : ICommunityService
    {
        public CommunityDTO[] GetByRegionId(long regionId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<CommunityEntity> community = new CommonService<CommunityEntity>(dbc);
                return community.GetAll().AsNoTracking()
                    .Where(p => p.RegionId == regionId).Select(p => ToDTO(p)).ToArray();
            }
        }
        private CommunityDTO ToDTO(CommunityEntity community)
        {
            return new CommunityDTO()
            {
                Id = community.Id,
                Name = community.Name,
                Traffic = community.Traffic,
                Location = community.Location,
                BuiltYear = community.BuiltYear,
                CreateDateTime = community.CreateDateTime,
                RegionId = community.RegionId
            };
        }
    }
}
