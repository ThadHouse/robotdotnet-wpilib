//File automatically generated using robotdotnet-tools. Please do not modify.

using System;
using System.Runtime.InteropServices;
using HAL_Base;

namespace HAL_RoboRIO
{
    internal class HAL
    {
        internal const string LibhalathenaSharedSo = "libHALAthena_shared.so";

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getPort")]
        public static extern IntPtr getPort(byte pin);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getPortWithModule")]
        public static extern IntPtr getPortWithModule(byte module, byte pin);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getHALErrorMessage")]
        public static extern IntPtr getHALErrorMessage(int code);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getFPGAVersion")]
        public static extern ushort getFPGAVersion(ref int status);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getFPGARevision")]
        public static extern uint getFPGARevision(ref int status);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getFPGATime")]
        public static extern uint getFPGATime(ref int status);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "getFPGAButton")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool getFPGAButton(ref int status);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALSetErrorData")]
        public static extern int HALSetErrorData(string errors, int errorsLength, int wait_ms);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetControlWord")]
        private static extern int NativeHALGetControlWord(ref uint data);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetAllianceStation")]
        public static extern int HALGetAllianceStation(ref HALAllianceStationID allianceStation);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickAxes")]
        public static extern int HALGetJoystickAxes(byte joystickNum, ref HALJoystickAxes axes);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickPOVs")]
        public static extern int HALGetJoystickPOVs(byte joystickNum, ref HALJoystickPOVs povs);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickButtons")]
        public static extern int HALGetJoystickButtons(byte joystickNum, ref HALJoystickButtons buttons);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickDescriptor")]
        public static extern int HALGetJoystickDescriptor(byte joystickNum, ref HALJoystickDescriptor desc);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickIsXbox")]
        public static extern int HALGetJoystickIsXbox(byte joystickNum);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickType")]
        public static extern int HALGetJoystickType(byte joystickNum);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickName")]
        public static extern IntPtr HALGetJoystickName(byte joystickNum);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetJoystickAxisType")]
        public static extern int HALGetJoystickAxisType(byte joystickNum, byte axis);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALSetJoystickOutputs")]
        public static extern int HALSetJoystickOutputs(byte joystickNum, uint outputs, ushort leftRumble, ushort rightRumble);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetMatchTime")]
        public static extern int HALGetMatchTime(ref float matchTime);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALSetNewDataSem")]
        public static extern void HALSetNewDataSem(IntPtr sem);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetSystemActive")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool HALGetSystemActive(ref int status);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALGetBrownedOut")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool HALGetBrownedOut(ref int status);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALInitialize")]
        public static extern int HALInitialize(int mode);

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALNetworkCommunicationObserveUserProgramStarting")]
        public static extern void HALNetworkCommunicationObserveUserProgramStarting();

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALNetworkCommunicationObserveUserProgramDisabled")]
        public static extern void HALNetworkCommunicationObserveUserProgramDisabled();

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALNetworkCommunicationObserveUserProgramAutonomous")]
        public static extern void HALNetworkCommunicationObserveUserProgramAutonomous();

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALNetworkCommunicationObserveUserProgramTeleop")]
        public static extern void HALNetworkCommunicationObserveUserProgramTeleop();

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALNetworkCommunicationObserveUserProgramTest")]
        public static extern void HALNetworkCommunicationObserveUserProgramTest();

        [DllImport(LibhalathenaSharedSo, EntryPoint = "HALReport")]
        public static extern uint HALReport(byte resource, byte instanceNumber, byte context, string feature = null);

        
        /// <summary>
        /// Gets the HAL Control Word
        /// </summary>
        /// <returns></returns>
        public static HALControlWord HALGetControlWord()
        {
            //HALControlWord temp = new HALControlWord();
            uint word = 0;
            NativeHALGetControlWord(ref word);
            return new HALControlWord((word & 1) != 0, ((word >> 1) & 1) != 0, ((word >> 2) & 1) != 0,
                ((word >> 3) & 1) != 0, ((word >> 4) & 1) != 0, ((word >> 5) & 1) != 0);
        }
    }
}
