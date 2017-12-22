namespace DesktopRecorder
{
    partial class DesktopRecorder
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
            this.recordButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.recordingNameLabel = new System.Windows.Forms.Label();
            this.endRecordingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recordButton
            // 
            this.recordButton.Location = new System.Drawing.Point(80, 143);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(232, 95);
            this.recordButton.TabIndex = 0;
            this.recordButton.Text = "Record";
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(80, 266);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(484, 94);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // recordingNameLabel
            // 
            this.recordingNameLabel.AutoSize = true;
            this.recordingNameLabel.Location = new System.Drawing.Point(80, 45);
            this.recordingNameLabel.Name = "recordingNameLabel";
            this.recordingNameLabel.Size = new System.Drawing.Size(351, 44);
            this.recordingNameLabel.TabIndex = 2;
            this.recordingNameLabel.Text = "Current Recording: ";
            // 
            // endRecordingButton
            // 
            this.endRecordingButton.Enabled = false;
            this.endRecordingButton.Location = new System.Drawing.Point(332, 143);
            this.endRecordingButton.Name = "endRecordingButton";
            this.endRecordingButton.Size = new System.Drawing.Size(232, 95);
            this.endRecordingButton.TabIndex = 3;
            this.endRecordingButton.Text = "Stop";
            this.endRecordingButton.UseVisualStyleBackColor = true;
            this.endRecordingButton.Click += new System.EventHandler(this.endRecordingButton_Click);
            // 
            // DesktopRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(22F, 42F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1316, 872);
            this.Controls.Add(this.endRecordingButton);
            this.Controls.Add(this.recordingNameLabel);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.recordButton);
            this.Name = "DesktopRecorder";
            this.Text = " n ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button recordButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Label recordingNameLabel;
        private System.Windows.Forms.Button endRecordingButton;
    }
}

