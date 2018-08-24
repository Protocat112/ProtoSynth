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
        private bool playing;
        public WaveTypes WaveType;
        public int Multi;
        public Envelope Envelope;
        public double Distortion;
        private int sampleNumber;
        private int[] pointsLeft;
        private int[] pointsLeftTemp;
        private int[] pointsRight;
        private int[] pointsRightTemp;
        private bool painting;
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
            UpdateFrequency();
            UpdateAmplitude();
            playing = false;
            UpdateMulti();
            Envelope = new Envelope(
                GetAttack(),
                GetDecay(),
                GetSustain(),
                GetRelease());
            UpdateDistortion();
            pointsLeft = new int[panWave.Width];
            pointsLeftTemp = new int[panWave.Width];
            pointsRight = new int[panWave.Width];
            pointsRightTemp = new int[panWave.Width];
            SingleTone = chkTone.Checked;
        }

        private void UpdateDistortion()
        {
            Distortion = (((double)barDist.Value) - 0.01) / barDist.Maximum;
        }

        private int GetRelease()
        {
            return barRelease.Value * cp.SampleRate / envScale;
        }

        private double GetSustain()
        {
            return (double)barSustain.Value / barSustain.Maximum;
        }

        private int GetDecay()
        {
            return barDecay.Value * cp.SampleRate / envScale;
        }

        private int GetAttack()
        {
            return barAttack.Value * cp.SampleRate / envScale;
        }

        private void UpdateMulti()
        {
            Multi = barMulti.Value;
        }

        private void UpdateAmplitude()
        {
            Amplitude = Convert.ToDouble(barVolume.Value) / barVolume.Maximum;
        }

        private void UpdateFrequency()
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

        private void TxtFrequency_TextChanged(object sender, EventArgs e)
        {
            UpdateFrequency();
        }

        private void BarVolume_ValueChanged(object sender, EventArgs e)
        {
            UpdateAmplitude();
        }

        private void BtnSineBuffer_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                Stop();
                
            }
            else
            {
                Play();
            }
        }

        private void Play()
        {
            ProtoSynth.Ue = UserEvent.Play;
            userEventWaitHandle.Set();
            btnSineBuffer.FlatStyle = FlatStyle.Flat;
            btnSineBuffer.Text = "Stop";
            playing = true;
        }

        private void Stop()
        {
            ProtoSynth.Ue = UserEvent.Stop;
            userEventWaitHandle.Set();
            btnSineBuffer.FlatStyle = FlatStyle.Standard;
            btnSineBuffer.Text = "Play";
            playing = false;
        }

        private void BoxWaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (WaveTypes waveType in Enum.GetValues(typeof(WaveTypes)))
            {
                if (waveType.ToString() == boxWaveType.Text)
                {
                    this.WaveType = waveType;
                }
            }
        }

        private void BtnWrite_Click(object sender, EventArgs e)
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

        private void BarMulti_Scroll(object sender, EventArgs e)
        {
            UpdateMulti();
        }

        private void BarPhase_Scroll(object sender, EventArgs e)
        {
            UpdatePhase();
        }

        private void UpdatePhase()
        {
            Phase = barPhase.Value;
        }

        private void BarAttack_Scroll(object sender, EventArgs e)
        {
            Envelope.attack = GetAttack();
        }

        private void BarDecay_Scroll(object sender, EventArgs e)
        {
            Envelope.decay = GetDecay();
        }

        private void BarSustain_Scroll(object sender, EventArgs e)
        {
            Envelope.sustain = (double)barSustain.Value / barSustain.Maximum;
        }

        private void BarRelease_Scroll(object sender, EventArgs e)
        {
            Envelope.release = barRelease.Value * cp.SampleRate / envScale;
        }

        private void BtnRetrigger_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                ProtoSynth.Ue = UserEvent.Retrigger;
                userEventWaitHandle.Set();
            }
        }

        private void BtnRelease_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                ProtoSynth.Ue = UserEvent.Release;
                userEventWaitHandle.Set();
            }
        }

        private void BarDist_Scroll(object sender, EventArgs e)
        {
            UpdateDistortion();
        }

        public void Draw(int sampleNumber, double left, double right)
        {
            this.sampleNumber = (sampleNumber) % panWave.Width;
            if (this.sampleNumber == 0 && !painting)
            {
                pointsLeftTemp.CopyTo(pointsLeft, 0);
                pointsRightTemp.CopyTo(pointsRight, 0);
                panWave.Invalidate();
            }
            pointsLeftTemp[this.sampleNumber] = Convert.ToInt32((panWave.Size.Height / 4) * (1 - left));
            pointsRightTemp[this.sampleNumber] = Convert.ToInt32((panWave.Size.Height / 4) * (3 - right));
        }

        private void PanWave_Paint(object sender, PaintEventArgs e)
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

        private void ChkTone_CheckedChanged(object sender, EventArgs e)
        {
            SingleTone = chkTone.Checked;
        }

        private void UserInterfaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProtoSynth.Ue = UserEvent.Close;
            userEventWaitHandle.Set();
        }

        private void BarVolume_Scroll(object sender, EventArgs e)
        {
            UpdateAmplitude();
        }
    }
}