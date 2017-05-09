// -----------------------------------------------------------------------
// <copyright file="projectDBContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace GetDataEveryday
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Entity;

    public class ProjectDBContext : DbContext
    {
        public ProjectDBContext()
            : base("Project_Records")
        { }

        public IDbSet<Project> projects { get; set; }
        public IDbSet<AdministratorUser> administratorUsers { get; set; }
        public IDbSet<ProjectAdministratorUser> projectAdministratorUsers { get; set; }
    }
}
