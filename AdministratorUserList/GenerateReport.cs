namespace AdministratorUserList
{
        public static void GenerateAdminstrator(List<TfsConfigurationServer> tcsList, string outputPath)
        {
            int count = 0;
            List<string> itemList = new List<string>();
            if (File.Exists(string.Format(outputPath + "\\" + "admin.csv")))
            {
                File.Delete(string.Format(outputPath + "\\" + "admin.csv"));
            }

            StreamWriter sw = new StreamWriter(string.Format(outputPath + "\\" + "admin.csv"), false, Encoding.UTF8);

            StringBuilder strColumn = new StringBuilder();
            StringBuilder strValue = new StringBuilder();
            string[] props = { "TFS Server Name", "Collection", "Project", "Group", "Members" };
            for (int i = 0; i < props.Length; i++)
            {
                strColumn.Append(props[i]);
                strColumn.Append(",");
            }
            strColumn.Remove(strColumn.Length - 1, 1);
            sw.WriteLine(strColumn);

            foreach (TfsConfigurationServer tcs in tcsList)
            {
                //Get the catalog of team project collections
                ReadOnlyCollection<CatalogNode> collectionNodes = tcs.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

                foreach (CatalogNode collectionNode in collectionNodes)
                {
                    Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                    TfsTeamProjectCollection tpc = tcs.GetTeamProjectCollection(collectionId);

                    ICommonStructureService css = null;
                    try
                    {
                        css = tpc.GetService<ICommonStructureService>();
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    var gss = tpc.GetService<IGroupSecurityService>();

                    var listAllProjects = css.ListAllProjects();

                    var list = from project in listAllProjects
                               orderby project.Name
                               select project;

                    foreach (var project in list)
                    {
                        var listGruop = gss.ListApplicationGroups(project.Uri);
                        foreach (var group in listGruop)
                        {
                            var sids = gss.ReadIdentity(SearchFactor.Sid, group.Sid, QueryMembership.Direct);

                            //if (sids != null && sids.Members.Length != 0 && (group.AccountName.Equals("Project Administrators")))
                            if (group.AccountName.Equals("Project Administrators"))
                            {
                                //string members = null;
                                string item = null;
                                string comma = ",";

                                item = item + tcs.Name;
                                item = item + comma;

                                item = item + tpc.Uri.ToString().Substring(tpc.Uri.ToString().LastIndexOf("/") + 1);
                                item = item + comma;

                                item = item + project.Name;
                                item = item + comma;

                                item = item + group.AccountName;
                                item = item + comma;

                                if (sids != null && sids.Members.Length != 0)
                                {
                                    var listUser = gss.ReadIdentities(SearchFactor.Sid, sids.Members, QueryMembership.None);
                                    foreach (var user in listUser)
                                    {
                                        itemList.Add(item + user.AccountName);
                                    }
                                }
                                else
                                {
                                    itemList.Add(item);
                                }
                                Console.WriteLine(++count);
                            }
                        }
                    }
                }
            }

            foreach (string item in itemList)
            {
                sw.WriteLine(item);
            }
            sw.Close();
        }
}        
