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
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarDelay = new System.Windows.Forms.TrackBar();
            this.trackBarQuality = new System.Windows.Forms.TrackBar();
            this.modeGroupBox = new System.Windows.Forms.GroupBox();
            this.inputRadioButton = new System.Windows.Forms.RadioButton();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.silentRadioButton = new System.Windows.Forms.RadioButton();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsPresenterColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsInputContollerColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roleValueLabel = new System.Windows.Forms.Label();
            this.roleLabel = new System.Windows.Forms.Label();
            this.conferenceIdValueLabel = new System.Windows.Forms.Label();
            this.conferenceIdlabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarQuality)).BeginInit();
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
            this.leftPanel.Controls.Add(this.groupBoxSettings);
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
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.label2);
            this.groupBoxSettings.Controls.Add(this.label1);
            this.groupBoxSettings.Controls.Add(this.trackBarDelay);
            this.groupBoxSettings.Controls.Add(this.trackBarQuality);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 150);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(270, 148);
            this.groupBoxSettings.TabIndex = 10;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Delay";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Quality";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trackBarDelay
            // 
            this.trackBarDelay.Location = new System.Drawing.Point(8, 97);
            this.trackBarDelay.Maximum = 1000;
            this.trackBarDelay.Name = "trackBarDelay";
            this.trackBarDelay.Size = new System.Drawing.Size(256, 45);
            this.trackBarDelay.TabIndex = 1;
            this.trackBarDelay.Scroll += new System.EventHandler(this.trackBarDelay_Scroll);
            // 
            // trackBarQuality
            // 
            this.trackBarQuality.Location = new System.Drawing.Point(8, 34);
            this.trackBarQuality.Maximum = 100;
            this.trackBarQuality.Name = "trackBarQuality";
            this.trackBarQuality.Size = new System.Drawing.Size(256, 45);
            this.trackBarQuality.TabIndex = 0;
            this.trackBarQuality.Value = 100;
            this.trackBarQuality.Scroll += new System.EventHandler(this.trackBarQuality_Scroll);
            // 
            // modeGroupBox
            // 
            this.modeGroupBox.Controls.Add(this.inputRadioButton);
            this.modeGroupBox.Controls.Add(this.drawRadioButton);
            this.modeGroupBox.Controls.Add(this.silentRadioButton);
            this.modeGroupBox.Location = new System.Drawing.Point(12, 52);
            this.modeGroupBox.Name = "modeGroupBox";
            this.modeGroupBox.Size = new System.Drawing.Size(270, 91);
            this.modeGroupBox.TabIndex = 9;
            this.modeGroupBox.TabStop = false;
            this.modeGroupBox.Text = "Mode";
            // 
            // inputRadioButton
            // 
            this.inputRadioButton.AutoSize = true;
            this.inputRadioButton.Location = new System.Drawing.Point(8, 65);
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
            this.dataGridView.Location = new System.Drawing.Point(0, 304);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(294, 156);
            this.dataGridView.TabIndex = 8;
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
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarQuality)).EndInit();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsPresenterColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsInputContollerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColorColumn;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarDelay;
        private System.Windows.Forms.TrackBar trackBarQuality;

    }
}