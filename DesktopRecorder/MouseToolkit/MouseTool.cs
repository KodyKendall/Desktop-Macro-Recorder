﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Gma.System.MouseKeyHook;

namespace MouseToolkit
{
    public class MouseTool
    {

        int numLeftClicks = 0;
        int numRightClicks = 0;

        //To listen for mouse events
        int temporaryNumClicks = 0;

        /// <summary>
        /// Simulate a mouse click wherever the current mouse's location is
        /// depending on the mouseButton state. 
        /// </summary>
        public void SimulateMouseClick(MouseButtons mouseButton, MouseButtons previousFrameMouseButton)
        {
            //Check the previous frame's mouse position to determine what to do 
            switch (previousFrameMouseButton) 
            {
                //If the left mouse button was held down on the last frame
                case (MouseButtons.Left):
                    switch (mouseButton)
                    {
                        case (MouseButtons.Left):
                            //Continue holding the left mouse button down 
                            break;
                        //If they've switched to the right click..
                        case (MouseButtons.Right):
                            DoLeftMouseUp();
                            DoRightMouseDown();
                            break;
                        default:
                            DoLeftMouseUp();
                            break;
                    }
                    break;
                //If the right mouse button was held down on the last frame..
                case (MouseButtons.Right):
                    switch (mouseButton)
                    {
                        case (MouseButtons.Right):
                            //Continue holding the right mouse button down
                            break;
                        case (MouseButtons.Left):
                            DoRightMouseUp();
                            DoLeftMouseDown();
                            break;
                        default:
                            DoRightMouseUp();
                            break;
                    }
                    break;
                //If there wasn't a mouse button held down at all on the last one
                case (MouseButtons.None):
                    switch (mouseButton)
                    {
                        case (MouseButtons.Left):
                            DoLeftMouseDown();
                            break;
                        case (MouseButtons.Right):
                            DoRightMouseDown();
                            break;
                    }
                    break;
            }
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

        /// <summary>
        /// Get the total number of left clicks this mouse tool has simulated. 
        /// </summary>
        /// <returns></returns>
        public int TotalNumLeftClicks()
        {
            return this.numLeftClicks;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
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

        private void DoLeftMouseDown()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
        }

        private void DoRightMouseDown()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN, X, Y, 0, 0);
        }

        private void DoLeftMouseUp()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void DoRightMouseUp()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }
    }
}
