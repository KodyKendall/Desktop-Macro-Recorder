using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recording
{

    public class Record
    {
        private Dictionary<int, RecordingEvent> record;
        private int recordLength = 0;
        
        public int Length
        {
            get { return this.recordLength; }
            set { this.recordLength = value; }
        }

        public Record()
        {
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
        /// Adds a frame to this record
        /// </summary>
        /// <param name="milliSecond"></param>
        /// <param name="rEvent"></param>
        public void AddFrame(int milliSecond, System.Drawing.Point cursorPoint)
        {
            this.record.Add(milliSecond, new RecordingEvent(cursorPoint));
        }

    }
}
