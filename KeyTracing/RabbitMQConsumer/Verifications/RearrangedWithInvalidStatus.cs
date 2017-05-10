// -----------------------------------------------------------------------
// <copyright file="RearrangedWithInvalidStatus.cs" company="">
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
    public class RearrangedWithInvalidStatus : Verification
    {
        public RearrangedWithInvalidStatus()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(255, 173, 216);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.RearrangedWithInvalidStatusId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.RearrangedWithInvalidStatusTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.RearrangedWithInvalidStatusDescription; }
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
        /// 当Rearranged不是Terminate或Deferred的时候，状态应该是Release,
        /// 若不是release则报错除非当状态为terminate或deferred时，状态可以为任何值。
        /// </summary>
        /// <param name="baseKey"></param>
        /// <returns></returns>
        public override Verification Verify(VKeyBase baseKey)
        {
            VKey key = baseKey as VKey;
            Debug.Assert(key != null);

            var status = key.Status;
            // for MR use state to replace status
            if ("MR_Requirement".Equals(key.TeamProject, StringComparison.OrdinalIgnoreCase))
            {
                status = key.State;
            }

            if (status == Status.Released)
            {
                Type = VerificationResultType.Succeed;
            }
            else
            {

                if (!string.IsNullOrEmpty(key.Rearranged)
                    && key.Rearranged == RearrangedStatus.Deferred
                    && key.Rearranged == RearrangedStatus.Terminated)
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
