using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recording;
using System.Threading;
using RecordPlayer;

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
            Thread.Sleep(6000); //Record 3 frames
            Record record = r.StopRecording();
            Assert.IsTrue(record.FrameCount() >= 2);


            RecordPlayer.RecordPlayer player = new RecordPlayer.RecordPlayer();
            int timePlaybackStarted = DateTime.Now.Millisecond;
            player.PlayRecord(record);
            int timePlaybackEnded = DateTime.Now.Millisecond;

            //Play back should have been AT LEAST longer than 5 seconds, since recording length was 6. 
            Assert.IsTrue(timePlaybackEnded - timePlaybackStarted > 5000);
        }
    }
}
