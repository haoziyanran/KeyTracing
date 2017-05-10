// -----------------------------------------------------------------------
// <copyright file="LocationEmptyVerification.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace RabbitMQ4Consumer.Verifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LocationEmptyVerification : Verification
    {
        public LocationEmptyVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(255, 182, 193);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.LocationEmptyVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.LocationEmptyVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.LocationEmptyVerificationDescription; }
        }

        public override VerificationResultType Type
        {
            get;
            protected set;
        }

        public override System.Drawing.Color BackgroundColor
        {
            get;
            protected set;
        }

        public override Verification Verify(VKeyBase baseKey)
        {
            VKey key = baseKey as VKey;
            Debug.Assert(key != null);

            if (key.IsDeferredOrTerminated)
            {
                Type = VerificationResultType.Succeed;
                return this;
            }

            if (string.IsNullOrEmpty(key.Location))
            {
                Type = VerificationResultType.Failed;
            }
            else
            {
                Type = VerificationResultType.Succeed;
            }

            return this;
        }
    }
}
