namespace WorkItemDuplicate
{
    public class DuplicateWorkitem
    {
        public event EventHandler threadStartEvent;
        public event EventHandler threadEvent;
        public event EventHandler threadEndEvent;
        public event EventHandler threadException;

        public Thread duplicateThread = null;
        public DuplicateWorkitem(List<WorkItem> subWIList, TfsTeamProjectCollection sourceTpc, TfsTeamProjectCollection targetTpc, string tarProjectName, List<string> wiTypeList, List<string> LinkTypeList, List<string> sourceProjectList)
        {
            this.srcTpc = sourceTpc;
            this.workitemTypeList = wiTypeList;
            this.workitemLinkTypeList = LinkTypeList;
            this.subWorkitemList = subWIList;
            this.desTpc = targetTpc;
            this.desProjectName = tarProjectName;
            this.sourceProjectList = sourceProjectList;
            
            duplicateThread = new Thread(new ThreadStart(WorkitemListDuplicate));
        }
        public void Start()
        {
            if (duplicateThread != null)
            {
                duplicateThread.Start();
            }
        }
        
        public void WorkitemListDuplicate()
        {
            try
            {
                threadStartEvent.Invoke(100, new EventArgs());

                GetQueryWorkItems(srcTpc, subWorkitemList);
                threadEvent.Invoke(25, new EventArgs());

                TraversalAllWorkitems(subWorkitemList, srcTpc);

                DuplicateAllWorkitemNoLink(allSrcWorkitemList, desTpc, desProjectName);

                BuildWorkItemLinks(allSrcWorkitemList, allNewWorkitemList, map, srcTpc);

                threadEvent.Invoke(100, new EventArgs());
                threadEndEvent.Invoke(allNewWorkitemList, new EventArgs());
            }
            catch (Exception e)
            {
                threadException.Invoke(e.Message, new EventArgs());
            }
        }
        
        
        //1.Get query Workitems
        private void GetQueryWorkItems(TfsTeamProjectCollection srcTpc, List<WorkItem> subWorkitemList)
        {
            srcProjectName = subWorkitemList[0].Project.Name;
            WorkItemStore workitemStore = (WorkItemStore)srcTpc.GetService(typeof(WorkItemStore));

            var workitemCollection = workitemStore.Query("select  [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State] from WorkItems where [System.TeamProject] = '" + srcProjectName + "'  AND ([System.WorkItemType] = 'RS' or [System.WorkItemType] = 'SFS' or [System.WorkItemType] = 'SSFS' or [System.WorkItemType] = 'DS' or [System.WorkItemType] = 'Test Case') and [MR.Requirement.RearrangedM3] <> 'Deferred' AND [MR.Requirement.RearrangedM3] <> 'Terminated' AND [MR.Requirement.ApplicativeProject] contains '[M3]' ORDER BY [System.Id]");
            foreach (WorkItem workitem in workitemCollection)
            {
                queryWorkitemList.Add(workitem);
            }
        }

        //2.Traversal selected WorkItems, And Record all RelatedLinks
        private void TraversalAllWorkitems(List<WorkItem> selectWorkitemsList, TfsTeamProjectCollection srcTpc)
        {
            int count = 1;
            List<WorkItem> subWorkitemList = new List<WorkItem>();
            foreach (WorkItem wi in selectWorkitemsList)
            {
                if (isRequireWorkitemType(wi))
                {
                    subWorkitemList.Add(wi);
                }
            }
            foreach (WorkItem wi in subWorkitemList)
            {
                if (!isExistWorkitem(wi, allSrcWorkitemList))
                {
                    Traversal(wi);
                }
                //Set ProgressBar Value form 25% to 50%
                threadEvent.Invoke((25 * (count++)) / subWorkitemList.Count + 25, new EventArgs());
            }
        }
        private void Traversal(WorkItem workitem)
        {
            if (workitem.WorkItemLinks.Count > 0)
            {
                foreach (WorkItemLink wiLink in workitem.WorkItemLinks)
                {
                    WorkItem targetWorkitem = workitem.Store.GetWorkItem(wiLink.TargetId);
                    if ((isRequireLinkType(wiLink))
                        && (!isExistLink(wiLink, allWorkitemLinkList))
                        && (isRequireWorkitemType(targetWorkitem))
                        && (isWorkitemInRightProject(targetWorkitem)))
                    {
                        allWorkitemLinkList.Add(wiLink);
                    }
                }
            }
        }

