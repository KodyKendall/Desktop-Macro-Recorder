using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Recording
{
    public class RecordingEvent
    {
        private Point eventPoint;

        public RecordingEvent(Point cursorPosition)
        {
            if (cursorPosition != null)
                this.eventPoint = cursorPosition;
            else
                throw new Exception("Mouse position cannot be null!");
        }

        public Point GetCursorPoint()
        {
            return this.eventPoint;
        }

        /// <summary>
        /// Executes the events
        /// </summary>
        public void Execute()
        {
            //For now, we just need to move the Cursor to it's current location
            MoveCursorToLocation();
        }

        /// <summary>
        /// Moves the cursor to the updated position
        /// </summary>
        private void MoveCursorToLocation()
        {
            Cursor.Position = new Point(this.eventPoint.X, this.eventPoint.Y);
        }

    }
}
