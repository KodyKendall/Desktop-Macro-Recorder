using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recording
{
    [Serializable]
    class AddFrameWithoutRecordingStartedException : Exception
    {
        public AddFrameWithoutRecordingStartedException()
            : base(String.Format("You cannot add a frame without first starting a recording!"))
        { }
    }

    [Serializable]
    public class RecorderAlreadyRecordingException : Exception
    {
        public RecorderAlreadyRecordingException()
            : base(String.Format("You cannot start recording when a recording is already in session!"))
        { }
    }

    [Serializable]
    public class RecorderNotRecordingException : Exception
    {
        public RecorderNotRecordingException()
            : base(String.Format("You cannot stop a recording session when no recording session is in place!"))
        { }
    } 


}
