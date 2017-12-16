//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MouseToolkit;
//using System.Threading;
//using System.Windows.Forms;
//using System.Diagnostics;

//namespace MouseToolTests
//{
//    [TestClass]
//    public class MouseToolTests
//    {
//        [TestMethod]
//        public void TestLeftClick()
//        {
//            MouseTool t = new MouseTool();

//            int timesMouseClicked = 0;

//            //Start new thread that listens for clicks.
//            Thread clickListener = new Thread(() => { timesMouseClicked = t.ListenForMouseClicks(MouseButtons.Left, 1000); });
//            clickListener.Start();

//            //Simulate a click on seperate thread
//            t.SimulateMouseClick(MouseButtons.Left);
//            //Wait for clickListener to be done, rejoin. 
//            clickListener.Join();

//            Assert.IsTrue(timesMouseClicked == 1);
//        }

//        //[TestMethod]
//        //public void TestMultipleLeftClicks()
//        //{
//        //    MouseTool t = new MouseTool();

//        //    int timesMouseClicked = 0;

//        //    ////Start new thread that listens for clicks.
//        //    Thread clickListener = new Thread(() => { timesMouseClicked = t.ListenForMouseClicks(MouseButtons.Left, 3000); });
//        //    clickListener.Start();

//        //    //Simulate 15 clicks on main thread

//        //    //for (int index = 0; index < 15; index++)
//        //    //    t.SimulateMouseClick(MouseButtons.Left);

//        //    //Wait for clickListener to be done, rejoin. 
//        //    clickListener.Join();

//        //    //Failing
//        //    Assert.IsTrue(timesMouseClicked == 15);
//        //}

//        [TestMethod]
//        public void ListForMouseClickManualSimple()
//        {
//            MouseTool t = new MouseTool();
//            int timesMouseClicked = t.ListenForMouseClicks(MouseButtons.Left, 3000);
//            t.SimulateMouseClick(MouseButtons.Left);
//            t.SimulateMouseClick(MouseButtons.Left);
//            t.SimulateMouseClick(MouseButtons.Left);
//            t.SimulateMouseClick(MouseButtons.Left);

//            Assert.IsTrue(timesMouseClicked > 0);
//        }

//        private void TestClickHelper()
//        {

//        }

//        [TestMethod]
//        public void SimulateBasisLeftClick()
//        {
//            MouseTool t = new MouseTool();

//            Stopwatch s = new Stopwatch();

//            while (s.ElapsedMilliseconds < 5000)
//            {
//                t.SimulateMouseClick(MouseButtons.Left);
//                t.SimulateMouseClick(MouseButtons.Right);
//            }

//            Assert.IsTrue(t.TotalNumLeftClicks() > 0);
//        }

//        [TestMethod]
//        public void TestRightClick()
//        {
//            throw new NotImplementedException();
//        }

//        [TestMethod]
//        public void TestNewCursorLocation()
//        {
//            throw new NotImplementedException();
//        }

//        [TestMethod]
//        public void TestClickListener()
//        {
//            MouseTool t = new MouseTool();
//            int numsClicked = t.ListenForMouseClicks(MouseButtons.Left);

//            //Simulate a number of clicks while the listener is running, confirm it 
//            //kept track of them all, both right and left clicks. 
//            Assert.IsTrue(numsClicked > 0);
//        }
//    }
//}
