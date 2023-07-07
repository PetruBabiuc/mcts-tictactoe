using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    interface IMCTS
    {
        /// <summary>
        /// Property for managing the number of simulations.
        /// </summary>
        int SimulationsNumber { get; set; }

        /// <summary>
        /// In many games the computer doesn't necessary want to win,
        /// it just wants to reduce player's score. This property makes the computer
        /// consider a draw being a win making the game harder to win for the player.
        /// This property set to true reduces the greediness for win of the computer,
        /// making it "also think to stop the player from winning".
        /// </summary>
        bool ConsiderDrawAsWin { get; set; }

        /// <summary>
        /// <para>The function that requests the computer to make a move (occupy a cell).</para>
        /// <para>It has side effects on the Monte Carlo tree (modifies the current node)</para>
        /// </summary>
        /// <param name="board">The state of the game (after the player moved)</param>
        /// <returns>The board (game state) after the computer made its move</returns>
        Board Occupy(Board board);

        /// <summary>
        /// The method that resets the accumulated knowledge, destroying the Monte Carlo tree.
        /// </summary>
        void ResetTree();

        /// <summary>
        /// <para>Function that resets the current node of the Monte Carlo tree (assigns the root of the tree to it).</para>
        /// <para>It should be used when restarting a game and not wanting to lose the previously gained knowledge.</para>
        /// </summary>
        void Restart();
    }
}
