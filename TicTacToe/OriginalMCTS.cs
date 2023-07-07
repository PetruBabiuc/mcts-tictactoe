using FunCs;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class OriginalMCTS : AbstractMCTS
    {
        public OriginalMCTS(Board board, int simulationsNumber, bool considerDrawAsWin): 
            base(board, simulationsNumber, considerDrawAsWin) { }

        public override Board Occupy(Board board)
        {
            // Sets the current node the node that has the board created by
            // the player's last move
            _currentNode = _currentNode.Children.Find(c => c.HasBoard(board));
            _pathToCurrent.Add(_currentNode);

            // The path to the selected node. 
            // It is used in the backpropagation phase to update the tree. 
            List<MCTNode> pathToSelected = new List<MCTNode>();
            bool isWin;
            MCTNode node;

            for (int i = 0; i < SimulationsNumber; ++i)
            {
                // Selecting a node
                node = Selection(_currentNode, pathToSelected);

                // If the node doesn't contain a finished game
                // one of its children can be expanded.
                if (!NodeContainsFinishedGame(node))
                {
                    // The first selected node (just after player's first action)
                    // is unexplored and stats should be added to it.
                    if (node.Total == 0)
                        node = Expand(node);
                    isWin = Simulate(node);
                }
                else
                {
                    isWin = NodeContainsWin(node);
                }

                // Backpropagate simulation's results.
                Backpropagate(pathToSelected, isWin);
            }

            // If computer can win from the current state, the winning
            // action is chosen. If not, the most explored node is chosen.
            node = _currentNode.GetWinnerChild(ConsiderDrawAsWin);
            if (node == null)
                _currentNode = _currentNode.Children.MaxBy(c => c.Total);
            else
                _currentNode = node;

            // Adding the current node to the path to the next node.
            _pathToCurrent.Add(_currentNode);

            return _currentNode.Board;
        }

        private bool NodeContainsWin(MCTNode node)
        {
            return node.Winner == Winner.Computer ||
                ConsiderDrawAsWin && node.Winner == Winner.NoOne;
        }

        private bool NodeContainsFinishedGame(MCTNode node)
        {
            return node.Winner == Winner.Player || 
                node.Winner == Winner.Computer || 
                node.Winner == Winner.NoOne;
        }

        /// <summary>
        /// Function that encapsulates the logic of one of the MCTS algorithm, the selection.
        /// <para></para>
        /// It computes the selected node using the UCB1 formula.
        /// </summary>
        /// <param name="node">The root of the tree from which the selected node is computed</param>
        /// <param name="pathToSelected">The path to the selected node (used to update the tree) after function's return</param>
        /// <returns>The selected node</returns>
        private MCTNode Selection(MCTNode node, List<MCTNode> pathToSelected)
        {
            MCTNode selectedNode;
            List<Tuple<MCTNode, double>> nodeToWeight;
            double maxWeight;

            pathToSelected.Clear();
            selectedNode = node;

            // While the current node is explored and it doesn't contain a finished game
            while (selectedNode.Total > 0 && !NodeContainsFinishedGame(selectedNode))
            {
                // Creating a list of pairs containing current node's 
                // children and their associated weights
                nodeToWeight = selectedNode.Children
                    .Map(c => Tuple.Create(c, ComputeChildWeight(selectedNode, c)))
                    .ToList();
                maxWeight = nodeToWeight.Max(t => t.Item2);
                nodeToWeight = nodeToWeight
                    .Filter(t => t.Item2 == maxWeight)
                    .ToList();
                selectedNode = nodeToWeight[_random.Next(nodeToWeight.Count)].Item1;
                pathToSelected.Add(selectedNode);
            }

            return selectedNode;
        }

        /// <summary>
        /// The function that incapsulates the logic of the Expansion phase.
        /// <para></para>
        /// Returns a
        /// </summary>
        /// <param name="node">The parent of the node that will be expanded</param>
        /// <returns>The node that will be updated through simulation</returns>
        private MCTNode Expand(MCTNode node)
        {
            var notExpandedChildren = node.Children
                .Filter(c => c.Total == 0)
                .ToList();
            int index = _random.Next(notExpandedChildren.Count);
            return notExpandedChildren[index];
        }

        /// <summary>
        /// The function that incapsulates the logic of the Simulation phase.
        /// <para></para>
        /// It simulates that the player and the computer take random actions until the game is finished.
        /// </summary>
        /// <param name="node">Node from wich the simulation starts</param>
        /// <returns>True if the computer won</returns>
        private bool Simulate(MCTNode node)
        {
            MCTNode newNode = node.Copy(); // Because the random moves are not saved
            while (!NodeContainsFinishedGame(newNode))
                newNode = newNode.Children[_random.Next(newNode.Children.Count)];
            return NodeContainsWin(newNode);
        }

        /// <summary>
        /// The function that incapsulates the logic of the Backpropagation phase.
        /// <para></para>
        /// Updates the tree, incrementing the total games and, 
        /// if necessary, the total wins of every node traversed.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="propagateWin"></param>
        private void Backpropagate(List<MCTNode> path, bool propagateWin)
        {
            _pathToCurrent.Concat(path).ForEach(n => {
                ++n.Total;
                if (propagateWin)
                    ++n.Wins;
            });
        }

        /// <summary>
        /// Function that computes a child's weight. It uses UCB1 formula.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>The weight</returns>
        private double ComputeChildWeight(MCTNode parent, MCTNode child)
        {
            // Win
            if (NodeContainsWin(child))
                return double.PositiveInfinity;
            // Lose
            else if (child.Winner == Winner.Player)
                return double.NegativeInfinity;
            // Node that should not be visited because the computer wins instantly from it
            // This gives other nodes the opportunity to be explored
            else if (child.GetWinnerChild(ConsiderDrawAsWin) != null)
                return double.NegativeInfinity;
            // Node that was created after expanding => infinite weight
            else if (child.Total == 0 && child.Wins == 0)
                return double.PositiveInfinity;
            // Weight computed by Upper Confidence Bound
            else
            {
                double result = (double)child.Wins / child.Total + Math.Sqrt(2 * Math.Log(parent.Total) / child.Total);
                return result;
            }
        }
    }
}
