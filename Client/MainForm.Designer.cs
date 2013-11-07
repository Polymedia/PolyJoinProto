namespace Client
{
    partial class MainForm
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.connectionStateValueLabel = new System.Windows.Forms.Label();
            this.connectionStateLabel = new System.Windows.Forms.Label();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.modeGroupBox = new System.Windows.Forms.GroupBox();
            this.drawFullScreenRadioButton = new System.Windows.Forms.RadioButton();
            this.inputRadioButton = new System.Windows.Forms.RadioButton();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.silentRadioButton = new System.Windows.Forms.RadioButton();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.roleValueLabel = new System.Windows.Forms.Label();
            this.roleLabel = new System.Windows.Forms.Label();
            this.conferenceIdValueLabel = new System.Windows.Forms.Label();
            this.conferenceIdlabel = new System.Windows.Forms.Label();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsPresenterColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsInputContollerColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.modeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.topPanel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.leftPanel, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(824, 505);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.topPanel.Controls.Add(this.connectionStateValueLabel);
            this.topPanel.Controls.Add(this.connectionStateLabel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(3, 3);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Size = new System.Drawing.Size(294, 33);
            this.topPanel.TabIndex = 0;
            // 
            // connectionStateValueLabel
            // 
            this.connectionStateValueLabel.AutoSize = true;
            this.connectionStateValueLabel.Location = new System.Drawing.Point(105, 10);
            this.connectionStateValueLabel.Name = "connectionStateValueLabel";
            this.connectionStateValueLabel.Size = new System.Drawing.Size(70, 13);
            this.connectionStateValueLabel.TabIndex = 3;
            this.connectionStateValueLabel.Text = "Connecting...";
            // 
            // connectionStateLabel
            // 
            this.connectionStateLabel.AutoSize = true;
            this.connectionStateLabel.Location = new System.Drawing.Point(9, 10);
            this.connectionStateLabel.Name = "connectionStateLabel";
            this.connectionStateLabel.Size = new System.Drawing.Size(90, 13);
            this.connectionStateLabel.TabIndex = 2;
            this.connectionStateLabel.Text = "Connection state:";
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.modeGroupBox);
            this.leftPanel.Controls.Add(this.dataGridView);
            this.leftPanel.Controls.Add(this.roleValueLabel);
            this.leftPanel.Controls.Add(this.roleLabel);
            this.leftPanel.Controls.Add(this.conferenceIdValueLabel);
            this.leftPanel.Controls.Add(this.conferenceIdlabel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.Location = new System.Drawing.Point(3, 42);
            this.leftPanel.MinimumSize = new System.Drawing.Size(0, 460);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(294, 460);
            this.leftPanel.TabIndex = 2;
            // 
            // modeGroupBox
            // 
            this.modeGroupBox.Controls.Add(this.drawFullScreenRadioButton);
            this.modeGroupBox.Controls.Add(this.inputRadioButton);
            this.modeGroupBox.Controls.Add(this.drawRadioButton);
            this.modeGroupBox.Controls.Add(this.silentRadioButton);
            this.modeGroupBox.Location = new System.Drawing.Point(12, 52);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(270, 112);
            this.modeGroupBox.TabIndex = 9;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Mode";
            // 
            // drawFullScreenRadioButton
            // 
            this.drawFullScreenRadioButton.AutoSize = true;
            this.drawFullScreenRadioButton.Location = new System.Drawing.Point(8, 65);
            this.drawFullScreenRadioButton.Name = "drawFullScreenRadioButton";
            this.drawFullScreenRadioButton.Size = new System.Drawing.Size(169, 17);
            this.drawFullScreenRadioButton.TabIndex = 3;
            this.drawFullScreenRadioButton.Text = "Draw full screen (Esc to leave)";
            this.drawFullScreenRadioButton.UseVisualStyleBackColor = true;
            // 
            // inputRadioButton
            // 
            this.inputRadioButton.AutoSize = true;
            this.inputRadioButton.Location = new System.Drawing.Point(8, 88);
            this.inputRadioButton.Name = "inputRadioButton";
            this.inputRadioButton.Size = new System.Drawing.Size(49, 17);
            this.inputRadioButton.TabIndex = 2;
            this.inputRadioButton.Text = "Input";
            this.inputRadioButton.UseVisualStyleBackColor = true;
            // 
            // drawRadioButton
            // 
            this.drawRadioButton.AutoSize = true;
            this.drawRadioButton.Location = new System.Drawing.Point(8, 42);
            this.drawRadioButton.Name = "drawRadioButton";
            this.drawRadioButton.Size = new System.Drawing.Size(50, 17);
            this.drawRadioButton.TabIndex = 1;
            this.drawRadioButton.Text = "Draw";
            this.drawRadioButton.UseVisualStyleBackColor = true;
            // 
            // silentRadioButton
            // 
            this.silentRadioButton.AutoSize = true;
            this.silentRadioButton.Checked = true;
            this.silentRadioButton.Location = new System.Drawing.Point(8, 19);
            this.silentRadioButton.Name = "silentRadioButton";
            this.silentRadioButton.Size = new System.Drawing.Size(51, 17);
            this.silentRadioButton.TabIndex = 0;
            this.silentRadioButton.TabStop = true;
            this.silentRadioButton.Text = "Silent";
            this.silentRadioButton.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdColumn,
            this.NameColumn,
            this.IsPresenterColumn,
            this.IsInputContollerColumn,
            this.ColorColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView.Location = new System.Drawing.Point(0, 170);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(294, 290);
            this.dataGridView.TabIndex = 8;
            // 
            // roleValueLabel
            // 
            this.roleValueLabel.AutoSize = true;
            this.roleValueLabel.Location = new System.Drawing.Point(105, 36);
            this.roleValueLabel.Name = "roleValueLabel";
            this.roleValueLabel.Size = new System.Drawing.Size(0, 13);
            this.roleValueLabel.TabIndex = 7;
            // 
            // roleLabel
            // 
            this.roleLabel.AutoSize = true;
            this.roleLabel.Location = new System.Drawing.Point(9, 36);
            this.roleLabel.Name = "roleLabel";
            this.roleLabel.Size = new System.Drawing.Size(32, 13);
            this.roleLabel.TabIndex = 6;
            this.roleLabel.Text = "Role:";
            // 
            // conferenceIdValueLabel
            // 
            this.conferenceIdValueLabel.AutoSize = true;
            this.conferenceIdValueLabel.Location = new System.Drawing.Point(105, 13);
            this.conferenceIdValueLabel.Name = "conferenceIdValueLabel";
            this.conferenceIdValueLabel.Size = new System.Drawing.Size(0, 13);
            this.conferenceIdValueLabel.TabIndex = 5;
            // 
            // conferenceIdlabel
            // 
            this.conferenceIdlabel.AutoSize = true;
            this.conferenceIdlabel.Location = new System.Drawing.Point(9, 13);
            this.conferenceIdlabel.Name = "conferenceIdlabel";
            this.conferenceIdlabel.Size = new System.Drawing.Size(79, 13);
            this.conferenceIdlabel.TabIndex = 4;
            this.conferenceIdlabel.Text = "Conference ID:";
            // 
            // IdColumn
            // 
            this.IdColumn.DataPropertyName = "Id";
            this.IdColumn.HeaderText = "Id";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            this.IdColumn.Visible = false;
            // 
            // NameColumn
            // 
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // IsPresenterColumn
            // 
            this.IsPresenterColumn.DataPropertyName = "IsPresenter";
            this.IsPresenterColumn.HeaderText = "Is presenter";
            this.IsPresenterColumn.Name = "IsPresenterColumn";
            this.IsPresenterColumn.ReadOnly = true;
            this.IsPresenterColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsPresenterColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // IsInputContollerColumn
            // 
            this.IsInputContollerColumn.DataPropertyName = "IsInputController";
            this.IsInputContollerColumn.HeaderText = "Is input contoller";
            this.IsInputContollerColumn.Name = "IsInputContollerColumn";
            this.IsInputContollerColumn.ReadOnly = true;
            this.IsInputContollerColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColorColumn
            // 
            this.ColorColumn.DataPropertyName = "BrushArgb";
            this.ColorColumn.HeaderText = "Color";
            this.ColorColumn.Name = "ColorColumn";
            this.ColorColumn.ReadOnly = true;
            this.ColorColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 505);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "MainForm";
            this.Text = "PolyJoin";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            this.modeGroupBox.ResumeLayout(false);
            this.modeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label connectionStateValueLabel;
        private System.Windows.Forms.Label connectionStateLabel;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Label conferenceIdValueLabel;
        private System.Windows.Forms.Label conferenceIdlabel;
        private System.Windows.Forms.Label roleValueLabel;
        private System.Windows.Forms.Label roleLabel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox modeGroupBox;
        private System.Windows.Forms.RadioButton inputRadioButton;
        private System.Windows.Forms.RadioButton drawRadioButton;
        private System.Windows.Forms.RadioButton silentRadioButton;
        private System.Windows.Forms.RadioButton drawFullScreenRadioButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsPresenterColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsInputContollerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColorColumn;

    }
}