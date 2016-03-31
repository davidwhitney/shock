using System;
using System.Collections.Generic;

namespace Shock.Conventions
{
    public interface IConventionDiscoverer
    {
        ActiveConventions AdjustConventions(ActiveConventions conventions, List<Type> allAvailableTypes);
    }
}