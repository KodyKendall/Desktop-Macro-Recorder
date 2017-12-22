using Recording;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RecordingController
{
    public class RecordTool
    {
        Recorder recorder;
        Record record;
        string name;

        /// <summary>
        /// Returns the length of the stored recording.
        /// </summary>
        /// <returns></returns>
        public double LengthOfRecording
        {
            get
            {
                if (this.record != null)
                    return record.SecondsLong();
                else return 0.0;
            }
        }

        /// <summary>
        /// The file name of this recording.
        /// </summary>
        public string Name
        {
            get
            {
                if (record == null)
                    return "No File Name";
                else
                    return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// The number of frames in the stored recording.
        /// </summary>
        public int NumberOfFrames
        {
            get
            {
                if (this.record != null)
                    return this.record.NumFrames;
                else return 0;
            }
        }

        /// <summary>
        /// A tool for recording, playing back, saving, and loading user input 
        /// into a replayable format. 
        /// </summary>
        public RecordTool()
        {
            this.recorder = new Recorder();
        }

        /// <summary>
        /// Start recording user input
        /// </summary>
        public void StartRecording()
        {
            this.recorder.StartRecording();
        }

        /// <summary>
        /// Stop the current recording from taking place.
        /// </summary>
        public void StopRecording()
        {
            this.record = this.recorder.StopRecording();
        }

        /// <summary>
        /// Play the loaded record. If the record isn't loaded (null), don't play. 
        /// </summary>
        public void Play()
        {
            if (this.record == null)
                return;
            else
                this.record.Play();
        }

        /// <summary>
        /// Returns true if there's currently a record ready to play. 
        /// Returns false otherwise. 
        /// </summary>
        /// <returns></returns>
        public bool RecordLoaded()
        {
            return this.record == null;
        }

        /// <summary>
        /// Stop a currently playing record from playing
        /// </summary>
        public void CancelPlayback()
        {
            this.record.PlaybackInProgress = false;
        }

        /// <summary>
        /// Save the current record to the specified file name
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveRecording(string fileName)
        {
            using (TextWriter xmlWriter = new StreamWriter(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Record));
                serializer.Serialize(xmlWriter, this.record);
            }
        }

        /// <summary>
        /// Load an existing record for playback
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadRecording(string fileName)
        {
            using (var streamReader = new StreamReader(fileName))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Record));
                try
                {
                    this.record = (Record)deserializer.Deserialize(streamReader);
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was a problem reading the loaded .dr file");
                    Console.WriteLine(e.Message);
                    throw new InvalidDataException("There was an incorrect file type loaded in Recordtool.LoadRecording!");
                }
            }
        }
        
    }
}
