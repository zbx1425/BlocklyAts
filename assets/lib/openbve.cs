public class ApiProxy : IRuntime {
  
  // Increase this value if you need more panel variables.
  // Please note that only OpenBVE supports more than 256 panel variables.
  private const int MAX_PANEL_INDEX = 255;
  private const string MAX_PANEL_ERROR_MSG = "Panel index must be between 0-255.";
  
  // Increase this value if you need more Ats sounds.
  // Please note that only OpenBVE supports more than 256 sounds.
  private const int MAX_SOUND_INDEX = 255;
  private const string MAX_SOUND_ERROR_MSG = "Sound index must be between 0-255.";
  
  private AtsProgram Impl; 
  private FunctionCompanion Func;
  
  public bool Load(LoadProperties _p1) {
    playSoundDelegate = _p1.PlaySound;
    _p1.Panel = Panel;
    for (int i = 0; i < 256; i++) emulatedSoundState[i] = -10000;
    
    Func = new FunctionCompanion(this);
    Impl = new AtsProgram(this, Func);
    Impl.Load();
    return true;
  }
  
  private static void RuntimeException(Exception ex) {
    new System.Threading.Thread(() => { RuntimeExceptionBlocking(ex); }).Start();
  }
  
  private static void RuntimeExceptionBlocking(Exception ex) {
    var result = MessageBox.Show(ex.ToString(), "BlocklyAts Runtime Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
    if (result == DialogResult.Cancel) System.Diagnostics.Process.GetCurrentProcess().Kill();
  }
  
  public void SetVehicleSpecs(VehicleSpecs _p1) { VSpec = _p1; }
  public void SetReverser(int _p1) { EData_Handles_Reverser = _p1; }
  public void SetPower(int _p1) { EData_Handles_PowerNotch = _p1; }
  public void SetBrake(int _p1) { EData_Handles_BrakeNotch = _p1; }
  public void PerformAI(AIData _p1) { }
  
  public void Elapse(ElapseData _p1) { EData = _p1; try { Func.UpdateTimer(); Impl.Elapse(); } catch (Exception ex) { RuntimeExceptionBlocking(ex); } }
  public void Initialize(InitializationModes _p1) { try { Impl.Initialize((int)_p1); } catch (Exception ex) { RuntimeExceptionBlocking(ex); } }
  public void KeyDown(VirtualKeys _p1) { 
    int _pkey = (int)_p1; if (_pkey > 15) return; KeyState[_pkey] = true; 
    try { Impl.KeyDown(_pkey); } catch (Exception ex) { RuntimeException(ex); } 
  }
  public void KeyUp(VirtualKeys _p1) { 
    int _pkey = (int)_p1; if (_pkey > 15) return; KeyState[_pkey] = false; 
    try { Impl.KeyUp(_pkey); } catch (Exception ex) { RuntimeException(ex); }
  }
  public void HornBlow(HornTypes _p1) { try { Impl.HornBlow((int)_p1); } catch (Exception ex) { RuntimeException(ex); } }
  public void DoorChange(DoorStates _p1, DoorStates _p2) { 
    _doorState = _p2; if ((_p1 == DoorStates.None) == (_p2 == DoorStates.None)) return; 
    try { Impl.DoorChange(); } catch (Exception ex) { RuntimeException(ex); }
  }
  public void SetSignal(SignalData[] _p1) { try { Impl.SetSignal(_p1[0].Aspect); } catch (Exception ex) { RuntimeException(ex); } }
  public void SetBeacon(BeaconData _p1) { try { Impl.SetBeacon(_p1.Type, _p1.Optional, _p1.Signal.Aspect, _p1.Signal.Distance); } catch (Exception ex) { RuntimeException(ex); } }
  public void Unload() { try { Impl.Unload(); } catch (Exception ex) { RuntimeExceptionBlocking(ex); } }
  
  public int VSpec_PowerNotches, VSpec_BrakeNotches, VSpec_AtsNotch, VSpec_B67Notch, VSpec_Cars;
  public int EData_Handles_Reverser, EData_Handles_PowerNotch, EData_Handles_BrakeNotch, EData_Handles_ConstSpeed;
  public double EData_Vehicle_Location, EData_Vehicle_Speed, EData_TotalTime;
  public double EData_Vehicle_BcPressure, EData_Vehicle_MrPressure, EData_Vehicle_ErPressure, EData_Vehicle_BpPressure, EData_Vehicle_SapPressure, EData_Vehicle_Current;
  
  private VehicleSpecs _vspec;
  private VehicleSpecs VSpec {
    get { return _vspec; }
    set {
      _vspec = value;
      VSpec_PowerNotches = value.PowerNotches;
      VSpec_BrakeNotches = value.BrakeNotches;
      VSpec_AtsNotch = value.AtsNotch;
      VSpec_B67Notch = value.B67Notch;
      VSpec_Cars = value.Cars;
    }
  }
  private ElapseData _edata;
  private ElapseData EData {
    get { return _edata; }
    set {
      _edata = value;
      EData_Handles_Reverser = value.Handles.Reverser;
      EData_Handles_PowerNotch = value.Handles.PowerNotch;
      EData_Handles_BrakeNotch = value.Handles.BrakeNotch;
      EData_Handles_ConstSpeed = value.Handles.ConstSpeed ? 1 : 0;
      EData_Vehicle_Location = value.Vehicle.Location;
      EData_Vehicle_Speed = value.Vehicle.Speed.KilometersPerHour;
      EData_TotalTime = value.TotalTime.Milliseconds;
      EData_Vehicle_BcPressure = value.Vehicle.BcPressure;
      EData_Vehicle_MrPressure = value.Vehicle.MrPressure;
      EData_Vehicle_ErPressure = value.Vehicle.ErPressure;
      EData_Vehicle_BpPressure = value.Vehicle.BpPressure;
      EData_Vehicle_SapPressure = value.Vehicle.SapPressure;
      EData_Vehicle_Current = 0;
    }
  }
  public bool VSpec_Ready {
    get {
      return _vspec != null;
    }
  }
  public bool EData_Ready {
    get {
      return _edata != null;
    }
  }
  
  private int[] Panel = new int[MAX_PANEL_INDEX + 1]; // Why not add some extra panel variables?
  private DoorStates _doorState;
  public int DoorState {
    get { return (int)this._doorState; }
  }
  public bool LegacyDoorState {
    get { return this._doorState != DoorStates.None; }
  }
  public bool[] KeyState = new bool[16];
  
  public string PluginDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).TrimEnd('\\', '/');
  
  private PlaySoundDelegate playSoundDelegate;
  private SoundHandle[] soundHandles = new SoundHandle[MAX_SOUND_INDEX + 1];
  // To achieve the same behavior as Win32 plugin interface at bve_get_sound_internal
  private int[] emulatedSoundState = new int[MAX_SOUND_INDEX + 1];
  
  public void SetHandle(int type, int pos) {
    switch (type) {
    case 0:
      EData.Handles.BrakeNotch = pos;
      break;
    case 1:
      EData.Handles.PowerNotch = pos;
      break;
    case 2:
      EData.Handles.Reverser = pos;
      break;
    case 3:
      if (pos == 2) return;
      EData.Handles.ConstSpeed = (pos == 1);
      break;
    }
  }
  
  public int GetLegacySound(int id) {
    if (id < 0 || id > MAX_SOUND_INDEX) throw new IndexOutOfRangeException(MAX_SOUND_ERROR_MSG);
    return emulatedSoundState[id];
  }

  public void SetLegacySound(int id, int state) {
    if (id < 0 || id > MAX_SOUND_INDEX) throw new IndexOutOfRangeException(MAX_SOUND_ERROR_MSG);
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
    } else {
      throw new ArgumentOutOfRangeException();
    }
  }
  
  public void SetLegacySoundLV(int id, double volume) {
    if (id < 0 || id > MAX_SOUND_INDEX) throw new IndexOutOfRangeException(MAX_SOUND_ERROR_MSG);
    int state;
    if (volume <= 0) {
      state = -10000;
    } else if (volume >= 100) {
      state = 0;
    } else {
      state = (int)(volume * 100) - 10000;
    }
    SetLegacySound(id, state);
  }
  
  public int GetPanel(int id) { 
    if (id < 0 || id > MAX_PANEL_INDEX) throw new IndexOutOfRangeException(MAX_PANEL_ERROR_MSG);
    return Panel[id]; 
  }
  
  public void SetPanel(int id, int data) {
    if (id < 0 || id > MAX_PANEL_INDEX) throw new IndexOutOfRangeException(MAX_PANEL_ERROR_MSG);
    Panel[id] = data; 
  }
  
  public void CallImplFunc(string name) {
    var methodInfo = Impl.GetType().GetMethod("_etimertick_" + name);
    if (methodInfo != null) methodInfo.Invoke(Impl, new object[] { });
  }
}
