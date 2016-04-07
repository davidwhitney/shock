using System;
using System.Reflection;

namespace Shock.Execution
{
    public class TaskStatus
    {
        public MethodInfo Method { get; set; }
        public bool ExecutedSuccessfully { get; set; }
        public Exception Exception { get; set; }
        public int? ReturnCode { get; set; }

        public TaskStatus(MethodInfo method, bool executedSuccessfully = true, Exception ex = null)
        {
            Method = method;
            ExecutedSuccessfully = executedSuccessfully;
            Exception = ex;
        }
    }
}