// -----------------------------------------------------------------------
// <copyright file="ParentRequirementVerification.cs" company="">
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
    public class ParentRequirementVerification : Verification
    {
        public ParentRequirementVerification()
        {
            BackgroundColor = System.Drawing.Color.Coral;
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.ParentRequirementVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.ParentRequirementVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.ParentRequirementVerificationDescription; }
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

            if (key.Type == VKeyTypes.RS)
            {
                // RS has nothing traced from
                Type = VerificationResultType.Succeed;
            }
            else if (key.Type == VKeyTypes.SFS)
            {
                // SFS has 0 or 1 traced from
                if (key.TracedFromIds.Length <= 1)
                {
                    Type = VerificationResultType.Succeed;
                }
                else
                {
                    Type = VerificationResultType.Failed;
                }
            }
            else if (key.Type == VKeyTypes.SSFS)
            {
                // SSFS has 0 or 1 traced from
                if (key.TracedFromIds.Length <= 1)
                {
                    Type = VerificationResultType.Succeed;
                }
                else
                {
                    Type = VerificationResultType.Failed;
                }
            }
            else if (key.Type == VKeyTypes.DS)
            {
                // DS has 0 or 1 traced from
                if (key.TracedFromIds.Length <= 1)
                {
                    Type = VerificationResultType.Succeed;
                }
                else
                {
                    Type = VerificationResultType.Failed;
                }
            }

            return this;

        }
    }
}
