using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace ProtoSynth
{
    //synth output
    public class WaveStream : NAudio.Wave.WaveStream
    {
        private List<byte> data;
        private List<WaveTone> waveTones;
        public WaveStreamProperties Wsp { get; }
        private int sampleNumber;
        private UserInterfaceForm userInterfaceForm;
        private readonly bool record;
        private bool tone;

        public WaveStream(
            WaveStreamProperties wsp,
            UserInterfaceForm userInterfaceForm,
            bool record,
            bool tone,
            double toneFrequency,
            double toneAmplitude)
        {
            Wsp = wsp;
            data = new List<byte>();
            waveTones = new List<WaveTone>();
            if (tone)
            {
                waveTones.Add(new WaveTone(
                        new WaveToneProperties(
                            Wsp,
                            toneFrequency,
                            toneAmplitude)
                        )
                    );

            }
            sampleNumber = 0;
            this.userInterfaceForm = userInterfaceForm;
            this.record = record;
        }

        public override long Position { get; set; }

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
                return new WaveFormat(Wsp.Cp.SampleRate, Wsp.Cp.BitDepth, Wsp.Cp.Channels);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int samples = count / 2;
            StereoSample stereoSample;
            double left = 0;
            double right = 0;
            if (waveTones.Count > 0)
            {
                for (int i = 0; i < samples; i += 2)
                {
                    foreach (WaveTone waveTone in waveTones)
                    {
                        stereoSample = waveTone.GetNextSample(sampleNumber);
                        left += stereoSample.Left;
                        right += stereoSample.Right;
                    }
                    userInterfaceForm.Draw(sampleNumber, left, right);
                    sampleNumber++;
                    ConvertToByte(buffer, i, left, right);
                }
            }
            return count;
        }

        public void ConvertToByte(byte[] buffer, int i, double left, double right)
        {
            short leftShort = (short)Math.Round(left * (Math.Pow(2, 15) - 1));
            short rightShort = (short)Math.Round(right * (Math.Pow(2, 15) - 1));
            buffer[i * 2] = (byte)(leftShort & 0x00ff);
            buffer[i * 2 + 1] = (byte)((leftShort & 0xff00) >> 8);
            buffer[i * 2 + 2] = (byte)(rightShort & 0x00ff);
            buffer[i * 2 + 3] = (byte)((rightShort & 0xff00) >> 8);
            if (record)
            {
                for (int j = 0; j < 4; j++)
                {
                    data.Add(buffer[i * 2 + j]);
                }
            }
        }

        public List<byte> GetData()
        {
            int length = Wsp.Cp.SampleRate * (Wsp.Cp.BitDepth / 8) * Wsp.Cp.Channels;
            byte[] buffer = new byte[length];
            Read(buffer, 0, length);
            return data;
        }

        public void Retrigger()
        {
            if (waveTones.Count > 0)
            {
                sampleNumber = 0;
                foreach (WaveTone waveTone in waveTones)
                {
                    waveTone.Retrigger();
                }
            }
        }

        public void Release()
        {
            if (waveTones.Count > 0)
            {
                foreach (WaveTone waveTone in waveTones)
                {
                    waveTone.Release(sampleNumber);
                }
            }
        }
    }
}