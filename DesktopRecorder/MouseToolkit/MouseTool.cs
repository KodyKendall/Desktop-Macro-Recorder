using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MouseToolkit
{
    public class MouseTool
    {
        int numLeftClicks;
        int numRightClicks;

        /// <summary>
        /// Simulate a mouse click wherever the current mouse's location is. 
        /// </summary>
        public void DoClick(MouseButtons mouseButton)
        {
            switch (mouseButton)
            {
                case (MouseButtons.Left):
                {
                    DoLeftMouseClick();
                    break;
                }
                case (MouseButtons.Right):
                {
                   throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// After starting this method, MouseTool will keep track of ALL clicks
        /// for the amount of millisecondsToWait. THIS DOES NOT run on it's own thread.
        /// </summary>
        public int ListenForMouseClicks(int millisecondsToWait = 5000)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.ElapsedMilliseconds < millisecondsToWait)
            {
                //Listen..? 
                //TODO: Keep track of mouse clicks during this period..
            }
            return -1;
        }

        /// <summary>
        /// Moves mouse location to given x, y coordinate. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveMouseCursor(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }

        public void MoveMouseCursor(Point newPoint)
        {
            Cursor.Position = newPoint;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private void DoLeftMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void DoRightMouseClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }
    }
}
