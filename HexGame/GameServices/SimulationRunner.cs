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
            int wins = 0;
            var winsLock = new object();

            Parallel.For(0, Repetitions, i =>
            {
                var playerStarting = (PlayerEnum)(i % 2);
                var newAlgorithm1 = Algorithm1.Copy(i * seed);
                var newAlgorithm2 = Algorithm2.Copy(i * seed);
                var gameState = new GameState();

                var result = RunSimulation(gameState, newAlgorithm1, newAlgorithm2, playerStarting);

                if (playerStarting == PlayerEnum.Red && result == GameResultEnum.RedVictory ||
                    playerStarting == PlayerEnum.Blue && result == GameResultEnum.BlueVictory)
                {
                    lock (winsLock)
                    {
                        wins++;
                    }
                }

            });

            return wins;
        }

        private static GameResultEnum RunSimulation(GameState gameState, IAlgorithm algorithm1, IAlgorithm algorithm2, PlayerEnum playerStarting)
        {
            GameResultEnum result;

            if (playerStarting == PlayerEnum.Red)
            {
                while (true)
                {
                    result = gameState.GetGameResult();

                    if (result != GameResultEnum.InconclusiveYet) break;

                    var move1 = algorithm1.CalculateNextMove(gameState, playerStarting);
                    gameState.PerformMove(move1);

                    var move2 = algorithm2.CalculateNextMove(gameState, playerStarting);
                    gameState.PerformMove(move2);
                }
            }
            else
            {
                while (true)
                {
                    result = gameState.GetGameResult();

                    if (result != GameResultEnum.InconclusiveYet) break;

                    var move1 = algorithm2.CalculateNextMove(gameState, playerStarting);
                    gameState.PerformMove(move1);

                    var move2 = algorithm1.CalculateNextMove(gameState, playerStarting);
                    gameState.PerformMove(move2);

                }

            }

            return result;
        }



    }
}
