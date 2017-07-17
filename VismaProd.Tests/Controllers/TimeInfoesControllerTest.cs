using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using VismaProd.Controllers;

namespace VismaProd.Tests.Controllers
{
    [TestClass]
    class TimeInfoesControllerTest
    {
        [TestMethod]
        public void Create()
        {
            TimeInfoesController controller = new TimeInfoesController();

            ViewResult result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("Add time to your projects!", result.ViewBag["Message"]);
        }
    }
}
