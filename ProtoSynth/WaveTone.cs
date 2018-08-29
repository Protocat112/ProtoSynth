using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace ProtoSynth
{
    //note output
    public class WaveTone
    {
        public WaveToneProperties Wtp { get; }
        private Osc osc0;
        private Osc osc1;
        private Osc osc2;
        private Osc oscLeft;
        private Osc oscRight;
        private WaveStream waveStream;

        public WaveTone(WaveStream waveStream, WaveToneProperties waveToneProperties)
        {
            this.waveStream = waveStream;
            Wtp = waveToneProperties;
            osc0 = new Osc(
                this,
                Wtp.Wsp.Cp.SampleRate,
                Wtp.Frequency,
                Wtp.Amplitude,
                Wtp.Wsp.Envelope,
                Wtp.Wsp.WaveType);
            if (Wtp.Wsp.Phase > 0)
            {
                oscLeft = new Osc(
                    this,
                    Wtp.Wsp.Cp.SampleRate,
                    Wtp.Frequency + Wtp.Wsp.Phase / 100,
                    Wtp.Amplitude,
                    Wtp.Wsp.Envelope,
                    Wtp.Wsp.WaveType);
                oscRight = new Osc(
                    this,
                    Wtp.Wsp.Cp.SampleRate,
                    Wtp.Frequency - Wtp.Wsp.Phase / 100,
                    Wtp.Amplitude,
                    Wtp.Wsp.Envelope,
                    Wtp.Wsp.WaveType);
            }
            if (Wtp.Wsp.Multi > 0)
            {
                osc1 = new Osc(
                    this,
                    Wtp.Wsp.Cp.SampleRate,
                    Wtp.Frequency * ((15 + (Wtp.Wsp.Phase / 100)) / 15),
                    Wtp.Amplitude,
                    Wtp.Wsp.Envelope,
                    Wtp.Wsp.WaveType);
                osc2 = new Osc(
                    this,
                    Wtp.Wsp.Cp.SampleRate,
                    Wtp.Frequency * ((16 - (Wtp.Wsp.Phase / 100)) / 16),
                    Wtp.Amplitude,
                    Wtp.Wsp.Envelope,
                    Wtp.Wsp.WaveType);
            }
        }

        public StereoSample GetNextSample(int sampleNumber)
        {
            double center = 0;
            center = osc0.GetNextSample(sampleNumber);
            if (Wtp.Wsp.Multi > 0)
            {
                center = (center + osc1.GetNextSample(sampleNumber) + osc2.GetNextSample(sampleNumber)) / 3;
            }
            double left;
            double right;
            if (Wtp.Wsp.Phase > 0)
            {
                left = (center + oscLeft.GetNextSample(sampleNumber)) / 2;
                right = (center + oscRight.GetNextSample(sampleNumber)) / 2;
            }
            else
            {
                left = center;
                right = center;
            }
            if (left > Wtp.Amplitude * (1 - Wtp.Wsp.Distortion))
            {
                left = Wtp.Amplitude * (1 - Wtp.Wsp.Distortion);
            }
            else if (left < Wtp.Amplitude * (Wtp.Wsp.Distortion - 1))
            {
                left = Wtp.Amplitude * (Wtp.Wsp.Distortion - 1);
            }
            left = left * (1 / (1 - Wtp.Wsp.Distortion));
            if (right > Wtp.Amplitude * (1 - Wtp.Wsp.Distortion))
            {
                right = Wtp.Amplitude * (1 - Wtp.Wsp.Distortion);
            }
            else if (right < Wtp.Amplitude * (Wtp.Wsp.Distortion - 1))
            {
                right = Wtp.Amplitude * (Wtp.Wsp.Distortion - 1);
            }
            right = right * (1 / (1 - Wtp.Wsp.Distortion));
            return new StereoSample(left, right);
        }

        internal void RemoveTone()
        {
            waveStream.RemoveTone(this);
        }

        public void Retrigger()
        {
            osc0.Retrigger();
            if (Wtp.Wsp.Phase > 0)
            {
                oscLeft.Retrigger();
                oscRight.Retrigger();
            }
            if (Wtp.Wsp.Multi > 0)
            {
                osc1.Retrigger();
                osc2.Retrigger();
            }
        }

        public void Release(int sampleNumber)
        {
            osc0.Release(sampleNumber);
            if (Wtp.Wsp.Phase > 0)
            {
                oscLeft.Release(sampleNumber);
                oscRight.Release(sampleNumber);
            }
            if (Wtp.Wsp.Multi > 0)
            {
                osc1.Release(sampleNumber);
                osc2.Release(sampleNumber);
            }
        }
    }
}