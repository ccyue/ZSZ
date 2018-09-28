using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseController : Controller
    {
        public IAdminUserService AdminUserService { get; set; }
        public IHouseService HouseService { get; set; }
        public IRegionService RegionService { get; set; }
        public ICommunityService CommunityService { get; set; }
        public IIdNameService IdNameService { get; set; }
        public IAttachementService AttachementService { get; set; }
        public ICityService CityService { get; set; }

        private SelectListItem firstItem = new SelectListItem() { Value = "0", Text = "请选择" };

        [CheckPermission("House.List")]
        public ActionResult List(long typeId,int pageIndex = 1)
        {
            long? userId = AdminHelper.GetLoginUserId(HttpContext);
            if(userId==null||userId==0)
            {
                return View("Error", (object)"总部不能进行房源管理");
            }
            long? cityId = AdminUserService.GetById(userId.Value).CityId;
            ViewBag.TotalCount = HouseService.GetTotalCount(cityId.Value, typeId);
            ViewBag.PageIndex = pageIndex;
            ViewBag.TypeId = typeId;
            var house = HouseService.GetPageData(cityId.Value, typeId, 10, (pageIndex-1)*10);
            return View(house);
        }

        [CheckPermission("House.Add")]
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

        [CheckPermission("House.Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(HouseAddNewDTO house)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            long houseId = HouseService.Add(house);
            CreateStaticPage(houseId);
            return Json(new AjaxResult() { Status = "ok"});
        }

        [CheckPermission("House.Delete")]
        [HttpPost]
        public ActionResult Delete(long id)
        {
            HouseService.Delete(id);
            return Json(new AjaxResult() { Status = "ok" });
        }

        [CheckPermission("House.Edit")]
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

        [CheckPermission("House.Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HouseUpdateDTO house)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            HouseService.Update(house);
            CreateStaticPage(house.Id);
            return Json(new AjaxResult() { Status = "ok" });
        }

        [HttpGet]
        public ActionResult PicList(long houseId)
        {
            HousePicDTO[] pics = HouseService.GetPics(houseId);
            ViewBag.HouseId = houseId;
            return View(pics);
        }
        [HttpGet]
        public ActionResult PicUpload(long houseId)
        {
            return View(houseId);
        }
        [HttpPost]
        public ActionResult PicUpload(long houseId,HttpPostedFileBase file)
        {
            //check houseId
            var house = HouseService.GetById(houseId);
            if(house==null)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "房源不存在" });
            }
            string fileName = Common.CommonHelper.CalcMD5(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string path = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd")+"/"+ fileName + extension;
            string thumbPath = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + fileName + "_thumb" + extension;
            string fullPath = HttpContext.Server.MapPath("~"+path);
            string thumbFullPath = HttpContext.Server.MapPath("~" + thumbPath);
            new FileInfo(fullPath).Directory.Create();
            new FileInfo(thumbFullPath).Directory.Create();
            file.InputStream.Position = 0;
            //file.SaveAs(path);

            //缩略图
            ImageProcessingJob jobThumb = new ImageProcessingJob();
            jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));
            jobThumb.SaveProcessedImageToFileSystem(file.InputStream, thumbFullPath);
            file.InputStream.Position = 0;

            //水印
            ImageWatermark imgWatermark =
                new ImageWatermark(HttpContext.Server.MapPath("~/images/watermark.jpg"));
            imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;
            imgWatermark.Alpha = 50;
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            jobNormal.Filters.Add(imgWatermark);//添加水印 
            jobNormal.Filters.Add(new FixedResizeConstraint(600, 600));
            jobNormal.SaveProcessedImageToFileSystem(file.InputStream, fullPath);
            HouseService.AddHousePic(new HousePicDTO()
            {
                HouseId = houseId,
                Url = path,
                ThumbUrl = thumbPath
            });
            CreateStaticPage(house.Id);
            return Json(new AjaxResult() { Status = "ok"});
        }
        [HttpPost]
        public ActionResult PicDelete(long[] deleteIds)
        {
            //TODO:
            foreach (var id in deleteIds)
            {
                HouseService.DeleteHousePic(id);
            }           
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

        public ActionResult RebuildAllStaticPage()
        {
            var houses = HouseService.GetAll();
            foreach(var h in houses)
            {
                HouseViewModel model = new HouseViewModel();
                model.House = h;
                model.HousePics = HouseService.GetPics(h.Id);
                model.HouseAttachment = AttachementService.GetByHouseId(h.Id);
                model.AllAttachment = AttachementService.GetAll();
                string html = MVCHelper.RenderViewToString(this.ControllerContext, @"~/Views/House/S_Detail.cshtml", model);
                System.IO.File.WriteAllText(string.Format(@"E:\GitHub\ZSZ\ZSZ.FrontWeb\{0}.html", h.Id), html);
            }
            return View();
        }

        private void CreateStaticPage(long houseId)
        {
            var house = HouseService.GetById(houseId);
            HouseViewModel model = new HouseViewModel();
            model.House = house;
            model.HousePics = HouseService.GetPics(houseId);
            model.HouseAttachment = AttachementService.GetByHouseId(houseId);
            model.AllAttachment = AttachementService.GetAll();
            string html = MVCHelper.RenderViewToString(this.ControllerContext, @"~/Views/House/S_Detail.cshtml", model);
            System.IO.File.WriteAllText(string.Format(@"E:\GitHub\ZSZ\ZSZ.FrontWeb\{0}.html",houseId), html);
        }
	}
}