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
using Gma.System.MouseKeyHook;

namespace DesktopRecorder
{
    public partial class DesktopRecorder : Form
    {
        IKeyboardEvents GlobalHook;
        RecordTool recordTool = new RecordTool();
        bool currentlyRecording = false;
        readonly string DEFAULT_FILENAME_LABEL = "Current Recording: ";

        public DesktopRecorder()
        {
            GlobalHook = Hook.GlobalEvents();
            GlobalHook.KeyPress += GlobalHookKeyPress;
            InitializeComponent();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (!currentlyRecording) //and not in playback mode..
                StartNewRecording();
        }

        /// <summary>
        /// Listens for Key Press Events inside and outside the form/window. 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="keyEventArg"></param>
        private void GlobalHookKeyPress(Object o, KeyPressEventArgs keyEventArg)
        {
            //If user hits the escape key, (char)27, cancel the playback.  
            if (keyEventArg.KeyChar == (char)27)
                CancelPlayback();
        }

        /// <summary>
        /// Enables the Record Button and Disables the Stop button if
        /// user isn't currently recording, or Disables the Record button
        /// and Enables the Stop button if user currently is recording.
        /// </summary>
        private void UpdateRecordStopButtons()
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
            MethodInvoker labelUpdateInvoker = new MethodInvoker(UpdateRecordStopButtons);
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
            MethodInvoker labelUpdateInvoker = new MethodInvoker(UpdateRecordStopButtons);
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

        /// <summary>
        /// Display the string parameter on the 
        /// recordingNameLabel without the file ".dr" extension. 
        /// </summary>
        /// <param name="recordingName"></param>
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
        private string GetFileNameFromPath(string path, bool extensionIncluded = false)
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
                Thread playbackThread = new Thread(recordTool.Play);
                playbackThread.Start();
            }
        }

        /// <summary>
        /// Cancels the playback currently in action
        /// </summary>
        private void CancelPlayback()
        {
            this.recordTool.CancelPlayback();
        }

        private void endRecordingButton_Click(object sender, EventArgs e)
        {
            if (currentlyRecording)
                EndRecording();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            string pathToLoad = null;

            using (OpenFileDialog fileExplorer = new OpenFileDialog())
            {
                //fileExplorer.Filter = "Desktop Recording Files (*.dr)";
                if (fileExplorer.ShowDialog() == DialogResult.OK)
                    pathToLoad = fileExplorer.FileName;
            }

            bool pathProperlyLoaded = pathToLoad != null;

            if (pathProperlyLoaded)
                LoadRecordingToRecordTool(pathToLoad);
        }

        private void LoadRecordingToRecordTool(string path)
        {
            try
            {
                this.recordTool.LoadRecording(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unreadable file was selected by user.");
                Console.WriteLine(e.Message);
                HandleInvalidFileLoaded(GetFileNameFromPath(path, true));
                return;
            }

            //Update the file name label if file loaded properly.
            string fileNameWithoutFullPath = GetFileNameFromPath(path);
            UpdateLoadedRecordingLabel(fileNameWithoutFullPath);
        }

        private void HandleInvalidFileLoaded(string fileName)
        {
            DialogResult loadNewFile 
                = MessageBox.Show
                ("The selected file was unreadable. (Are you selecting a "
                + "file with a \".dr\" extension type?) Do you want to try a "
                + "different file?"
                , "Unreadable File"
                , MessageBoxButtons.YesNo);

            if (loadNewFile == DialogResult.Yes)
            {
                //Try to load file again. 
                //This method doesn't use the object or event args, so they can be null.
                loadButton_Click(null, null);
            }
            else
                return;
        }
    }
}
