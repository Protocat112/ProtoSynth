namespace ProtoSynth
{
    public struct ConstantProperties
    {
        public int SampleRate { get; }
        public int BitDepth { get; }
        public int Channels { get; }

        public ConstantProperties(int sampleRate, int bitDepth, int channels)
        {
            SampleRate = sampleRate;
            BitDepth = bitDepth;
            Channels = channels;
        }
    }
}
