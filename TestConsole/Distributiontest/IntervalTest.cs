using Core.Distributions;
using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Distributiontest
{
    internal class IntervalTest
    {
        public void CheckIntervals()
        {
            var intervals = new List<Scope>
                        {
                            new Scope(1, 2, 0.1),
                            new Scope(2, 3, 0.1),
                            new Scope(3, 4, 0.1),
                            new Scope(5, 6, 0.1),
                            new Scope(6, 7, 0.1),
                            new Scope(7, 8, 0.1),
                            new Scope(8, 9, 0.1),
                            new Scope(9, 10, 0.1),
                            new Scope(10, 11, 0.1),
                            new Scope(11, 12, 0.1)
                        };
            Empiric emp = new Empiric(isDiscrete: false, intervals);

            intervals = new List<Scope>
            {
                new Scope(10, 20, 0.1),
                new Scope(20, 32, 0.5),
                new Scope(32, 45, 0.2),
                new Scope(45, 75, 0.15),
                new Scope(75, 85, 0.05)
            };
            emp = new Empiric(isDiscrete: false, intervals);
        }
    }
}
