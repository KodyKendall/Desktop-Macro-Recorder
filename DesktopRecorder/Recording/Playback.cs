using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recording
{
    public class Playback
    {
        LinkedList<RecordingEvent> events;

        public Playback(Record record)
        {
            this.events = (record.GetPlayback());
        }

        public int ExecuteFrame()
        {
            //Execute the next event up
            events.First.Value.Execute();

            int executedEventTime = events.First.Value.TimeExecuted;
            //Remove that event
            events.RemoveFirst();

            int nextEventToExecuteTime = events.First.Value.TimeExecuted;

            return nextEventToExecuteTime - executedEventTime;
        }

        public bool HasNextFrame()
        {
            return events.Count > 1; 
        }
    }
}
