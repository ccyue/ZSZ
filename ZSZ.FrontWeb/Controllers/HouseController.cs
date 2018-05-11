using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class HouseController : Controller
    {
        public IHouseService HouseService { get; set; }
        public IAttachementService AttachementService { get; set; }
        public IRegionService RegionService { get; set; }

        public ActionResult Index(long id)
        {
            var house = HouseService.GetById(id);
            if (house == null)
            {
                return View("Error", (object)"不存在的房源id");
            }
            HouseViewModel model = new HouseViewModel();
            model.House = house;
            model.HousePics = HouseService.GetPics(id);
            model.HouseAttachment = AttachementService.GetByHouseId(id);
            model.AllAttachment = AttachementService.GetAll();
            return View(model);
        }
        public ActionResult Search(long typeId, string keyWords, string monthRent, string orderByType, long? regionId)
        {
            long cityId = FrontUnit.GetCityID(HttpContext);
            var options = new DTO.HouseSearchOptions();
            options.CityId = cityId;
            options.Keywords = keyWords;
            //解析月租
            int? startMonthRent, endMonthRent;
            ParseMonthRent(monthRent, out startMonthRent, out endMonthRent);
            options.StartMonthRent = startMonthRent;
            options.EndMonthRent = endMonthRent;
            options.RegionId = regionId;
            options.CurrentIndex = 1;
            switch(orderByType)
            {
                case "MonthRentAsc": options.OrderByType = DTO.HouseSearchOrderByType.MonthRentAsc;break;
                case "MonthRentDesc": options.OrderByType = DTO.HouseSearchOrderByType.MonthRentDesc; break;
                case "AreaAsc": options.OrderByType = DTO.HouseSearchOrderByType.AreaAsc; break;
                case "AreaDesc": options.OrderByType = DTO.HouseSearchOrderByType.AreaDesc; break;
            }
            options.PageSize = 10;

            HouseSearchViewModel model = new HouseSearchViewModel();
            model.houses = HouseService.Search(options).result;
            model.regions = RegionService.GetAll(cityId);

            return View(model);
        }
        private void ParseMonthRent(string monthRent, out int? startMonthRent, out int? endMonthRent)
        {
            if(string.IsNullOrEmpty(monthRent))
            {
                startMonthRent = null;
                endMonthRent = null;
                return;
            }
            else
            {
                string[] values = monthRent.Split('-');
                string strStart = values[0];
                string strEnd = values[2];
                if(strStart == "*")
                {
                    startMonthRent = null;
                }
                else
                {
                    startMonthRent = Convert.ToInt32(strStart);
                }
                if (strEnd == "*")
                {
                    endMonthRent = null;
                }
                else
                {
                    endMonthRent = Convert.ToInt32(strEnd);
                }
            }
        }
    }
}