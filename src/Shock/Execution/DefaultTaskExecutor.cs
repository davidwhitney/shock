using System;
using System.Collections.Generic;
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
                var parameters = GetMethodParameters(method, args);
                method.Invoke(instance, parameters);
                return new TaskStatus(method);
            }
            catch (Exception ex)
            {
                return new TaskStatus(method, false, ex.InnerException);
            }
        }

        private static object[] GetMethodParameters(MethodInfo info, Arguments args)
        {
            var parameters = new List<object>();
            var parameterInfos = info.GetParameters();

            foreach (var paramInfo in parameterInfos)
            {
                object value;
                args.TryGetValue(paramInfo.Name, out value);

                var typedValue = Convert.ChangeType(value, paramInfo.ParameterType);
                parameters.Add(typedValue);
            }

            return parameters.ToArray();
        }
    }
}