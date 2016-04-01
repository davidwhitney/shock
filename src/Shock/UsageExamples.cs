using System;
using System.Collections.Generic;
using System.Reflection;
using Shock.Logging;

namespace Shock
{
    public class UsageExamples
    {
        private readonly IOutput _output;

        public UsageExamples(IOutput output)
        {
            _output = output;
        }

        public void Basic()
        {
            _output.WriteLine("usages:");
            _output.WriteLine("\t shock (assemblies autoloaded, default tasks executed)");
            _output.WriteLine("\t shock task [options] (assemblies autoloaded, task executed)");
            _output.WriteLine("\t shock assembly.dll task [options]");
            _output.WriteLine("");
            _output.WriteLine("flags:");
            _output.WriteLine("\t verbose - Extra diagnostics");
            _output.WriteLine("\t help | ? - Usage examples and task list");
            _output.WriteLine("\t continue - Continue processing on single task errors");
            _output.WriteLine("\t interactive - Don't exit without user input");
        }

        public void Tasks(List<MethodInfo> allTasks)
        {
            Basic();

            _output.WriteLine("tasks:" + Environment.NewLine);
            foreach (var task in allTasks)
            {
                _output.WriteLine(
                    $"\t{task.DeclaringType.Namespace}.{task.DeclaringType.Name}.{task.Name}{Environment.NewLine}");
            }
        }
    }
}