using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing;
using System.Threading;
using static ProtoSynth.ProtoSynth;

namespace ProtoSynth
{
    public enum WaveTypes {Sine, Triangle, Saw, Square};

    public partial class UserInterfaceForm : Form
    {
        //[DllImport("winmm.dll")]
        // private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        private ConstantProperties cp;
        public const int envScale = 100;
        public double Frequency;
        public int Phase;
        public double Amplitude;
        private DirectSoundOut output;
        private BlockAlignReductionStream stream;
        private bool playing;
        public WaveTypes WaveType;
        public int Multi;
        public Envelope Envelope;
        private WaveStream tone;
        public double Distortion;
        private int sampleNumber;
        private int[] pointsLeft;
        private int[] pointsLeftTemp;
        private int[] pointsRight;
        private int[] pointsRightTemp;
        private bool painting;
        private int offset;
        private EventWaitHandle userEventWaitHandle;
        public bool Record;
        public bool SingleTone;

        internal UserInterfaceForm(EventWaitHandle userEventWaitHandle, ConstantProperties cp)
        {
            this.cp = cp;
            this.userEventWaitHandle = userEventWaitHandle;
            InitializeComponent();
            foreach (WaveTypes waveType in Enum.GetValues(typeof(WaveTypes)))
            {
                boxWaveType.Items.Add(waveType);
            }
            Frequency = Convert.ToDouble(txtFrequency.Text);
            Amplitude = Convert.ToDouble(barVolume.Value) / barVolume.Maximum;
            output = null;
            stream = null;
            playing = false;
            Multi = barMulti.Value;
            Envelope = new Envelope(
                barAttack.Value * cp.SampleRate / envScale,
                barDecay.Value * cp.SampleRate / envScale,
                (double)barSustain.Value / barSustain.Maximum,
                barRelease.Value * cp.SampleRate / envScale);
            Distortion = ((double)barDist.Value)/barDist.Maximum;
            pointsLeft = new int[panWave.Width];
            pointsLeftTemp = new int[panWave.Width];
            pointsRight = new int[panWave.Width];
            pointsRightTemp = new int[panWave.Width];
        }

        private void txtFrequency_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Frequency = Convert.ToDouble(txtFrequency.Text);
            }
            catch
            {
                Frequency = 0;
            }
        }

        private void barVolume_ValueChanged(object sender, EventArgs e)
        {
            Amplitude = Convert.ToDouble(barVolume.Value) / barVolume.Maximum;
        }

        private void btnSineBuffer_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                stop();
            }
            else
            {
                ProtoSynth.Ue = UserEvent.Play;
                userEventWaitHandle.Set();
                btnSineBuffer.FlatStyle = FlatStyle.Flat;
                btnSineBuffer.Text = "Pause";
                playing = true;
            }
        }

        private void ProtoSynth_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (output != null)
            {
                output.Dispose();
                output = null;
            }
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        private void boxWaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (WaveTypes waveType in Enum.GetValues(typeof(WaveTypes)))
            {
                if (waveType.ToString() == boxWaveType.Text)
                {
                    this.WaveType = waveType;
                }
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            /*if (play)
            {
                stop();
            }
            List<byte> data = new List<byte>();
            tone = new WaveStream(
                new WaveStreamProperties(
                    cp,
                    Multi,
                    Phase,
                    Envelope,
                    WaveType,
                    Distortion),
                this,
                record,
                false);
            data = tone.GetData();
            FileWriter fileWriter = new FileWriter(data.ToArray(), cp.SampleRate, cp.SampleRate);*/
        }

        private void barMulti_Scroll(object sender, EventArgs e)
        {
            Multi = barMulti.Value;
        }

        private void barPhase_Scroll(object sender, EventArgs e)
        {
            Phase = barPhase.Value;
        }

        private void barAttack_Scroll(object sender, EventArgs e)
        {
            Envelope.attack = barAttack.Value * cp.SampleRate / envScale;
        }

        private void barDecay_Scroll(object sender, EventArgs e)
        {
            Envelope.decay = barDecay.Value * cp.SampleRate / envScale;
        }

        private void barSustain_Scroll(object sender, EventArgs e)
        {
            Envelope.sustain = (double)barSustain.Value / barSustain.Maximum;
        }

        private void barRelease_Scroll(object sender, EventArgs e)
        {
            Envelope.release = barRelease.Value * cp.SampleRate / envScale;
        }

        private void btnRetrigger_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                tone.Retrigger();
            }
        }

        public void stop()
        {
            offset = 0;
            output.Stop();
            playing = false;
            btnSineBuffer.FlatStyle = FlatStyle.Standard;
            btnSineBuffer.Text = "Play";
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                tone.Release();
            }
        }

        private void barDist_Scroll(object sender, EventArgs e)
        {
            Distortion = (double)barDist.Value / barDist.Maximum;
        }

        public void Draw(int sampleNumber, double left, double right)
        {
            this.sampleNumber = (sampleNumber - offset) % panWave.Width;
            if (this.sampleNumber == 0 && !painting)
            {
                pointsLeftTemp.CopyTo(pointsLeft, 0);
                pointsRightTemp.CopyTo(pointsRight, 0);
                panWave.Invalidate();
            }
            pointsLeftTemp[this.sampleNumber] = Convert.ToInt32((panWave.Size.Height / 4) * (1 - left));
            pointsRightTemp[this.sampleNumber] = Convert.ToInt32((panWave.Size.Height / 4) * (3 - right));
        }

        private void panWave_Paint(object sender, PaintEventArgs e)
        {
            painting = true;
            for (int i = 0; i < panWave.Width - 1; i++)
            {
                e.Graphics.FillRectangle(Brushes.Black, i, pointsLeft[i], 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, i, pointsRight[i], 1, 1);
                int diff = pointsLeft[i] - pointsLeft[i + 1];
                if (diff > 1 || diff < -1)
                {
                   e.Graphics.DrawLine(Pens.Black, i, pointsLeft[i], i + 1, pointsLeft[i + 1]);
                }
                diff = pointsRight[i] - pointsRight[i + 1];
                if (diff > 1 || diff < -1)
                {
                   e.Graphics.DrawLine(Pens.Black, i, pointsRight[i], i + 1, pointsRight[i + 1]);
                }
            }
            painting = false;
        }

        private void chkTone_CheckedChanged(object sender, EventArgs e)
        {
            SingleTone = chkTone.Checked;
        }

        private void UserInterfaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProtoSynth.Ue = UserEvent.Close;
            userEventWaitHandle.Set();
        }
    }
}