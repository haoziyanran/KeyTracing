// -----------------------------------------------------------------------
// <copyright file="DifferentPRAVerification.cs" company="">
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
    public class DifferentPRAVerification : Verification
    {
        public DifferentPRAVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(240, 128, 128);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.DifferentPRAVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.DifferentPRAVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.DifferenttRearrangedDescription; }
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
            // vertical
            VKey key = baseKey as VKey;
            Debug.Assert(key != null);

            Type = VerificationResultType.Succeed;

            if (key.IsDeferredOrTerminated)
            {
                return this;
            }

            var tracedFrom = key.TracedFromCollection
                .Where(x => !x.PRA.Equals(key.PRA, StringComparison.OrdinalIgnoreCase));

            if (tracedFrom.Count() > 0)
            {
                Type = VerificationResultType.Failed;
            }

            var tracedTo = key.TracedToCollection
                .Where(x => !x.PRA.Equals(key.PRA, StringComparison.OrdinalIgnoreCase));

            if (tracedTo.Count() > 0)
            {
                Type = VerificationResultType.Failed;
            }

            // horizontal
            var testedBy = key.TestedByCollection
                .Where(x =>
                {
                    if (!x.PRA.Equals(key.PRA, StringComparison.OrdinalIgnoreCase))
                    {
                        // If the key PRA is No, the test case PRA is ok either Yes or No.
                        if (key.PRA.Equals(PRA.No, StringComparison.OrdinalIgnoreCase)
                            && x.PRA.Equals(PRA.Yes, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return false;
                });

            if (testedBy.Count() > 0)
            {
                Type = VerificationResultType.Failed;
            }

            return this;
        }
    }
}
