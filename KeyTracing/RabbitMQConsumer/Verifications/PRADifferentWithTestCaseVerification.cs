// -----------------------------------------------------------------------
// <copyright file="PRADifferentWithTestCaseVerification.cs" company="">
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
    public class PRADifferentWithTestCaseVerification : Verification
    {
        public PRADifferentWithTestCaseVerification()
        {
            BackgroundColor = Color.Blue;
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.PRADifferentWithTestCaseVerificationId); }
        }

        public override string Title
        {
            get
            {
                if (DifferentIds.Length == 0)
                {
                    return VModelVerificationResult.PRADifferentWithTestCaseVerificationTitle;
                }

                return VModelVerificationResult.PRADifferentWithTestCaseVerificationTitle
                    + string.Join(" ", DifferentIds);
            }
        }

        public override string Description
        {
            get { return VModelVerificationResult.PRADifferentWithTestCaseVerificationDescription; }
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

            DifferentIds = key.TestedByCollection
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
