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
            this.inputsListView = new System.Windows.Forms.ListView();
            this.Input = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            // inputsListView
            // 
            this.inputsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Input});
            this.inputsListView.GridLines = true;
            this.inputsListView.Location = new System.Drawing.Point(13, 42);
            this.inputsListView.Name = "inputsListView";
            this.inputsListView.Size = new System.Drawing.Size(293, 376);
            this.inputsListView.TabIndex = 2;
            this.inputsListView.UseCompatibleStateImageBehavior = false;
            this.inputsListView.View = System.Windows.Forms.View.List;
            // 
            // Input
            // 
            this.Input.Width = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 430);
            this.Controls.Add(this.inputsListView);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Xbox One Idle Boosting";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ListView inputsListView;
        private System.Windows.Forms.ColumnHeader Input;
    }
}

