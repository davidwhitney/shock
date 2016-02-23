using System;
using System.Collections.Generic;

namespace Shock.Discovery
{
    public interface ILoadTaskSources
    {
        AppDomain LoadDetectedSourcesFrom(List<string> args);
    }
}
