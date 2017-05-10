namespace WebSite.Controllers
{
    public class ImportController : Controller
    {
        private static object Lock = new object();
        
        ......
            
        [HttpPost]
        public JsonResult CreateWorkItems()
        {
            lock (Lock)
            {
                HttpFileCollectionBase wordFile = Request.Files;
                Stream wordStream = wordFile[0].InputStream;

                OpenXML.WorkItem.CreateWorkitem.newNumber = 0;
                OpenXML.WorkItem.CreateWorkitem.updateNumber = 0;
                OpenXML.WorkItem.CreateWorkitem.failNumber = 0;
                OpenXML.WorkItem.CreateWorkitem.ignoreNumber = 0;
            }
        }
            
        
    }
}
