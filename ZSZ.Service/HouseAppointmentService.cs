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
            throw new NotImplementedException();
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

        public HouseAppointmentDTO[] GetPageData(long cityId, string status, int pageSize, int currentIndex)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                var houseApps = cs.GetAll().Include(p => p.House)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(p => p.FollowAdminUser).AsNoTracking()
                    .Where(p => p.House.Community.Region.CityId == cityId)
                    .Where(p => p.Status == status)
                    .OrderByDescending(p => p.CreateDateTime)
                    .Skip(currentIndex).Take(pageSize)
                    .Select(p => ToDTO(p)).ToArray();
                return houseApps;
            }
        }

        public long GetTotalCount(long cityId, string status)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                return cs.GetAll().LongCount(p => p.House.Community.Region.CityId == cityId && p.Status == status);
            }
        }
        private HouseAppointmentDTO ToDTO(HouseAppointmentEntity houseApp)
        {
            HouseAppointmentDTO dto = new HouseAppointmentDTO();
            dto.UserId = houseApp.UserId;
            dto.Name = houseApp.Name;
            dto.PhoneNum = houseApp.PhoneNum;
            dto.VisitDate = houseApp.VisitDate;
            dto.HouseId = houseApp.HouseId;
            dto.Status = houseApp.Status;
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
