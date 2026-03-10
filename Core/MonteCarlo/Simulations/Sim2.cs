using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    /// <summary>
    /// Path: Z-D-R-S-Z
    /// </summary>
    public class Sim2 : BaseCore
    {
        public Sim2(int replications) : base(replications) 
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

            //Strecno-Rajecke Teplice
            _path1 = CalculatePathTime(5, color: BLUE) + CalculatePathTime(8, color: BLUE);
            _path2 = CalculatePathTime(5, color: BLACK) + CalculatePathTime(8, color: BLUE);
            _path3 = CalculatePathTime(4, color: BLUE) + CalculatePathTime(2, color: GREEN, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Rajecke Teplice - Divinka
            _path1 = CalculatePathTime(1, color: BLUE) + CalculatePathTime(3, color: RED) + CalculatePathTime(1, color: BLACK);
            _path2 = CalculatePathTime(1, color: BLUE) + CalculatePathTime(2, color: RED) + CalculatePathTime(1, color: BLUE) + CalculatePathTime(1, color: BLACK);
            _path3 = CalculatePathTime(2, color: GREEN) + CalculatePathTime(2, color: RED, true); //K
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
