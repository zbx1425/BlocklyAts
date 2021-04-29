using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BlocklyAts {
    public partial class ApiProxy {
        /// <summary>
        /// ATS Plug-in Version
        /// </summary>
        const int Version = 0x00020000;

        /// <summary>
        /// ATS Keys
        /// </summary>
        public enum AtsKey {
            S = 0,          // S Key
            A1,             // A1 Key
            A2,             // A2 Key
            B1,             // B1 Key
            B2,             // B2 Key
            C1,             // C1 Key
            C2,             // C2 Key
            D,              // D Key
            E,              // E Key
            F,              // F Key
            G,              // G Key
            H,              // H Key
            I,              // I Key
            J,              // J Key
            K,              // K Key
            L               // L Key
        }

        /// <summary>
        /// Initial Position of Handle
        /// </summary>
        public enum AtsInitialHandlePosition {
            ServiceBrake = 0,   // Service Brake
            EmergencyBrake,     // Emergency Brake
            Removed             // Handle Removed
        }

        /// <summary>
        /// Sound Control Instruction
        /// </summary>
        public static class AtsSoundControlInstruction {
            public const int Stop = -10000;     // Stop
            public const int Play = 1;          // Play Once
            public const int PlayLooping = 0;   // Play Repeatedly
            public const int Continue = 2;      // Continue
        }

        /// <summary>
        /// Type of Horn
        /// </summary>
        public enum AtsHornType {
            Primary = 0,    // Horn 1
            Secondary,      // Horn 2
            Music           // Music Horn
        }

        /// <summary>
        /// Constant Speed Control Instruction
        /// </summary>
        public static class AtsCscInstruction {
            public const int Continue = 0;       // Continue
            public const int Enable = 1;         // Enable
            public const int Disable = 2;        // Disable
        }

        /// <summary>
        /// Vehicle Specification
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AtsVehicleSpec {
            public int BrakeNotches;   // Number of Brake Notches
            public int PowerNotches;   // Number of Power Notches
            public int AtsNotch;       // ATS Cancel Notch
            public int B67Notch;       // 80% Brake (67 degree)
            public int Cars;           // Number of Cars
        };

        /// <summary>
        /// State Quantity of Vehicle
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AtsVehicleState {
            public double Location;    // Train Position (Z-axis) (m)
            public float Speed;        // Train Speed (km/h)
            public int Time;           // Time (ms)
            public float BcPressure;   // Pressure of Brake Cylinder (Pa)
            public float MrPressure;   // Pressure of MR (Pa)
            public float ErPressure;   // Pressure of ER (Pa)
            public float BpPressure;   // Pressure of BP (Pa)
            public float SapPressure;  // Pressure of SAP (Pa)
            public float Current;      // Current (A)
        };

        /// <summary>
        /// Received Data from Beacon
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AtsBeaconData {
            public int Type;       // Type of Beacon
            public int Signal;     // Signal of Connected Section
            public float Distance; // Distance to Connected Section (m)
            public int Optional;   // Optional Data
        };

        /// <summary>
        /// Train Operation Instruction
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AtsHandles {
            public int Brake;               // Brake Notch
            public int Power;               // Power Notch
            public int Reverser;            // Reverser Position
            public int ConstantSpeed;       // Constant Speed Control
        };

        /// <summary>
        /// Unmanaged array operations for Panel / Sound.
        /// </summary>
        public class AtsIoArray {
            /// <summary>
            /// Address of unmanaged array.
            /// </summary>
            private IntPtr Address { get; set; } = IntPtr.Zero;

            /// <summary>
            /// Array length of unmanaged array.
            /// </summary>
            public int Length { get; private set; } = -1;

            /// <summary>
            /// Gets an element from unmanaged array by index.
            /// </summary>
            /// <param name="index">The array index that indicates position of element in unmanaged array.</param>
            /// <returns>Element of unmanaged array.</returns>
            public unsafe int this[int index] {
                get {
                    if ((index >= Length) || (index < 0)) {
                        throw new IndexOutOfRangeException("Unmanaged array index is out of range: " + AppDomain.CurrentDomain.BaseDirectory);
                    }

                    var pointer = (int*)Address.ToPointer();
                    return pointer[index];      // Get an element.
                }
                set {
                    if ((index >= Length) || (index < 0)) {
                        throw new IndexOutOfRangeException("Unmanaged array index is out of range: " + AppDomain.CurrentDomain.BaseDirectory);
                    }

                    var pointer = (int*)Address.ToPointer();
                    pointer[index] = value;     // Set an element.
                }
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public AtsIoArray() {
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="source">Pointer of unmanaged array.</param>
            /// <param name="length">Array length of unmanaged array.</param>
            public AtsIoArray(IntPtr source, int length = 256) {
                SetSource(source, length);
            }

            /// <summary>
            /// Sets an unmanaged array.
            /// </summary>
            /// <param name="source">Pointer of unmanaged array.</param>
            /// <param name="length">Array length of unmanaged array.</param>
            public void SetSource(IntPtr source, int length = 256) {
                Address = source;
                Length = length;
            }
        }
    }
}
