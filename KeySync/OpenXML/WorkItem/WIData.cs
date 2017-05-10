namespace OpenXML.WorkItem
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WIData
    {
        public static List<Dictionary<string, string>> GetTableDataList(......)
        {
            ......
            //获取TestCase的TestStep
            ITestManagementService tms = tpc.GetService<ITestManagementService>();
            ITestManagementTeamProject projectEntity = tms.GetTeamProject(projectName);
                       
            if (workitem.Type.Name.Equals("Test Case"))
            {
                TestActionCollection actions = projectEntity.TestCases.Find(workitem.Id).Actions;
                if (actions != null && actions.Count > 0)
                {
                    int id = 1;
                    foreach (var action in actions)
                    {
                        ITestStep step = action as ITestStep;
                        if (step != null)
                        {
                            string title = step.Title.ToString();
                            char[] titleArray = title.ToCharArray();
                            string result = step.ExpectedResult.ToString();
                            char[] resultArray = result.ToCharArray();
                            for (int i = 0; i < titleArray.Length; i++)
                            {
                                int value = Convert.ToInt32(titleArray[i]);
                                if (value < 32)
                                {
                                    title = title.Replace(titleArray[i].ToString(), "");
                                }
                            }
                            for (int i = 0; i < resifultArray.Length; i++)
                            {
                                int value = Convert.ToInt32(resultArray[i]);
                                if (value < 32)
                                {
                                    result = result.Replace(resultArray[i].ToString(),"");
                                }
                            }
                            wiData.Add(id + "." + title, result);
                            id++;
                        }
                        ISharedStepReference sharedStep = action as ISharedStepReference;
                        if (sharedStep != null)
                        {
                            string shareTitle = workitemStore.GetWorkItem(sharedStep.SharedStepId).Title;
                            wiData.Add(id + "." + shareTitle, "");
                            id++;
                        }
                        else
                        {
                            //ignore
                        }
                    }
                }
            }//if
            
            //对所有数据排序
            List<Dictionary<string, string>> wiListDataSorted = new List<Dictionary<string, string>>();
            if (sortWay.Equals("ID"))
            {
                var list = from wiData in wiListData
                           orderby Convert.ToInt32(Regex.Replace(wiData["ID"], "[a-z]", "", RegexOptions.IgnoreCase).Trim())
                           select wiData;
                wiListDataSorted = list.ToList();
            }
            else
            {
                var list = from wiData in wiListData
                           orderby wiData[sortWay], Convert.ToInt32(Regex.Replace(wiData["ID"], "[a-z]", "", RegexOptions.IgnoreCase).Trim())
                           select wiData;
                wiListDataSorted = list.ToList();
            }
            
            
        }
    }
}
