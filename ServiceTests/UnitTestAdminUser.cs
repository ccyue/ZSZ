using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSZ.Service;
using ZSZ.DTO;

namespace ServiceTests
{
    [TestClass]
    public class UnitTestAdminUser
    {
        private AdminUserService service = new AdminUserService();
        [TestMethod]
        public void Add()
        {
            long uid = service.Add("test", "123456", "123", "test@qq.com", null);
            var user = service.GetbyPhoneNum("123456");
            Assert.AreEqual(user.Name, "test");
            Assert.AreEqual(user.PhoneNum, "123456");
            Assert.AreEqual(user.Email, "test@qq.com");
            Assert.IsNull(user.CityId);
            Assert.IsTrue(service.CheckLogin("123456", "123"));
            Assert.IsFalse(service.CheckLogin("1234576", "1234"));
            service.Delete(uid);
        }
        [TestMethod]
        public void GetAll()
        {
            AdminUserDTO[] dto= service.GetAll();
            AdminUserDTO[] dto1 = service.GetAll(2);
        }
        [TestMethod]
        public void Update()
        {
            service.Update(3, "test1", "123456", "", "123@qq.com", 1);
        }
        [TestMethod]
        public void HasPermission()
        {
            RoleService roleService = new RoleService();
            var user = service.GetbyPhoneNum("123456");
            roleService.GrantRoleToAdmin(user.Id,new long[]{ 3 });
            Assert.IsTrue(service.HasPermission(user.Id, "AdminUser.Add"));
        }
        [TestMethod]
        public void RecordLoginError()
        {
            if(!service.CheckLogin("123456", "1234"))
            {
                service.ResetLoginError(3);
            }
        }
        [TestMethod]
        public void Attachement()
        {
            AttachementService service = new AttachementService();
            service.GetAll();
            service.GetByHouseId(1);
        }
        [TestMethod]
        public void City()
        {
            CityService service = new CityService();
            var citys = service.GetAll();
            foreach(var city in citys)
            {
                service.GetById(city.Id);
            }
            var cityId = service.Add("大连");
            var newCity = service.GetById(cityId);
            Assert.AreEqual(newCity.Name, "大连");
        }
        [TestMethod]
        public void Community()
        {
            CommunityService service = new CommunityService();
            service.GetByRegionId(1);
        }
        [TestMethod]
        public void HouseAppointment()
        {
            HouseAppointmentService service = new HouseAppointmentService();
            service.Add(null, "cc", "12345678901", 1, DateTime.Now.AddDays(2));
        }
        public void House()
        {
        }
    }
}
