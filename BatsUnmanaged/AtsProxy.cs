using System;
using System.Diagnostics;
using System.IO;
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
            return SoundArray[id];
        }

        public void SetLegacySound(int id, int state) {
            SoundArray[id] = state;
        }

        public void SetLegacySoundLV(int id, double volume) {
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

        public int GetPanel(int id) { return PanelArray[id]; }
        public void SetPanel(int id, int data) { PanelArray[id] = data; }

        public void CallImplFunc(string name) {
            var methodInfo = Impl.ImplType.GetMethod("_etimertick_" + name);
            if (methodInfo != null) methodInfo.Invoke(Impl.ImplInstance, new object[] { });
        }

        public string PluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd('\\', '/');

        /// <summary>
        /// Called when this plug-in is loaded
        /// </summary>
        [DllExport(CallingConvention.StdCall)]
        public static void Load() {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Assembly targetAssembly = null;
            try {
                using (var fs = new FileStream(Assembly.GetExecutingAssembly().Location, FileMode.Open, FileAccess.Read)) {
                    fs.Seek(0x6C, SeekOrigin.Begin);
                    byte[] sizeBuf = new byte[4];
                    fs.Read(sizeBuf, 0, 4);
                    var exeSize = BitConverter.ToInt32(sizeBuf, 0);
                    fs.Seek(exeSize, SeekOrigin.Begin);

                    byte[] dataBuf = new byte[fs.Length - exeSize + 1];
                    fs.Read(dataBuf, 0, (int)fs.Length - exeSize);
                    targetAssembly = Assembly.Load(dataBuf);
                }

                /*var testFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ats-sn_net.dll");
                targetAssembly = Assembly.Load(File.ReadAllBytes(testFile));*/
            
                Impl = new CallConverter(
                    targetAssembly.GetType("BlocklyAts.AtsProgram"),
                    targetAssembly.GetType("BlocklyAts.FunctionCompanion"),
                    Instance
                );
                Impl.Load();
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "BlocklyAts Loading Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
                return;
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            if (args.Name.Contains("BlocklyAtsUnmanaged")) return Assembly.GetExecutingAssembly();
            return null;
        }

        private static void RuntimeException(Exception ex) {
            var result = MessageBox.Show(ex.ToString(), "BlocklyAts Runtime Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            if (result == DialogResult.Cancel) Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// Called when this plug-in is unloaded
        /// </summary>
        [DllExport(CallingConvention.StdCall)]
        public static void Dispose() {
            Impl.Unload();
        }

        /// <summary>
        /// Returns the version numbers of ATS plug-in
        /// </summary>
        /// <returns>Version numbers of ATS plug-in.</returns>
        [DllExport(CallingConvention.StdCall)]
        public static int GetPluginVersion() {
            return Version;
        }

        /// <summary>
        /// Called when the train is loaded
        /// </summary>
        /// <param name="vehicleSpec">Spesifications of vehicle.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void SetVehicleSpec(AtsVehicleSpec vehicleSpec) {
            Instance.VSpec_PowerNotches = vehicleSpec.PowerNotches;
            Instance.VSpec_BrakeNotches = vehicleSpec.BrakeNotches;
            Instance.VSpec_AtsNotch = vehicleSpec.AtsNotch;
            Instance.VSpec_B67Notch = vehicleSpec.B67Notch;
            Instance.VSpec_Cars = vehicleSpec.Cars;
            Instance.VSpec_Ready = true;
        }

        /// <summary>
        /// Called when the game is started
        /// </summary>
        /// <param name="initialHandlePosition">Initial position of control handle.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void Initialize(int initialHandlePosition) {
            try {
                Impl.Initialize(initialHandlePosition);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="vehicleState">Current state of vehicle.</param>
        /// <param name="panel">Current state of panel.</param>
        /// <param name="sound">Current state of sound.</param>
        /// <returns>Driving operations of vehicle.</returns>
        [DllExport(CallingConvention.StdCall)]
        public static AtsHandles Elapse(AtsVehicleState vehicleState, IntPtr panel, IntPtr sound) {
            try {
                PanelArray = new AtsIoArray(panel);
                SoundArray = new AtsIoArray(sound);
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
                Impl.Func_UpdateTimer();
                Impl.Elapse();
                
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

        /// <summary>
        /// Called when the power is changed
        /// </summary>
        /// <param name="handlePosition">Position of traction control handle.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void SetPower(int handlePosition) {
            Instance.EData_Handles_PowerNotch = handlePosition;
        }

        /// <summary>
        /// Called when the brake is changed
        /// </summary>
        /// <param name="handlePosition">Position of brake control handle.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void SetBrake(int handlePosition) {
            Instance.EData_Handles_BrakeNotch = handlePosition;
        }

        /// <summary>
        /// Called when the reverser is changed
        /// </summary>
        /// <param name="handlePosition">Position of reveerser handle.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void SetReverser(int handlePosition) {
            Instance.EData_Handles_Reverser = handlePosition;
        }

        /// <summary>
        /// Called when any ATS key is pressed
        /// </summary>
        /// <param name="keyIndex">Index of key.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void KeyDown(int keyIndex) {
            try {
                Instance.KeyState[keyIndex] = true;
                Impl.KeyDown(keyIndex);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called when any ATS key is released
        /// </summary>
        /// <param name="keyIndex">Index of key.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void KeyUp(int keyIndex) {
            try {
                Instance.KeyState[keyIndex] = false;
                Impl.KeyUp(keyIndex);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called when the horn is used
        /// </summary>
        /// <param name="hornIndex">Type of horn.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void HornBlow(int hornIndex) {
            try {
                Impl.HornBlow(hornIndex);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called when the door is opened
        /// </summary>
        [DllExport(CallingConvention.StdCall)]
        public static void DoorOpen() {
            try {
                Instance.DoorState = 3;
                Instance.LegacyDoorState = true;
                Impl.DoorChange();
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called when the door is closed
        /// </summary>
        [DllExport(CallingConvention.StdCall)]
        public static void DoorClose() {
            try {
                Instance.DoorState = 0;
                Instance.LegacyDoorState = false;
                Impl.DoorChange();
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called when current signal is changed
        /// </summary>
        /// <param name="signalIndex">Index of signal.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void SetSignal(int signalIndex) {
            try {
                Impl.SetSignal(signalIndex);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }

        /// <summary>
        /// Called when the beacon data is received
        /// </summary>
        /// <param name="beaconData">Received data of beacon.</param>
        [DllExport(CallingConvention.StdCall)]
        public static void SetBeaconData(AtsBeaconData beaconData) {
            try {
                Impl.SetBeacon(beaconData.Type, beaconData.Optional, beaconData.Signal, beaconData.Distance);
            } catch (Exception ex) {
                RuntimeException(ex);
            }
        }
    }
}
