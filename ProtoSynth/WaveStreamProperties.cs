namespace ProtoSynth
{
    public struct WaveStreamProperties
    {
        public int SampleRate { get; }
        public int Channels { get; }
        public int BitDepth { get; }
        public double Multi { get; set; }
        public double Phase { get; set; }
        public Envelope Envelope { get; set; }
        public WaveTypes WaveType { get; set; }
        public double Distortion { get; set; }

        public WaveStreamProperties(
            int sampleRate,
            int channels,
            int bitDepth,
            double multi,
            double phase,
            Envelope envelope,
            WaveTypes waveType, 
            double distortion)
        {
            SampleRate = sampleRate;
            Channels = channels;
            BitDepth = bitDepth;
            Multi = multi;
            Phase = phase;
            Envelope = envelope;
            WaveType = waveType;
            Distortion = distortion;
        }
    }
}
