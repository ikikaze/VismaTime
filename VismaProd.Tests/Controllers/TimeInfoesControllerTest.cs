using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;
using VismaProd.Controllers;

namespace VismaProd.Tests.Controllers
{
    [TestClass]
    public class TimeInfoesControllerTest
    {
        [TestMethod]
        public void Create()
        {
            TimeInfoesController controller = new TimeInfoesController();

            ViewResult result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);           
            Assert.AreEqual("Add time to your projects!", result.ViewData["Message"]);
        }

        [TestMethod]
        public void Edit()
        {
            TimeInfoesController controller = new TimeInfoesController();

            //pass random Guid to controller
            ViewResult result = controller.Edit(new System.Guid()) as ViewResult;
            //should return nothing because bad ID
            Assert.IsNull(result);
            
            VismaDB db = new VismaDB();

            //pick random entry
            Random rand = new Random();
            int toSkip = rand.Next(1, db.TimeInfoes.Count());

            TimeInfo info = db.TimeInfoes.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).First();

            result = controller.Edit(info.Id) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }
    }
}
