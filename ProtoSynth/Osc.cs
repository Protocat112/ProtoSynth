using System;

namespace ProtoSynth
{
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
}