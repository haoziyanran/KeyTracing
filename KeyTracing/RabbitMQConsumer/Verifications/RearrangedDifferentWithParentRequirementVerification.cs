// -----------------------------------------------------------------------
// <copyright file="RearrangedDifferentWithParentRequirementVerification.cs" company="">
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
    public class RearrangedDifferentWithParentRequirementVerification : Verification
    {
        public override int Id
        {
            get { throw new NotImplementedException(); }
        }

        public override string Title
        {
            get { throw new NotImplementedException(); }
        }

        public override string Description
        {
            get { throw new NotImplementedException(); }
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

            throw new NotImplementedException();
            ///1）	当需求被设置成terminated或deferred时，如果还没有下级需求或者是test case与之关联，
            ///那么就不再需要进行垂直与水平追踪（即不会对于no traced to, no test进行报错）。 
            ///当需求被设置成terminated或deferred时，所有关联的test case 和子级需求如果存在，
            ///必须是terminated或deferred的状态。如果不是则报错。
            ///如下例中，SSFS49136已终止，同时没有test case与之关联，此时不需要报错。
            //if (key.Rearranged == RearrangedStatus.Terminated || key.Rearranged == RearrangedStatus.Deferred)
            //{

            //}
        }
    }
}
