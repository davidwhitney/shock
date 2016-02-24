using System;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.Execution
{
    public class DefaultTaskExecutor : IExecuteATask
    {
        public TaskStatus TryExecuteTask(MethodInfo method, Arguments args)
        {
            try
            {
                var instance = Activator.CreateInstance(method.DeclaringType);
                method.Invoke(instance, null);
                return new TaskStatus(method);
            }
            catch (Exception ex)
            {
                return new TaskStatus(method, false, ex);
            }
        }
    }
}