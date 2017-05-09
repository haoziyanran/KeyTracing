namespace WorkItemDuplicate
{
        private void SourceChooseTFSButton_Click(object sender, EventArgs e)
        {
            TeamProjectPicker tfsPicker = new TeamProjectPicker(TeamProjectPickerMode.NoProject, false);
            if (tfsPicker.ShowDialog() != System.Windows.Forms.DialogResult.OK || tfsPicker.SelectedTeamProjectCollection == null)
            {
                return;
            }
            srcTpc = tfsPicker.SelectedTeamProjectCollection;
        }
        private void sourceProjectButton_Click(object sender, EventArgs e)
        {
            TeamProjectPicker tfsPicker = new TeamProjectPicker(TeamProjectPickerMode.MultiProject, false);
            if (tfsPicker.ShowDialog() != System.Windows.Forms.DialogResult.OK || tfsPicker.SelectedProjects == null)
            {
                return;
            }
            var srcProjectList = tfsPicker.SelectedProjects;
        }  
        
        //线程封装
        private void StartButton_Click(object sender, EventArgs e)
        {
            DuplicateWorkitem dw = new DuplicateWorkitem(workitemList, srcTpc, desTpc, desProjectName, workitemTypeList, workitemLinkTypeList, sourceProjectList);

            dw.threadStartEvent += new EventHandler(method_threadStartEvent);
            dw.threadEvent += new EventHandler(method_threadEvent);
            dw.threadEndEvent += new EventHandler(method_threadEndEvent);
            dw.threadException += new EventHandler(method_threadExceptionEvent);

            dw.Start();
        }


        private delegate void maxValueDelegate(int maxValue);
        private delegate void nowValueDelegate(int nowValue);
        private delegate void resultValueDelegate(List<WorkItem> allNewWorkitemList);
        private delegate void showExceptionDelegate(string message);
        
        private void method_threadStartEvent(object sender, EventArgs e)
        {
            int maxValue = Convert.ToInt32(sender);
            maxValueDelegate max = new maxValueDelegate(setMax);
            this.Invoke(max, maxValue);
        }
        private void method_threadEvent(object sender, EventArgs e)
        {
            int nowValue = Convert.ToInt32(sender);
            nowValueDelegate now = new nowValueDelegate(setNow);
            this.Invoke(now, nowValue);
        }
        private void method_threadEndEvent(object sender, EventArgs e)
        {
            List<WorkItem> resultValue = sender as List<WorkItem>;
            resultValueDelegate result = new resultValueDelegate(getResult);
            this.Invoke(result, resultValue);
        }
        private void method_threadExceptionEvent(object sender, EventArgs e)
        {
            string message = sender as string;
            showExceptionDelegate exception = new showExceptionDelegate(showExceptionMessage);
            this.Invoke(exception, message);
        }

        private void setMax(int maxValue)
        {
            this.progressBar1.Maximum = maxValue;
        }
        private void setNow(int nowValue)
        {
            this.progressBar1.Value = nowValue;
            this.percentLabel.Text = string.Format(nowValue.ToString() + "%");
        }
        private void getResult(List<WorkItem> allNewWorkitemList)
        {
            this.desWorkitemListBox.Items.Add(string.Format("The Duplicated WorkItems is: ({0} workitems)", allNewWorkitemList.Count));
            foreach (WorkItem wi in allNewWorkitemList)
            {
                this.desWorkitemListBox.Items.Add(string.Format("    {0}", wi.Id + ":" + wi.Title));
            }
            this.desWorkitemListBox.Items.Add("");
            this.desWorkitemListBox.Items.Add("Duplicated complete！");

            MessageBox.Show("Duplicate Accomplishment");
        }
        private void showExceptionMessage(string message)
        {
            MessageBox.Show(message);
        }
}        
