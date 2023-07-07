namespace TicTacToe
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBoxTable = new System.Windows.Forms.GroupBox();
            this.textBoxSimulations = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBoardSize = new System.Windows.Forms.TextBox();
            this.buttonResetAiExperience = new System.Windows.Forms.Button();
            this.checkBoxDrawAsWin = new System.Windows.Forms.CheckBox();
            this.comboBoxAlgorithm = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 361);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(191, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_click);
            // 
            // groupBoxTable
            // 
            this.groupBoxTable.Location = new System.Drawing.Point(209, 12);
            this.groupBoxTable.Name = "groupBoxTable";
            this.groupBoxTable.Size = new System.Drawing.Size(650, 650);
            this.groupBoxTable.TabIndex = 2;
            this.groupBoxTable.TabStop = false;
            // 
            // textBoxSimulations
            // 
            this.textBoxSimulations.Location = new System.Drawing.Point(12, 91);
            this.textBoxSimulations.Name = "textBoxSimulations";
            this.textBoxSimulations.Size = new System.Drawing.Size(144, 22);
            this.textBoxSimulations.TabIndex = 3;
            this.textBoxSimulations.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Number of simulations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Board size";
            // 
            // textBoxBoardSize
            // 
            this.textBoxBoardSize.Location = new System.Drawing.Point(12, 30);
            this.textBoxBoardSize.Name = "textBoxBoardSize";
            this.textBoxBoardSize.Size = new System.Drawing.Size(144, 22);
            this.textBoxBoardSize.TabIndex = 8;
            this.textBoxBoardSize.Text = "3";
            // 
            // buttonResetAiExperience
            // 
            this.buttonResetAiExperience.Location = new System.Drawing.Point(12, 321);
            this.buttonResetAiExperience.Name = "buttonResetAiExperience";
            this.buttonResetAiExperience.Size = new System.Drawing.Size(191, 23);
            this.buttonResetAiExperience.TabIndex = 9;
            this.buttonResetAiExperience.Text = "Reset AI experience";
            this.buttonResetAiExperience.UseVisualStyleBackColor = true;
            this.buttonResetAiExperience.Click += new System.EventHandler(this.buttonResetAiExperience_Click);
            // 
            // checkBoxDrawAsWin
            // 
            this.checkBoxDrawAsWin.AutoSize = true;
            this.checkBoxDrawAsWin.Checked = true;
            this.checkBoxDrawAsWin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDrawAsWin.Location = new System.Drawing.Point(10, 119);
            this.checkBoxDrawAsWin.Name = "checkBoxDrawAsWin";
            this.checkBoxDrawAsWin.Size = new System.Drawing.Size(146, 38);
            this.checkBoxDrawAsWin.TabIndex = 10;
            this.checkBoxDrawAsWin.Text = "AI should consider\r\ndraw being a win";
            this.checkBoxDrawAsWin.UseVisualStyleBackColor = true;
            // 
            // comboBoxAlgorithm
            // 
            this.comboBoxAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgorithm.FormattingEnabled = true;
            this.comboBoxAlgorithm.Items.AddRange(new object[] {
            "Original MCTS",
            "Modified MCTS"});
            this.comboBoxAlgorithm.Location = new System.Drawing.Point(10, 200);
            this.comboBoxAlgorithm.Name = "comboBoxAlgorithm";
            this.comboBoxAlgorithm.Size = new System.Drawing.Size(121, 24);
            this.comboBoxAlgorithm.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "AI Algorithm";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(872, 673);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxAlgorithm);
            this.Controls.Add(this.checkBoxDrawAsWin);
            this.Controls.Add(this.buttonResetAiExperience);
            this.Controls.Add(this.textBoxBoardSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSimulations);
            this.Controls.Add(this.groupBoxTable);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "TicTacToe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupBoxTable;
        private System.Windows.Forms.TextBox textBoxSimulations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBoardSize;
        private System.Windows.Forms.Button buttonResetAiExperience;
        private System.Windows.Forms.CheckBox checkBoxDrawAsWin;
        private System.Windows.Forms.ComboBox comboBoxAlgorithm;
        private System.Windows.Forms.Label label3;
    }
}

