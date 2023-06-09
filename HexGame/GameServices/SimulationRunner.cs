using HexGame.Engine;
using HexGame.Enums;
using HexGame.Models;
using System.Threading.Tasks;

namespace HexGame.GameServices
{
    internal class SimulationRunner
    {
        private readonly IAlgorithm Algorithm1;
        private readonly IAlgorithm Algorithm2;
        private readonly int Repetitions;

        public SimulationRunner(IAlgorithm algorithm1, IAlgorithm algorithm2, int repetitions)
        {
            Algorithm1 = algorithm1;
            Algorithm2 = algorithm2;
            Repetitions = repetitions;
        }

        public int RunSimulations(int seed)
        {
            int firstAlgorithmWins = 0;
            var winsLock = new object();

            Parallel.For(0, Repetitions, i =>
            {  
                var newAlgorithm1 = Algorithm1.Copy(i * seed);
                var newAlgorithm2 = Algorithm2.Copy(i * seed);
                

                // każdy algorytm zaczyna w połowie gier
                if(i % 2 == 0)
                {
                    var result = RunSimulation(newAlgorithm1, newAlgorithm2);

                    if(result == GameResultEnum.RedVictory)
                    {
                        lock(winsLock)
                        {
                            ++firstAlgorithmWins;
                        }
                    }
                }
                else
                {
                    var result = RunSimulation(newAlgorithm2, newAlgorithm1);

                    if (result == GameResultEnum.BlueVictory)
                    {
                        lock (winsLock)
                        {
                            ++firstAlgorithmWins;
                        }
                    }
                }

            });

            return firstAlgorithmWins;
        }

        private static GameResultEnum RunSimulation(IAlgorithm algorithm1, IAlgorithm algorithm2)
        {
            var gameState = new GameState();
            GameResultEnum result;

            while (true)
            {
                result = gameState.GetGameResult();

                if (result != GameResultEnum.InconclusiveYet) break;

                var move1 = algorithm1.CalculateNextMove(gameState, PlayerEnum.Red);
                gameState.PerformMove(move1);

                var move2 = algorithm2.CalculateNextMove(gameState, PlayerEnum.Blue);
                gameState.PerformMove(move2);
            }

            return result;
        }



    }
}
