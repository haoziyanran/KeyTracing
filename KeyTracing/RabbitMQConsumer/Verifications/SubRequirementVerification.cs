// -----------------------------------------------------------------------
// <copyright file="SubRequirementVerification.cs" company="">
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
    public class SubRequirementVerification : Verification
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public SubRequirementVerification()
        {
            BackgroundColor = System.Drawing.Color.Lavender;
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.SubRequirementVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.SubRequirementVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.SubRequirementVerificationDescription; }
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
            //
            if (key.Rearranged == RearrangedStatus.Terminated || key.Rearranged == RearrangedStatus.Deferred)
            {
                Type = VerificationResultType.Succeed;
                return this;
            }

            /// Traced to verification
            if (key.Type == VKeyTypes.DS)
            {
                // DS has nothing traced to
                Type = VerificationResultType.Succeed;
            }
            else if (key.Type == VKeyTypes.RS)
            {
                RSAssignment assignment = key.Assigment as RSAssignment;
                if (assignment == null)
                {
                    logger.Warn("key {0} assignment is null.", key.Id);
                }

                // RS has 1-n SFS
                if (key.TracedToIds.Length >= 1)
                {
                    Type = VerificationResultType.Succeed;
                }
                else
                {
                    Type = VerificationResultType.Failed;
                }
            }
            else if (key.Type == VKeyTypes.SFS)
            {
                SFSAssignment assignment = key.Assigment as SFSAssignment;
                if (assignment == null)
                {
                    logger.Warn("key {0} assignment is null.", key.Id);
                }
                else
                {
                    if (Constants.Self.Contains(assignment.SSFSStrip))
                    {
                        Type = VerificationResultType.Succeed;
                    }
                    else
                    {
                        // SFS has 1-n SSFS
                        if (key.TracedToIds.Length >= 1)
                        {
                            Type = VerificationResultType.Succeed;
                        }
                        else
                        {
                            Type = VerificationResultType.Failed;
                        }
                    }
                }
            }
            else if (key.Type == VKeyTypes.SSFS)
            {
                SSFSAssignment assignment = key.Assigment as SSFSAssignment;
                if (assignment == null)
                {
                    logger.Warn("key {0} assignment is null.", key.Id);
                }
                else if (Constants.NoDS.Contains(assignment.DSStrip))
                {
                    Type = VerificationResultType.Succeed;
                }
                else
                {
                    // SSFS has 0-n DS
                    if (key.TracedToIds.Length >= 0)
                    {
                        Type = VerificationResultType.Succeed;
                    }
                    else
                    {
                        Type = VerificationResultType.Failed;
                    }
                }
            }

            return this;
        }
    }
}
