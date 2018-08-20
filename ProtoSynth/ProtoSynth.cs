using NAudio.Wave;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ProtoSynth
{
    static class ProtoSynth
    {
        public enum UserEvent {
            Unset,
            Play,

        };
        private static UserInterfaceForm userInterfaceForm;
        private const int SAMPLE_RATE = 44100;
        private const int BIT_DEPTH = 16;
        private const int CHANNELS = 2;

        internal static void Run()
        {
            // start user interface thread
            EventWaitHandle userEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            userInterfaceForm = new UserInterfaceForm(userEventWaitHandle);
            Thread userInterfaceThread = new Thread(RunUserInterfaceForm);
            UserEvent userEvent = UserEvent.Unset;
            bool exit = false;
            userInterfaceThread.Start();
            // event loop
            while (!exit)
            {
                // wait for event signal
                userEventWaitHandle.WaitOne();
                switch (userEvent)
                {
                    case UserEvent.Unset:
                        throw new Exception("User event unset!");
                    case UserEvent.Play:
                        Play();
                        break;
                }
            }
        }

        private static void RunUserInterfaceForm()
        {
            Application.Run(userInterfaceForm);
        }

        private static void Play()
        {
            WaveStream waveStream = new WaveStream(SAMPLE_RATE, BIT_DEPTH, CHANNELS);
            //BlockAlignReductionStream stream = new BlockAlignReductionStream(waveStream);
            DirectSoundOut output = new DirectSoundOut();
            output.Init(waveStream);
            output.Play();
        }
    }
}
