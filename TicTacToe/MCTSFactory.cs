using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    enum MCTSType { Original, Modified };

    static class MCTSFactory
    {
        public static IMCTS CreateMCTS(MCTSType type, Board board, int simulationsNumber, bool considerDrawAsWin)
        {
            switch(type)
            {
                case MCTSType.Original:
                    return new OriginalMCTS(board, simulationsNumber, considerDrawAsWin);
                case MCTSType.Modified:
                    return new ModifiedMCTS(board, simulationsNumber, considerDrawAsWin);
                default:
                    throw new Exception($"Error: there is no MCTS implementation for type \"{type}\".");
            }
        }
    }
}
