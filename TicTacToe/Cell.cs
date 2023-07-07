namespace TicTacToe
{
    /// <summary>
    /// Enum containing the possible occupiers of a cell.
    /// </summary>
    enum CellOccupier { NoOne, Player, Computer };
    class Cell
    {
        /// <summary>
        /// X coordinate of the cell.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// X coordinate of the cell.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Cell's occupier
        /// </summary>
        public CellOccupier Occupier { get; set; }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="occupier">Cell's occupier. It's default value is CellOccupier.NoOne</param>
        public Cell(int x, int y, CellOccupier occupier = CellOccupier.NoOne)
        {
            X = x;
            Y = y;
            Occupier = occupier;
        }

        /// <summary>
        /// Method that checks if a cell is free or not.
        /// </summary>
        /// <returns>True if the cell is free</returns>
        public bool isFree()
        {
            return Occupier == CellOccupier.NoOne;
        }

        /// <summary>
        /// Method that clones the cell
        /// </summary>
        /// <returns>A refrence to a new Cell object having the same X, Y, Occupier</returns>
        public Cell Clone()
        {
            return new Cell(X, Y, Occupier);
        }
    }
}
