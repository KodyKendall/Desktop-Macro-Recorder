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
        private int recordingLength;

        //This is the essential part of our recording.
        private bool recordingInSession;
        private int frameLength;

        private Record recording;

        /// <summary>
        /// Recorder has the capability to record user input and return the record object. 
        /// </summary>
        public Recorder(int frameLength = 20)
        {
            recordingInSession = false;
            this.frameLength = frameLength;
        }

        public void StartRecording()
        {
            if (!recordingInSession)
            {
                this.recording = new Record(this.frameLength, DateTime.Now);
                this.recordingInSession = true;
                Thread recordThread = new Thread(() => RecordUserMovement());
                recordThread.Start();
            }
            else
            {
                throw new Exception("You cannot start a new recording when one's in progress.");
            }
        }


        private void RecordUserMovement(int frameLength = 20)
        {
            while (this.recordingInSession)
            {
                RecordSingleFrame();
                Thread.Sleep(frameLength);//Get frame snapshot every frameLength milliseconds
            }
        }


        //Records a single frame
        private void RecordSingleFrame()
        {
            if (MouseLeftDown())
            {
                this.recording.AddFrame(Cursor.Position, MouseButtons.Left);
            }

            this.recording.AddFrame(Cursor.Position);
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
            {
                throw new Exception("A recording session must have started before you can stop recording.");
            }
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
        private bool MouseLeftDown()
        {
            return (Control.MouseButtons == MouseButtons.Left);
        }
        #endregion 
    }
}
