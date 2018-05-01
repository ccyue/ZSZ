using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ZSZ.Service
{
    public class HouseService : IHouseService
    {
        public long Add(HouseDTO house)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AttachmentEntity> csAttachment = new CommonService<AttachmentEntity>(dbc);
                var attachments = csAttachment.GetAll().Where(p => house.AttachmentIds.Contains(p.Id)).ToArray();
                HouseEntity entity = new HouseEntity()
                {
                    CommunityId = house.CommunityId,
                    RoomTypeId = house.RoomTypeId,
                    Address = house.Address,
                    MonthRent = house.MonthRent,
                    StatusId = house.StatusId,
                    Area = house.Area,
                    DecorateStatusId = house.DecorateStatusId,
                    TotalFloorCount = house.TotalFloorCount,
                    FloorIndex = house.FloorIndex,
                    Direction = house.Direction,
                    LookableDateTime = house.LookableDateTime,
                    CheckInDateTime =house.CheckInDateTime,
                    OwnerName = house.OwnerName,
                    OwnerPhoneNum = house.OwnerPhoneNum,
                    Description = house.Description,
                    Attachments = attachments,
                    CreateDateTime = DateTime.Now
                };
                dbc.Houses.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public long AddHousePic(HousePicDTO housePic)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                bool exist = csHouse.GetAll().Any(p => p.Id == housePic.HouseId);
                if(!exist)
                {
                    throw new ArgumentException("The house is not exist.");
                }
                var pic = new HousePicEntity()
                {
                    HouseId = housePic.HouseId,
                    ThumbUrl = housePic.ThumbUrl,
                    Url = housePic.Url
                };
                dbc.HousePics.Add(pic);
                dbc.SaveChanges();
                return pic.Id;
            }
        }
        public void Delete(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                csHouse.MarkDeleted(id);
            }
        }

        public void DeleteHousePic(long housePicId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                var housePic = dbc.HousePics.SingleOrDefault(p => p.IsDeleted == false && p.Id == housePicId);
                if(housePic!=null)
                {
                    dbc.HousePics.Remove(housePic);
                    dbc.SaveChanges();
                }
            }
        }

        public HouseDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                var house = csHouse.GetAll()
                    .Include(p => p.Attachments).Include(p => p.HousePics).Include(p => p.Community)
                    .Include(p=>p.Status).Include(p=>p.DecorateStatus).Include(p=>p.RoomType).Include(p=>p.Type)
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
                    .AsNoTracking().SingleOrDefault(p => p.Id == id);
                if(house==null)
                {
                    return null;
                }
                return ToDTO(house);
            }
        }

        public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                return csHouse.GetAll()
                    .LongCount(p => p.Community.Region.CityId == cityId && p.CreateDateTime >= startDateTime
                    && p.CreateDateTime <= endDateTime);
            }
        }

        public HouseDTO[] GetPageData(long cityId, long typeId, int pageSize, int currentIndex)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                return csHouse.GetAll()
                    .Include(p => p.Attachments).Include(p => p.HousePics).Include(p => p.Community)
                    .Include(p => p.Status).Include(p => p.DecorateStatus).Include(p => p.RoomType).Include(p => p.Type)
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
                    .AsNoTracking()
                    .Where(p => p.Community.Region.CityId == cityId && p.TypeId == typeId)
                    .OrderByDescending(p => p.CreateDateTime).Skip(currentIndex).Take(pageSize)
                    .Select(p => ToDTO(p))
                    .ToArray();
            }
        }

        public long GetTotalCount(long cityId, long typeId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                return csHouse.GetAll()
                    .LongCount(p => p.Community.Region.CityId == cityId && p.TypeId == typeId);
            }
        }
        public HousePicDTO[] GetPics(long houseId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HousePicEntity> csHousePic = new CommonService<HousePicEntity>(dbc);
                return csHousePic.GetAll().Where(p => p.HouseId == houseId)
                    .Select(p => new HousePicDTO()
                    {
                        Id = p.Id,
                        HouseId = p.HouseId,
                        ThumbUrl = p.ThumbUrl,
                        Url = p.Url,
                        CreateDateTime = p.CreateDateTime
                    }).ToArray();
            }
        }
        public HouseSearchResult Search(HouseSearchOptions options)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                var houses = csHouse.GetAll()
                    .Where(p => p.Community.Region.CityId == options.CityId && p.TypeId == options.TypeId);
                if (options.RegionId != null)
                {
                    houses = houses.Where(p => p.Community.RegionId == options.RegionId);
                }
                if(options.StartMonthRent != null)
                {
                    houses = houses.Where(p => p.MonthRent >= options.StartMonthRent);
                }
                if (options.EndMonthRent != null)
                {
                    houses = houses.Where(p => p.MonthRent <= options.EndMonthRent);
                }
                houses = houses.Include(p => p.Attachments).Include(p => p.HousePics).Include(p => p.Community)
                    .Include(p => p.Status).Include(p => p.DecorateStatus).Include(p => p.RoomType).Include(p => p.Type)
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City))
                    .AsNoTracking();
                long totalCount = houses.LongCount();
                switch (options.OrderByType)
                {
                    case HouseSearchOrderByType.AreaAsc: houses = houses.OrderBy(p => p.Area);break;
                    case HouseSearchOrderByType.AreaDesc: houses = houses.OrderByDescending(p => p.Area); break;
                    case HouseSearchOrderByType.CreateDateDesc: houses = houses.OrderByDescending(p => p.CreateDateTime); break;
                    case HouseSearchOrderByType.MonthRentAsc: houses = houses.OrderBy(p => p.MonthRent); break;
                    case HouseSearchOrderByType.MonthRentDesc: houses = houses.OrderByDescending(p => p.MonthRent); break;
                };
                HouseDTO[] dto = houses.Skip((options.CurrentIndex - 1) * options.PageSize).Take(options.PageSize)
                .Select(p => ToDTO(p)).ToArray();
                return new HouseSearchResult()
                {
                    totalCount = totalCount,
                    result = dto
                };

            }
        }
        public int GetTodayNewHouseCount(long cityId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);
                return csHouse.GetAll()
                    .Count(p => p.Community.Region.CityId == cityId
                    && SqlFunctions.DateDiff("hh", p.CreateDateTime, DateTime.Now) <= 24);
            }
        }
        public void Update(HouseDTO house)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<HouseEntity> csHouse = new CommonService<HouseEntity>(dbc);               
                var dbhouse = csHouse.GetAll().Include(p => p.Attachments).SingleOrDefault(p => p.Id == house.Id);
                dbhouse.Attachments.Clear();
                CommonService<AttachmentEntity> csAttachment = new CommonService<AttachmentEntity>(dbc);
                var attachments = csAttachment.GetAll().Where(p => house.AttachmentIds.Contains(p.Id)).ToArray();
                dbhouse.CommunityId = house.CommunityId;
                dbhouse.RoomTypeId = house.RoomTypeId;
                dbhouse.Address = house.Address;
                dbhouse.MonthRent = house.MonthRent;
                dbhouse.StatusId = house.StatusId;
                dbhouse.Area = house.Area;
                dbhouse.DecorateStatusId = house.DecorateStatusId;
                dbhouse.TotalFloorCount = house.TotalFloorCount;
                dbhouse.FloorIndex = house.FloorIndex;
                dbhouse.Direction = house.Direction;
                dbhouse.LookableDateTime = house.LookableDateTime;
                dbhouse.CheckInDateTime = house.CheckInDateTime;
                dbhouse.OwnerName = house.OwnerName;
                dbhouse.OwnerPhoneNum = house.OwnerPhoneNum;
                dbhouse.Description = house.Description;
                dbhouse.Attachments = attachments;
                dbc.SaveChanges();
            }
        }
        public HouseDTO ToDTO(HouseEntity house)
        {
            var dto = new HouseDTO()
            {
                Id = house.Id,
                CommunityId = house.CommunityId,
                RoomTypeId = house.RoomTypeId,
                Address = house.Address,
                MonthRent = house.MonthRent,
                StatusId = house.StatusId,
                Area = house.Area,
                DecorateStatusId = house.DecorateStatusId,
                TotalFloorCount = house.TotalFloorCount,
                FloorIndex = house.FloorIndex,
                Direction = house.Direction,
                LookableDateTime = house.LookableDateTime,
                OwnerName = house.OwnerName,
                OwnerPhoneNum = house.OwnerPhoneNum,
                Description = house.Description,
                AttachmentIds = house.Attachments.Select(p => p.Id).ToArray(),
                CheckInDateTime = house.CheckInDateTime,
                CityId = house.Community.Region.CityId,
                CityName = house.Community.Region.City.Name,
                CommunityBuiltYear = house.Community.BuiltYear,
                CommunityLocation = house.Community.Location,
                CommunityName = house.Community.Name,
                CommunityTraffic = house.Community.Traffic,
                DecorateStatusName = house.DecorateStatus.Name,
                RegionId = house.Community.RegionId,
                RegionName = house.Community.Region.Name,
                StatusName = house.Status.Name,
                RoomTypeName = house.RoomType.Name,
                TypeId = house.TypeId,
                TypeName = house.Type.Name,
                CreateDateTime = house.CreateDateTime
            };
            var firstPic = house.HousePics.FirstOrDefault();
            if(firstPic!=null)
            {
                dto.FirstThumbUrl = firstPic.ThumbUrl;
            }
            return dto;
        }
    }
}
