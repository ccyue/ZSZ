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
        HouseDTO[] GetPageData(long cityId, int pageSize, int currentIndex);
        long Add(HouseDTO house);
        void Update(HouseDTO house);
        void Delete(long id);
        long AddHousePic(HousePicDTO housePic);
        void DeleteHousePic(long housePicId);
        HouseSearchResult Search(HouseSearchOptions options);
        int GetCount(long cityId, DateTime startDateTime, DateTime endDateTime);
    }
}
