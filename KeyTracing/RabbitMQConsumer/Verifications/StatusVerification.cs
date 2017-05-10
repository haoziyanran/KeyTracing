// -----------------------------------------------------------------------
// <copyright file="StatusVerification.cs" company="">
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
    public class StatusVerification : Verification
    {
        public StatusVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(32, 178, 170);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.StatusVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.StatusVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.StatusVerificationDescription; }
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

        /// <summary>
        /// Duplicate with RearrangedWithInvalidStatus
        /// </summary>
        /// <param name="baseKey"></param>
        /// <returns></returns>
        public override Verification Verify(VKeyBase baseKey)
        {
            VKey key = baseKey as VKey;
            Debug.Assert(key != null);

            if (!key.IsDeferredOrTerminated
                && key.Status != Status.Released)
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
