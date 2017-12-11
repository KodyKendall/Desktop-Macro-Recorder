using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Recording;

namespace DesktopRecorder
{
    public partial class DesktopRecorder : Form
    {
        Recorder recording;

        public DesktopRecorder()
        {
            InitializeComponent();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            //Start a new recording
            recording = new Recording.Recorder();
        }


    }
}
