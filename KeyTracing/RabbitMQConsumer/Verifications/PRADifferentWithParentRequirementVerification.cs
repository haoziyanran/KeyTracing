// -----------------------------------------------------------------------
// <copyright file="PRADifferentWithParentRequirementVerification.cs" company="">
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
    public class PRADifferentWithParentRequirementVerification : Verification
    {
        public PRADifferentWithParentRequirementVerification()
        {
            this.BackgroundColor = Color.BurlyWood;
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.PRADifferentWithParentRequirementId); }
        }

        public override string Title
        {
            get
            {
                if (DifferentIds.Length == 0)
                {
                    return VModelVerificationResult.PRADifferentWithParentRequirementTitle;
                }

                return VModelVerificationResult.PRADifferentWithParentRequirementTitle
                    + string.Join(" ", DifferentIds);
            }
        }

        public override string Description
        {
            get { return VModelVerificationResult.PRADifferentWithParentRequirementDescription; }
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

            DifferentIds = key.TracedFromCollection
                .Where(x => x.PRA != key.PRA)
                .Select(x => x.Id).ToArray();
            if (DifferentIds.Length == 0)
            {
                Type = VerificationResultType.Succeed;
            }
            else
            {
                Type = VerificationResultType.Failed;
            }

            return this;
        }

        #region Private Properties
        private int[] DifferentIds { get; set; }
        #endregion
    }
}
