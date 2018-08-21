namespace ProtoSynth
{
    public struct WaveToneProperties
    {
        public WaveStreamProperties Wsp { get; }
        public double Frequency { get; set; }
        public double Amplitude { get; set; }

        public WaveToneProperties(
            WaveStreamProperties wsp,
            double frequency,
            double amplitude)
        {
            Wsp = wsp;
            Frequency = frequency;
            Amplitude = amplitude;
        }
    }
}
