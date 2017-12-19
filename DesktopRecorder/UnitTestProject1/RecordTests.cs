using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recording;
using System.Threading;

namespace MouseToolTests
{
    [TestClass]
    public class RecordTests
    {
        [TestMethod]
        public void BasicRecorderTest()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            r.StopRecording();

            //Error wasn't thrown. Was able to call these two methods with no error.
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MoreThanOneFrameRecording()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            Thread.Sleep(20);
            Record record = r.StopRecording();
            
            //Should have recorded at least one frame. 
            Assert.IsTrue(record.FrameCount() > 0);

        }

        [TestMethod]
        public void SixtyMillisecondRecording()
        {
            Recorder r = new Recorder();

            r.StartRecording();

            //Should be the test thread sleeping, not recording thread. 
            System.Threading.Thread.Sleep(600);

            Record play = r.StopRecording();
            Assert.IsTrue(play.FrameCount() > 4);
        }

        [TestMethod]
        public void TestErrorMessages()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            try
            {
                r.StartRecording();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestGetElapsedMillisecond()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            Thread.Sleep(500); //Sleep for 500 milliseconds
            Record testRecord = r.StopRecording();

            Assert.IsTrue(testRecord.SecondsLong() == 0);
            Assert.IsTrue(testRecord.MillisecondsLong() >= 500);
        }

        [TestMethod]
        public void TestRecorderAlreadyRecordingException()
        {
            Recorder r = new Recorder();
            r.StartRecording();
            try
            {
                r.StartRecording();
                Assert.Fail();
            }
            catch (RecorderAlreadyRecordingException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestRecorderNotRecordingException()
        {
            Recorder r = new Recorder();
            try
            {
                r.StopRecording();
                Assert.Fail();
            }
            catch (RecorderNotRecordingException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
