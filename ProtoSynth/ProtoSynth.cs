using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            Close,
            Stop,
            Release,
            Retrigger,
            KeyDown,
            KeyUp
        };

        private static UserInterfaceForm userInterfaceForm;
        private static WaveStream waveStream;
        private static bool exit;
        public static UserEvent Ue { get; internal set; }
        private static DirectSoundOut output;
        public static Keys KeyDown { get; internal set; }
        public static Keys KeyUp { get; internal set; }
        private static List<Note> notes;

        internal static void Run()
        {
            // start user interface thread
            EventWaitHandle userEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            userInterfaceForm = new UserInterfaceForm(userEventWaitHandle, new ConstantProperties(SAMPLE_RATE, BIT_DEPTH, CHANNELS));
            Thread userInterfaceThread = new Thread(RunUserInterfaceForm);
            userInterfaceThread.Start();
            Ue = UserEvent.Unset;
            notes = JsonConvert.DeserializeObject<List<Note>>(JsonNotes.JsonNoteString);
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
                    case UserEvent.Stop:
                        Stop();
                        break;
                    case UserEvent.Close:
                        Close();
                        break;
                    case UserEvent.Retrigger:
                        Retrigger();
                        break;
                    case UserEvent.Release:
                        Release();
                        break;
                    case UserEvent.KeyDown:
                        KeyDownEvent();
                        break;
                    case UserEvent.KeyUp:
                        KeyUpEvent();
                        break;
                }
            }
        }

        private static void KeyDownEvent()
        {
            waveStream.AddTone(GetFrequency(KeyDown), userInterfaceForm.Amplitude);
        }

        private static void KeyUpEvent()
        {
            waveStream.ReleaseTone(GetFrequency(KeyUp));
        }

        private static double GetFrequency(Keys key)
        {
            string note = "";
            switch (key)
            {
                case Keys.A:
                    note = "A4";
                    break;
                case Keys.W:
                    note = "A#4";
                    break;
                case Keys.S:
                    note = "B4";
                    break;
                case Keys.D:
                    note = "C5";
                    break;
                case Keys.R:
                    note = "C#5";
                    break;
                case Keys.F:
                    note = "D5";
                    break;
                case Keys.T:
                    note = "D#5";
                    break;
                case Keys.G:
                    note = "E5";
                    break;
            }
            if (note == "")
            {
                return 0;
            }
            else
            {
                return notes.Find(x => x.note == note).frequency;
            }
        }


        private static void Release()
        {
            waveStream.Release();
        }

        private static void Retrigger()
        {
            waveStream.Retrigger();
        }

        private static void Stop()
        {
            output.Stop();
            output.Dispose();
            waveStream.Dispose();
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
            output = new DirectSoundOut();
            output.Init(waveStream);
            output.Play();
        }
    }
}
