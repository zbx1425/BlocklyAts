public class FunctionCompanion {
  
  private dynamic _c;
  
  public FunctionCompanion(object c) {
    _c = c;
  }
  
  private class TimerTuple {
    public double Interval;
    public double LastTrigger;
    public bool Cycle;
    public bool Enabled;
  }
  
  public class AtsCustomException : Exception {
    public AtsCustomException(string message) : base(message) { }
  }
  
  private Dictionary<string, TimerTuple> TimerState = new Dictionary<string, TimerTuple>();
  private Dictionary<string, Dictionary<string, dynamic>> ConfigData;
  
  public void LoadConfig(string path) {
    if (!File.Exists(Path.Combine(_c.PluginDirectory, path))) return;
    ConfigData = new Dictionary<string, Dictionary<string, dynamic>>();
    var section = "";
    foreach (var line in System.IO.File.ReadAllLines(Path.Combine(_c.PluginDirectory, path))) {
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
    System.IO.File.WriteAllText(Path.Combine(_c.PluginDirectory, path), sb.ToString());
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
    _c.CallImplFunc(name);
  }
  
  public void ResetTimer(string name, bool callHandler) {
    if (!TimerState.ContainsKey(name)) return;
    if (!_c.EData_Ready) {
      // Force a timer reset later if the ElapseData is currently not ready.
      TimerState[name].LastTrigger = double.MaxValue;
    } else {
      TimerState[name].LastTrigger = _c.EData_TotalTime;
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
      if (entry.Value.LastTrigger > _c.EData_TotalTime) {
        // From the future? Maybe caused by a station jump, just reset it
        ResetTimer(entry.Key, false);
      } else if (_c.EData_TotalTime >= entry.Value.LastTrigger + entry.Value.Interval) {
        CallTimerHandler(entry.Key);
        if (entry.Value.Cycle) {
          ResetTimer(entry.Key, false);
        } else {
          CancelTimer(entry.Key, false);
        }
      }
    }
  }
  
  public void MsgBox(string text) {
    MessageBox.Show(text, "BlocklyATS Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
  }
}

// This helper class simulates the behavior of implicit type conversion like Javascript.
public static class C {

  public static int Int(object src) {
    try {
      try {
        return Convert.ToInt32(src); 
      } catch {
        return (int)Convert.ToDouble(src);
      }
    } catch (FormatException ex) {
      throw new FormatException("\"" + src.ToString() + "\" is not numeric; " + ex.ToString());
    }
  }

  public static double Dbl(object src) {
    try {
      return Convert.ToDouble(src);
    } catch (FormatException ex) {
      throw new FormatException("\"" + src.ToString() + "\" is not numeric; " + ex.ToString());
    }
  }

  public static bool CanConvertToDouble(object src) {
    try {
      Convert.ToDouble(src);
      return true;
    } catch {
      return false;
    }
  }

  public static bool Bool(object src) {
    try {
      return Convert.ToBoolean(src);
    } catch (FormatException ex) {
      throw new FormatException("\"" + src.ToString() + "\" cannot be represented by Boolean; " + ex.ToString());
    }
  }

  public static bool CanConvertToBool(object src) {
    try {
      Convert.ToBoolean(src);
      return true;
    } catch {
      return false;
    }
  }
  
  public static string Str(object src) {
    return Convert.ToString(src);
  }
  
  public static IEnumerable<double> EDbl(IEnumerable<object> src) {
    return src.Select(e => Dbl(e));
  }

  public static dynamic Add(dynamic a, dynamic b) {
    if (IsNumeric(a) && IsNumeric(b)) {
      return Dbl(a) + Dbl(b);
    } else {
      return a + b;
    }
  }
  
  public static dynamic Sub(dynamic a, dynamic b) {
    if (IsNumeric(a) && IsNumeric(b)) {
      return Dbl(a) - Dbl(b);
    } else {
      return a - b;
    }
  }

  public static dynamic Mul(dynamic a, dynamic b) {
    if (IsNumeric(a) && IsNumeric(b)) {
      return Dbl(a) * Dbl(b);
    } else {
      return a * b;
    }
  }
  public static dynamic Div(dynamic a, dynamic b) {
    if (IsNumeric(a) && IsNumeric(b)) {
      return Dbl(a) / Dbl(b);
    } else {
      return a / b;
    }
  }

  public static bool IsNumeric(object o) {   
    switch (Type.GetTypeCode(o.GetType())) {
      case TypeCode.Byte:
      case TypeCode.SByte:
      case TypeCode.UInt16:
      case TypeCode.UInt32:
      case TypeCode.UInt64:
      case TypeCode.Int16:
      case TypeCode.Int32:
      case TypeCode.Int64:
      case TypeCode.Decimal:
      case TypeCode.Double:
      case TypeCode.Single:
        return true;
      case TypeCode.String:
        double discard;
        return double.TryParse(o.ToString(), out discard);
      default:
        return false;
    }
  }
}
