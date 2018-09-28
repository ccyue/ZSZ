using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.FrontWeb.Models
{
    [Serializable]
    public class HouseViewModel
    {
        public HouseDTO House { get; set; }
        public HousePicDTO[] HousePics { get; set; }
        public AttachmentDTO[] HouseAttachment { get; set; }
        public AttachmentDTO[] AllAttachment { get; set; }
        public HouseDTO[] SimilarHouse { get; set; }


    }
}