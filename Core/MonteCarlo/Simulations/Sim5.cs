using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    /// <summary>
    /// Path: Z-R-D-S-Z
    /// </summary>
    public class Sim5 : BaseCore
    {
        public Sim5(int replications) : base(replications) 
        {

        }

        public override void OneReplication()
        {
            //Zilina - Rajecke Teplice
            _path3 = CalculatePathTime(2, color: BLACK) + CalculatePathTime(2, color: GREEN, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Rajecke Teplice - Divinka
            _path1 = CalculatePathTime(1, color: BLUE) + CalculatePathTime(3, color: RED) + CalculatePathTime(1, color: BLACK);
            _path2 = CalculatePathTime(1, color: BLUE) + CalculatePathTime(2, color: RED) + CalculatePathTime(1, color: BLUE) + CalculatePathTime(1, color: BLACK);
            _path3 = CalculatePathTime(2, color: GREEN) + CalculatePathTime(2, color: RED, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Divinka - Strecno
            _path3 = CalculatePathTime(2, color: RED) + CalculatePathTime(4, color: BLUE, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Strecno - Zilina
            _path1 = CalculatePathTime(4, color: RED) + CalculatePathTime(3, color: RED);
            _path2 = CalculatePathTime(3, color: BLACK) + CalculatePathTime(4, color: GREEN);
            _path3 = CalculatePathTime(4, color: BLUE) + CalculatePathTime(2, color: BLACK, true); //K
            CalculateBestPathTime();
            ResetPaths();
        }
    }
}
