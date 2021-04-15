using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBveApi.Runtime;

// Start of BlocklyAts boilerplate code.
internal class BlocklyAtsCompanion {
    
    public IRuntime Plugin;
  
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
    
    public class TimerTuple {
        public double Interval;
        public double LastTrigger;
        public bool Cycle;
        public bool Enabled;
    }
    
    public Dictionary<string, TimerTuple> TimerState = new Dictionary<string, TimerTuple>();
    public Dictionary<string, Dictionary<string, dynamic>> ConfigData;
    
    private string pluginDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).TrimEnd('\\', '/');
    private PlaySoundDelegate playSoundDelegate;
    private SoundHandle[] soundHandles = new SoundHandle[256];
    // To achieve the same behavior as Win32 plugin interface at bve_get_sound_internal
    private int[] emulatedSoundState = new int[256];
    
    public BlocklyAtsCompanion(LoadProperties prop, IRuntime plugin) {
        playSoundDelegate = prop.PlaySound;
        prop.Panel = Panel;
        for (int i = 0; i < 256; i++) emulatedSoundState[i] = -10000;
        this.Plugin = plugin;
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
    
    private void CallTimerHandler(string name) {
        var methodInfo = Plugin.GetType().GetMethod("_etimertick_" + name);
        if (methodInfo != null) methodInfo.Invoke(Plugin, new object[] { });
    }
    
    public void ResetTimer(string name, bool callHandler) {
        if (!TimerState.ContainsKey(name)) return;
        if (EData == null) {
            // Force a timer reset later if the ElapseData is currently not ready.
            TimerState[name].LastTrigger = double.MaxValue;
        } else {
            TimerState[name].LastTrigger = EData.TotalTime.Milliseconds;
        }
        TimerState[name].Enabled = true;
        if (callHandler) CallTimerHandler(name);
    }
    
    public void CancelTimer(string name, bool callHandler) {
        if (!TimerState.ContainsKey(name)) return;
        TimerState[name].Enabled = false;
        if (callHandler) CallTimerHandler(name);
    }
    
    public void SetTimer(string name, double interval, bool cycle) {
        if (interval > 0) {
            TimerState[name] = new TimerTuple();
            TimerState[name].Interval = interval;
            TimerState[name].Cycle = cycle;
            ResetTimer(name, false);
        } else {
            CancelTimer(name, false);
        }
    }
    
    public void UpdateTimer() {
        foreach (var entry in TimerState) {
            if (!entry.Value.Enabled) continue;
            if (entry.Value.LastTrigger > EData.TotalTime.Milliseconds) {
                // From the future? Maybe caused by a station jump, just reset it
                ResetTimer(entry.Key, false);
            } else if (EData.TotalTime.Milliseconds >= entry.Value.LastTrigger + entry.Value.Interval) {
                CallTimerHandler(entry.Key);
                if (entry.Value.Cycle) {
                    ResetTimer(entry.Key, false);
                } else {
                    CancelTimer(entry.Key, false);
                }
            }
        }
    }
}
