// -----------------------------------------------------------------------
// <copyright file="Verification.cs" company="">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class Verification
    {
        public abstract int Id { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract VerificationResultType Type { get; protected set; }
        public abstract Color BackgroundColor { get; protected set; }

        public abstract Verification Verify(VKeyBase baseKey);
    }
}
