using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace ProtoSynth
{
    //synth output
    public class WaveStream : NAudio.Wave.WaveStream
    {
        private List<byte> data;
        private double left;
        private double right;
        private int sampleRate;
        private int bitDepth;
        private int channels;
        private List<WaveTone> waveTones;

        public WaveStream(int sampleRate, int bitDepth, int channels)
        {
            this.sampleRate = sampleRate;
            this.bitDepth = bitDepth;
            this.channels = channels;
            data = new List<byte>();
            waveTones = new List<WaveTone>();
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
                return new WaveFormat(sampleRate, bitDepth, channels);
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
}