namespace ProtoSynth
{
    public struct StereoSample
    {
        public double Left { get; }
        public double Right { get; }

    public StereoSample(double left, double right)
        {
            Left = left;
            Right = right;
        }
    }
}