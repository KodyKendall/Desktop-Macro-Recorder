using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recording;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using MouseToolkit;


namespace MouseToolTests
{
    [TestClass]
    public class PlayTests
    {
        [TestMethod]
        public void PlayBasicRecording()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            Thread.Sleep(3000); //Record for 3 seconds
            Record recording = r.StopRecording();

            Assert.IsTrue(recording.FrameCount() >= 2);

            //Play back should have been AT LEAST longer than 2.5 seconds, since recording length was 3. 
            Assert.IsTrue(recording.MillisecondsLong() >= 2500);
        }

        /// <summary>
        /// This test is here just to showcase the functionality 
        /// of the model. 
        /// </summary>
        [TestMethod]
        public void ShowcaseRecording()
        {
            //Recorder r = new Recorder();
            //r.StartRecording(); //record for 10 secs 
            //Thread.Sleep(5000);
            //Record recording = r.StopRecording();

            //recording.Play();

            Assert.IsTrue(true);
        }
    }
}
