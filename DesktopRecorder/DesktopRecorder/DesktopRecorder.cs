using System;
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
using System.Diagnostics;

namespace DesktopRecorder
{
    public partial class DesktopRecorder : Form
    {
        RecordTool recordTool = new RecordTool();
        bool currentlyRecording = false;
        string DEFAULT_FILENAME_LABEL = "Current Recording: ";

        public DesktopRecorder()
        {
            InitializeComponent();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (!currentlyRecording)
                StartNewRecording();
            else
                EndRecording();
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

            string pathToSave = GetSavePathFromUser();

            //If an empty string is returned, user declined to save. 
            if (pathToSave != "")
            {
                recordTool.SaveRecording(pathToSave + ".dr"); //Desktop Recording extension .dr
                string fileName = GetFileNameFromPath(pathToSave, false);
                UpdateLoadedRecordingLabel(fileName);
            }
        }

        #region helpers

        /// <summary>
        /// Gets the user's desired save path for the recording. 
        /// </summary>
        private string GetSavePathFromUser()
        {
            SaveFileDialog fileSaver = new SaveFileDialog();

            fileSaver.Title = "Save Your Recording";
            //fileSaver.InitialDirectory = "C:\\Program Files\\Desktop Recorder\\Recordings";
            fileSaver.ShowDialog();

            string pathToSave = fileSaver.FileName;

            if (pathToSave == "") //If user closed file explorer. 
            {
                DialogResult userWantsToSaveRecording = 
                    MessageBox.Show("Are you sure you want to delete this recording?"
                    , "Save Recording"
                    , MessageBoxButtons.YesNo);

                if (userWantsToSaveRecording == DialogResult.No)
                    return GetSavePathFromUser();

                else if (userWantsToSaveRecording == DialogResult.Yes)
                     return "";
            }
            return pathToSave;
        }

        private void UpdateLoadedRecordingLabel(string recordingName)
        {
            this.recordingNameLabel.Text = DEFAULT_FILENAME_LABEL + recordingName;
        }

        /// <summary>
        /// Given a file path as a string, return the name after 
        /// the backslash without the extension. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetFileNameFromPath(string path, bool extensionIncluded)
        {
            string[] filePathParts = path.Split('\\');
            int lastItemIndex = filePathParts.Length - 1;
            string fileNameWithExtension = filePathParts[lastItemIndex];

            if (extensionIncluded)
                return fileNameWithExtension;
            else
            {
                //We want to strip out the last part of the name (part with extension)
                filePathParts = fileNameWithExtension.Split('.');
                //First element should be the file name we want without the extension
                string fileNameWithoutExtension = filePathParts[0];
                return fileNameWithExtension;
            }
        }
        
        #endregion
    }
}
