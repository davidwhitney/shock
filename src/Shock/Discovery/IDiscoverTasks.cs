using System;
using System.Collections.Generic;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.Discovery
{
    public interface IDiscoverTasks
    {
        List<MethodInfo> FindTasks(Arguments args);
    }
}
