namespace WebSite.Controllers
{
    public class ExportController : Controller
    {
        [HttpPost]
        public JsonResult CreateWord()
        {
            HttpFileCollectionBase uploadFiles = Request.Files;
            string firstFileName = uploadFiles[0].FileName;
            Stream firstFileStream = uploadFiles[0].InputStream;
            string fileString = ReadXMLtoString(firstFileStream);
            
            string fields = Request.Form["fields"];
            ......
        }
    
        private string ReadXMLtoString(Stream stream)
        {
            string fileString = string.Empty;
            StreamReader sr = new StreamReader(stream);
            string strLine = sr.ReadLine();
            while (strLine != null)
            {
                fileString += strLine;
                strLine = sr.ReadLine();
            }
            return fileString;
        }
    
        public ActionResult GenerateDetailList()
        {
            string filePath = Server.MapPath("~/Content/WordList");
            string userName = User.Identity.Name;

            if (System.IO.File.Exists(string.Format(filePath + "\\" + Session["fileNameDetail"].ToString())))
            {
                logger.Info(User.Identity.Name + " DownLoad: " + Session["fileNameDetail"].ToString());
                return File(new FileStream(string.Format(filePath + "\\" + Session["fileNameDetail"].ToString()), FileMode.Open), "application/x-zip-compressed", Session["fileNameDetail"].ToString());
            }
            else
            {
                logger.Fatal(User.Identity.Name + " DownLoad Failure : " + Session["fileNameTotal"].ToString());
                return View();
            }
        }
        
    }
}
