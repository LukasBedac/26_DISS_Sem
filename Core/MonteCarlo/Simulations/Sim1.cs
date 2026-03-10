using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    /// <summary>
    /// Path: Z-S-R-D-Z
    /// </summary>
    public class Sim1 : BaseCore
    {
        public Sim1(int replications) : base(replications)
        {
        }

        public override void OneReplication()
        {
            //Zilina - Divinka
            _path1 = CalculatePathTime(4, color: GREEN);
            _path2 = CalculatePathTime(4, color: RED);
            _path3 = CalculatePathTime(2, color: BLACK) + CalculatePathTime(2, color: RED, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Divinka-Rajecke Teplice
            _path1 = CalculatePathTime(1, color: BLACK) + CalculatePathTime(3, color: RED) + CalculatePathTime(1, color: BLUE);
            _path2 = CalculatePathTime(1, color: BLACK) + CalculatePathTime(1, color: BLUE) + CalculatePathTime(2, color: RED) + CalculatePathTime(1, color: BLUE);
            _path3 = CalculatePathTime(2, color: RED) + CalculatePathTime(2, color: GREEN, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Rajecke Teplice - Strecno
            _path1 = CalculatePathTime(8, color: BLUE) + CalculatePathTime(5, color: BLUE);
            _path2 = CalculatePathTime(8, color: BLUE) + CalculatePathTime(5, color: BLACK);
            _path3 = CalculatePathTime(2, color: GREEN) + CalculatePathTime(4, color: BLUE, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Strecno - Zilina
            _path1 = CalculatePathTime(3, color: BLACK) + CalculatePathTime(4, color: GREEN);
            _path2 = CalculatePathTime(4, color: RED) + CalculatePathTime(3, color: RED);
            _path3 = CalculatePathTime(4, color: BLUE) + CalculatePathTime(2, color: BLACK, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Console.WriteLine($"Actual time: {ActualTime}");
        }
    }
}
