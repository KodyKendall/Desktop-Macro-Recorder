using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recording;
using System.Threading;

namespace RecordPlayer
{
    public class RecordPlayer
    {
        private Record record;

        public RecordPlayer()
        {
        }

        public void PlayRecord(Record record)
        {
            Playback player = new Playback(record);
            while (player.HasNextFrame())
            {
                int timeToWait = player.ExecuteFrame();
                Thread.Sleep(timeToWait);
            }
        }


    }
}
