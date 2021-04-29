using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BlocklyAts {
    class CallConverter {

        public Type ImplType, FuncType;
        public object ImplInstance, FuncInstance;
        private MethodInfo MLoad, MElapse, MInitialize, MKeyDown, MKeyUp, MHornBlow, 
            MDoorChange, MSetSignal, MSetBeacon, MUnload, MUpdateTimer;

        public CallConverter(Type ImplType, Type FuncType, ApiProxy c) {
            try {
                this.ImplType = ImplType;
                this.FuncType = FuncType;
                FuncInstance = Activator.CreateInstance(FuncType, new object[] { c });
                ImplInstance = Activator.CreateInstance(ImplType, new object[] { c, FuncInstance });
                MLoad = ImplType.GetMethod("Load");
                MElapse = ImplType.GetMethod("Elapse");
                MInitialize = ImplType.GetMethod("Initialize");
                MKeyDown = ImplType.GetMethod("KeyDown");
                MKeyUp = ImplType.GetMethod("KeyUp");
                MHornBlow = ImplType.GetMethod("HornBlow");
                MDoorChange = ImplType.GetMethod("DoorChange");
                MSetSignal = ImplType.GetMethod("SetSignal");
                MSetBeacon = ImplType.GetMethod("SetBeacon");
                MUnload = ImplType.GetMethod("Unload");
                MUpdateTimer = FuncType.GetMethod("UpdateTimer");
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "BlocklyAts Loading Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
                return;
            }
        }

        public void Func_UpdateTimer() {
            MUpdateTimer.Invoke(FuncInstance, null);
        }

        public void Load() {
            MLoad.Invoke(ImplInstance, null);
        }
        public void Elapse() {
            MElapse.Invoke(ImplInstance, null);
        }
        public void Initialize(int _pinitindex) {
            MInitialize.Invoke(ImplInstance, new object[] { _pinitindex });
        }
        public void KeyDown(int _pkey) {
            MKeyDown.Invoke(ImplInstance, new object[] { _pkey });
        }
        public void KeyUp(int _pkey) {
            MKeyUp.Invoke(ImplInstance, new object[] { _pkey });
        }
        public void HornBlow(int _phorntype) {
            MHornBlow.Invoke(ImplInstance, new object[] { _phorntype });
        }
        public void DoorChange() {
            MDoorChange.Invoke(ImplInstance, null);
        }
        public void SetSignal(int _psignal) {
            MSetSignal.Invoke(ImplInstance, new object[] { _psignal });
        }
        public void SetBeacon(int _p1, int _p2, int _p3, double _p4) {
            MSetBeacon.Invoke(ImplInstance, new object[] { _p1, _p2, _p3, _p4 });
        }
        public void Unload() {
            MUnload.Invoke(ImplInstance, null);
        }
    }
}
