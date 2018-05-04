using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseController : Controller
    {
        public IHouseService HouseService { get; set; }
        public IRegionService RegionService { get; set; }
        public ICommunityService CommunityService { get; set; }
        public IIdNameService IdNameService { get; set; }
        public IAttachementService AttachementService { get; set; }

        private SelectListItem firstItem = new SelectListItem() { Value = "0", Text = "请选择" };
        public ActionResult List()
        {
            HouseSearchOptions options = new HouseSearchOptions()
            {
                OrderByType = HouseSearchOrderByType.CreateDateDesc,
                PageSize = 10,
                CurrentIndex = 1,
                CityId = (long)Session["CityId"]
            };
            options.CityId = (long)Session["CityId"];
            HouseSearchResult result = HouseService.Search(options);
            ViewBag.RoomTypes = GetListByName("户型");
            ViewBag.Status = GetListByName("房屋状态");
            ViewBag.DecorateStatus = GetListByName("装修状态");
            return View(result);
        }
        [HttpGet]
        public ActionResult Add()
        {
            var regions = RegionService.GetAll((long)Session["CityId"]).Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();
            regions.Insert(0, firstItem);
            ViewBag.Regions = regions;
            ViewBag.Communities = new List<SelectListItem>() { firstItem };
            ViewBag.RoomTypes = GetListByName("户型");
            ViewBag.Status = GetListByName("房屋状态");
            ViewBag.DecorateStatus = GetListByName("装修状态");
            ViewBag.Types = GetListByName("房屋类别");
            ViewBag.Attachements = AttachementService.GetAll().Select(p => new SelectListItem() { Text = p.Name, Value = p.Id.ToString() }).ToList(); ;
            return View(new HouseAddNewDTO());
        }
        [HttpPost]
        public ActionResult Add(HouseAddNewDTO house)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            HouseService.Add(house);
            return Json(new AjaxResult() { Status = "ok"});
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            HouseService.Delete(id);
            return Json(new AjaxResult() { Status = "ok" });
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var house = HouseService.GetById(id);
            if(house==null)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "此房源不存在" });
            }
            var regions = RegionService.GetAll((long)Session["CityId"]).Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = p.Id == house.RegionId
            }).ToList();
            regions.Insert(0, firstItem);
            ViewBag.Regions = regions;
            var communities = CommunityService.GetByRegionId(house.RegionId).Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name, Selected = p.Id == house.CommunityId }).ToList();
            communities.Insert(0, firstItem);
            ViewBag.Communities = communities;
            ViewBag.RoomTypes = GetListByName("户型",house.RoomTypeId);
            ViewBag.Status = GetListByName("房屋状态", house.StatusId);
            ViewBag.DecorateStatus = GetListByName("装修状态",house.DecorateStatusId);
            ViewBag.Types = GetListByName("房屋类别",house.TypeId);
            ViewBag.Attachements = AttachementService.GetAll().Select(p => new SelectListItem() { Text = p.Name, Value = p.Id.ToString() }).ToList();
            return View(house);
        }
        [HttpPost]
        public ActionResult Edit(HouseUpdateDTO house)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            HouseService.Update(house);
            return Json(new AjaxResult() { Status = "ok" });
        }

        [HttpPost]
        public ActionResult GetCommunityByRegion(long regionId)
        {
            var communities = CommunityService.GetByRegionId(regionId).Select(p => new SelectListItem() {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();
            communities.Insert(0, firstItem);
            return Json(new AjaxResult() { Status= "ok", Data = communities });
        }

        private List<SelectListItem> GetListByName(string name, long id = 0)
        {
            IdNameDTO[] dtos = IdNameService.GetAll(name);
            var newList = new List<SelectListItem>() { firstItem };
            if (dtos != null)
            {
                newList.AddRange(dtos.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name, Selected = p.Id == id }).ToList());
            }
            return newList;
        }
	}
}