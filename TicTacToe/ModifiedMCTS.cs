using System;
using System.Linq;
using FunCs;
using System.Collections.Generic;

namespace TicTacToe
{
    class ModifiedMCTS: AbstractMCTS
    {
        public ModifiedMCTS(Board board, int simulationsNumber, bool considerDrawAsWin): 
            base(board, simulationsNumber, considerDrawAsWin) { }

        /// <summary>
        /// <para>The function that requests the computer to make a move (occupy a cell).</para>
        /// <para>It has side effects on the Monte Carlo tree (modifies the current node)</para>
        /// </summary>
        /// <param name="board">The state of the game (after the player moved)</param>
        /// <returns>The board (game state) after the computer made its move</returns>
        public override Board Occupy(Board board)
        {
            // Sets the current node the node that has the board created by
            // the player's last move
            _currentNode = _currentNode.Children.Find(c => c.HasBoard(board));

            // If the node is not explored or the property SimulateEveryMove is set
            // the current node is explored
            if (_currentNode.Winner == Winner.NotFinishedYet || _currentNode.Total == 0)
            {
                int wins = 0;
                // The current node is explored
                for (int i = 0; i < SimulationsNumber; ++i)
                    if (Simulate(_currentNode))
                        ++wins;
                // The nodes in the path traversed to the current node are updated
                // with stats from the simulations
                _pathToCurrent.ForEach(n => 
                {
                    n.Total += SimulationsNumber;
                    n.Wins += wins;
                });
            }

            // Adding the current node to the path to the next node
            _pathToCurrent.Add(_currentNode);
            _currentNode = _currentNode.Selection();
            _pathToCurrent.Add(_currentNode);

            return _currentNode.Board;
        }

        /// <summary>
        /// The recursive function that simulates a playout.
        /// </summary>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        private bool Simulate(MCTNode currentNode)
        {
            ++currentNode.Total;
            if (currentNode.Winner == Winner.Computer
                || (ConsiderDrawAsWin && currentNode.Winner == Winner.NoOne)
                )
            {
                ++currentNode.Wins;
                return true;
            }
            else if (currentNode.Winner == Winner.NotFinishedYet)
            {
                // In a playout, the explored path is chosen completly random.
                // If it would use heuristics/deterministic optimizations, the algoritm could be improved.
                int index = _random.Next(0, currentNode.Children.Count);
                MCTNode next = currentNode.Children[index];
                bool wonSimulation = Simulate(next);
                if (wonSimulation)
                    ++currentNode.Wins;
                return wonSimulation;
            }
            else // Winner == NoOne || Winner == Player
                return false;
        }
    }
}
