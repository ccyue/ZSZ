using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class IdNameService : IIdNameService
    {
        public long Add(string typeName, string name)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                bool exist = cs.GetAll().Any(p => p.TypeName == typeName && p.Name == name);
                if(exist)
                {
                    throw new ArgumentException("The Name has been already exist.");
                }
                var entity = new IdNameEntity()
                {
                    TypeName = typeName,
                    Name = name,
                    CreateDateTime = DateTime.Now
                };
                dbc.IdNames.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public IdNameDTO[] GetAll(string typeName)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                return cs.GetAll().Where(p => p.TypeName == typeName)
                    .Select(p => new IdNameDTO()
                    {
                        Id = p.Id,
                        TypeName = p.TypeName,
                        Name = p.Name,
                        CreateDateTime = p.CreateDateTime
                    }).ToArray();
            }
        }

        public IdNameDTO GetById(long Id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                var entity =  cs.GetById(Id);
                if(entity==null)
                {
                    return null;
                }
                return new IdNameDTO()
                {
                    Id = entity.Id,
                    TypeName = entity.TypeName,
                    Name = entity.Name,
                    CreateDateTime = entity.CreateDateTime
                };
            }
        }
    }
}
