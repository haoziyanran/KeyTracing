// -----------------------------------------------------------------------
// <copyright file="DifferenttRearrangedVerification.cs" company="">
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
    public class DifferentRearrangedVerification : Verification
    {
        public DifferenttRearrangedVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(124, 252, 0);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.DifferenttRearrangedId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.DifferenttRearrangedTitle; }
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

        /// <summary>
        /// 需求的rearranged属性不为terminated或deferred时, 
        /// 与之关联的test case如果只有单个，且值为terminated或deferred时，
        /// 应当报错（说明这个需求没有被测试）。
        /// 如果与之关联的test case有不止一个，则不需要报错（说明即使某个用例终止或延迟，还有其它的用例来测试这条需求）
        /// </summary>
        /// <param name="baseKey"></param>
        /// <returns></returns>
        public override Verification Verify(VKeyBase baseKey)
        {
            VKey key = baseKey as VKey;
            Debug.Assert(key != null);

            Type = VerificationResultType.Succeed;

            if (!key.IsDeferredOrTerminated)
            {
                if (key.TestedByCollection.Length == 1)
                {
                    TestCase t = key.TestedByCollection[0];
                    if (t.IsDeferredOrTerminated)
                    {
                        Type = VerificationResultType.Failed;
                    }
                }
                else
                {
                    bool rst = false;
                    foreach (var t in key.TestedByCollection)
                    {
                        if (!t.IsDeferredOrTerminated)
                        {
                            rst = true;
                            break;
                        }
                    }

                    if (!rst)
                    {
                        Type = VerificationResultType.Failed;
                    }
                }
            }

            return this;
        }
    }
}
