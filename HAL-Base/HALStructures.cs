﻿using System;
using System.Runtime.InteropServices;

//These are all of the structures used by HAL-RoboRIO and HAL-Simulation. 
//Changes to these will always require a rebuild of the local HALs, which we want to avoid doing.
//So please do not change these without explicit reasoning.
namespace HAL_Base
{

    #region HAL
    /// <summary>
    /// 
    /// </summary>
    public enum HALAllianceStationID
    {
        // ReSharper disable InconsistentNaming
        HALAllianceStationID_red1,

        HALAllianceStationID_red2,

        HALAllianceStationID_red3,

        HALAllianceStationID_blue1,

        HALAllianceStationID_blue2,


        HALAllianceStationID_blue3,
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Joystick Axes Struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HALJoystickAxes
    {
        /// unsigned short
        public ushort count;

        /// short[] hack
        public HALJoystickAxesArray axes;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct HALJoystickPOVs
    {
        /// unsigned short
        public ushort count;

        /// short[] hack
        public HALJoystickPOVArray povs;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HALJoystickAxesArray
    {
        public Int16 axes0;
        public Int16 axes1;
        public Int16 axes2;
        public Int16 axes3;
        public Int16 axes4;
        public Int16 axes5;
        public Int16 axes6;
        public Int16 axes7;
        public Int16 axes8;
        public Int16 axes9;
        public Int16 axes10;
        public Int16 axes11;

        public Int16 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return axes0;
                    case 1:
                        return axes1;
                    case 2:
                        return axes2;
                    case 3:
                        return axes3;
                    case 4:
                        return axes4;
                    case 5:
                        return axes5;
                    case 6:
                        return axes6;
                    case 7:
                        return axes7;
                    case 8:
                        return axes8;
                    case 9:
                        return axes9;
                    case 10:
                        return axes10;
                    case 11:
                        return axes11;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (i)
                {
                    case 0:
                        axes0 = value;
                        return;
                    case 1:
                        axes1 = value;
                        return;
                    case 2:
                        axes2 = value;
                        return;
                    case 3:
                        axes3 = value;
                        return;
                    case 4:
                        axes4 = value;
                        return;
                    case 5:
                        axes5 = value;
                        return;
                    case 6:
                        axes6 = value;
                        return;
                    case 7:
                        axes7 = value;
                        return;
                    case 8:
                        axes8 = value;
                        return;
                    case 9:
                        axes9 = value;
                        return;
                    case 10:
                        axes10 = value;
                        return;
                    case 11:
                        axes11 = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();

                }
            }

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HALJoystickPOVArray
    {
        private Int16 pov0;
        private Int16 pov1;
        private Int16 pov2;
        private Int16 pov3;
        private Int16 pov4;
        private Int16 pov5;
        private Int16 pov6;
        private Int16 pov7;
        private Int16 pov8;
        private Int16 pov9;
        private Int16 pov10;
        private Int16 pov11;

        public Int16 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return pov0;
                    case 1:
                        return pov1;
                    case 2:
                        return pov2;
                    case 3:
                        return pov3;
                    case 4:
                        return pov4;
                    case 5:
                        return pov5;
                    case 6:
                        return pov6;
                    case 7:
                        return pov7;
                    case 8:
                        return pov8;
                    case 9:
                        return pov9;
                    case 10:
                        return pov10;
                    case 11:
                        return pov11;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (i)
                {
                    case 0:
                        pov0 = value;
                        return;
                    case 1:
                        pov1 = value;
                        return;
                    case 2:
                        pov2 = value;
                        return;
                    case 3:
                        pov3 = value;
                        return;
                    case 4:
                        pov4 = value;
                        return;
                    case 5:
                        pov5 = value;
                        return;
                    case 6:
                        pov6 = value;
                        return;
                    case 7:
                        pov7 = value;
                        return;
                    case 8:
                        pov8 = value;
                        return;
                    case 9:
                        pov9 = value;
                        return;
                    case 10:
                        pov10 = value;
                        return;
                    case 11:
                        pov11 = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();

                }
            }

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HALJoystickButtons
    {
        /// unsigned int
        public uint buttons;

        /// byte
        public byte count;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct HALJoystickDescriptor
    {
        /// byte
        public byte isXbox;

        /// byte
        public byte type;

        /// char[256]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;

        /// byte
        public byte axisCount;

        /// byte[] hack
        public HALJoystickAxesTypesArray axisTypes;

        /// byte
        public byte buttonCount;

        /// byte
        public byte povCount;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct HALJoystickAxesTypesArray
    {
        public Byte axes0;
        public Byte axes1;
        public Byte axes2;
        public Byte axes3;
        public Byte axes4;
        public Byte axes5;
        public Byte axes6;
        public Byte axes7;
        public Byte axes8;
        public Byte axes9;
        public Byte axes10;
        public Byte axes11;

        public Byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return axes0;
                    case 1:
                        return axes1;
                    case 2:
                        return axes2;
                    case 3:
                        return axes3;
                    case 4:
                        return axes4;
                    case 5:
                        return axes5;
                    case 6:
                        return axes6;
                    case 7:
                        return axes7;
                    case 8:
                        return axes8;
                    case 9:
                        return axes9;
                    case 10:
                        return axes10;
                    case 11:
                        return axes11;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (i)
                {
                    case 0:
                        axes0 = value;
                        return;
                    case 1:
                        axes1 = value;
                        return;
                    case 2:
                        axes2 = value;
                        return;
                    case 3:
                        axes3 = value;
                        return;
                    case 4:
                        axes4 = value;
                        return;
                    case 5:
                        axes5 = value;
                        return;
                    case 6:
                        axes6 = value;
                        return;
                    case 7:
                        axes7 = value;
                        return;
                    case 8:
                        axes8 = value;
                        return;
                    case 9:
                        axes9 = value;
                        return;
                    case 10:
                        axes10 = value;
                        return;
                    case 11:
                        axes11 = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();

                }
            }

        }

    }

    public struct HALControlWord
    {
        private bool m_enabled;
        private bool m_autonomous;
        private bool m_test;
        private bool m_eStop;
        private bool m_fmsAttached;
        private bool m_dsAttached;

        public HALControlWord(bool enabled, bool autonomous, bool test, bool eStop,
            bool fmsAttached, bool dsAttached)
        {
            m_enabled = enabled;
            m_autonomous = autonomous;
            m_test = test;
            m_eStop = eStop;
            m_fmsAttached = fmsAttached;
            m_dsAttached = dsAttached;
        }

        public bool GetEnabled()
        {
            return m_enabled;
        }

        public bool GetAutonomous()
        {
            return m_autonomous;
        }

        public bool GetTest()
        {
            return m_test;
        }

        public bool GetEStop()
        {
            return m_eStop;
        }

        public bool GetFMSAttached()
        {
            return m_fmsAttached;
        }

        public bool GetDSAttached()
        {
            return m_dsAttached;
        }
    }

    #endregion

    #region Accelerometer

    public enum AccelerometerRange
    {
        /// kRange_2G -> 0
        Range_2G = 0,

        /// kRange_4G -> 1
        Range_4G = 1,

        /// kRange_8G -> 2
        Range_8G = 2,
    }

    #endregion

    #region Analog
    public enum AnalogTriggerType
    {
        /// kInWindow -> 0
        InWindow = 0,

        /// kState -> 1
        State = 1,

        /// kRisingPulse -> 2
        RisingPulse = 2,

        /// kFallingPulse -> 3
        FallingPulse = 3,
    }
    #endregion

    #region CANTalonSRX
    public enum CTR_Code
    {
        CTR_OKAY,

        CTR_RxTimeout,

        CTR_TxTimeout,

        CTR_InvalidParamValue,

        CTR_UnexpectedArbId,

        CTR_TxFailed,

        CTR_SigNotUpdated,
    }
    #endregion

    #region Digital
    public enum Mode
    {
        /// kTwoPulse -> 0
        TwoPulse = 0,

        /// kSemiperiod -> 1
        Semiperiod = 1,

        /// kPulseLength -> 2
        PulseLength = 2,

        /// kExternalDirection -> 3
        ExternalDirection = 3,
    }
    #endregion

    #region CAN

    [StructLayout(LayoutKind.Sequential)]
    public struct CANStreamMessage
    {
        public uint messageID;
        public uint timeStamp;
        public CANDataArray data;
        public byte dataSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CANDataArray
    {
        private byte data0;
        private byte data1;
        private byte data2;
        private byte data3;
        private byte data4;
        private byte data5;
        private byte data6;
        private byte data7;

        public byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return data0;
                    case 1:
                        return data1;
                    case 2:
                        return data2;
                    case 3:
                        return data3;
                    case 4:
                        return data4;
                    case 5:
                        return data5;
                    case 6:
                        return data6;
                    case 7:
                        return data7;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        data0 = value;
                        return;
                    case 1:
                        data1 = value;
                        return;
                    case 2:
                        data2 = value;
                        return;
                    case 3:
                        data3 = value;
                        return;
                    case 4:
                        data4 = value;
                        return;
                    case 5:
                        data5 = value;
                        return;
                    case 6:
                        data6 = value;
                        return;
                    case 7:
                        data7 = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }


    #endregion
}
