namespace ProtoSynth
{
    public struct WaveStreamProperties
    {
        public ConstantProperties Cp { get;}
    public double Multi { get; set; }
        public double Phase { get; set; }
        public Envelope Envelope { get; set; }
        public WaveTypes WaveType { get; set; }
        public double Distortion { get; set; }

        public WaveStreamProperties(
            ConstantProperties cp,
            double multi,
            double phase,
            Envelope envelope,
            WaveTypes waveType, 
            double distortion)
        {
            Cp = cp;
            Multi = multi;
            Phase = phase;
            Envelope = envelope;
            WaveType = waveType;
            Distortion = distortion;
        }
    }
}
