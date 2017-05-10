namespace OpenXML.WorkItem
{
    using System.Net;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WIAttachments
    {
        public static void SavePicToLocalTemp(List<WorkItem> workitemList)
        {
            foreach (WorkItem workitem in workitemList)
            {
                List<Attachment> atmList = GetAttachments(workitem);
                if ((atmList == null) || (atmList.Count == 0))
                {
                    continue;
                }
                string idString = workitem.Type.Name + workitem.Id;

                //创建子文件夹，以IdString命名，存储图片附件
                //..........
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                string savePathID = Path.Combine(TempFilePath, idString);
                if (!Directory.Exists(savePathID))
                {
                    Directory.CreateDirectory(savePathID);
                }
                foreach (Attachment atm in atmList)
                {
                    webClient.DownloadFile(atm.Uri, Path.Combine(savePathID, atm.Name));
                }

                //Version 2
                //WorkItemServer wise = tcs.GetService<WorkItemServer>();
                //string file = wise.DownloadFile(atm.Id);
                //File.Copy(file, Path.Combine(savePathID, atm.Name));
            }
        }
        
        //获取WorkItem的Attachment
        private static List<Attachment> GetAttachments(WorkItem item)
        {
            AttachmentCollection attachments = item.Attachments;
            List<Attachment> attachs = new List<Attachment>();
            if (attachments == null || attachments.Count <= 0)
            {
                return null;
            }

            foreach (Attachment attach in item.Attachments)
            {
                string extension = attach.Extension;
                foreach (string type in PictureTypes)
                {
                    if (IsSameExtension(extension, type))
                    {
                        attachs.Add(attach);
                    }
                }
            }
            return attachs;
        }
        
        //类的属性值
        private static List<string> _pictureTypes;
        public static List<string> PictureTypes
        {
            get
            {
                _pictureTypes = new List<string>();
                _pictureTypes.AddRange(new string[]{
                    ".jpg", ".tif", ".gif", ".rng", ".png", ".bmp"});
                return _pictureTypes;
            }
        }
        public static string TempFilePath
        {
            get
            {
                return String.Format(Path.Combine(Path.GetTempPath(), "AAAAA"));
            }
        }
        
    }   
}
