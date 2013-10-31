namespace Polymedia.PolyJoin.Client
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
            this.roleValueLabel = new System.Windows.Forms.Label();
            this.roleLabel = new System.Windows.Forms.Label();
            this.conferenceIdValueLabel = new System.Windows.Forms.Label();
            this.conferenceIdlabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
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
            this.leftPanel.Controls.Add(this.roleValueLabel);
            this.leftPanel.Controls.Add(this.roleLabel);
            this.leftPanel.Controls.Add(this.conferenceIdValueLabel);
            this.leftPanel.Controls.Add(this.conferenceIdlabel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanel.Location = new System.Drawing.Point(3, 42);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(294, 460);
            this.leftPanel.TabIndex = 2;
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

    }
}