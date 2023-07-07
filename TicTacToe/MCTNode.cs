using System;
using System.Collections.Generic;
using System.Linq;
using FunCs;
using static System.Linq.Enumerable;

namespace TicTacToe
{
    /// <summary>
    /// The class that encapsulates the logic of a Monte Carlo tree node
    /// </summary>
    class MCTNode
    {
        /// <summary>
        /// The weight that adjusts how important should 
        /// exporing the Monte Carlo tree (exploration) be
        /// in the detriment of traversing the best path yet found (greediness).
        /// </summary>
        private static double EXPLORATION_WEIGHT = 0.5;

        /// <summary>
        /// The node's data (Winner, Children) is comuted lazily. 
        /// This attribute is true if the data was already computed, false otherwise.
        /// </summary>
        private bool _nodeInfoComputed = false;

        /// <summary>
        /// The winner of the board encapsulated by the node. Note: lazily computed (at the first use of the "Children" property)
        /// </summary>
        private Winner _winner;

        /// <summary>
        /// The player that made the last move (that created the board encapsulated in this node).
        /// All of this node's children occupiers will be the other player.
        /// </summary>
        private CellOccupier _occupier;

        /// <summary>
        /// The children of the node. Note: lazily computed (at the first use of the "Children" property)
        /// </summary>
        private List<MCTNode> _children;

        /// <summary>
        /// The property for accessing node's children.
        /// </summary>
        public List<MCTNode> Children
        {
            get
            {
                // Lazy computation
                if (!_nodeInfoComputed)
                    ComputeNodeInfo();
                return _children;
            }

            private set
            {
                _children = value;
            }
        }

        /// <summary>
        /// The property that accesses the winner of the board encapsulated by the node
        /// </summary>
        public Winner Winner
        {
            get
            {
                // Lazy computation
                if (!_nodeInfoComputed)
                    ComputeNodeInfo();
                return _winner;
            }
            private set {
                _winner = value;
            }
        }

        /// <summary>
        /// The number of all the playouts/rollouts that traversed this node
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The number of the playouts/rollouts that were wins and traversed this node
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// The boad (game state) encapsulated by the node
        /// </summary>
        public Board Board { get; private set; }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="board"></param>
        /// <param name="occupier">
        /// The player that made the last move (that created the board encapsulated in this node).
        /// All of this node's children occupiers will be the other player.
        /// </param>
        public MCTNode(Board board, CellOccupier occupier)
        {
            Board = board;
            Total = Wins = 0;
            _occupier = occupier;
        }

        /// <summary>
        /// <para>The function that selects one of the children using the UCB1 (Upper Confidence Bound) algorithm.</para> 
        /// <para>Computes the lazily computed members of the node if not already computed.</para>
        /// </summary>
        /// <returns>The selected child if there are any children, null otherwise</returns>
        public MCTNode Selection()
        {
            // Lazy computation
            if (!_nodeInfoComputed)
                ComputeNodeInfo();

            if (Children == null || Children.Count == 0)
                return null;

            // The selected node. 
            // Initialization is made just to because the application wouldn't compile
            // because it would think that "next" could be not initialized in the end.
            MCTNode next = Children[0]; 

            double maxWeight = double.MinValue;
            double actualWeight;

            Children
                .Filter(c => c.Total > 0)
                .ToList()
                .ForEach(c =>
                {
                    actualWeight = ComputeChildWeight(c);
                    if (actualWeight > maxWeight)
                    {
                        maxWeight = actualWeight;
                        next = c;
                    }
                });

            return next;
        }

        /// <summary>
        /// Function that computes the values of the lazily computed attributes.
        /// </summary>
        private void ComputeNodeInfo()
        {
            _nodeInfoComputed = true;
            CellOccupier nextOccupier = _occupier == CellOccupier.Computer ? CellOccupier.Player : CellOccupier.Computer;
            Winner = Board.GetWinner();
            if (Winner == Winner.NotFinishedYet)
                Children = Board
                    .OccupiableCells() // For each occupiable cell
                    .Map(c => Board.Occupy(c.X, c.Y, nextOccupier)) // Create a new board with the cell occupied by the next player
                    .Map(b => new MCTNode(b, nextOccupier)) // For each new board create a new node encapsulating it
                    .ToList();
        }

        /// <summary>
        /// Function that checks if the node has the board given as parameter using the Board.IsEqual function.
        /// </summary>
        /// <param name="board">The board</param>
        /// <returns></returns>
        public bool HasBoard(Board board)
        {
            return board.IsEqual(Board);
        }

        /// <summary>
        /// <para>Function that computes the child's weigth using the UCB1 (Upper Confidence Bound) formula.</para>
        /// <para>Note: the attributes of both the current node and the node of the child are used in the process.</para>
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        private double ComputeChildWeight(MCTNode child)
        {
            return (double)child.Wins / child.Total + Math.Sqrt(EXPLORATION_WEIGHT * Math.Log(Total) / child.Total);
        }

        // NEW
        public List<MCTNode> ExploredChildren()
        {
            return Children.Filter(c => c.Total > 0).ToList();
        }

        public MCTNode GetWinnerChild(bool considerDrawAsWin)
        {
            if (Children == null)
                return null;
            return Children.Find(c => 
                c.Winner == Winner.Computer || 
                considerDrawAsWin && c.Winner == Winner.NoOne);
        }

        public MCTNode Copy()
        {
            return new MCTNode(Board, _occupier);
        }
    }
}
