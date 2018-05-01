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
    public class AttachementService : IAttachementService
    {
        public AttachmentDTO[] GetAll()
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AttachmentEntity> cs = new CommonService<AttachmentEntity>(dbc);
                return cs.GetAll().Select(p => ToDTO(p)).ToArray();
            }
        }

        public AttachmentDTO[] GetByHouseId(long houseId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> houses = new CommonService<HouseEntity>(dbc);
                var house = houses.GetAll().Include(p => p.Attachments).AsNoTracking().SingleOrDefault(p => p.Id == houseId);
                if(house==null)
                {
                    return null;
                }
                return house.Attachments.Select(p => ToDTO(p)).ToArray();
            }
        }
        private AttachmentDTO ToDTO(AttachmentEntity entity)
        {
            return new AttachmentDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                IconName = entity.IconName,
                CreateDateTime =entity.CreateDateTime
            };
        }

    }
}
