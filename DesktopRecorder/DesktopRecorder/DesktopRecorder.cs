﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RecordingController;
using System.Threading;

namespace DesktopRecorder
{
    public partial class DesktopRecorder : Form
    {
        RecordTool recordTool = new RecordTool();
        bool currentlyRecording = false;

        public DesktopRecorder()
        {
            InitializeComponent();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (!currentlyRecording)
            {
                StartNewRecording();

            }
            else
            {
                EndRecording();
            }
        }

        /// <summary>
        /// Updates the Record Button Label to "Record" if 
        /// user isn't currently recording, or "Stop Recording"
        /// if user currently is recording.
        /// </summary>
        private void UpdateRecordButtonLabel()
        {
            if (!currentlyRecording)
                this.recordButton.Text = "Record";
            else
                this.recordButton.Text = "Stop Recording";
        }

        /// <summary>
        /// Handles all the logic for stuff that needs to happen when 
        /// starting a new recording on the windows form. 
        /// </summary>
        private void StartNewRecording()
        {
            this.currentlyRecording = true;

            //Start on seperate thread to not freeze up GUI
            Thread recordThread = new Thread(recordTool.StartRecording);
            recordThread.Start();

            //We want to make sure to call this AFTER this.currentlyRecording = true. 
            MethodInvoker labelUpdateInvoker = new MethodInvoker(UpdateRecordButtonLabel);
            this.Invoke(labelUpdateInvoker);
        }

        /// <summary>
        /// Handles all the logic for stuff that needs to happen when 
        /// ending a recording on the windows form. 
        /// </summary>
        private void EndRecording()
        {
            this.currentlyRecording = false;

            recordTool.StopRecording(); //May get a cross-thread exception.. not sure.. 
            MethodInvoker labelUpdateInvoker = new MethodInvoker(UpdateRecordButtonLabel);
            this.Invoke(labelUpdateInvoker);

            //TODO: Save prompt screen for saved recording. 
            recordTool.SaveRecording(@"c:\users\kody\desktop\testrecording2");
        }
    }
}
