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
            //Thread.Sleep(5000); //Give the playback thread enough time to play back..

            //Play back should have been AT LEAST longer than 2.5 seconds, since recording length was 6. 
            Assert.IsTrue(recording.MillisecondsLong() >= 2500);
        }

        [TestMethod]
        public void PlayClickRecording()
        {
            Recorder recorder = new Recorder();
            MouseTool tool = new MouseTool();

            recorder.StartRecording();
            tool.DoClick(MouseButtons.Left); //Simulate left click. 
            Thread.Sleep(3000);//Sleep for three seconds

            Record recording = recorder.StopRecording();

            recording.Play();
            //Let's have this main thread listen for mouse clicks during the duration of the recording
            int timesClicked = tool.ListenForMouseClicks(3000);

            Assert.IsTrue(timesClicked == 1);
            Assert.IsTrue(timesClicked == recording.NumLeftClicks);
        }
    }
}
