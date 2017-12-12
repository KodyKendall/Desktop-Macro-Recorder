using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recording;
using System.Threading;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class PlayTests
    {
        [TestMethod]
        public void PlayBasicRecording()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            Thread.Sleep(3000); //Record for 6 seconds
            Record recording = r.StopRecording();

            Assert.IsTrue(recording.FrameCount() >= 2);
            Thread.Sleep(5000); //Give the playback thread enough time to play back..

            //Play back should have been AT LEAST longer than 5 seconds, since recording length was 6. 
            Assert.IsTrue(recording.MillisecondsLong() >= 2500);
        }
    }
}
