namespace WorkItemDuplicate
{
    class WorkitemCloneHelper
    {
        public static WorkItem CopyWorkitem(TfsTeamProjectCollection desTpc, string desProjectName, WorkItem srcItem)
        {
            WorkItemStore workitemStore = (WorkItemStore)desTpc.GetService(typeof(WorkItemStore));
            WorkItemTypeCollection workitemTypes = workitemStore.Projects[desProjectName].WorkItemTypes;

            //1.Copy from SrcWorkitem
            string targetWorkitemType = srcItem.Fields["Work Item Type"].Value as string;
            WorkItem targetWorkItem = srcItem.Copy(workitemTypes[targetWorkitemType], WorkItemCopyFlags.CopyFiles);

            //2.Copy Custom Field Mapping
            CopyCustomFieldMap(srcItem, targetWorkItem);

            //3.Adjust Fields to Save
            FieldVerify(srcItem, targetWorkItem);
            targetWorkItem.Save();

            //4.Copy Special Fields
            CopySpecialField(srcItem, targetWorkItem);
            targetWorkItem.Save();

            return targetWorkItem;
        }


        //2.Copy Fields Base on Requirement Document
        public static void CopyCustomFieldMap(WorkItem srcItem, WorkItem desItem)
        {
            if (desItem.Fields["Work Item Type"].Value.Equals("RS"))
            {
                desItem.Fields["Title"].Value = srcItem.Fields["Title"].Value;
                desItem.Fields["Location"].Value = srcItem.Fields["Location"].Value;
                CopyArea(srcItem, desItem);
                desItem.Fields["Source Project"].Value = "[Kylin]";
                desItem.Fields["Applicative project"].Value = "[Kylin]";
                desItem.Fields["SFS_M3"].Value = srcItem.Fields["SFS_M3"].Value;
                desItem.Fields["STS_M3"].Value = srcItem.Fields["STS_M3"].Value;
                if (srcItem.Fields["Classification_M3"].Value.Equals("[Extension]"))
                {
                    desItem.Fields["Classification_M3"].Value = null;
                }
                else
                {
                    desItem.Fields["Classification_M3"].Value = srcItem.Fields["Classification_M3"].Value;
                }
                desItem.Fields["Rearranged_M3"].Value = srcItem.Fields["Rearranged_M3"].Value;
                desItem.Fields["Rearranged reason_M3"].Value = srcItem.Fields["Rearranged reason_M3"].Value;
                desItem.Fields["DEV_Keyword_M3"].Value = srcItem.Fields["DEV_Keyword_M3"].Value;
                desItem.Fields["M3_Phase"].Value = srcItem.Fields["M3_Phase"].Value;
            }
            else if (desItem.Fields["Work Item Type"].Value.Equals("SFS"))
            {
                desItem.Fields["Title"].Value = srcItem.Fields["Title"].Value;
                desItem.Fields["Location"].Value = srcItem.Fields["Location"].Value;
                CopyArea(srcItem, desItem);
                desItem.Fields["Source Project"].Value = "[Kylin]";
                desItem.Fields["PRA"].Value = srcItem.Fields["PRA"].Value;
                desItem.Fields["Applicative project"].Value = "[Kylin]";
                desItem.Fields["M3_Phase"].Value = srcItem.Fields["M3_Phase"].Value;
                desItem.Fields["SSFS_M3"].Value = srcItem.Fields["SSFS_M3"].Value;
                desItem.Fields["SITS_M3"].Value = srcItem.Fields["SITS_M3"].Value;
                if (srcItem.Fields["Classification_M3"].Value.Equals("[Extension]"))
                {
                    desItem.Fields["Classification_M3"].Value = null;
                }
                else
                {
                    desItem.Fields["Classification_M3"].Value = srcItem.Fields["Classification_M3"].Value;
                }
                desItem.Fields["Rearranged reason_M3"].Value = srcItem.Fields["Rearranged reason_M3"].Value;
                desItem.Fields["Rearranged_M3"].Value = srcItem.Fields["Rearranged_M3"].Value;
                desItem.Fields["DEV_Keyword_M3"].Value = srcItem.Fields["DEV_Keyword_M3"].Value;
            }
            else if (desItem.Fields["Work Item Type"].Value.Equals("SSFS"))
            {
                desItem.Fields["Title"].Value = srcItem.Fields["Title"].Value;
                desItem.Fields["Location"].Value = srcItem.Fields["Location"].Value;
                CopyArea(srcItem, desItem);
                desItem.Fields["Source Project"].Value = "[Kylin]";
                desItem.Fields["PRA"].Value = srcItem.Fields["PRA"].Value;
                desItem.Fields["Applicative project"].Value = "[Kylin]";
                desItem.Fields["M3_Phase"].Value = srcItem.Fields["M3_Phase"].Value;
                desItem.Fields["DS_M3"].Value = srcItem.Fields["DS_M3"].Value;
                desItem.Fields["SSITS_M3"].Value = srcItem.Fields["SSITS_M3"].Value;
                desItem.Fields["STS_M3"].Value = srcItem.Fields["STS_M3"].Value;
                if (srcItem.Fields["Classification_M3"].Value.Equals("[Extention]"))
                {
                    desItem.Fields["Classification_M3"].Value = null;
                }
                else
                {
                    desItem.Fields["Classification_M3"].Value = srcItem.Fields["Classification_M3"].Value;
                }
                desItem.Fields["Rearranged reason_M3"].Value = srcItem.Fields["Rearranged reason_M3"].Value;
                desItem.Fields["Rearranged_M3"].Value = srcItem.Fields["Rearranged_M3"].Value;
                desItem.Fields["DEV_Keyword_M3"].Value = srcItem.Fields["DEV_Keyword_M3"].Value;
            }
            else if (desItem.Fields["Work Item Type"].Value.Equals("DS"))
            {
                desItem.Fields["Title"].Value = srcItem.Fields["Title"].Value;
                desItem.Fields["Location"].Value = srcItem.Fields["Location"].Value;
                CopyArea(srcItem, desItem);
                desItem.Fields["Source Project"].Value = "[Kylin]";
                desItem.Fields["PRA"].Value = srcItem.Fields["PRA"].Value;
                desItem.Fields["Applicative project"].Value = "[Kylin]";
                desItem.Fields["M3_Phase"].Value = srcItem.Fields["M3_Phase"].Value;
                desItem.Fields["UTS_M3"].Value = srcItem.Fields["UTS_M3"].Value;
                if (srcItem.Fields["Classification_M3"].Value.Equals("[Extension]"))
                {
                    desItem.Fields["Classification_M3"].Value = null;
                }
                else
                {
                    desItem.Fields["Classification_M3"].Value = srcItem.Fields["Classification_M3"].Value;
                }
                desItem.Fields["Rearranged reason_M3"].Value = srcItem.Fields["Rearranged reason_M3"].Value;
                desItem.Fields["Rearranged_M3"].Value = srcItem.Fields["Rearranged_M3"].Value;
                desItem.Fields["DEV_Keywords_M3"].Value = srcItem.Fields["DEV_Keywords_M3"].Value;
            }
            else if (desItem.Fields["Work Item Type"].Value.Equals("Test Case"))
            {
                CopyArea(srcItem, desItem);
            }
        }

