namespace ProtoSynth
{
    class WaveToneProperties
    {
        private readonly int sampleRate;
        private double Frequency { get; set; }
        private double Amplitude { get; set; }
        private WaveTypes WaveType { get; set; }

        public WaveToneProperties(int sampleRate)
        {
            this.sampleRate = sampleRate;
        }
    }
}
