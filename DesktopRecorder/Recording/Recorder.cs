using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Input;

namespace Recording
{
    public class Recorder
    {
        private bool recordingInSession;
        private int realFrameLength;
        private const int DEFAULT_FRAME_LENGTH = 15;
        private Record recording;

        /// <summary>
        /// Recorder has the capability to record user input and return the record object. 
        /// </summary>
        public Recorder(int frameLength = DEFAULT_FRAME_LENGTH)
        {
            recordingInSession = false;
            this.realFrameLength = frameLength;
        }

        /// <summary>
        /// Start a new recording of user mouse movements. 
        /// </summary>
        public void StartRecording()
        {
            if (!recordingInSession)
            {
                this.recording = new Record(this.realFrameLength);
                this.recordingInSession = true;
                Thread recordThread = new Thread(() => RecordUserMovement());
                recordThread.Start();
            }
            else
                throw new RecorderAlreadyRecordingException();
        }

        private void RecordUserMovement(int frameLength = 20)
        {
            while (this.recordingInSession)
            {
                RecordSingleFrame();
                Thread.Sleep(frameLength);//Get frame snapshot every frameLength milliseconds
            }
        }

        /// <summary>
        /// Records a single 'snapshot' or frame. 
        /// </summary>
        private void RecordSingleFrame()
        {
            if (this.recording == null)
                throw new AddFrameWithoutRecordingStartedException();
            else
                this.recording.AddFrame(Cursor.Position, DetermineWhichMouseButtonDown());
        }

        /// <summary>
        /// Stops recording a session that's in place and returns the Record. 
        /// </summary>
        public Record StopRecording()
        {
            if (recordingInSession)
            {
                //Should stop the recordFrame from being triggered by EventElapsedArgs
                this.recordingInSession = false;
                return this.recording;
            }
            else
                throw new RecorderNotRecordingException();
        }

        /// <summary>
        /// If this Recorder is currently recording, return true. Otherwise, false. 
        /// </summary>
        /// <returns></returns> 
        public bool IsRecording()
        {
            return this.recordingInSession;
        }

        #region Checks for buttons being held down: 

        /// <summary>
        /// Checks and returns the current MouseButton beind held down by the user. 
        /// </summary>
        private MouseButtons DetermineWhichMouseButtonDown()
        {
            switch (Control.MouseButtons)
            {
                case (MouseButtons.Left):
                    return MouseButtons.Left;
                case (MouseButtons.Right):
                    return MouseButtons.Right;
                case (MouseButtons.Middle):
                    return MouseButtons.Middle;
                case (MouseButtons.XButton1):
                    return MouseButtons.XButton1;
                case (MouseButtons.XButton2):
                    return MouseButtons.XButton2;
                default:
                    return MouseButtons.None;
            }
        }
        #endregion 
    }
}
