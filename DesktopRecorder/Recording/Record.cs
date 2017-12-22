using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Recording
{

    public class Record
    {
        //int is the frame's order. 1 = first frame in recording. 
        //public Dictionary<int, RecordingEvent> record;
        public List<Frame> record;
        private int recordLength = 0;
        private int frameLength = 0;
        private bool playbackInProgress = false;

        #region Mouse stats for record
        private int numLeftClicks = 0;
        private int numRightClicks = 0;


        public double LengthInSeconds
        {
            get { return SecondsLong(); }

            //Do nothing (here for xml serialization)
            set
            { 
            }
        }

        /// <summary>
        /// The length in milliseconds each frame represents.
        /// Default is 15. 
        /// </summary>
        public int FrameDuration
        {
            get { return this.frameLength; }
            set { this.frameLength = value; }
        }

        /// <summary>
        /// The number of left clicks in this record
        /// </summary>
        public int NumLeftClicks
        {
            get { return this.numLeftClicks; }
        }

        /// <summary>
        /// The number of right clicks in this record
        /// </summary>
        public int NumRightCLicks

        {
            get { return this.numRightClicks; }
        }

        #endregion

        private DateTime dateRecorded;
        
        public int NumFrames
        {
            get { return this.recordLength; }
            set { this.recordLength = value; }
        }

        /// <summary>
        /// If the record is currently playing, this playback will be set to true. 
        /// To stop a record from currently playing, set this to false. 
        /// </summary>
        public bool PlaybackInProgress
        {
            get { return this.playbackInProgress; }
            set { this.playbackInProgress = value; }
        }

        /// <summary>
        /// Create a record with frameLengthTime milliseconds per frame
        /// </summary>
        /// <param name="frameLength"></param>
        public Record(int frameLengthTime = 15)
        {
            this.dateRecorded = DateTime.Now;
            this.frameLength = frameLengthTime;
            record = new List<Frame>();
        }

        /// <summary>
        /// Create a record.
        /// 
        /// </summary>
        public Record()
        {
            this.dateRecorded = DateTime.Now;
            this.frameLength = 15; //Default fvalue
            record = new List<Frame>();
        }

        /// <summary>
        /// Returns the number of frames in this record
        /// </summary>
        /// <returns></returns>
        public int FrameCount()
        {
            return this.record.Count;
        }

        /// <summary>
        /// Returns the total time of this recording in milliseconds
        /// </summary>
        /// <returns></returns>
        public double MillisecondsLong()
        {
            return this.record.Count * frameLength;
        }

        /// <summary>
        /// Return the total time of this recording in seconds
        /// </summary>
        /// <returns></returns>
        public double SecondsLong()
        {
            return MillisecondsLong() / 1000;
        }

        /// <summary>
        /// Return the total time of this record in minutes
        /// </summary>
        /// <returns></returns>
        public double MinutesLong()
        {
            return SecondsLong() / 60;
        }

        /// <summary>
        /// Adds a frame to this record
        /// </summary>
        /// <param name="milliSecond"></param>
        /// <param name="rEvent"></param>
        public void AddFrame(System.Drawing.Point cursorPoint)
        {
            this.record.Add(new Frame(record.Count + 1, new RecordingEvent(cursorPoint)));
        }

        /// <summary>
        /// Add a frame with a mouseButton.
        /// </summary>
        /// <param name="cursorPoint"></param>
        /// <param name="buttons"></param>
        public void AddFrame(System.Drawing.Point cursorPoint, MouseButtons button)
        {
            this.record.Add(new Frame(record.Count + 1, new RecordingEvent(cursorPoint,button)));
            if (button == MouseButtons.Left)
                this.numLeftClicks++;
            else if (button == MouseButtons.Right)
                this.numRightClicks++;
        }

        /// <summary>
        /// Play back the recorded user events
        /// </summary>
        /// <returns>The milliseconds of the record's playbac</returns>
        public long Play()
        {
            Stopwatch s = new Stopwatch();
            this.playbackInProgress = true;
            Console.WriteLine("Starting to play the recorded user input");
            s.Start();

            //Start out with no MouseButtons being held down at start.
            MouseButtons previousMouseButton = MouseButtons.None; 

            foreach (Frame frame in record)
            {

                //We want the option to abort the playback if needed.
                if (this.playbackInProgress)
                {
                    frame.Execute(previousMouseButton);
                    System.Threading.Thread.Sleep(this.frameLength);
                }
                else
                {
                    Console.WriteLine("PlaybackInProgres got changed to false");
                    Console.WriteLine("Aborting playback..");
                    break;  
                }

                previousMouseButton = frame.EventInRecording.mouseButtonClick;
            }

            this.playbackInProgress = false;
            s.Stop();
            return s.ElapsedMilliseconds;
        }
    }
}
