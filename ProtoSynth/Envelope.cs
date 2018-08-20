namespace ProtoSynth
{
    public class Envelope
    {
        public int attack;
        public int decay;
        public double sustain;
        public int release;

        public Envelope(int attack, int decay, double sustain, int release)
        {
            this.attack = attack;
            this.decay = decay;
            this.sustain = sustain;
            this.release = release;
        }
    }
}