        //3.Duplicate all WorkItems besides Links, And Record oldID and newID maps
        private void DuplicateAllWorkitemNoLink(List<WorkItem> allSrcWorkitemList, TfsTeamProjectCollection desTpc, string desProjectName)
        {
            allSrcWorkitemList = queryWorkitemList;

            var store = desTpc.GetService<WorkItemStore>();

            var query = new Query(store, "SELECT [System.Id], [System.Links.LinkType], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State] FROM WorkItemLinks WHERE ([Source].[System.TeamProject] = "
                + "'MR_Requirement'"
                + "  AND ( [Source].[System.WorkItemType] = 'RS'  OR  [Source].[System.WorkItemType] = 'SFS'  OR  [Source].[System.WorkItemType] = 'SSFS'  OR  [Source].[System.WorkItemType] = 'DS'  OR  [Source].[System.WorkItemType] = 'Test Case' ) AND  [Source].[MR.Requirement.RearrangedM3] <> 'Deferred'  AND  [Source].[MR.Requirement.RearrangedM3] <> 'Terminated'  AND  [Source].[MR.Requirement.ApplicativeProject] CONTAINS '[M3]') And ([System.Links.LinkType] <> '') And ([Target].[System.TeamProject] = 'PETMR') ORDER BY [System.Id] mode(MustContain)");
            WorkItemLinkInfo[] links = query.RunLinkQuery();
            Dictionary<int, int> linkDict = new Dictionary<int, int>();
            foreach (var item in links)
            {
                if (item.LinkTypeId == 0) { continue; }
                linkDict.Add(item.SourceId, item.TargetId);
            }

            int count = 1;
            foreach (WorkItem wi in allSrcWorkitemList)
            {
                //Set ProgressBar Value form 50% to 75%
                threadEvent.Invoke((25 * (count++)) / allSrcWorkitemList.Count + 50, new EventArgs());

                if (linkDict.ContainsKey(wi.Id))
                {
                    WorkItem newWI = store.GetWorkItem(linkDict[wi.Id]);
                    allNewWorkitemList.Add(newWI);
                    map.Add(wi.Id, newWI);
                    continue;
                }

                WorkItem newWorkitem = WorkitemClone.CopyWorkitem(desTpc, desProjectName, wi);
                allNewWorkitemList.Add(newWorkitem);
                map.Add(wi.Id, newWorkitem);
            }

        }

        //4.Build new Links
        private void BuildWorkItemLinks(List<WorkItem> allSrcWorkitemList, List<WorkItem> allNewWorkitemList, Dictionary<int, WorkItem> map, TfsTeamProjectCollection srcTpc)
        {
            int count = 1;
            WorkItemStore workitemStore = ((WorkItemStore)srcTpc.GetService(typeof(WorkItemStore)));

            foreach (WorkItemLink wiLink in allWorkitemLinkList)
            {
                //Set ProgressBar Value form 75% to 100%
                threadEvent.Invoke((25 * (count++)) / allWorkitemLinkList.Count + 75, new EventArgs());

                WorkItem linkSourceWorkitem = workitemStore.GetWorkItem(wiLink.SourceId);
                WorkItem linkTargetWorkitem = workitemStore.GetWorkItem(wiLink.TargetId);

                if (isExistWorkitem(linkSourceWorkitem, queryWorkitemList) && isExistWorkitem(linkTargetWorkitem, queryWorkitemList))
                {
                    WorkItem B1workitem;
                    WorkItem B2workitem;
                    map.TryGetValue(wiLink.SourceId, out B1workitem);
                    map.TryGetValue(wiLink.TargetId, out B2workitem);
                    WorkItemLinkTypeEnd linkEnd = wiLink.LinkTypeEnd;
                    WorkItemLink newLink = new WorkItemLink(linkEnd, B1workitem.Id, B2workitem.Id);
                    B1workitem.Links.Add(newLink);
                    B1workitem.Save();
                }
                else if (isExistWorkitem(linkSourceWorkitem, queryWorkitemList) && !isExistWorkitem(linkTargetWorkitem, queryWorkitemList))
                {
                    WorkItem B1workitem;
                    map.TryGetValue(wiLink.SourceId, out B1workitem);
                    WorkItemLinkTypeEnd linkEnd = wiLink.LinkTypeEnd;
                    WorkItemLink newLink = new WorkItemLink(linkEnd, B1workitem.Id, wiLink.TargetId);
                    B1workitem.Links.Add(newLink);
                    B1workitem.Save();
                }
            }
        }

        private bool isExistLink(WorkItemLink relatedLink, List<WorkItemLink> allWorkitemLinkList)
        {
            foreach (WorkItemLink wiL in allWorkitemLinkList)
            {
                if (((wiL.SourceId == relatedLink.SourceId) && (wiL.TargetId == relatedLink.TargetId)) || ((wiL.SourceId == relatedLink.TargetId) && (wiL.TargetId == relatedLink.SourceId)))
                {
                    return true;
                }
            }
            return false;
        }
        private bool isExistWorkitem(WorkItem wi, List<WorkItem> allSrcWorkitemList)
        {
            var workitem = from wiList in allSrcWorkitemList
                           where wi.Id == wiList.Id
                           select wiList;
            if (workitem.Count() == 0)
                return false;
            else
                return true;
        }
        private bool isExistWorkitem(WorkItem wi, Stack<WorkItem> workitemStack)
        {
            var workitem = from currentWorkitem in workitemStack
                           where wi.Id == currentWorkitem.Id
                           select currentWorkitem;
            if (workitem.Count() == 0)
                return false;
            else
                return true;
        }
        private bool isRequireLinkType(WorkItemLink link)
        {
            foreach (string linkType in workitemLinkTypeList)
            {
                if (linkType.Equals(link.LinkTypeEnd.Name))
                    return true;
            }
            return false;
        }
        private bool isRequireWorkitemType(WorkItem workitem)
        {
            string wiType = workitem.Fields["Work Item Type"].Value as string;
            foreach (string type in workitemTypeList)
            {
                if (type.Equals(wiType))
                    return true;
            }
            return false;
        }
        private bool isWorkitemInRightProject(WorkItem workitem)
        {
            foreach (string srcProjectName in sourceProjectList)
            {
                if (workitem.Project.Name.Equals(srcProjectName))
                    return true;
            }
            return false;
        }
        
    }
}
