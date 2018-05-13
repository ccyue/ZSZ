using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IHouseAppointmentService:IServiceSupport
    {
        long Add(long? userId, string name, string phoneNum, long houseId, DateTime visitDate);
        bool Follow(long adminUserId, long houseAppointmentId);
        HouseAppointmentDTO GetById(long id);
        long GetTotalCount(long cityId, string status, long? userId);
        HouseAppointmentDTO[] GetPageData(long cityId, string status, int pageSize, int currentIndex, long? userId);
    }
}
