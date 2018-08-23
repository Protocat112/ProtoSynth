using NAudio.Wave;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ProtoSynth
{
    public static class ProtoSynth
    {
        private const int SAMPLE_RATE = 44100;
        private const int BIT_DEPTH = 16;
        private const int CHANNELS = 2;
        public enum UserEvent {
            Unset,
            Play,
            SetRecord,
            UnsetRecord,
            SetTone,
            UnsetTone,
            Close
        };
        private static UserInterfaceForm userInterfaceForm;
        private static WaveStream waveStream;
        private static bool exit;
        public static UserEvent Ue;

        internal static void Run()
        {
            // start user interface thread
            EventWaitHandle userEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            userInterfaceForm = new UserInterfaceForm(userEventWaitHandle, new ConstantProperties(SAMPLE_RATE, BIT_DEPTH, CHANNELS));
            Thread userInterfaceThread = new Thread(RunUserInterfaceForm);
            userInterfaceThread.Start();
            Ue = UserEvent.Unset;
            exit = false;
            // event loop
            while (!exit)
            {
                // wait for event signal
                userEventWaitHandle.WaitOne();
                switch (Ue)
                {
                    case UserEvent.Unset:
                        throw new Exception("User event unset!");
                    case UserEvent.Play:
                        Play();
                        break;
                    case UserEvent.Close:
                        Close();
                        break;
                }
            }
        }

        private static void Close()
        {
            exit = true;
        }

        private static void RunUserInterfaceForm()
        {
            Application.Run(userInterfaceForm);
            userInterfaceForm.Record = false;
        }

        private static void Play()
        {
            waveStream = new WaveStream(
                new WaveStreamProperties(
                    new ConstantProperties(SAMPLE_RATE, BIT_DEPTH, CHANNELS),
                    userInterfaceForm.Multi,
                    userInterfaceForm.Phase,
                    userInterfaceForm.Envelope,
                    userInterfaceForm.WaveType,
                    userInterfaceForm.Distortion),
                userInterfaceForm,
                userInterfaceForm.Record,
                userInterfaceForm.SingleTone,
                userInterfaceForm.Frequency,
                userInterfaceForm.Amplitude);
            DirectSoundOut output = new DirectSoundOut();
            output.Init(waveStream);
            output.Play();
        }
    }
}
