using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BlocklyAts {

    public partial class ApiProxy {
        
        private static ApiProxy Instance = new ApiProxy();
        private static CallConverter Impl;

        public int VSpec_PowerNotches, VSpec_BrakeNotches, VSpec_AtsNotch, VSpec_B67Notch, VSpec_Cars;
        public int EData_Handles_Reverser, EData_Handles_PowerNotch, EData_Handles_BrakeNotch, EData_Handles_ConstSpeed = 0;
        public double EData_Vehicle_Location, EData_Vehicle_Speed, EData_TotalTime;
        public double EData_Vehicle_BcPressure, EData_Vehicle_MrPressure, EData_Vehicle_ErPressure, EData_Vehicle_BpPressure, EData_Vehicle_SapPressure, EData_Vehicle_Current;
        public bool VSpec_Ready = false, EData_Ready = false;
        public int DoorState;
        public bool LegacyDoorState;
        public bool[] KeyState = new bool[16];

        private static AtsIoArray PanelArray, SoundArray;

        private static int[] TempHandle = new int[4];

        public void SetHandle(int type, int pos) {
            TempHandle[type] = pos;
        }

        public int GetLegacySound(int id) {
            if (id < 0 || id > 255) throw new IndexOutOfRangeException("Sound index must be between 0-255.");
            return SoundArray[id];
        }

        public void SetLegacySound(int id, int state) {
            if (id < 0 || id > 255) throw new IndexOutOfRangeException("Sound index must be between 0-255.");
            SoundArray[id] = state;
        }

        public void SetLegacySoundLV(int id, double volume) {
            if (id < 0 || id > 255) throw new IndexOutOfRangeException("Sound index must be between 0-255.");
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
            if (id < 0 || id > 255) throw new IndexOutOfRangeException("Panel index must be between 0-255.");
            return PanelArray[id];
        }

        public void SetPanel(int id, int data) {
            if (id < 0 || id > 255) throw new IndexOutOfRangeException("Panel index must be between 0-255.");
            PanelArray[id] = data;
        }

        public void CallImplFunc(string name) {
            var methodInfo = Impl.ImplType.GetMethod("_etimertick_" + name);
            if (methodInfo != null) methodInfo.Invoke(Impl.ImplInstance, new object[] { });
        }

        public string PluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd('\\', '/');

        [DllExport(CallingConvention.StdCall)]
        public static void Load() {
            Assembly targetAssembly = null;
            try {
                AssemblyResolveHelper.SetupResolveHandler();
                using (var fs = new FileStream(Assembly.GetExecutingAssembly().Location, FileMode.Open, FileAccess.Read)) {
                    fs.Seek(0x6C, SeekOrigin.Begin);
                    byte[] sizeBuf = new byte[4];
                    fs.Read(sizeBuf, 0, 4);
                    var exeSize = BitConverter.ToInt32(sizeBuf, 0);
                    fs.Read(sizeBuf, 0, 4);
                    var dllSize = BitConverter.ToInt32(sizeBuf, 0);
                    fs.Seek(exeSize, SeekOrigin.Begin);

                    byte[] dllBuf = new byte[dllSize];
                    fs.Read(dllBuf, 0, dllSize);
                    if (fs.Length > exeSize + dllSize) {
                        byte[] pdbBuf = new byte[fs.Length - exeSize - dllSize];
                        fs.Read(pdbBuf, 0, (int)fs.Length - exeSize - dllSize);
                        targetAssembly = Assembly.Load(dllBuf, pdbBuf);
                    } else {
                        targetAssembly = Assembly.Load(dllBuf);
                    }
                }

                var types = targetAssembly.GetExportedTypes();
                Impl = new CallConverter(
                    types.First(t => t.Name == "AtsProgram"),
                    types.First(t => t.Name == "FunctionCompanion"),
                    Instance
                );
                Impl.Load();

                AssemblyResolveHelper.RemoveResolveHandler();
            } catch (Exception ex) {
                RuntimeException(ex);
                return;
            }
        }

        private static void RuntimeException(Exception ex) {
            if (ex is System.Reflection.TargetInvocationException) ex = ex.InnerException;
            if (ex.GetType().Name == "AtsCustomException") {
                MessageBox.Show(ex.Message, "BlocklyAts Customized Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            } else {
                var result = MessageBox.Show(ex.ToString(), "BlocklyAts Runtime Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Cancel) System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private static Queue<LAFC> lateCallQueue = new Queue<LAFC>();

        // Late Ats Function Call
        // Postpone some events to the time of the first Elapse
        // To make sure the ElapseData is always available
        // And the sound states behave as the developer expects
        private struct LAFC {
            private readonly short type;
            private readonly double f1;
            private readonly int i1, i2, i3;

            private LAFC(short type, double f1, int i1, int i2, int i3) {
                this.type = type;
                this.f1 = f1;
                this.i1 = i1;
                this.i2 = i2;
                this.i3 = i3;
            }

            internal static LAFC DoorChange() { return new LAFC( 0, 0.0f, 0, 0, 0 ); }
            internal static LAFC SetSignal(int i1) { return new LAFC( 1, 0.0f, i1, 0, 0); }
            internal static LAFC SetBeaconData(int i1, int i2, int i3, double f1) { return new LAFC( 2, f1, i1, i2, i3); }
            internal static LAFC HornBlow(int i1) { return new LAFC( 3, 0.0f, i1, 0, 0); }
            internal static LAFC KeyDown(int i1) { return new LAFC( 4, 0.0f, i1, 0, 0); }
            internal static LAFC KeyUp(int i1) { return new LAFC( 5, 0.0f, i1, 0, 0); }

            internal void Invoke() {
                try {
                    switch (type) {
                        case 0:
                            Impl.DoorChange();
                            break;
                        case 1:
                            Impl.SetSignal(i1);
                            break;
                        case 2:
                            Impl.SetBeacon(i1, i2, i3, f1);
                            break;
                        case 3:
                            Impl.HornBlow(i1);
                            break;
                        case 4:
                            Impl.KeyDown(i1);
                            break;
                        case 5:
                            Impl.KeyUp(i1);
                            break;
                    }
                } catch (Exception ex) {
                    new System.Threading.Thread(() => {
                        RuntimeException(ex);
                    }).Start();
                }
            }

            internal void Schedule() {
                if (Instance.EData_Ready) {
                    Invoke();
                } else {
                    lateCallQueue.Enqueue(this);
                }
            }
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void Dispose() {
            try {
                Impl.Unload();
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static int GetPluginVersion() {
            return Version;
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void SetVehicleSpec(AtsVehicleSpec vehicleSpec) {
            Instance.VSpec_PowerNotches = vehicleSpec.PowerNotches;
            Instance.VSpec_BrakeNotches = vehicleSpec.BrakeNotches;
            Instance.VSpec_AtsNotch = vehicleSpec.AtsNotch;
            Instance.VSpec_B67Notch = vehicleSpec.B67Notch;
            Instance.VSpec_Cars = vehicleSpec.Cars;
            Instance.VSpec_Ready = true;
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void Initialize(int initialHandlePosition) {
            try {
                Impl.Initialize(initialHandlePosition);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static AtsHandles Elapse(AtsVehicleState vehicleState, IntPtr panel, IntPtr sound) {
            try {
                PanelArray = new AtsIoArray(panel);
                SoundArray = new AtsIoArray(sound);
                bool firstElapse = !Instance.EData_Ready;
                if (firstElapse) {
                    for (int i = 0; i < SoundArray.Length; i++) {
                        // Reset the sound states
                        // So that a sound can be played in loop at 100% volume from the very first Elapse
                        // Otherwise the behavior might be confusing to new developers
                        SoundArray[i] = -10000;
                    }
                }
                Instance.EData_Vehicle_Location = vehicleState.Location;
                Instance.EData_Vehicle_Speed = vehicleState.Speed;
                Instance.EData_TotalTime = vehicleState.Time;
                Instance.EData_Vehicle_BcPressure = vehicleState.BcPressure;
                Instance.EData_Vehicle_MrPressure = vehicleState.MrPressure;
                Instance.EData_Vehicle_ErPressure = vehicleState.ErPressure;
                Instance.EData_Vehicle_BpPressure = vehicleState.BpPressure;
                Instance.EData_Vehicle_SapPressure = vehicleState.SapPressure;
                Instance.EData_Vehicle_Current = vehicleState.Current;
                Instance.EData_Ready = true;
                TempHandle[0] = Instance.EData_Handles_BrakeNotch;
                TempHandle[1] = Instance.EData_Handles_PowerNotch;
                TempHandle[2] = Instance.EData_Handles_Reverser;
                TempHandle[3] = AtsCscInstruction.Continue;

                // Don't run the handlers yet, wait for the sound states to take effect
                if (!firstElapse) {
                    while (lateCallQueue.Count > 0) {
                        lateCallQueue.Dequeue().Invoke();
                    }
                    Impl.Func_UpdateTimer();
                    Impl.Elapse();
                }
            } catch (Exception ex) {
                RuntimeException(ex);
            }
            return new AtsHandles() {
                Power = TempHandle[1],
                Brake = TempHandle[0],
                ConstantSpeed = TempHandle[3],
                Reverser = TempHandle[2]
            };
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void SetPower(int handlePosition) {
            Instance.EData_Handles_PowerNotch = handlePosition;
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void SetBrake(int handlePosition) {
            Instance.EData_Handles_BrakeNotch = handlePosition;
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void SetReverser(int handlePosition) {
            Instance.EData_Handles_Reverser = handlePosition;
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void KeyDown(int keyIndex) {
            Instance.KeyState[keyIndex] = true;
            LAFC.KeyDown(keyIndex).Schedule();
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void KeyUp(int keyIndex) {
            Instance.KeyState[keyIndex] = false;
            LAFC.KeyUp(keyIndex).Schedule();
        }

        [DllExport(CallingConvention.StdCall)]
        public static void HornBlow(int hornIndex) {
            LAFC.HornBlow(hornIndex).Schedule();
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void DoorOpen() {
            Instance.DoorState = 3;
            Instance.LegacyDoorState = true;
            LAFC.DoorChange().Schedule();
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void DoorClose() {
            Instance.DoorState = 0;
            Instance.LegacyDoorState = false;
            LAFC.DoorChange().Schedule();
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void SetSignal(int signalIndex) {
            LAFC.SetSignal(signalIndex).Schedule();
        }
        
        [DllExport(CallingConvention.StdCall)]
        public static void SetBeaconData(AtsBeaconData beaconData) {
            LAFC.SetBeaconData(beaconData.Type, beaconData.Optional, beaconData.Signal, beaconData.Distance).Schedule();
        }
    }
}
