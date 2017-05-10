// -----------------------------------------------------------------------
// <copyright file="RealizationButNotPlannedVerification.cs" company="">
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
    public class RealizationButNotPlannedVerification : Verification
    {
        public RealizationButNotPlannedVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(0, 100, 0);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.RealizationButNotPlannedVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.RealizationButNotPlannedVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.RealizationButNotPlannedVerificationDescription; }
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

            if (key.IsDeferredOrTerminated)
            {
                Type = VerificationResultType.Succeed;
                return this;
            }

            if (IsPlanned(key))
            {
                Type = VerificationResultType.Succeed;
            }
            else
            {
                Type = VerificationResultType.Failed;
            }

            return this;
        }

        private bool IsPlanned(VKey key)
        {
            var location = key.Location.Split(Constants.GlobalUnderline)[0];

            var isValid = false;

            foreach (var k in key.TracedFromCollection)
            {
                // the key type is test case?
                if (k.Assigment == null)
                {
                    continue;
                }
                //FIXME: 在不同项目中，上下级的名称会不一致， 比如有的是 XXX_SSIT, 而有的又是 SSIT_XXX.
                var assignmentProperties = k.Assigment.GetType().GetProperties();
                foreach (var p in assignmentProperties)
                {
                    var planned = p.GetValue(k.Assigment, null).ToString().Split(Constants.GlobalSplitChar)
                       .Select(x => x.TrimStart(Constants.GlobalValuePrefix).TrimEnd(Constants.GlobalValueSuffix)).ToArray();

                    foreach (var s in planned)
                    {
                        if (s.Contains(location))
                        {
                            isValid = true;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        break;
                    }
                }

                if (isValid)
                {
                    break;
                }
            }

            return isValid;
        }
    }
}
