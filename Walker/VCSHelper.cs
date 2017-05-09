// -----------------------------------------------------------------------
// <copyright file="TFVC.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Walker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Transactions;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Microsoft.TeamFoundation.Client;
    using System.Messaging;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class VCSHelper
    {
        public static IEnumerable<String> GetAny(VersionControlServer vcs, String serverPath)
        {
            if (!serverPath.StartsWith("$"))
            {
                throw new WalkerException("Invalid server path.");
            }
            var itemSet = vcs.GetItems(serverPath, RecursionType.OneLevel);
            var query = from item in itemSet.Items
                        select item.ServerItem;
            return query;
        }

        public static ItemSet GetItemSet(VersionControlServer vcs, String serverPath)
        {
            return vcs.GetItems(path: serverPath, recursion: RecursionType.OneLevel);
        }

        public static TfsTeamProjectCollection GetCollection(string url)
        {
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(url));
            try
            {
                tpc.EnsureAuthenticated();
            }
            catch (Exception ex)
            {
                throw new WalkerException("Connect tfs team collection failed.", ex);
            }
            return tpc;
        }
    }
}
