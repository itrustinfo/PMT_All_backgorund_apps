using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementToolAutomation
{
    public class Constants
    {
        /// <summary>
        /// This dictionary is the mapping of current status and next status.
        /// This will be applicable for only step 5 of Works A.
        /// Here Key is current status and Value is next status.
        /// </summary>
        public static Dictionary<string, string> Step5CurentNextStatusMap = new Dictionary<string, string>
        {
            { "Recommended-Code A", "PC Recommended-Code A" },
            { "Recommended-Code B", "PC Recommended-Code B" },
            { "Recommended-Code C", "Code C" },
            { "Recommended-Code D", "Code D" },
        };
    }
}
