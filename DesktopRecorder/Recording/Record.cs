using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Recording
{

    public class Record
    {
        private Dictionary<int, RecordingEvent> record;
        private int recordLength = 0;
        private int frameLength = 0;

        private DateTime dateRecorded;
        
        public int Length
        {
            get { return this.recordLength; }
            set { this.recordLength = value; }
        }

        /// <summary>
        /// Create a record with frameLengthTime milliseconds per frame
        /// </summary>
        /// <param name="frameLength"></param>
        public Record(int frameLengthTime, DateTime dateRecorded)
        {
            this.dateRecorded = dateRecorded;
            this.frameLength = frameLengthTime;
            record = new Dictionary<int, RecordingEvent>();
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
        public int MillisecondsLong()
        {
            return this.record.Count * frameLength;
        }

        /// <summary>
        /// Return the total time of this recording in seconds
        /// </summary>
        /// <returns></returns>
        public int SecondsLong()
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
            this.record.Add(record.Count+1, new RecordingEvent(cursorPoint));
        }

        /// <summary>
        /// Play back the recorded user events
        /// </summary>
        /// <returns>The milliseconds of the record</returns>
        public long Play()
        {
            Stopwatch s = new Stopwatch();

            s.Start();
            foreach (RecordingEvent frame in record.Values)
            {
                frame.Execute();
                System.Threading.Thread.Sleep(this.frameLength);
            }
            s.Stop();

            return s.ElapsedMilliseconds;

        }
    }
}
