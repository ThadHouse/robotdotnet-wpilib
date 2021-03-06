﻿using System;
using System.Runtime.InteropServices;
using System.Threading;

// ReSharper disable RedundantAssignment
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
#pragma warning disable 1591

namespace HAL_Simulator
{
    //Semaphore

    [StructLayout(LayoutKind.Sequential)]
    internal struct MUTEX_ID
    {
        public object lockObject;
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct SEMAPHORE_ID
    {
        public Semaphore semaphore;
        //public object lockObject;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MULTIWAIT_ID
    {
        public object lockObject;
    }


    //HAL
    [StructLayout(LayoutKind.Sequential)]
    internal struct Port
    {
        public int pin;
        public int module;
    }

    //Analog
    [StructLayout(LayoutKind.Sequential)]
    internal struct AnalogPort
    {
        public Port port;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AnalogTrigger
    {
		public IntPtr analogPortPointer;
        public int index;
    }

    //Compressor
    [StructLayout(LayoutKind.Sequential)]
    internal struct PCM
    {
        public int module;
    }

    //Digital
    [StructLayout(LayoutKind.Sequential)]
    internal struct DigitalPort
    {
        public Port port;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PWM
    {
        public int idx;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Counter
    {
        public int idx;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Encoder
    {
        public int idx;
    }

    //Interrupts
    //[StructLayout(LayoutKind.Sequential)]
    internal class Interrupt
    {
        public int DIOPin;
        public Action<uint, IntPtr> Callback;
        public Action<dynamic, dynamic> DictCallback;
        public IntPtr Param;
        public bool Watcher;
        public double RisingTimestamp;
        public double FallingTimestamp;
        public bool PreviousState;

        public bool FireOnUp;
        public bool FireOnDown;
    }

    //Notifier
    [StructLayout(LayoutKind.Sequential)]
    internal struct Notifier
    {
        public Action<uint, IntPtr> Callback;
        public Thread alarm;
    }

    //Solenoid
    [StructLayout(LayoutKind.Sequential)]
    internal struct SolenoidPort
    {
        public Port port;
    }

    //TalonSRX
    [StructLayout(LayoutKind.Sequential)]
    internal struct TalonSRX
    {
        public int deviceNumber;
    }

}
