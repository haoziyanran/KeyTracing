// -----------------------------------------------------------------------
// <copyright file="WalkerException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Walker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WalkerException : Exception
    {
        public WalkerException()
            : base()
        {

        }

        public WalkerException(String message)
            : base(message)
        {

        }

        public WalkerException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
