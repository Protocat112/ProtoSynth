using System;
using System.Windows.Forms;
using System.IO;

namespace ProtoSynth
{
    public class FileWriter
    {
        private FileStream stream;
        private BinaryWriter writer;
        private int RIFF;
        private int WAVE;
        private int formatChunkSize;
        private int headerSize;
        private int format;
        private short formatType;
        private short tracks;
        private short bitsPerSample;
        private short frameSize;
        private int bytesPerSecond;
        private int waveSize;
        private int data;
        private int samples;
        private int dataChunkSize;
        private int fileSize;
        private long bytes;

        public FileWriter(byte[] sampleData, long sampleCount, int sampleRate)
        {
            try
            {
                stream = File.Create("temp.wav");
                writer = new BinaryWriter(stream);
                RIFF = 0x46464952;
                WAVE = 0x45564157;
                formatChunkSize = 16;
                headerSize = 8;
                format = 0x20746D66;
                formatType = 1;
                tracks = 2;
                bitsPerSample = 16;
                frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                bytesPerSecond = sampleRate * frameSize;
                waveSize = 4;
                data = 0x61746164;
                samples = (int)sampleCount;
                dataChunkSize = samples * frameSize;
                fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
                writer.Write(RIFF);
                writer.Write(fileSize);
                writer.Write(WAVE);
                writer.Write(format);
                writer.Write(formatChunkSize);
                writer.Write(formatType);
                writer.Write(tracks);
                writer.Write(sampleRate);
                writer.Write(bytesPerSecond);
                writer.Write(frameSize);
                writer.Write(bitsPerSample);
                writer.Write(data);
                writer.Write(dataChunkSize);
                bytes = sampleCount * tracks * (bitsPerSample / 8);
                for (int i = 0; i < bytes; i++)
                {
                    stream.WriteByte(sampleData[i]);
                }
                stream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}