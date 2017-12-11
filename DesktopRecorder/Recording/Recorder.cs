using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Threading;

namespace Recording
{
    public class Recorder
    {
        System.Timers.Timer recordTimer;
        private int recordingLength;

        //This is the essential part of our recording.
        //Dictionary<int, RecordingEvent> recording = new Dictionary<int, RecordingEvent>();
        private bool recordingInSession;
        private int startTime;
        private int endTime;

        private Record recording;

        /// <summary>
        /// Recorder has the capability to record user input and return the record object. 
        /// </summary>
        public Recorder()
        {
            recordingInSession = false;
            recordTimer = new System.Timers.Timer();
        }

        public void StartRecording()
        {
            if (!recordingInSession)
            {
                this.recording = new Record();
                this.recordingInSession = true;
                Thread recordThread = new Thread(() => RecordUserMovement());
                recordThread.Start();
                //RecordUserMovement();
            }
            else
            {
                throw new Exception("You cannot start a new recording when one's in progress.");
            }
        }


        private void RecordUserMovement(int frameLength= 20)
        {
            startTime = DateTime.Now.Millisecond;

            while (this.recordingInSession)
            {
                RecordFrame();
                Thread.Sleep(frameLength);
            }
            //recordTimer.Interval = 20; //Record snapshot every 20 seconds. 
            //recordTimer.Elapsed += RecordFrame();
            //recordTimer.Start();
        }

        private void RecordFrame()
        {
            this.recording.AddFrame(GetElapsedMillisecond(), Cursor.Position);
        }

        /// <summary>
        /// Get's the elapsed milliseconds since the recorder started
        /// </summary>
        /// <returns></returns>
        public int GetElapsedMillisecond()
        {
            return DateTime.Now.Millisecond - this.startTime;
        }

        /// <summary>
        /// Stops recording a session that's in place and returns the Record object. 
        /// </summary>
        public Record StopRecording()
        {
            if (recordingInSession)
            {
                //Should stop the recordFrame from being triggered by EventElapsedArgs
                recordTimer.Stop();

                this.endTime = DateTime.Now.Millisecond;
                recordingLength = this.endTime - this.startTime;
                recording.Length = this.recordingLength;
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
    }
}
