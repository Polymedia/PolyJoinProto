namespace Client
{
    partial class ConnectForm
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
            this.startButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.createRadioButton = new System.Windows.Forms.RadioButton();
            this.connectionTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.conferenceIdLabel = new System.Windows.Forms.Label();
            this.conferenceIdTextBox = new System.Windows.Forms.TextBox();
            this.joinRadioButton = new System.Windows.Forms.RadioButton();
            this.serverIPTextBox = new System.Windows.Forms.TextBox();
            this.serverIpLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.connectionTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(130, 43);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(255, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Create and start";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(130, 112);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(255, 23);
            this.joinButton.TabIndex = 3;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            // 
            // createRadioButton
            // 
            this.createRadioButton.AutoSize = true;
            this.createRadioButton.Location = new System.Drawing.Point(6, 19);
            this.createRadioButton.Name = "createRadioButton";
            this.createRadioButton.Size = new System.Drawing.Size(113, 17);
            this.createRadioButton.TabIndex = 6;
            this.createRadioButton.Text = "Create conference";
            this.createRadioButton.UseVisualStyleBackColor = true;
            // 
            // connectionTypeGroupBox
            // 
            this.connectionTypeGroupBox.Controls.Add(this.conferenceIdLabel);
            this.connectionTypeGroupBox.Controls.Add(this.conferenceIdTextBox);
            this.connectionTypeGroupBox.Controls.Add(this.joinRadioButton);
            this.connectionTypeGroupBox.Controls.Add(this.createRadioButton);
            this.connectionTypeGroupBox.Controls.Add(this.joinButton);
            this.connectionTypeGroupBox.Controls.Add(this.startButton);
            this.connectionTypeGroupBox.Location = new System.Drawing.Point(12, 87);
            this.connectionTypeGroupBox.Name = "connectionTypeGroupBox";
            this.connectionTypeGroupBox.Size = new System.Drawing.Size(391, 140);
            this.connectionTypeGroupBox.TabIndex = 7;
            this.connectionTypeGroupBox.TabStop = false;
            this.connectionTypeGroupBox.Text = "Connection type";
            // 
            // conferenceIdLabel
            // 
            this.conferenceIdLabel.AutoSize = true;
            this.conferenceIdLabel.Location = new System.Drawing.Point(29, 89);
            this.conferenceIdLabel.Name = "conferenceIdLabel";
            this.conferenceIdLabel.Size = new System.Drawing.Size(76, 13);
            this.conferenceIdLabel.TabIndex = 9;
            this.conferenceIdLabel.Text = "Conference ID";
            // 
            // conferenceIdTextBox
            // 
            this.conferenceIdTextBox.Location = new System.Drawing.Point(130, 86);
            this.conferenceIdTextBox.Name = "conferenceIdTextBox";
            this.conferenceIdTextBox.Size = new System.Drawing.Size(255, 20);
            this.conferenceIdTextBox.TabIndex = 8;
            // 
            // joinRadioButton
            // 
            this.joinRadioButton.AutoSize = true;
            this.joinRadioButton.Checked = true;
            this.joinRadioButton.Location = new System.Drawing.Point(6, 65);
            this.joinRadioButton.Name = "joinRadioButton";
            this.joinRadioButton.Size = new System.Drawing.Size(101, 17);
            this.joinRadioButton.TabIndex = 7;
            this.joinRadioButton.TabStop = true;
            this.joinRadioButton.Text = "Join conference";
            this.joinRadioButton.UseVisualStyleBackColor = true;
            // 
            // serverIPTextBox
            // 
            this.serverIPTextBox.Location = new System.Drawing.Point(142, 6);
            this.serverIPTextBox.Name = "serverIPTextBox";
            this.serverIPTextBox.Size = new System.Drawing.Size(255, 20);
            this.serverIPTextBox.TabIndex = 8;
            this.serverIPTextBox.Text = "localhost";
            // 
            // serverIpLabel
            // 
            this.serverIpLabel.AutoSize = true;
            this.serverIpLabel.Location = new System.Drawing.Point(15, 9);
            this.serverIpLabel.Name = "serverIpLabel";
            this.serverIpLabel.Size = new System.Drawing.Size(51, 13);
            this.serverIpLabel.TabIndex = 9;
            this.serverIpLabel.Text = "Server IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Your name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(142, 45);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(255, 20);
            this.nameTextBox.TabIndex = 11;
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 239);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverIPTextBox);
            this.Controls.Add(this.serverIpLabel);
            this.Controls.Add(this.connectionTypeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectForm";
            this.Text = "Connect to server";
            this.connectionTypeGroupBox.ResumeLayout(false);
            this.connectionTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.RadioButton createRadioButton;
        private System.Windows.Forms.GroupBox connectionTypeGroupBox;
        private System.Windows.Forms.Label conferenceIdLabel;
        private System.Windows.Forms.TextBox conferenceIdTextBox;
        private System.Windows.Forms.RadioButton joinRadioButton;
        private System.Windows.Forms.TextBox serverIPTextBox;
        private System.Windows.Forms.Label serverIpLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
    }
}