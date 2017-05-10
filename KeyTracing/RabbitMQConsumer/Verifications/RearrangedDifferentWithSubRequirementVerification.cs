// -----------------------------------------------------------------------
// <copyright file="RearrangedDifferentWithSubRequirementVerification.cs" company="">
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
    public class RearrangedDifferentWithSubRequirementVerification : Verification
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
        }
    }
}
