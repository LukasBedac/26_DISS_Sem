using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    /// <summary>
    /// Path: Z-R-S-D-Z
    /// </summary>
    public class Sim6 : BaseCore
    {
        public Sim6(int replications) : base(replications) 
        {

        }

        public override void OneReplication()
        {
            //Zilina - Rajecke Teplice
            _path3 = CalculatePathTime(2, color: BLACK) + CalculatePathTime(2, color: GREEN, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Rajecke Teplice - Strecno
            _path1 = CalculatePathTime(8, color: BLUE) + CalculatePathTime(5, color: BLUE);
            _path2 = CalculatePathTime(8, color: BLUE) + CalculatePathTime(5, color: BLACK);
            _path3 = CalculatePathTime(2, color: GREEN) + CalculatePathTime(4, color: BLUE, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Strecno - Divinka
            _path3 = CalculatePathTime(4, color: BLUE) + CalculatePathTime(2, color: RED, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Divinka - Zilina
            _path1 = CalculatePathTime(4, color: GREEN);
            _path2 = CalculatePathTime(4, color: RED);
            _path3 = CalculatePathTime(2, color: RED) + CalculatePathTime(2, color: BLACK, true); //K
            CalculateBestPathTime();
            ResetPaths();
        }
    }
}