        //3.Adjust Fields to Save
        public static void FieldVerify(WorkItem srcItem, WorkItem desItem)
        {
            if (srcItem.Fields["Assigned To"].Value == null || srcItem.Fields["Assigned To"].Value == "")
                desItem.Fields["Assigned To"].Value = null;

            foreach (Field field in desItem.Fields)
            {
                if (!field.IsValid)
                {
                    switch (field.Name)
                    {
                        case "Assigned To": desItem.Fields["Assigned To"].Value = null;
                            break;
                        default: break;
                    }
                }
            }
        }
        
        //4.Copy Special Fields
        public static void CopySpecialField(WorkItem srcItem, WorkItem desItem)
        {
            if (srcItem.Fields["State"].Value.Equals("Closed"))
            {
                desItem.Fields["State"].Value = "Resolved";
                desItem.Save();
                desItem.Open();
                desItem.Fields["State"].Value = "Closed";
            }
            if (srcItem.Fields["State"].Value.Equals("Released"))
            {
                desItem.Fields["State"].Value = "to be reviewed";
                desItem.Save();
                desItem.Open();
                desItem.Fields["State"].Value = "reviewed";
                desItem.Save();
                desItem.Open();
                desItem.Fields["State"].Value = "Released";
            }
            if (srcItem.Fields["State"].Value.Equals("reviewed"))
            {
                desItem.Fields["State"].Value = "to be reviewed";
                desItem.Save();
                desItem.Open();
                desItem.Fields["State"].Value = "reviewed";
            }
        }


        private static void CopyArea(WorkItem srcItem, WorkItem desItem)
        {
            string srcArea = srcItem.Fields[CoreField.AreaPath].Value.ToString();
            int cutIndex = srcArea.IndexOf("\\");
            if (cutIndex > 0)
            {
                string lastArea = srcArea.Substring(cutIndex);
                string headArea = desItem.Project.Name;
                string desArea = headArea + lastArea;
                desItem.Fields["Area Path"].Value = desArea;
            }
        }
        
    }
}
