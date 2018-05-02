using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSZ.Service;

namespace ServiceTests
{
    [TestClass]
    public class UnitTestAdminLog
    {
        [TestMethod]
        public void Add()
        {
            new AdminLogService().Add(2, "test service");
        }
    }
}
