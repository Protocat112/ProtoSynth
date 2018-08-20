namespace ProtoSynth
{
    class WaveStreamProperties
    {
        private readonly int sampleRate;
        private readonly int channels;
        private readonly int bitDepth;
        private double Multi { get; set; }
        private double Phase { get; set; }
        private Envelope Env { get; set; }

        public WaveStreamProperties(int sampleRate)
        {
            this.sampleRate = sampleRate;
        }
    }
}
