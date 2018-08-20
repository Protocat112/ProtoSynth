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

        public const int sampleRate = 44100;
        public const int envScale = 100;
        private int phase;
        private double frequency;
        private double amplitude;
        private DirectSoundOut output;
        private BlockAlignReductionStream stream;
        private bool play;
        private WaveTypes waveType;
        private int multi;
        private Envelope env;
        private WaveStream tone;
        private double dist;
        private int sampleNumber;
        private int[] pointsLeft;
        private int[] pointsLeftTemp;
        private int[] pointsRight;
        private int[] pointsRightTemp;
        private bool painting;
        private int offset;
        private EventWaitHandle userEventWaitHandle;

        internal UserInterfaceForm(EventWaitHandle userEventWaitHandle)
        {
            this.userEventWaitHandle = userEventWaitHandle;
            InitializeComponent();
            foreach (WaveTypes waveType in Enum.GetValues(typeof(WaveTypes)))
            {
                boxWaveType.Items.Add(waveType);
            }
            amplitude = Convert.ToDouble(barVolume.Value) / barVolume.Maximum;
            frequency = Convert.ToDouble(txtFrequency.Text);
            output = null;
            stream = null;
            play = false;
            multi = barMulti.Value;
            env = new Envelope(barAttack.Value * sampleRate / envScale, barDecay.Value * sampleRate / envScale, (double)barSustain.Value / barSustain.Maximum, barRelease.Value * sampleRate / envScale);
            dist = ((double)barDist.Value)/barDist.Maximum;
            pointsLeft = new int[panWave.Width];
            pointsLeftTemp = new int[panWave.Width];
            pointsRight = new int[panWave.Width];
            pointsRightTemp = new int[panWave.Width];
        }

        private void txtFrequency_TextChanged(object sender, EventArgs e)
        {
            int frequency;
            try
            {
                frequency = Convert.ToDouble(txtFrequency.Text);
            }
            catch
            {
                frequency = 0;
            }
            ProtoSynth.UpdateFrequency();
        }

        private void barVolume_ValueChanged(object sender, EventArgs e)
        {
            amplitude = Convert.ToDouble(barVolume.Value) / barVolume.Maximum;
        }

        private void btnSineBuffer_Click(object sender, EventArgs e)
        {
            if (play)
            {
                stop();
            }
            else
            {
                ProtoSynth.userEvent = UserEvent.Play;
                userEventWaitHandle.Set();
                btnSineBuffer.FlatStyle = FlatStyle.Flat;
                btnSineBuffer.Text = "Pause";
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
                    this.waveType = waveType;
                }
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (play)
            {
                stop();
            }
            List<byte> data = new List<byte>();
            tone = new WaveStream(this, frequency, amplitude, waveType, sampleRate, phase, multi, true, env, dist);
            data = tone.getData();
            FileWriter fileWriter = new FileWriter(data.ToArray(), sampleRate, sampleRate);
        }

        private void barMulti_Scroll(object sender, EventArgs e)
        {
            multi = barMulti.Value;
        }

        private void barPhase_Scroll(object sender, EventArgs e)
        {
            phase = barPhase.Value;
        }

        private void barAttack_Scroll(object sender, EventArgs e)
        {
            env.attack = barAttack.Value * sampleRate / envScale;
        }

        private void barDecay_Scroll(object sender, EventArgs e)
        {
            env.decay = barDecay.Value * sampleRate / envScale;
        }

        private void barSustain_Scroll(object sender, EventArgs e)
        {
            env.sustain = (double)barSustain.Value / barSustain.Maximum;
        }

        private void barRelease_Scroll(object sender, EventArgs e)
        {
            env.release = barRelease.Value * sampleRate / envScale;
        }

        private void btnRetrigger_Click(object sender, EventArgs e)
        {
            if (play)
            {
                tone.Retrigger();
            }
        }

        public void stop()
        {
            offset = 0;
            output.Stop();
            play = false;
            btnSineBuffer.FlatStyle = FlatStyle.Standard;
            btnSineBuffer.Text = "Play";
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (play)
            {
                tone.Release();
            }
        }

        private void barDist_Scroll(object sender, EventArgs e)
        {
            dist = (double)barDist.Value / barDist.Maximum;
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
    }
}