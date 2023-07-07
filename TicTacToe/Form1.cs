using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FunCs;
using static System.Linq.Enumerable;
using System.Runtime.InteropServices;


namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Bitmap _xBitmap = new Bitmap("x.png");
        private Bitmap _0Bitmap = new Bitmap("0.png");
        private Bitmap _noneBitmap = new Bitmap("none.png");

        private Board _board;
        private int _oldBoardSize = 3;
        private IMCTS _mcts;
        private MCTSType _mctsType = MCTSType.Original;

        private bool _isPlaying = false;
        private List<Control> _controlsActiveWhileNotPlaying;

        private List<PictureBox> _boardPictureBoxes = new List<PictureBox>();
        private int _spaceBetweenPictureBoxes = 20;

        public Form1()
        {
            InitializeComponent();

            // The form should not be resizeable
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            comboBoxAlgorithm.SelectedIndex = 0;

            _controlsActiveWhileNotPlaying = new List<Control>
            {
                textBoxBoardSize, textBoxSimulations, comboBoxAlgorithm,
                checkBoxDrawAsWin, buttonResetAiExperience, buttonStart
            };
        }

        private void pictureBox_click(object sender, EventArgs e)
        {
            if (!_isPlaying)
                return;
            if (!HandlePlayerClick((PictureBox)sender))
                return;

            if (CheckIsFinished())
                return;

            HandleComputerAction();

            CheckIsFinished();
        }

        private void buttonStart_click(object sender, EventArgs e)
        {
            int boardSize, simulationsNumber;
            try
            {
                boardSize = int.Parse(textBoxBoardSize.Text);
                if (boardSize < 3)
                    throw new ArgumentException();
            }
            catch (Exception ex)
            when (ex is ArgumentException || ex is FormatException || ex is OverflowException)
            {
                MessageBox.Show("Board size should be an integer greater than 2!", "Error!");
                return;
            }
            try
            {
                simulationsNumber = int.Parse(textBoxSimulations.Text);
                if (simulationsNumber < 1)
                    throw new ArgumentException();
            }
            catch (Exception ex)
            when (ex is ArgumentException || ex is FormatException || ex is OverflowException)
            {
                MessageBox.Show("Number of simulations should be an integer greater than 0!", "Error!");
                return;
            }

            _board = new Board(boardSize);

            if (boardSize == _oldBoardSize && _mcts != null && comboBoxAlgorithm.SelectedIndex == (int)_mctsType)
            {
                _mcts.SimulationsNumber = simulationsNumber;
                _mcts.ConsiderDrawAsWin = checkBoxDrawAsWin.Checked;
                _mcts.Restart();
            }
            else
                _mcts = MCTSFactory.CreateMCTS((MCTSType)comboBoxAlgorithm.SelectedIndex, _board, simulationsNumber, checkBoxDrawAsWin.Checked);

            _mctsType = (MCTSType)comboBoxAlgorithm.SelectedIndex;
            _oldBoardSize = boardSize;

            ToggleIsPlayingState();
            InitPictureBoxes();
            UpdatePictureBoxes();
        }

        private bool HandlePlayerClick(PictureBox pictureBox)
        {
            int index = _boardPictureBoxes.FindIndex(p => p == pictureBox);
            if (!_board.CellCanBeOccupied(index))
                return false;
            pictureBox.Image = _xBitmap;
            _board = _board.Occupy(index, CellOccupier.Player);
            return true;
        }

        private bool CheckIsFinished()
        {
            Winner winner = _board.GetWinner();
            if (winner != Winner.NotFinishedYet)
                FinishGame(winner);
            return winner != Winner.NotFinishedYet;
        }

        private void FinishGame(Winner occupier)
        {
            MessageBox.Show($"Winner: {occupier}");
            ToggleIsPlayingState();
        }

        private void HandleComputerAction()
        {
            _board = _mcts.Occupy(_board);
            UpdatePictureBoxes();
        }

        private void UpdatePictureBoxes()
        {
            _board.Cells.ForEach(c =>
            {
                int index = c.Y * _oldBoardSize + c.X;
                if (c.Occupier == CellOccupier.Computer)
                    _boardPictureBoxes[index].Image = _0Bitmap;
                else if (c.Occupier == CellOccupier.Player)
                    _boardPictureBoxes[index].Image = _xBitmap;
                else
                    _boardPictureBoxes[index].Image = _noneBitmap;
            });
        }

        private void ToggleIsPlayingState()
        {
            _controlsActiveWhileNotPlaying.ForEach(c => c.Enabled = _isPlaying);
            _isPlaying = !_isPlaying;
        }

        private void InitPictureBoxes()
        {
            _boardPictureBoxes.Clear();
            groupBoxTable.Controls.Clear();

            PictureBox pictureBox;
            int line, column;

            int pictureBoxWidth = (groupBoxTable.ClientRectangle.Width - _spaceBetweenPictureBoxes * (_oldBoardSize + 1)) / _oldBoardSize;
            int pictureBoxHeight = (groupBoxTable.ClientRectangle.Height - _spaceBetweenPictureBoxes * (_oldBoardSize + 1)) / _oldBoardSize;

            for (int i = 0; i < _oldBoardSize * _oldBoardSize; ++i)
            {
                line = i / _oldBoardSize;
                column = i % _oldBoardSize;

                pictureBox = new PictureBox();
                groupBoxTable.Controls.Add(pictureBox);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                // pictureBox.BorderStyle = BorderStyle.FixedSingle;

                pictureBox.Height = pictureBoxWidth;
                pictureBox.Width = pictureBoxWidth;

                pictureBox.Top = _spaceBetweenPictureBoxes * (line + 1) + line * pictureBoxHeight;
                pictureBox.Left = _spaceBetweenPictureBoxes * (column + 1) + column * pictureBoxWidth;

                pictureBox.Image = _noneBitmap;
                pictureBox.Click += pictureBox_click;
                _boardPictureBoxes.Add(pictureBox);
            }
        }

        private void buttonResetAiExperience_Click(object sender, EventArgs e)
        {
            if (_mcts == null)
                return;
            _mcts.ResetTree();
        }
    }
}
