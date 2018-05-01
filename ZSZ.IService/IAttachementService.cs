using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IAttachementService:IServiceSupport
    {
        AttachmentDTO[] GetAll();
        AttachmentDTO[] GetByHouseId(long houseId);

    }
}
