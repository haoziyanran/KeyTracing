// -----------------------------------------------------------------------
// <copyright file="KeyTestLevelVerification.cs" company="">
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
    public class KeyTestLevelVerification : Verification
    {
        public KeyTestLevelVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(240, 100, 100);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.KeyTestLevelVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.KeyTestLevelVerificationTitle; }
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

        /// <summary>
        /// The field test level is mulit-option value.
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

            if (string.IsNullOrEmpty(key.TestLevel))
            {
                Type = VerificationResultType.Succeed;
                return this;
            }

            // the result is succeed by default.
            Type = VerificationResultType.Succeed;

            if (key.TestLevelValues.Length > 0)
            {
                if (key.TestedByIds.Length == 0)
                {
                    Type = VerificationResultType.Failed;
                }
                else
                {
                    foreach (var test in key.TestedByCollection)
                    {
                        if (!key.TestLevelValues.Contains(test.TestLevel, StringComparer.OrdinalIgnoreCase))
                        {
                            Type = VerificationResultType.Failed;
                        }
                    }
                }
            }

            return this;
        }
    }
}
