using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using System.Drawing;

namespace ProtoSynth
{
    public enum WaveTypes {Sine, Triangle, Saw, Square};

    public partial class ProtoSynthForm : Form
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
        private WaveTone tone;
        private double dist;
        private int sampleNumber;
        private int[] pointsLeft;
        private int[] pointsLeftTemp;
        private int[] pointsRight;
        private int[] pointsRightTemp;
        private bool painting;
        private int offset;

        public ProtoSynthForm()
        {
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
            try
            {
                frequency = Convert.ToDouble(txtFrequency.Text);
            }
            catch
            {
                frequency = 0;
            }
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
                tone = new WaveTone(this, frequency, amplitude, waveType, sampleRate, phase, multi, false, env, dist);
                stream = new BlockAlignReductionStream(tone);
                output = new DirectSoundOut();
                output.Init(stream);
                output.Play();
                play = true;
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
            tone = new WaveTone(this, frequency, amplitude, waveType, sampleRate, phase, multi, true, env, dist);
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

    public class WaveTone : WaveStream
    {
        private double frequency;
        private double amplitude;
        private int sampleNumber;
        private WaveTypes waveType;
        private int sampleRate;
        private Osc osc0;
        private Osc osc1;
        private Osc osc2;
        private Osc oscLeft;
        private Osc oscRight;
        private double multi;
        private bool write;
        private List<byte> data;
        private double phase;
        private double left;
        private double right;
        private Envelope env;
        private double dist;
        private ProtoSynthForm form;

        public WaveTone(ProtoSynthForm form, double frequency, double amplitude, WaveTypes waveType, int sampleRate, double phase, double multi, bool write, Envelope env, double dist)
        {
            data = new List<byte>();
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.waveType = waveType;
            this.sampleRate = sampleRate;
            this.phase = phase;
            this.write = write;
            this.multi = multi;
            this.env = env;
            this.dist = dist * 0.9;
            this.form = form;
            osc0 = new Osc(sampleRate, frequency, amplitude, env, waveType);
            if (phase > 0)
            {
                oscLeft = new Osc(sampleRate, frequency + phase / 100, amplitude, env, waveType);
                oscRight = new Osc(sampleRate, frequency - phase / 100, amplitude, env, waveType);
            }
            if (multi > 0)
            {
                osc1 = new Osc(sampleRate, frequency * ((15 + (multi / 100)) / 15), amplitude, env, waveType);
                osc2 = new Osc(sampleRate, frequency * ((16 - (multi / 100)) / 16), amplitude, env, waveType);
            }
        }

        public override long Position
        {
            get;
            set;
        }

        public override long Length
        {
            get
            {
                return long.MaxValue;
            }
        }

        public override WaveFormat WaveFormat
        {
            get
            {
                return new WaveFormat(44100, 16, 2);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            double value = 0;
            int samples = count / 2;
            for (int i = 0; i < samples; i += 2)
            {
                value = osc0.GetNextSample(sampleNumber);
                if (multi > 0)
                {
                    value = (value + osc1.GetNextSample(sampleNumber) + osc2.GetNextSample(sampleNumber)) / 3;
                }
                if (phase > 0)
                {
                    left = (value + oscLeft.GetNextSample(sampleNumber)) / 2;
                    right = (value + oscRight.GetNextSample(sampleNumber)) / 2;
                }
                else
                {
                    left = value;
                    right = value;
                }
                if (left > amplitude * (1 - dist))
                {
                    left = amplitude * (1 - dist);
                }
                else if (left < amplitude * (dist - 1))
                {
                    left = amplitude * (dist - 1);
                }
                left = left * (1 / (1 - dist));
                if (right > amplitude * (1 - dist))
                {
                    right = amplitude * (1 - dist);
                }
                else if (right < amplitude * (dist - 1))
                {
                    right = amplitude * (dist - 1);
                }
                right = right * (1 / (1 - dist));
                form.Draw(sampleNumber, left, right);
                sampleNumber++;
                ConvertToByte(buffer, i, left, right, write);
            } 
            return count;
        }

        public void ConvertToByte(byte[] buffer, int i, double left, double right, bool write)
        {
            short leftShort = (short)Math.Round(left * (Math.Pow(2, 15) - 1));
            short rightShort = (short)Math.Round(right * (Math.Pow(2, 15) - 1));
            buffer[i * 2] = (byte)(leftShort & 0x00ff);
            buffer[i * 2 + 1] = (byte)((leftShort & 0xff00) >> 8);
            buffer[i * 2 + 2] = (byte)(rightShort & 0x00ff);
            buffer[i * 2 + 3] = (byte)((rightShort & 0xff00) >> 8);
            if (write)
            {
                for (int j = 0; j < 4; j++)
                {
                    data.Add(buffer[i * 2 + j]);
                }
            }
        }

        public List<byte> getData()
        {
            byte[] buffer = new byte[sampleRate * 4];
            Read(buffer, 0, sampleRate * 4);
            return data;
        }

        public void Retrigger()
        {
            sampleNumber = 0;
            osc0.Retrigger();
            if (phase > 0)
            {
                oscLeft.Retrigger();
                oscRight.Retrigger();
            }
            if (multi > 0)
            {
                osc1.Retrigger();
                osc2.Retrigger();
            }
        }

        public void Release()
        {
            osc0.Release(sampleNumber);
            if (phase > 0)
            {
                oscLeft.Release(sampleNumber);
                oscRight.Release(sampleNumber);
            }
            if (multi > 0)
            {
                osc1.Release(sampleNumber);
                osc2.Release(sampleNumber);
            }
        }
    }

    public class Osc
    {
        private int sampleRate;
        private double frequency;
        private double amplitude;
        private double samplesPerOsc;
        private double depthInOsc;
        private double result;
        private Envelope env;
        private WaveTypes waveType;
        private double envf;
        private double releaseEnvf;
        private bool release;
        private int releaseSample;

        public Osc(int sampleRate, double frequency, double amplitude, Envelope env, WaveTypes waveType)
        {
            this.frequency = frequency;
            this.sampleRate = sampleRate;
            this.amplitude = amplitude;
            this.env = env;
            this.waveType = waveType;
            samplesPerOsc = sampleRate / frequency;
        }

        public double GetNextSample(int sample)
        {
            depthInOsc = (sample % samplesPerOsc) / samplesPerOsc;
            switch (waveType)
            {
                case WaveTypes.Sine:
                    result = Math.Sin(depthInOsc * Math.PI * 2);
                    break;
                case WaveTypes.Triangle:
                    if (depthInOsc < 0.25)
                    {
                        result = depthInOsc * 4;
                    }
                    else if (depthInOsc < 0.75)
                    {
                        result = 2 - depthInOsc * 4;
                    }
                    else
                    {
                        result = depthInOsc * 4 - 4;
                    }
                    break;
                case WaveTypes.Saw:
                    if (depthInOsc < 0.5)
                    {
                        result = depthInOsc * 2;
                    }
                    else
                    {
                        result = depthInOsc * 2 - 2;
                    }
                    break;
                case WaveTypes.Square:
                    if (depthInOsc < 0.5)
                    {
                        result = 1;
                    }
                    else
                        result = -1;
                    break;
            }
            if (release)
            {
                if (sample < releaseSample + env.release)
                {
                    envf = ((double)(releaseSample - sample) / env.release + 1) * releaseEnvf;
                }
                else
                {
                    envf = 0;
                }
            }
            else
            {
                if (sample < env.attack)
                {
                    envf = ((double)sample / env.attack);
                }
                else if (sample < env.attack + env.decay)
                {
                    envf = (((double)env.decay - (sample - env.attack)) / env.decay) * (1 - env.sustain) + env.sustain;
                }
                else
                {
                    envf = env.sustain;
                }
            }
            return result * envf * amplitude;
        }

        public void Retrigger()
        {
            release = false;
        }

        public void Release(int releaseSample)
        {
            this.releaseSample = releaseSample;
            releaseEnvf = envf;
            release = true;
        }
    }

    public class FileWriter
    {
        private FileStream stream;
        private BinaryWriter writer;
        private int RIFF;
        private int WAVE;
        private int formatChunkSize;
        private int headerSize;
        private int format;
        private short formatType;
        private short tracks;
        private short bitsPerSample;
        private short frameSize;
        private int bytesPerSecond;
        private int waveSize;
        private int data;
        private int samples;
        private int dataChunkSize;
        private int fileSize;
        private long bytes;

        public FileWriter(byte[] sampleData, long sampleCount, int sampleRate)
        {
            try
            {
                stream = File.Create("temp.wav");
                writer = new BinaryWriter(stream);
                RIFF = 0x46464952;
                WAVE = 0x45564157;
                formatChunkSize = 16;
                headerSize = 8;
                format = 0x20746D66;
                formatType = 1;
                tracks = 2;
                bitsPerSample = 16;
                frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                bytesPerSecond = sampleRate * frameSize;
                waveSize = 4;
                data = 0x61746164;
                samples = (int)sampleCount;
                dataChunkSize = samples * frameSize;
                fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
                writer.Write(RIFF);
                writer.Write(fileSize);
                writer.Write(WAVE);
                writer.Write(format);
                writer.Write(formatChunkSize);
                writer.Write(formatType);
                writer.Write(tracks);
                writer.Write(sampleRate);
                writer.Write(bytesPerSecond);
                writer.Write(frameSize);
                writer.Write(bitsPerSample);
                writer.Write(data);
                writer.Write(dataChunkSize);
                bytes = sampleCount * tracks * (bitsPerSample / 8);
                for (int i = 0; i < bytes; i++)
                {
                    stream.WriteByte(sampleData[i]);
                }
                stream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }

    public class Envelope
    {
        public int attack;
        public int decay;
        public double sustain;
        public int release;

        public Envelope(int attack, int decay, double sustain, int release)
        {
            this.attack = attack;
            this.decay = decay;
            this.sustain = sustain;
            this.release = release;
        }
    }
}