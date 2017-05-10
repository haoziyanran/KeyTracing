// -----------------------------------------------------------------------
// <copyright file="SourceVerification.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace RabbitMQ4Consumer.Verifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SourceVerification : Verification
    {
        public SourceVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(240, 50, 50);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.SourceVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.SourceVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.SourceVerificationDescription; }
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
        /// The field source has three values.
        /// 1. Derived
        /// 2. Transferred
        /// 3. New
        /// 
        /// if the source does not exists or the value is empty ignore this verification.
        /// </summary>
        /// <param name="baseKey"></param>
        /// <returns></returns>
        public override Verification Verify(VKeyBase baseKey)
        {
            var key = baseKey as VKey;
            if (key == null)
            {
                throw new VModelException("Does not support to verify test case now.");
            }

            if (string.IsNullOrEmpty(key.Source))
            {
                Type = VerificationResultType.Succeed;
                return this;
            }

            if (key.Source == Source.Derived)
            {
                if (key.TracedFromIds.Length == 0)
                {
                    Type = VerificationResultType.Failed;
                }
                else
                {
                    Type = VerificationResultType.Succeed;
                }
            }
            else if (key.Source == Source.Transferred)
            {
                if (key.RelatedIds.Length == 0)
                {
                    Type = VerificationResultType.Failed;
                }
                else
                {
                    Type = VerificationResultType.Succeed;
                }
            }
            else if (key.Source == Source.New)
            {
                // ignore
                Type = VerificationResultType.Succeed;
            }

            return this;
        }
    }
}
