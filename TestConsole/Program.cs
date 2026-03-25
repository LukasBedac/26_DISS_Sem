// See https://aka.ms/new-console-template for more information

using Core.Distributions;
using Core.Distributions.Helpers;
using Core.EventSimulation.TestSimulation;
using Core.MonteCarlo.Simulations;
using TestConsole.Distributiontest;

//DataGenerationTest dataGen = new DataGenerationTest();
//IntervalTest intervalTest = new IntervalTest();

//dataGen.GenerateDiscreteUniform();
//dataGen.GenerateContinuousUniform();
//dataGen.GenerateEmpiricDiscrete();
//dataGen.GenerateEmpiricContinuous();
//dataGen.GenerateTriangular();
//dataGen.GenerateExponential();

//intervalTest.CheckIntervals();

//Sim1 sim1 = new Sim1(100000);
//Sim2 sim2 = new Sim2(100000);
//Sim3 sim3 = new Sim3(100000);
//Sim4 sim4 = new Sim4(100000);
//Sim5 sim5 = new Sim5(100000);
//Sim6 sim6 = new Sim6(100000);

TestSimCore testSim = new TestSimCore(1);
testSim.Run();

