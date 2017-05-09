using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Controls;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSAPIExtension;

namespace WorkItemDuplicate
{
    public partial class WorkItemQuery : Form
    {
        private PickWorkItemsControl pickWorkItemsControl;
        private TfsTeamProjectCollection tpc;

        public List<WorkItem> SelectedWorkItems
        {
            get
            {
                return pickWorkItemsControl.SelectedWorkItems();
            }
        }

        public WorkItemQuery()
        {
            InitializeComponent();
        }

        public WorkItemQuery(TfsTeamProjectCollection tpc)
            : this()
        {
            this.tpc = tpc;
        }

        //Occurs before a form is displayed for the first time
        private void WorkItemQuery_Load(object sender, EventArgs e)
        {
            if (tpc == null)
            {
                MessageBox.Show("Please choose the Source TeamProjectCollection!");
                this.Close();
            }
            else
            {
                pickWorkItemsControl = new PickWorkItemsControl(tpc.WorkItemStore());
                this.panel1.Controls.Add(pickWorkItemsControl);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
