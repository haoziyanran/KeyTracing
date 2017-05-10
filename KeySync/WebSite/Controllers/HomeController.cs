using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ClearOutOfDayFile(-30);

            return View();
        }

        private void ClearOutOfDayFile(int earlyDay)
        {
            string filePath = Server.MapPath("~/Content/WordList");
            string logPath = Server.MapPath("~/logs");
            DirectoryInfo folder = new DirectoryInfo(filePath);
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.CreationTime.CompareTo(DateTime.Now.AddDays(earlyDay)) < 0)
                {
                    file.Delete();
                }
            }

            folder = new DirectoryInfo(logPath);
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.CreationTime.CompareTo(DateTime.Now.AddDays(earlyDay)) < 0)
                {
                    file.Delete();
                }
            }
        }
    }
}
