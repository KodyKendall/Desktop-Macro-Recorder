using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recording
{
    public class Frame
    {
        private int id;
        private RecordingEvent rEvent;

        /// <summary>
        /// The RecordingEvent that happened on this frame. 
        /// </summary>
        public RecordingEvent EventInRecording
        {
            get { return this.rEvent; }
            set { this.rEvent = value; }
        }

        /// <summary>
        /// The ID of this frame. 
        /// (Usually to indicate it's sequence in the recording)
        /// </summary>
        public int ID
        {
            get {return this.id;}
            set { this.id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameSequenceNumber"></param>
        /// <param name="recordingEvent"></param>
        public Frame(int frameSequenceNumber, RecordingEvent recordingEvent)
        {
            this.id = frameSequenceNumber;
            this.rEvent = recordingEvent;
        }

        /// <summary>
        /// Here solely for the purpose of being able to serialize frame with the
        /// xmlSerialize class. 
        /// </summary>
        private Frame()
        { }

        /// <summary>
        /// Executes this frame's recording event. 
        /// The previousMouseButton
        /// </summary>
        /// <param name="previousMouseButton"></param>
        public void Execute(MouseButtons previousMouseButton)
        {
            this.rEvent.Execute(previousMouseButton);
        }
    }
}
