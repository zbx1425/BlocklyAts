using OpenBveApi.Runtime;
public class BlocklyAtsPlugin : IRuntime {
private VehicleSpecs __bve_vs;
private ElapseData __bve_ed;
private bool __bve_doorstate;
private bool[] __bve_keystate = new bool[16];
public void SetVehicleSpecs(VehicleSpecs __zbx_1) { __bve_vs = __zbx_1; }
public void SetReverser(int __zbx_1) { __bve_ed.Handles.Reverser = __zbx_1; }
public void SetPower(int __zbx_1) { __bve_ed.Handles.PowerNotch = __zbx_1; }
public void SetBrake(int __zbx_1) { __bve_ed.Handles.BrakeNotch = __zbx_1; }

Dictionary<string, Dictionary<string, dynamic>> atsfnc_iniload(string path) {
    var data = new Dictionary<string, Dictionary<string, dynamic>>();
    var section = "";
    foreach (var line in System.IO.File.ReadAllLines(path)) {
        var tline = line.Trim();
        if (tline.StartsWith("[") && tline.EndsWith("]")) {
            section = tline.Substring(1, tline.Length - 2);
            if (!data.ContainsKey(section)) data[section] = new Dictionary<string, dynamic>();
        }
        if (!tline.StartsWith("#") && tline.Contains("=")) {
            var parts = tline.Split(new[] { '=' }, 2);
            if (double.TryParse(parts[1], out double td)) data[section][parts[0]] = td;
            else if (parts[1].ToLowerInvariant() == "true") data[section][parts[0]] = true;
            else if (parts[1].ToLowerInvariant() == "false") data[section][parts[0]] = false;
            else data[section][parts[0]] = parts[1];
        }
    }
  return data;
}

void atsfnc_inisave(string path, Dictionary<string, Dictionary<string, dynamic>> data) {
    var sb = new System.Text.StringBuilder();
    foreach (var section in data) {
        sb.AppendLine(section.Key);
        foreach (var pair in section.Value) {
            sb.AppendFormat("{0}={1}\n", pair.Key, pair.Value);
        }
        sb.AppendLine();
    }
    System.IO.File.WriteAllText(path, sb.ToString());
}

private int __atsfnc_sound(int id, int state = int.MaxValue, double volume = double.NaN) {
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
        if (__bve_soundhandle[id] != null && __bve_soundhandle[id].Playing) {
            __bve_soundhandle[id].Stop();
            __bve_soundhandle[id] = null;
        }
        __atsval_sound_emulated[id] = state;
    } else if (state > -10000 && state <= 0) {
        if (__bve_soundhandle[id] != null && __bve_soundhandle[id].Playing) {
            __bve_soundhandle[id].Volume = (state + 10000) / 10000;
        } else {
            __bve_soundhandle[id] = __bve_playsound(id, (state + 10000) / 10000, 1, true);
        }
        __atsval_sound_emulated[id] = state;
    } else if (state == 1) {
        if (__bve_soundhandle[id] != null && __bve_soundhandle[id].Playing) {
            __bve_soundhandle[id].Stop();
        }
        __bve_soundhandle[id] = __bve_playsound(id, 1, 1, false);
        __atsval_sound_emulated[id] = 2;
    } else if (state == 2) {
        __atsval_sound_emulated[id] = state;
    } else if (state > 10000) {
        return __atsval_sound_emulated[id];
    } else {
        throw new System.ArgumentOutOfRangeException();
    }
    return 0;
}
public bool Load(LoadProperties __zbx_1) {
  ...
return true;
}
public void Unload() {
  ...
}
public void Initialize(InitializationModes __zbx_1) { int __atsarg_initindex = (int)__zbx_1;
  ...
}
public void Elapse(ElapseData __zbx_1) { __bve_ed = __zbx_1;
  ...
}
public void KeyDown(VirtualKeys __zbx_1) { int __atsarg_key = (int)__zbx_1; if (__atsarg_key > 15) return; __bve_keystate[__atsarg_key] = true;
  ...
}
public void KeyUp(VirtualKeys __zbx_1) { int __atsarg_key = (int)__zbx_1; if (__atsarg_key > 15) return; __bve_keystate[__atsarg_key] = false;
  ...
}
public void HornBlow(HornTypes __zbx_1) { int __atsarg_horntype = (int)__zbx_1;
  ...
}
public void DoorChange(DoorStates __zbx_1, DoorStates __zbx_2) { if ((__zbx_1 == DoorStates.None) == (__zbx_2 == DoorStates.None)) return; __bve_doorstate = __zbx_2 != DoorStates.None;
  ...
}
public void SetSignal(SignalData[] __zbx_1) { int __atsarg_signal = __zbx_1[0].Aspect;
  ...
}
public void SetBeacon(BeaconData __atsarg_beacondata) {
  ...
}
public void PerformAI(AIData __zbx_1) { }
}