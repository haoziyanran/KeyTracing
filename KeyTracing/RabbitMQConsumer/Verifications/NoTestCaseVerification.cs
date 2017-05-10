// -----------------------------------------------------------------------
// <copyright file="NoTestCaseVerification.cs" company="">
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
    public class NoTestCaseVerification : Verification
    {
        public NoTestCaseVerification()
        {
            BackgroundColor = Color.Yellow;
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.NoTestCaseVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.NoTestCaseVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.NoTestCaseVerificationDescription; }
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
        /// The key have no test case is ok when the key Rearranged is "Deferred" or "Terminated".
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Verification Verify(VKeyBase baseKey)
        {
            VKey key = baseKey as VKey;
            Debug.Assert(key != null);

            if (key.IsDeferredOrTerminated)
            {
                Type = VerificationResultType.Succeed;
                return this;
            }

            bool noTest = false;

            if (key.Assigment != null)
            {
                var properties = key.Assigment.GetType().GetProperties();
                var t = new string[]
                {
                RegisteredCustomizedFields.STS,
                RegisteredCustomizedFields.SSITS,
                RegisteredCustomizedFields.SITS,
                RegisteredCustomizedFields.UTS
                };

                foreach (var p in properties)
                {
                    if (!t.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var val = p.GetValue(key.Assigment, null).ToString();
                    // remove prefix and suffix
                    if (val.Contains(Constants.GlobalValuePrefix))
                    {
                        val = val.Substring(val.IndexOf(Constants.GlobalValuePrefix) + 1);
                        val = val.Substring(0, val.IndexOf(Constants.GlobalValueSuffix));
                    }

                    if (val.StartsWith("No", StringComparison.OrdinalIgnoreCase) || val.Equals("self", StringComparison.OrdinalIgnoreCase))
                    {
                        noTest = true;
                    }
                }
            }

            // if key is deferred or terminated dont check the test case.
            if ((key.Rearranged == RearrangedStatus.Deferred || key.Rearranged == RearrangedStatus.Terminated) || noTest)
            {
                Type = VerificationResultType.Succeed;
            }
            else if (key.TestedByIds.Length > 0)
            {
                Type = VerificationResultType.Succeed;
            }
            else
            {
                Type = VerificationResultType.Failed;
            }

            return this;
        }
    }
}
