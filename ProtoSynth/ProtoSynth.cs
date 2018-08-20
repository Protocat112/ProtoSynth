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
        private const int SAMPLE_RATE = 44100;
        private const int BIT_DEPTH = 16;
        private const int CHANNELS = 2;
        private static UserInterfaceForm UserInterfaceForm;
        private static WaveStream WaveStream;

        internal static void Run()
        {
            // start user interface thread
            EventWaitHandle userEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            UserInterfaceForm = new UserInterfaceForm(userEventWaitHandle);
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
            Application.Run(UserInterfaceForm);
        }

        private static void Play()
        {
            WaveStream = new WaveStream(SAMPLE_RATE, BIT_DEPTH, CHANNELS);
            DirectSoundOut output = new DirectSoundOut();
            output.Init(WaveStream);
            output.Play();
        }
    }
}
