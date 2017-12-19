using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MouseToolkit;
using System.Runtime.InteropServices;

namespace Recording
{
    public class RecordingEvent
    {
        public Point mouseLocation;
        public MouseButtons mouseButtonClick;

        public RecordingEvent(Point cursorPosition)
        {
            if (cursorPosition != null)
                this.mouseLocation = cursorPosition;
            else
                throw new Exception("Mouse position cannot be null!");
        }

        /// <summary>
        /// Creates a recording event based on a mouse action (clicking, scrolling, etc.)
        /// </summary>
        /// <param name="mouseEvent"></param>
        public RecordingEvent(Point cursorPosition, MouseButtons mouseButton) : this(cursorPosition)
        {
            this.mouseButtonClick = mouseButton;
        }

        /// <summary>
        /// Here solely for the purpose of xmlSerializer. 
        /// </summary>
        private RecordingEvent()
        { }

        /// <summary>
        /// Get this frame's x,y mouse location.
        /// </summary>
        /// <returns></returns>
        public Point GetCursorPoint()
        {
            return this.mouseLocation;
        }

        /// <summary>
        /// Executes the Recording Event. 
        /// How this event is executed depends on the previousMouseButton event.
        /// If the previousFrameMouseButton was MouseButtons.None, for example, 
        /// but this recording event's mouseButtonClick was a MouseButtons.left, 
        /// then you'd have a Left Mouse Press down event. 
        /// If the opposite were true, then you'd have a Left Mouse Press Up simulation.
        /// If the Previous was MouseButtons.Left, the this Recording Event was MouseButtons.Left, 
        /// then there'd be no change (to simulate that the mouse button was continually held down). 
        /// </summary>
        public void Execute(MouseButtons previousFrameMouseButton)
        {
            //For now, we just need to move the Cursor to it's current location
            MouseTool mouseTool = new MouseTool(); 
            mouseTool.MoveMouseCursor(this.mouseLocation);
            mouseTool.SimulateMouseClick(this.mouseButtonClick, previousFrameMouseButton);
        }

        /// <summary>
        /// Moves the cursor to the updated position
        /// </summary>
        private void MoveCursorToLocation(Point newMouseLocation)
        {
            Cursor.Position = newMouseLocation;
        }

        /// <summary>
        /// Simulates a mouse event based on this.mouseButtonClick's state. 
        /// If mouseButtonClick is null, no event will happen. 
        /// </summary>
        private void SimulateMouseClick(MouseButtons buttonToClick)
        {
            //Only simulate if this RecordingEvent has a non-null mouseEvent.
            //This should be tracked by the clickEvent boolean. 
            switch (buttonToClick)
            {
                case (MouseButtons.Left):
                {
                    DoMouseClick();
                    break;
                }
                    
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private void DoMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

    }
}
