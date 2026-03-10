using Core.Distributions;
using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Distributiontest
{
    internal class DataGenerationTest
    {
        private const int NumberOfGenerations = 500_000;
        private List<int> discreteValues = new List<int>();
        public void GenerateDiscreteUniform()
        {
            string path = "DiscreteUniform.csv";
            DiscreteUniform uni = new DiscreteUniform(55, 76);
            using (StreamWriter writer = new StreamWriter(path, append: false))
            { 
                for (int i = 0; i < NumberOfGenerations; i++)
                {
                    writer.WriteLine(uni.NextInt());
                }
                writer.Close();
            }
        }
        /// <summary>
        /// Generated using AI, [1 Vytvorenie testov pre generátory]
        /// </summary>
        public void GenerateContinuousUniform()
        {
            string path = "ContinuousUniform.csv";
            ContinuousUniform uni = new ContinuousUniform(50, 80);
            using (StreamWriter writer = new StreamWriter(path, append: false))
            {
                for (int i = 0; i < NumberOfGenerations; i++)
                {
                    writer.WriteLine(uni.NextDouble());
                }
                writer.Close();
            }
        }
        /// <summary>
        /// Generated using AI, [1 Vytvorenie testov pre generátory]
        /// </summary>
        public void GenerateEmpiricDiscrete()
        {
            string path = "EmpiricDiscrete.csv";
            var intervals = new List<Scope>
            {
                new Scope(15, 29, 0.2),
                new Scope(29, 45, 0.4),
                new Scope(45, 65, 0.4)
            };
            Empiric emp = new Empiric(isDiscrete: true, intervals);
            using (StreamWriter writer = new StreamWriter(path, append: false))
            {
                for (int i = 0; i < NumberOfGenerations; i++)
                {
                    writer.WriteLine(emp.NextInt());
                }
                writer.Close();
            }
        }
        /// <summary>
        /// Generated using AI, [1 Vytvorenie testov pre generátory]
        /// </summary>
        public void GenerateEmpiricContinuous()
        {
            string path = "EmpiricContinuous.csv";
            var intervals = new List<Scope>
            {
                new Scope(10, 20, 0.1),
                new Scope(20, 32, 0.5),
                new Scope(32, 45, 0.2),
                new Scope(45, 75, 0.15),
                new Scope(75, 85, 0.05)
            };
            Empiric emp = new Empiric(isDiscrete: false, intervals);
            using (StreamWriter writer = new StreamWriter(path, append: false))
            {
                for (int i = 0; i < NumberOfGenerations; i++)
                {
                    writer.WriteLine(emp.NextDouble());
                }
                writer.Close();
            }
        }
    }
}
