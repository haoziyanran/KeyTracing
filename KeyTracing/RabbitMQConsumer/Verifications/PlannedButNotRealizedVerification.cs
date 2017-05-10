// -----------------------------------------------------------------------
// <copyright file="PlannedButNotRealizedVerification.cs" company="">
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
    public class PlannedButNotRealizedVerification : Verification
    {
        public PlannedButNotRealizedVerification()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(165, 42, 42);
        }

        public override int Id
        {
            get { return int.Parse(VModelVerificationResult.PlannedButNotRealizedVerificationId); }
        }

        public override string Title
        {
            get { return VModelVerificationResult.PlannedButNotRealizedVerificationTitle; }
        }

        public override string Description
        {
            get { return VModelVerificationResult.PlannedButNotRealizedVerificationDescription; }
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
        /// The key assignment is planned, it's traced to items must realization. 
        /// these location contains the key assigment value.
        /// </summary>
        /// <param name="baseKey"></param>
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

            if (IsRealization(key))
            {
                Type = VerificationResultType.Succeed;
            }
            else
            {
                Type = VerificationResultType.Failed;
            }

            return this;
        }

        private bool IsRealization(VKey key)
        {
            // the key type is test case 
            if (key.Assigment == null)
            {
                return true;
            }

            var assignmentProperties = key.Assigment.GetType().GetProperties();

            var isValid = true;

            foreach (var p in assignmentProperties)
            {
                var val = p.GetValue(key.Assigment, null).ToString();

                string[] planned = val.Split(Constants.GlobalSplitChar)
                           .Select(x => x.TrimStart(Constants.GlobalValuePrefix).TrimEnd(Constants.GlobalValueSuffix))
                           .ToArray();

                // when the assignment is start with No, its tracing is end.
                // both of vertical and horizontal tracing.
                if (planned.Length == 1
                    && (val.StartsWith("No", StringComparison.OrdinalIgnoreCase) || val.Equals("self", StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var v = new string[] { 
                    RegisteredCustomizedFields.SFS,
                    RegisteredCustomizedFields.SSFS,
                    RegisteredCustomizedFields.DS
                };

                var h = new string[] {
                    RegisteredCustomizedFields.STS,
                    RegisteredCustomizedFields.SITS,
                    RegisteredCustomizedFields.SSITS,
                    RegisteredCustomizedFields.UTS
                };

                // for vertical
                if (v.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                {
                    foreach (var k in key.TracedToCollection)
                    {
                        var location = k.Location.Split(Constants.GlobalUnderline)[0];
                        if (!Contains(location, planned))
                        {
                            isValid = false;
                        }
                    }
                }

                //FIXME: for horizontal
                // key should be assigned to one or more test case.
                //if (h.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                //{
                //    foreach (var k in key.TestedByCollection)
                //    {
                //        var location = k.Location.Remove(k.Location.IndexOf(p.Name), p.Name.Length);
                //        location = location.Remove(location.IndexOf(Constants.GlobalUnderline), 1);

                //        if (!Contains(location, planned))
                //        {
                //            isValid = false;
                //        }
                //    }
                //}
            }

            return isValid;
        }

        private bool Contains(string str, IEnumerable<string> collection)
        {
            foreach (var s in collection)
            {
                var newS = s;
                if (s.Contains(Constants.GlobalUnderline))
                {
                    newS = s.Split(Constants.GlobalUnderline)[0];
                }

                if (newS.ToUpper().Contains(str.ToUpper()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
