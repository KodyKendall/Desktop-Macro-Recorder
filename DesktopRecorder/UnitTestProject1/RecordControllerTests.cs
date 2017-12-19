using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordingController;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class RecordControllerTests
    {
        [TestMethod]
        public void RecordXmlSaveAndLoad()
        {
            RecordTool recordUtilSave = new RecordTool();

            //Record for 1 second
            recordUtilSave.StartRecording();
            System.Threading.Thread.Sleep(1000);
            recordUtilSave.StopRecording();

            recordUtilSave.SaveRecording(@"C:\Users\Kody\Desktop\TestRecording1");
            RecordTool recordUtilLoad = new RecordTool();
            recordUtilLoad.LoadRecording(@"C:\Users\Kody\Desktop\TestRecording1");

            //recordUtilLoad.Play();

            Assert.IsTrue(recordUtilLoad.LengthOfRecording == recordUtilSave.LengthOfRecording);
            Assert.IsTrue(recordUtilLoad.NumberOfFrames == recordUtilSave.NumberOfFrames);
        }
    }
}
