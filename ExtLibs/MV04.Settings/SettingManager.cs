﻿using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Settings
{
    public enum Setting
    {
        CameraIP,
        CameraControlPort,
        CameraStreamUrl,
        AutoConnect,
        VideoSegmentLength,
        MediaSaveFolder,
        IrColorMode,
        GPSType,
        AltFormat,
        DistFormat,
        SpeedFormat,
        AutoRecordVideoStream,
        AutoStartSingleYaw,
        AutoStartCameraStream
    }

    public class SettingItem
    {
        public Setting Setting { get; set; }
        public string Value { get; set; }
        public Func<string, bool> Valid { get; set; }
        public SettingItem(Setting setting, string defaultValue, Func<string, bool> validator)
        {
            Setting = setting;
            Value = defaultValue;
            Valid = validator;
        }
    }

    public static class SettingManager
    {
        private static string FileName = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_settings.json";

        private static HashSet<SettingItem> SettingCollection_backup;
        private static HashSet<SettingItem> _SettingCollection = null;
        private static HashSet<SettingItem> SettingCollection
        {
            get
            {
                if (_SettingCollection == null)
                {
                    // Define settings and default values
                    _SettingCollection = new HashSet<SettingItem>
                    {
                        new SettingItem(Setting.CameraIP, "192.168.70.203", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && value.Count(c => c == '.') == 3
                            && value.Split('.').Length == 4
                            && value.Split('.').All(s => int.TryParse(s, out int i))
                            && value.Split('.').All(s => int.Parse(s) >= 0 && int.Parse(s) <= 255)
                        ),
                        new SettingItem(Setting.CameraControlPort, "10024", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && int.TryParse(value, out int port)
                            && port >= 1024 && port <= 65536
                        ),
                        new SettingItem(Setting.CameraStreamUrl, $"rtsp://192.168.70.203:554/live0", value =>
                            !string.IsNullOrWhiteSpace(value)
                            // TODO: Add more validation
                        ),
                        new SettingItem(Setting.AutoConnect, true.ToString(), value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                true.ToString(),
                                false.ToString()
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.VideoSegmentLength, "60", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && int.TryParse(value, out int i)
                            && i > 0
                        ),
                        new SettingItem(Setting.MediaSaveFolder, MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_media" +      Path.DirectorySeparatorChar, value =>
                            string.IsNullOrWhiteSpace(value)
                        ),
                        new SettingItem(Setting.IrColorMode, "WhiteHot", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                "WhiteHot",
                                "BlackHot",
                                "Color",
                                "ColorInverse"
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.GPSType, "WGS84", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                "WGS84",
                                "MGRS"
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.AltFormat, "m", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                "m",
                                "ft"
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.DistFormat, "m", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                "m",
                                "km",
                                "ft"
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.SpeedFormat, "mps", value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                "mps",
                                "kmph",
                                "knots"
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.AutoRecordVideoStream, true.ToString(), value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                true.ToString(),
                                false.ToString()
                            }.Contains(value)
                            ),
                        new SettingItem(Setting.AutoStartSingleYaw, true.ToString(), value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                true.ToString(),
                                false.ToString()
                            }.Contains(value)
                        ),
                        new SettingItem(Setting.AutoStartCameraStream, true.ToString(), value =>
                            !string.IsNullOrWhiteSpace(value)
                            && new List<string>(){
                                true.ToString(),
                                false.ToString()
                            }.Contains(value)
                        ),
                    };

                    // Create reset backup
                    SettingCollection_backup = new HashSet<SettingItem>();
                    foreach (var item in SettingCollection)
                    {
                        SettingCollection_backup.Add(new SettingItem(item.Setting, item.Value, item.Valid));
                    }

                    // Load saved values
                    Load();
                }
                return _SettingCollection;
            }
            set => _SettingCollection = value;
        }

        /// <summary>
        /// Retrieves the setting value for a setting type
        /// </summary>
        public static string Get(Setting setting)
        {
            return SettingCollection.FirstOrDefault(s => s.Setting == setting).Value;
        }

        /// <summary>
        /// Sets the value for a setting type
        /// </summary>
        /// <exception cref="InvalidConstraintException"></exception>
        public static void Set(Setting setting, string value)
        {
            SettingItem si = SettingCollection.FirstOrDefault(s => s.Setting == setting);
            if (si.Valid(value))
            {
                si.Value = value;
            }
            else
            {
                throw new InvalidConstraintException($"Invalid value ({value}) for setting {setting.ToString()}");
            }
        }

        /// <summary>
        /// Resets the settings to their original values
        /// </summary>
        public static void Reset()
        {
            SettingCollection = new HashSet<SettingItem>();
            foreach (var item in SettingCollection_backup)
            {
                SettingCollection.Add(new SettingItem(item.Setting, item.Value, null) { Valid = item.Valid });
            }
        }

        /// <summary>
        /// Saves the current settings to a JSON file
        /// </summary>
        public static void Save()
        {
            Dictionary<Setting, string> toSave = new Dictionary<Setting, string>();
            foreach (var item in SettingCollection)
            {
                toSave.Add(item.Setting, item.Value);
            }
            File.WriteAllText(FileName, toSave.ToJSON()); // create or owerwrite
        }

        /// <summary>
        /// Loads the repviously saved settings from a JSON file
        /// </summary>
        public static void Load()
        {
            try
            {
                // Read JSON
                Dictionary<Setting, string> loaded = File.ReadAllText(FileName).FromJSON<Dictionary<Setting, string>>();
                foreach (var item in loaded)
                {
                    SettingCollection.FirstOrDefault(s => s.Setting == item.Key).Value = item.Value;
                }
            }
            catch (Exception)
            {
                // If unsuccessful, reset and save the default values
                Reset();
                Save();
            }
        }

        /// <summary>
        /// Imports the settings from a chosen JSON file
        /// </summary>
        /// <param name="filePath">Path to the chosen JSON file</param>
        public static void Import(string filePath)
        {
            try
            {
                // Read JSON
                Dictionary<Setting, string> loaded = File.ReadAllText(filePath).FromJSON<Dictionary<Setting, string>>();
                foreach (var item in loaded)
                {
                    SettingCollection.FirstOrDefault(s => s.Setting == item.Key).Value = item.Value;
                }
            }
            catch (Exception)
            {
                // If unsuccessful, reset
                Reset();
            }

            Save();
        }

        /// <summary>
        /// Opens a dialog window where the settings can be set by hand
        /// </summary>
        public async static void OpenDialog()
        {
            // Open form
            using (SettingForm form = new SettingForm(SettingCollection))
            {
                // Show (async) dialog
                DialogResult dr = await Task.Run(() => form.ShowDialog());

                if (dr == DialogResult.OK)
                {
                    // Set values
                    SettingCollection = form.returnData;

                    // Save values
                    Save();
                }
            }
        }

        public static HashSet<SettingItem> GetSettings()
        {
            return SettingCollection;
        }

    }
}
