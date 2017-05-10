// -----------------------------------------------------------------------
// <copyright file="ConnectTFS.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenXML.TFS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Framework.Common;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ConnectTFS
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static TfsConfigurationServer ConnectImpersion(string url, string userName)
        {
            try
            {
                TfsTeamProjectCollection baseUserTpcCollection = new TfsTeamProjectCollection(new Uri(url));
                IIdentityManagementService ims = baseUserTpcCollection.GetService<IIdentityManagementService>();
                TeamFoundationIdentity identity = ims.ReadIdentity(
                                    IdentitySearchFactor.AccountName,
                                    userName,
                                    MembershipQuery.None,
                                    ReadIdentityOptions.None
                    );

                if (identity == null)
                {
                    logger.Error(string.Format(userName + " TEAMFOUNDATION IDENTITY is null：" + url));
                    return null;
                }

                TfsConfigurationServer tcs = new TfsConfigurationServer(new Uri(url), identity.Descriptor);
                tcs.EnsureAuthenticated();

                return tcs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static TfsTeamProjectCollection ConnectImpersonate(string url, string userName)
        {
            try
            {
                TfsTeamProjectCollection baseUserTpcCollection = new TfsTeamProjectCollection(new Uri(url));
                IIdentityManagementService ims = baseUserTpcCollection.GetService<IIdentityManagementService>();
                TeamFoundationIdentity identity = ims.ReadIdentity(
                                    IdentitySearchFactor.AccountName,
                                    userName,
                                    MembershipQuery.None,
                                    ReadIdentityOptions.None
                    );
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(url), identity.Descriptor);
                tpc.EnsureAuthenticated();

                return tpc;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
