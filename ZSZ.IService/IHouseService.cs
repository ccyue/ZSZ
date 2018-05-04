using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IHouseService:IServiceSupport
    {
        HouseDTO GetById(long id);
        long GetTotalCount(long cityId, long typeId);
        HouseDTO[] GetPageData(long cityId, long typeId, int pageSize, int currentIndex);
        long Add(HouseAddNewDTO house);
        void Update(HouseUpdateDTO house);
        void Delete(long id);
        long AddHousePic(HousePicDTO housePic);
        void DeleteHousePic(long housePicId);
        HouseSearchResult Search(HouseSearchOptions options);
        long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime);
        HousePicDTO[] GetPics(long houseId);
        int GetTodayNewHouseCount(long cityId);
    }
}
