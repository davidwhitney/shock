using System;
using Shock.Conventions;

namespace Shock
{
    public class DefibrillatorFactory
    {
        public static ActiveConventions Conventions { get; set; }

        public DefibrillatorFactory(ActiveConventions conventions)
        {
           Conventions = conventions;
        }

        public Defibrillator Manufacture()
        {
            return new Defibrillator(
                Conventions.TaskDiscoverer,
                Conventions.TaskSelector,
                Conventions.TaskRunner,
                Conventions.Output);

            // Detect execution environment and Conventions
            // Can we see log4net? nlog? If not, console + diagnostics writers
            throw new NotImplementedException();
        }
    }
}
