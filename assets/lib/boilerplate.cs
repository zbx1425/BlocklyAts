using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBveApi.Runtime;

internal class AtsCompanion {
  
    public VehicleSpecs VSpec;
    public ElapseData EData;
    public int[] Panel = new int[256];
    public DoorStates DoorState;
    public bool LegacyDoorState {
        get {
            return this.DoorState != DoorStates.None;
        }
    }
    public bool[] KeyState = new bool[16];
    public Dictionary<string, Dictionary<string, dynamic>> ConfigData;
    
    private string pluginDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).TrimEnd('\\', '/');
    private PlaySoundDelegate playSoundDelegate;
    private SoundHandle[] soundHandles = new SoundHandle[256];
    // To achieve the same behavior as Win32 plugin interface at bve_get_sound_internal
    private int[] emulatedSoundState = new int[256];
    
    public AtsCompanion(LoadProperties prop) {
        playSoundDelegate = prop.PlaySound;
        prop.Panel = Panel;
        for (int i = 0; i < 256; i++) emulatedSoundState[i] = -10000;
    }
  
    public void LoadConfig(string path) {
      if (!File.Exists(Path.Combine(pluginDir, path))) return;
      ConfigData = new Dictionary<string, Dictionary<string, dynamic>>();
      var section = "";
      foreach (var line in System.IO.File.ReadAllLines(Path.Combine(pluginDir, path))) {
          var tline = line.Trim();
          if (tline.StartsWith("[") && tline.EndsWith("]")) {
              section = tline.Substring(1, tline.Length - 2).Trim().ToLowerInvariant();
              if (!ConfigData.ContainsKey(section)) ConfigData[section] = new Dictionary<string, dynamic>();
          }
          if (!tline.StartsWith("#") && tline.Contains("=")) {
              var parts = tline.Split(new[] { '=' }, 2);
              string confKey = parts[0].Trim().ToLowerInvariant();
              string confValue = parts[1].Trim();
              double td;
              if (double.TryParse(confValue, out td)) ConfigData[section][confKey] = td;
              else if (confValue.ToLowerInvariant() == "true") ConfigData[section][confKey] = true;
              else if (confValue.ToLowerInvariant() == "false") ConfigData[section][confKey] = false;
              else ConfigData[section][confKey] = confValue;
          }
      }
    }

    public void SaveConfig(string path) {
      var sb = new System.Text.StringBuilder();
      foreach (var section in ConfigData) {
          sb.AppendFormat("[{0}]\n", section.Key);
          foreach (var pair in section.Value) {
              sb.AppendFormat("{0}={1}\n", pair.Key, pair.Value);
          }
          sb.AppendLine();
      }
      System.IO.File.WriteAllText(Path.Combine(pluginDir, path), sb.ToString());
    }

    public int AccessLegacySound(int id, int state = int.MaxValue, double volume = double.NaN) {
      if (state == 0 && double.IsNaN(volume)) {
          if (volume <= 0) {
              state = -10000;
          } else if (volume >= 100) {
              state = 0;
          } else {
              state = (int)(volume * 100) - 10000;
          }
      }
      if (state == -10000) {
          if (soundHandles[id] != null && soundHandles[id].Playing) {
              soundHandles[id].Stop();
              soundHandles[id] = null;
          }
          emulatedSoundState[id] = state;
      } else if (state > -10000 && state <= 0) {
          if (soundHandles[id] != null && soundHandles[id].Playing) {
              soundHandles[id].Volume = (state + 10000) / 10000;
          } else {
              soundHandles[id] = playSoundDelegate(id, (state + 10000) / 10000, 1, true);
          }
          emulatedSoundState[id] = state;
      } else if (state == 1) {
          if (soundHandles[id] != null && soundHandles[id].Playing) {
              soundHandles[id].Stop();
          }
          soundHandles[id] = playSoundDelegate(id, 1, 1, false);
          emulatedSoundState[id] = 2;
      } else if (state == 2) {
          emulatedSoundState[id] = state;
      } else if (state > 10000) {
          return emulatedSoundState[id];
      } else {
          throw new ArgumentOutOfRangeException();
      }
      return 0;
    }
    
    public dynamic GetConfig(string part, string key, dynamic defaultValue = null) {
        part = part.Trim().ToLowerInvariant();
        key = key.Trim().ToLowerInvariant();
        if (ConfigData != null && ConfigData.ContainsKey(part) && ConfigData[part].ContainsKey(key)) {
            return ConfigData[part][key];
        } else {
            return defaultValue;
        }
    }
    
    public void SetConfig(string part, string key, dynamic confValue = null) {
        part = part.Trim().ToLowerInvariant();
        key = key.Trim().ToLowerInvariant();
        if (ConfigData == null) ConfigData = new Dictionary<string, Dictionary<string, dynamic>>();
        if (!ConfigData.ContainsKey(part)) ConfigData.Add(part, new Dictionary<string, dynamic>());
        if (!ConfigData[part].ContainsKey(key)) {
            ConfigData[part].Add(key, confValue);
        } else {
            ConfigData[part][key] = confValue;
        }
    }
}