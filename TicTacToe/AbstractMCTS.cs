using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    abstract class AbstractMCTS: IMCTS
    {
        // Interface's members
        public int SimulationsNumber { get; set; }
        public bool ConsiderDrawAsWin { get; set; }

        /// <summary>
        /// The root of the Monte Carlo tree. 
        /// It's preserved to be able to restart the game at any point without losing the knowledge gained by the simulations.
        /// It should contain a board with all the cells unoccupied.
        /// </summary>
        protected MCTNode _root;

        /// <summary>
        /// The node containing the current state of the game
        /// </summary>
        protected MCTNode _currentNode;

        /// <summary>
        /// The path to the current node. 
        /// It is used to update the statistics of the nodes in the list
        /// with the data from simulations started from nodes from one of the subtrees
        /// having one of their children as roots.
        /// It is useful when restarting the game without reseting the tree.
        /// </summary>
        protected List<MCTNode> _pathToCurrent = new List<MCTNode>();

        protected Random _random = new Random();

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="board"></param>
        /// <param name="simulationsNumber"></param>
        /// <param name="simulateEveryMove"></param>
        /// <param name="considerDrawAsWin"></param>
        public AbstractMCTS(Board board, int simulationsNumber, bool considerDrawAsWin)
        {
            _root = new MCTNode(board, CellOccupier.Computer);
            _pathToCurrent.Add(_root);
            _currentNode = _root;

            SimulationsNumber = simulationsNumber;
            ConsiderDrawAsWin = considerDrawAsWin;
        }

        /// <summary>
        /// <para>Function that resets the current node of the Monte Carlo tree (assigns the root of the tree to it).</para>
        /// <para>It should be used when restarting a game and not wanting to lose the previously gained knowledge.</para>
        /// </summary>
        public void Restart()
        {
            _currentNode = _root;
            _pathToCurrent.Clear();
            _pathToCurrent.Add(_root);
        }

        /// <summary>
        /// The method that resets the accumulated knowledge, destroying the Monte Carlo tree.
        /// </summary>
        public void ResetTree()
        {
            _root = new MCTNode(_root.Board, CellOccupier.Computer);
        }

        /// <summary>
        /// <para>The function that requests the computer to make a move (occupy a cell).</para>
        /// <para>It has side effects on the Monte Carlo tree (modifies the current node)</para>
        /// </summary>
        /// <param name="board">The state of the game (after the player moved)</param>
        /// <returns>The board (game state) after the computer made its move</returns>
        public abstract Board Occupy(Board board);
    }
}
