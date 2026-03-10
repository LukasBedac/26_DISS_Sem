using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MonteCarlo.Simulations
{
    /// <summary>
    /// Path: Z-D-S-R-Z
    /// </summary>
    public class Sim4 : BaseCore
    {
        public Sim4(int replications) : base(replications) 
        {

        }

        public override void OneReplication()
        {
            //Zilina - Divinka
            _path2 = CalculatePathTime(4, color: GREEN);
            _path1 = CalculatePathTime(4, color: RED);
            _path3 = CalculatePathTime(2, color: BLACK) + CalculatePathTime(2, color: RED, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Divinka - Strecno
            _path3 = CalculatePathTime(2, color: RED) + CalculatePathTime(4, color: BLUE, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Strecno - Rajecke Teplice
            _path1 = CalculatePathTime(5, color: BLUE) + CalculatePathTime(8, color: BLUE);
            _path2 = CalculatePathTime(5, color: BLACK) + CalculatePathTime(8, color: BLUE);
            _path3 = CalculatePathTime(4, color: BLUE) + CalculatePathTime(2, color: GREEN, true); //K
            CalculateBestPathTime();
            ResetPaths();

            //Rajecke Teplice - Zilina
            _path3 = CalculatePathTime(2, color: GREEN) + CalculatePathTime(2, color: BLACK, true); //K
            CalculateBestPathTime();
            ResetPaths();
        }
    }
}
