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
        readonly string DEFAULT_FILENAME_LABEL = "Current Recording: ";

        public DesktopRecorder()
        {
            InitializeComponent();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (!currentlyRecording) //and not in playback mode..
                StartNewRecording();
        }

        /// <summary>
        /// Enables the Record Button if
        /// user isn't currently recording, or disables
        /// if user currently is recording.
        /// </summary>
        private void UpdateRecordButtonLabel()
        {
            if (currentlyRecording)
            {
                this.endRecordingButton.Enabled = true;
                this.recordButton.Enabled = false;
                this.recordButton.Text = "Recording";
            }
            else
            {
                this.endRecordingButton.Enabled = false;
                this.recordButton.Enabled = true;
                this.recordButton.Text = "Record";
            }
        }
        
        /// <summary>
        /// Disables the Stop Button if
        /// user isn't currently recording, or enables
        /// if user currently is recording.
        /// </summary>
        private void ToggleEnabledStopButton()
        {
            if (currentlyRecording)
                this.endRecordingButton.Enabled = true;
            else
                this.endRecordingButton.Enabled = false;
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
                bool extensionAlreadyInPath = HasDrExtension(pathToSave);

                if (extensionAlreadyInPath)
                    //If the path already has the extension at the end, don't include it.
                    recordTool.SaveRecording(pathToSave); 
                else
                    //Desktop Recording file extension = .dr
                    recordTool.SaveRecording(pathToSave + ".dr"); 

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
                    MessageBox.Show("Are you sure you don't want to save this recording?"
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
            //We don't want to display the .dr extension
            string nameToDisplay = StripDrFileExtension(recordingName);
            this.recordingNameLabel.Text = this.DEFAULT_FILENAME_LABEL + nameToDisplay;
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

        /// <summary>
        /// Checks if the string ends with ".dr" 
        /// (Not Caps Sensitive)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool HasDrExtension(string fileName)
        {
            return fileName.ToLower().Substring(fileName.Length - 3) == ".dr";
        }

        /// <summary>
        /// If the given string ends in a ".dr" extension, return the string 
        /// with ".dr" removed from the end. 
        /// 
        /// If it doesn't end in a ".dr" extension, the string remains unchanged.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string StripDrFileExtension(string fileName)
        {
            if (HasDrExtension(fileName))
                return fileName.Substring(0, fileName.Length - 3);
            else
                return fileName;
        }

        #endregion

        private void playButton_Click(object sender, EventArgs e)
        {
            if (recordTool != null)
            {
                recordTool.Play();
            }
        }

        private void CancelPlayback()
        {

        }

        private void endRecordingButton_Click(object sender, EventArgs e)
        {
            if (currentlyRecording)
                EndRecording();
        }
    }
}
