using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity.Infrastructure;

namespace ZSZ.Service
{
    public class HouseAppointmentService : IHouseAppointmentService
    {
        public long Add(long? userId, string name, string phoneNum, long houseId, DateTime visitDate)
        {

            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                //check userId
                if (userId != null)
                {
                    CommonService<UserEntity> user = new CommonService<UserEntity>(dbc);
                    bool existUser = user.GetAll().Any(p => p.Id == userId);
                    if(!existUser)
                    {
                        throw new ArgumentException("user is not exist.");
                    }
                }
                //check house TODO:状态
                CommonService<HouseEntity> house = new CommonService<HouseEntity>(dbc);
                bool existHouse = house.GetAll().Any(p => p.Id == houseId);
                if (!existHouse)
                {
                    throw new ArgumentException("House is not exist.");
                }
                var appointment = new HouseAppointmentEntity()
                {
                    HouseId = houseId,
                    UserId = userId,
                    Name = name,
                    PhoneNum = phoneNum,
                    Status = "未处理",
                    VisitDate = visitDate,
                    CreateDateTime = DateTime.Now
                };

                dbc.HouseAppointments.Add(appointment);
                dbc.SaveChanges();
                return appointment.Id;
            }
        }

        public bool Follow(long adminUserId, long houseAppointmentId)
        {
            using (ZSZDbContext dbcontext = new ZSZDbContext())
            {
                CommonService<HouseAppointmentEntity> bs = new CommonService<HouseAppointmentEntity>(dbcontext);
                var app = bs.GetById(houseAppointmentId);
                if(app==null)
                {
                    throw new ArgumentException("订单不存在");
                }
                else if (app.FollowAdminUserId != null)
                {
                    return app.FollowAdminUserId == adminUserId;
                }                   
                else
                {
                    app.FollowAdminUserId = adminUserId;
                    app.Status = "已接单";
                    try
                    {
                        dbcontext.SaveChanges();
                        return true;
                    }
                    catch(DbUpdateConcurrencyException ex)
                    {
                        return false;
                    }
                }
            }
        }

        public HouseAppointmentDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                var houseApp = cs.GetAll().Include(p => p.House)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(p => p.FollowAdminUser)
                    .AsNoTracking().SingleOrDefault(p => p.Id == id);
                return houseApp == null ? null : ToDTO(houseApp);
            }
        }

        public HouseAppointmentDTO[] GetPageData(long cityId, string status, int pageSize, int currentIndex,long? userId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                var houseApps = cs.GetAll().Include(p => p.House)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(p => p.FollowAdminUser).AsNoTracking()
                    .Where(p => p.House.Community.Region.CityId == cityId);
                if (!string.IsNullOrEmpty(status))
                {
                    houseApps = houseApps.Where(p => p.Status == status);
                }
                if(userId!=null)
                {
                    houseApps = houseApps.Where(p => p.FollowAdminUserId == userId);
                }
                houseApps = houseApps.OrderByDescending(p => p.CreateDateTime)
                .Skip(currentIndex).Take(pageSize);
                var result = houseApps.ToList().Select(p => ToDTO(p)).ToArray();
                return result;
            }
        }

        public long GetTotalCount(long cityId, string status, long? userId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                var houseApps = cs.GetAll().Where(p => p.House.Community.Region.CityId == cityId);
                if (!string.IsNullOrEmpty(status))
                {
                    houseApps = houseApps.Where(p => p.Status == status);
                }
                if (userId != null)
                {
                    houseApps = houseApps.Where(p => p.FollowAdminUserId == userId);
                }
                return houseApps.LongCount();
            }
        }

        private HouseAppointmentDTO ToDTO(HouseAppointmentEntity houseApp)
        {
            HouseAppointmentDTO dto = new HouseAppointmentDTO();
            dto.Id = houseApp.Id;
            dto.UserId = houseApp.UserId;
            dto.Name = houseApp.Name;
            dto.PhoneNum = houseApp.PhoneNum;
            dto.VisitDate = houseApp.VisitDate;
            dto.HouseId = houseApp.HouseId;
            dto.Status = houseApp.Status;
            dto.HouseAddress = houseApp.House.Address;
            dto.FollowAdminUserId = houseApp.FollowAdminUserId;
            if (dto.FollowAdminUserId != null && houseApp.FollowAdminUser != null)
            {
                dto.FollowAdminUserName = houseApp.FollowAdminUser.Name;
            }
            dto.RegionName = houseApp.House.Community.Region.Name;
            dto.CommunityName = houseApp.House.Community.Name;
            return dto;
        }
    }
}
