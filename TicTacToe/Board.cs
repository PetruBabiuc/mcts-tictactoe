using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunCs;
using static System.Linq.Enumerable;

namespace TicTacToe
{
    /// <summary>
    /// The enum that has all the possible winners of a board (game state).
    /// </summary>
    enum Winner { NoOne, Player, Computer, NotFinishedYet }

    /// <summary>
    /// The class which contains the state of the game.
    /// </summary>
    class Board
    {
        /// <summary>
        /// The number of columns and lines of the board 
        /// (it can be seen as a Size x Size Cell matrix).
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// <para>Board's cells. They are stored in a list, not in a list of lists (matrix).</para>
        /// <para>It's content is:</para>
        /// <para>[*lines[0], *lines[1], ..., *lines[Size - 1]],</para>
        /// <para>where "*" = "..." = spread/unpack operator.</para>
        /// </summary>
        public List<Cell> Cells { get; set; }

        /// <summary>
        /// Primary constructor. It also initializes the cells.
        /// </summary>
        /// <param name="size">The size of the board.</param>
        public Board(int size)
        {
            Size = size;
            Cells = Range(0, Size * Size)
                .Map(i => new Cell(i % Size, i / Size))
                .ToList();
        }

        /// <summary>
        /// The function that clones the board.
        /// </summary>
        /// <returns>A clone of the board containing a deep copy of the cells list</returns>
        public Board Clone()
        {
            Board result = new Board(Size);
            result.Cells = Cells
                .Map(c => c.Clone())
                .ToList();
            return result;
        }

        /// <summary>
        /// Function that checks if the cell at the specified index can be occupied.
        /// </summary>
        /// <param name="index">The index of the cell</param>
        /// <returns>True if the cell at the specified index can be occupied, false otherwise</returns>
        public bool CellCanBeOccupied(int index)
        {
            return Cells[index].isFree();
        }

        /// <summary>
        /// Method that provides a list of occupiable cells.
        /// </summary>
        /// <returns>The list of occupiable cells</returns>
        public List<Cell> OccupiableCells()
        {
            return Cells
                .Filter(c => c.isFree())
                .ToList();
        }

        /// <summary>
        /// Function that occupies a cell.
        /// </summary>
        /// <param name="x">The X coordinate of the cell to be occupied</param>
        /// <param name="y">The Y coordinate of the cell to be occupied</param>
        /// <param name="occupier">The occupier of the cell</param>
        /// <returns>A new board having the desired cell occupied</returns>
        public Board Occupy(int x, int y, CellOccupier occupier)
        { 
            return Occupy(FromPositionToIndex(x, y), occupier);
        }

        /// <summary>
        /// Function that occupies a cell.
        /// </summary>
        /// <param name="index">The index of the cell to be occupied</param>
        /// <param name="occupier">The occupier of the cell</param>
        /// <returns>A new board having the desired cell occupied</returns>
        public Board Occupy(int index, CellOccupier occupier)
        {
            if (Cells[index].Occupier != CellOccupier.NoOne)
                throw new Exception("You can't occupy an already occupied cell!");
            Board nextBoard = Clone();
            nextBoard.Cells[index].Occupier = occupier;
            return nextBoard;
        }

        /// <summary>
        /// <para>Function that checks the equality of two boards.</para>
        /// <para>It compares their sizes then proceeds to compare the cells' occupiers.</para>
        /// <para>It doesn't check the refrence equalities (neither boards' nor cells').</para>
        /// </summary>
        /// <param name="board">The board to be compared with</param>
        /// <returns>True </returns>
        public bool IsEqual(Board board)
        {
            if (Size != board.Size)
                return false;
            for (int i = 0; i < Cells.Count; ++i)
                if (board.Cells[i].Occupier != Cells[i].Occupier)
                    return false;
            return true;
        }

        /// <summary>
        /// Function that computes the winner of a board.
        /// </summary>
        /// <returns>The winner</returns>
        public Winner GetWinner()
        {
            CellOccupier occupier = CellOccupier.NoOne;

            // Checking every line's and column's winner
            for (int i = 0; i < Size; ++i)
            {
                occupier = ComputeCellOccupier(GetCellsLine(i));
                if (occupier != CellOccupier.NoOne)
                    break;
                occupier = ComputeCellOccupier(GetCellsColumn(i));
                if (occupier != CellOccupier.NoOne)
                    break;
            }
            if (occupier != CellOccupier.NoOne)
                return ToWinner(occupier);

            // Checking diagonals' winner
            occupier = ComputeCellOccupier(GetCellDiagonal(true));
            if (occupier != CellOccupier.NoOne)
                return ToWinner(occupier);
            occupier = ComputeCellOccupier(GetCellDiagonal(false));
            if (occupier != CellOccupier.NoOne)
                return ToWinner(occupier);

            // Checking if it's a draw (no cells free while no winner found)
            if (Cells.All(c => c.Occupier != CellOccupier.NoOne))
                return ToWinner(occupier);

            return Winner.NotFinishedYet;
        }

        /// <summary>
        /// Function that converts a cell occupier to a game winner
        /// </summary>
        /// <param name="occupier"></param>
        /// <returns></returns>
        private Winner ToWinner(CellOccupier occupier)
        {
            if (occupier == CellOccupier.Computer)
                return Winner.Computer;
            else if (occupier == CellOccupier.Player)
                return Winner.Player;
            else if (occupier == CellOccupier.NoOne)
                return Winner.NoOne;
            throw new Exception("The \"occupier\" parameter should be either CellOccupier.Computer or CellOccupier.Player");
        }

        /// <summary>
        /// Method that converts 2D coordinates to list index.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int FromPositionToIndex(int x, int y)
        {
            return y * Size + x;
        }

        /// <summary>
        /// Getter for the board's line identfied by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<Cell> GetCellsLine(int index)
        {
            return Cells.GetRange(Size * index, Size);
        }

        /// <summary>
        /// Getter for a board's column identified by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<Cell> GetCellsColumn(int index)
        {
            return Range(0, Size)
                .Map(i => Cells.ElementAt(i * Size + index))
                .ToList();
        }

        /// <summary>
        /// Getter of one of the board's diagonals
        /// </summary>
        /// <param name="first">If the diagonal should be the first diagonal(upper left to bottom right)</param>
        /// <returns></returns>
        private List<Cell> GetCellDiagonal(bool first)
        {
            if (first)
                return Range(0, Size)
                    .Map(i => Cells.ElementAt(i * Size + i))
                    .ToList();
            else
                return Range(0, Size)
                    .Map(i => Cells.ElementAt((i + 1) * Size - i - 1))
                    .ToList();
        }

        /// <summary>
        /// Method that computes the Cell occupier of a list of cells.
        /// </summary>
        /// <param name="cells"></param>
        /// <returns>If one of the opponents occupies all the cells, that opponent type is returned, else CellOccupier.NoOne is returned.</returns>
        private CellOccupier ComputeCellOccupier(List<Cell> cells)
        {
            if (cells.All(c => c.Occupier == CellOccupier.Computer))
                return CellOccupier.Computer;
            if (cells.All(c => c.Occupier == CellOccupier.Player))
                return CellOccupier.Player;
            return CellOccupier.NoOne;
        }
    }
}
