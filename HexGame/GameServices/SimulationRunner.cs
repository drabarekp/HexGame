using HexGame.Engine;
using HexGame.Enums;
using System;

namespace HexGame.GameServices
{
    internal class SimulationRunner
    {
        private IAlgorithm Algorithm1;
        private IAlgorithm Algorithm2;
        private int Repetitions;

        public SimulationRunner(IAlgorithm algorithm1, IAlgorithm algorithm2, int repetitions)
        {
            Algorithm1 = algorithm1;
            Algorithm2 = algorithm2;
            Repetitions = repetitions;
        }

        public int RunSimulations()
        {
            int wins = 0;

            // TODO

            return wins;
        }

        private GameResultEnum RunSimulation()
        {
            throw new NotImplementedException();
        }



    }
}
