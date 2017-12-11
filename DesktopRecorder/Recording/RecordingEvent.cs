using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Recording
{
    class RecordingEvent
    {
        private Point eventPoint;


        public RecordingEvent(Point cursorPosition)
        {
            this.eventPoint = cursorPosition;
        }

        public Point GetCursorPoint()
        {
            return this.eventPoint;
        }

    }
}
