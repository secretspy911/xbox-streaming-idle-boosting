namespace XboxStreamingIdleBoosting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.controllerLogsCheckBox = new System.Windows.Forms.CheckBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.resetKeyLoggerButton = new System.Windows.Forms.Button();
            this.keyLoggerCheckBox = new System.Windows.Forms.CheckBox();
            this.copyToClipboardLinkLabel = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ClearLogsbutton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(12, 113);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(262, 21);
            this.logTextBox.TabIndex = 30;
            // 
            // controllerLogsCheckBox
            // 
            this.controllerLogsCheckBox.AutoSize = true;
            this.controllerLogsCheckBox.Location = new System.Drawing.Point(10, 90);
            this.controllerLogsCheckBox.Name = "controllerLogsCheckBox";
            this.controllerLogsCheckBox.Size = new System.Drawing.Size(92, 17);
            this.controllerLogsCheckBox.TabIndex = 20;
            this.controllerLogsCheckBox.Text = "Controller logs";
            this.controllerLogsCheckBox.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(93, 12);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // resetKeyLoggerButton
            // 
            this.resetKeyLoggerButton.Location = new System.Drawing.Point(81, 15);
            this.resetKeyLoggerButton.Name = "resetKeyLoggerButton";
            this.resetKeyLoggerButton.Size = new System.Drawing.Size(75, 23);
            this.resetKeyLoggerButton.TabIndex = 1;
            this.resetKeyLoggerButton.Text = "Reset";
            this.resetKeyLoggerButton.UseVisualStyleBackColor = true;
            this.resetKeyLoggerButton.Visible = false;
            this.resetKeyLoggerButton.Click += new System.EventHandler(this.resetKeyLoggerButton_Click);
            // 
            // keyLoggerCheckBox
            // 
            this.keyLoggerCheckBox.AutoSize = true;
            this.keyLoggerCheckBox.Location = new System.Drawing.Point(6, 19);
            this.keyLoggerCheckBox.Name = "keyLoggerCheckBox";
            this.keyLoggerCheckBox.Size = new System.Drawing.Size(56, 17);
            this.keyLoggerCheckBox.TabIndex = 0;
            this.keyLoggerCheckBox.Text = "Active";
            this.keyLoggerCheckBox.UseVisualStyleBackColor = true;
            this.keyLoggerCheckBox.CheckedChanged += new System.EventHandler(this.keyLoggerCheckBox_CheckedChanged);
            // 
            // copyToClipboardLinkLabel
            // 
            this.copyToClipboardLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyToClipboardLinkLabel.AutoSize = true;
            this.copyToClipboardLinkLabel.Location = new System.Drawing.Point(13, 141);
            this.copyToClipboardLinkLabel.Name = "copyToClipboardLinkLabel";
            this.copyToClipboardLinkLabel.Size = new System.Drawing.Size(89, 13);
            this.copyToClipboardLinkLabel.TabIndex = 40;
            this.copyToClipboardLinkLabel.TabStop = true;
            this.copyToClipboardLinkLabel.Text = "Copy to clipboard";
            this.copyToClipboardLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.copyToClipboardLinkLabel_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.keyLoggerCheckBox);
            this.groupBox1.Controls.Add(this.resetKeyLoggerButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 43);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key logger";
            // 
            // ClearLogsbutton
            // 
            this.ClearLogsbutton.Location = new System.Drawing.Point(199, 510);
            this.ClearLogsbutton.Name = "ClearLogsbutton";
            this.ClearLogsbutton.Size = new System.Drawing.Size(75, 23);
            this.ClearLogsbutton.TabIndex = 41;
            this.ClearLogsbutton.Text = "Clear";
            this.ClearLogsbutton.UseVisualStyleBackColor = true;
            this.ClearLogsbutton.Click += new System.EventHandler(this.ClearLogsbutton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 165);
            this.Controls.Add(this.ClearLogsbutton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.copyToClipboardLinkLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.controllerLogsCheckBox);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Xbox One Idle Boosting";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.CheckBox controllerLogsCheckBox;
        internal System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.Button resetKeyLoggerButton;
        private System.Windows.Forms.CheckBox keyLoggerCheckBox;
        private System.Windows.Forms.LinkLabel copyToClipboardLinkLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button ClearLogsbutton;
    }
}

