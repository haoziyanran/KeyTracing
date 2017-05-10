// -----------------------------------------------------------------------
// <copyright file="PRAEmptyVerification.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace RabbitMQ4Consumer.Verifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PRAEmptyVerification : Verification
    {
        public PRAEmptyVerification()
        {
            BackgroundColor = Color.FromArgb(189, 183, 107);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.PRAEmptyVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.PRAEmptyVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.PRAEmptyVerificationDescription; }
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

            if (string.IsNullOrEmpty(key.PRA))
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
