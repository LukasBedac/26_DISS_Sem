using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    /// <summary>
    /// Path: Z-S-D-R-Z
    /// </summary>
    public class Sim3 : BaseCore
    {
        public Sim3(int replications) : base(replications) 
        {
            
        }

        public override void OneReplication()
        {
            //Zilina - Strecno
            _path1 = CalculatePathTime(3, color: RED) + CalculatePathTime(4, color: RED);
            _path2 = CalculatePathTime(4, color: GREEN) + CalculatePathTime(3, color: BLACK);
            _path3 = CalculatePathTime(2, color: BLACK) + CalculatePathTime(4, color: BLUE, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Strecno - Divinka
            _path3 = CalculatePathTime(4, color: BLUE) + CalculatePathTime(2, color: RED, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Divinka - Rajecke Teplice
            _path1 = CalculatePathTime(1, color: BLACK) + CalculatePathTime(3, color: RED) + CalculatePathTime(1, color: BLUE);
            _path2 = CalculatePathTime(1, color: BLACK) + CalculatePathTime(1, color: BLUE) + CalculatePathTime(2, color: RED) + CalculatePathTime(1, color: BLUE);
            _path3 = CalculatePathTime(2, color: RED) + CalculatePathTime(2, color: GREEN, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Rajecke Teplice - Zilina
            _path3 = CalculatePathTime(2, color: GREEN) + CalculatePathTime(2, color: BLACK, true); //K
            CalculateBestPathTime();
            ResetPaths();
        }
    }
}
