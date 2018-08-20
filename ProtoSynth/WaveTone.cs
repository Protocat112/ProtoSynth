using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace ProtoSynth
{
    //note output
    public class WaveTone
    {
        internal WaveToneProperties waveToneProperties;
        private Osc osc0;
        private Osc osc1;
        private Osc osc2;
        private Osc oscLeft;
        private Osc oscRight;
        private List<byte> data;
        private double left;
        private double right;

        public WaveTone(int sampleRate)
        {
            waveToneProperties = new WaveToneProperties(sampleRate);
            data = new List<byte>();
            //this.dist = dist * 0.9;
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
}