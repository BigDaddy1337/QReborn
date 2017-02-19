using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace TheQuestionReborn.Settings
{
    public sealed class ProgramState
    {
        private const string SettingsFileName = "Settings.xml";
        private static ProgramState state;
        private static readonly object SyncRoot = new object();

        public ProgramState()
        {
            Settings = new Settings();
        }

        public Settings Settings
        {
            get;
            set;
        }

        public void SaveSettings()
        {
            using (var storage = GetIsolatedStorageFile())
            {
                if (storage.FileExists(SettingsFileName))
                    storage.DeleteFile(SettingsFileName);

                using (var file = storage.OpenFile(SettingsFileName, FileMode.Create, FileAccess.Write))
                {
                    var serialisation = Serialize();

                    file.Write(serialisation, 0, serialisation.Length);
                }
            }
        }

        public static ProgramState CurrentState
        {
            get
            {
                if (state != null) return state;

                lock (SyncRoot)
                {
                    if (state == null)
                        // ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
                        state = LoadSettings();
                }

                return state;
            }
        }

        private static ProgramState LoadSettings()
        {
            try
            {
                using (var storage = GetIsolatedStorageFile())
                {
                    if (!storage.FileExists(SettingsFileName))
                        return new ProgramState();

                    var file = storage.OpenFile(SettingsFileName, FileMode.Open, FileAccess.Read);

                    using (var reader = new BinaryReader(file))
                    {
                        var bytes = reader.ReadBytes((int)file.Length);

                        return Deserialize(bytes);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return new ProgramState();
        }

        private static IsolatedStorageFile GetIsolatedStorageFile()
        {
            return IsolatedStorageFile.GetUserStoreForApplication();
        }

        private byte[] Serialize()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ProgramState));

                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, this);

                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        private static ProgramState Deserialize(byte[] bytes)
        {
            var serializer = new XmlSerializer(typeof(ProgramState));

            using (var stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);

                var result = serializer.Deserialize(stream) as ProgramState;

                return result ?? new ProgramState();
            }
        }
    }
}
