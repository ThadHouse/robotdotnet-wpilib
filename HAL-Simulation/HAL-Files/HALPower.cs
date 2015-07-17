﻿using System.Runtime.InteropServices;
using static HAL_Simulator.SimData;

// ReSharper disable RedundantAssignment
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
#pragma warning disable 1591

namespace HAL_Simulator
{
    ///<inheritdoc cref="HAL"/>
    public class HALPower
    {
        [CalledSimFunction]
        public static float getVinVoltage(ref int status)
        {
            status = 0;
            return (float)halData["power"]["vin_voltage"];
        }

        [CalledSimFunction]
        public static float getVinCurrent(ref int status)
        {
            status = 0;
            return (float)halData["power"]["vin_current"];
        }


        /// Return Type: float
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserVoltage6V")]
        [CalledSimFunction]
        public static extern float getUserVoltage6V(ref int status);


        /// Return Type: float
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserCurrent6V")]
        [CalledSimFunction]
        public static extern float getUserCurrent6V(ref int status);


        /// Return Type: boolean
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserActive6V")]
        [return: MarshalAs(UnmanagedType.I1)]
        [CalledSimFunction]
        public static extern bool getUserActive6V(ref int status);


        /// Return Type: int
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserCurrentFaults6V")]
        [CalledSimFunction]
        public static extern int getUserCurrentFaults6V(ref int status);


        [CalledSimFunction]
        public static float getUserVoltage5V(ref int status)
        {
            status = 0;
            return (float)halData["power"]["user_voltage_5v"];
        }


        /// Return Type: float
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserCurrent5V")]
        [CalledSimFunction]
        public static extern float getUserCurrent5V(ref int status);


        /// Return Type: boolean
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserActive5V")]
        [return: MarshalAs(UnmanagedType.I1)]
        [CalledSimFunction]
        public static extern bool getUserActive5V(ref int status);


        /// Return Type: int
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserCurrentFaults5V")]
        [CalledSimFunction]
        public static extern int getUserCurrentFaults5V(ref int status);


        [CalledSimFunction]
        public static float getUserVoltage3V3(ref int status)
        {
            status = 0;
            return (float)halData["power"]["user_voltage_3v3"];
        }


        /// Return Type: float
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserCurrent3V3")]
        [CalledSimFunction]
        public static extern float getUserCurrent3V3(ref int status);


        /// Return Type: boolean
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserActive3V3")]
        [return: MarshalAs(UnmanagedType.I1)]
        [CalledSimFunction]
        public static extern bool getUserActive3V3(ref int status);


        /// Return Type: int
        ///status: int*
        [DllImport("libHALAthena_shared.so", EntryPoint = "getUserCurrentFaults3V3")]
        [CalledSimFunction]
        public static extern int getUserCurrentFaults3V3(ref int status);
    }
}