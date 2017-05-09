// -----------------------------------------------------------------------
// <copyright file="Project.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace GetDataEveryday
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Project")]
    public class Project
    {
        [Key]
        public int PID { get; set; }
        public string tfsServerName { get; set; }
        public string collectionName { get; set; }
        public string projectName { get; set; }
        public virtual ICollection<AdministratorUser> administratorUsers { get; set; }
        public int userNumbers { get; set; }
        public DateTime time { get; set; }
    }

    [Table("AdministratorUser")]
    public class AdministratorUser
    {
        [Key]
        public int UID { get; set; }
        public string administratorUserName { get; set; }
        public virtual ICollection<Project> projects { get; set; }
    }

    [Table("ProjectAdministratorUser")]
    public class ProjectAdministratorUser
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Project")]
        public int PID { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("AdministratorUser")]
        public int UID { get; set; }

        public virtual Project Project { get; set; }
        public virtual AdministratorUser AdministratorUser { get; set; }
    }
}
