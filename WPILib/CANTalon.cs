﻿using System;
using HAL_Base;
using NetworkTables.Tables;
using WPILib.Interfaces;
using WPILib.LiveWindows;
using static HAL_Base.HALCanTalonSRX;
using static HAL_Base.HALCanTalonSRX.Constants;
using static WPILib.Utility;

namespace WPILib
{
    /// <summary>
    /// This Class represents a CAN Talon SRX Motor Controller.
    /// </summary>
    public class CANTalon : IMotorSafety, ICANSpeedController, IPIDInterface, ILiveWindowSendable, ITableListener, IDisposable
    {
        private MotorSafetyHelper m_safetyHelper;

        /// <summary>
        /// Feedback type for CAN Talon
        /// </summary>
        public enum FeedbackDevice
        {
            QuadEncoder = 0,
            AnalogPotentiometer = 2,
            AnalogEncoder = 3,
            EncoderRising = 4,
            EncoderFalling = 5
        }

        /// <summary>
        /// Status Rate for CAN Talon
        /// </summary>
        public enum StatusFrameRate
        {
            General = 0,
            Feedback = 1,
            QuadEncoder = 2,
            AnalogTempVbat = 3
        }


        private ControlMode m_controlMode;
        private IntPtr m_impl;
        private const double DelayForSolicitedSignals = 0.004;
        private bool m_controlEnabled;
        private int m_profile;
        private double m_setPoint;

        public bool Inverted { get; set; }



        public CANTalon(int deviceNumber, int controlPeriodMs = 10)
        {
            DeviceID = deviceNumber;
            m_impl = C_TalonSRX_Create(deviceNumber, controlPeriodMs);
            m_safetyHelper = new MotorSafetyHelper(this);
            m_controlEnabled = true;
            m_setPoint = 0;
            Profile = 0;
            ApplyControlMode(ControlMode.PercentVbus);
            LiveWindow.AddActuator("CANTalonSRX", deviceNumber, this);
            HAL.Report(ResourceType.kResourceType_CANTalonSRX, (byte)(deviceNumber + 1), (byte)m_controlMode);
        }

        public void Dispose()
        {
            C_TalonSRX_Destroy(m_impl);
        }

        private double GetParam(ParamID id)
        {
            C_TalonSRX_RequestParam(m_impl, (int)id);
            Timer.Delay(DelayForSolicitedSignals);
            var value = 0.0;
            var status = C_TalonSRX_GetParamResponse(m_impl, (int)id, ref value);
            if (status != CTR_Code.CTR_OKAY)
                CheckStatus((int)status);
            return value;
        }

        private int GetParamInt32(ParamID id)
        {
            int value;
            var status = GetParamInt32(id, out value);
            if (status != CTR_Code.CTR_OKAY)
                CheckStatus((int)status);
            return value;
        }

        private CTR_Code GetParamInt32(ParamID id, out int value)
        {
            C_TalonSRX_RequestParam(m_impl, (int)id);
            Timer.Delay(DelayForSolicitedSignals);
            value = 0;
            return C_TalonSRX_GetParamResponseInt32(m_impl, (int)id, ref value);
        }

        private void SetParam(ParamID id, double value)
        {
            var errorCode = C_TalonSRX_SetParam(m_impl, (int)id, value);
            if (errorCode != CTR_Code.CTR_OKAY)
                CheckStatus((int)errorCode);
        }

        [Obsolete("Use the Dispose method or a using block instead of Delete")]
        public void Delete()
        {
            Dispose();
        }

        public void ReverseSensor(bool flip)
        {
            C_TalonSRX_SetRevFeedbackSensor(m_impl, flip ? 1 : 0);
        }

        public void ReverseOutput(bool flip)
        {
            C_TalonSRX_SetRevMotDuringCloseLoopEn(m_impl, flip ? 1 : 0);
        }

        public int GetEncoderPosition()
        {
            int pos = 0;
            C_TalonSRX_GetEncPosition(m_impl, ref pos);
            return pos;
        }

        public int GetEncoderVelocity()
        {
            int vel = 0;
            C_TalonSRX_GetEncVel(m_impl, ref vel);
            return vel;
        }

        public int GetNumberOfQuadIdxRises()
        {
            int state = 0;
            C_TalonSRX_GetEncIndexRiseEvents(m_impl, ref state);
            return state;
        }

        public int GetPinStateQuadA()
        {
            int state = 0;
            C_TalonSRX_GetQuadApin(m_impl, ref state);
            return state;
        }

        public int GetPinStateQuadB()
        {
            int state = 0;
            C_TalonSRX_GetQuadBpin(m_impl, ref state);
            return state;
        }

        public int GetPinStateQuadIdx()
        {
            int state = 0;
            C_TalonSRX_GetQuadIdxpin(m_impl, ref state);
            return state;
        }

        public int GetAnalogInPosition()
        {
            int position = 0;
            C_TalonSRX_GetAnalogInWithOv(m_impl, ref position);
            return position;
        }

        public int GetAnalogInRaw()
        {
            return GetAnalogInPosition() & 0x3FF;
        }

        public int GetAnalogInVelocity()
        {
            int velocity = 0;
            C_TalonSRX_GetAnalogInVel(m_impl, ref velocity);
            return velocity;
        }

        public int GetClosedLoopError()
        {
            int error = 0;
            C_TalonSRX_GetCloseLoopErr(m_impl, ref error);
            return error;
        }

        public bool IsForwardLimitSwitchClosed()
        {
            int state = 0;
            C_TalonSRX_GetLimitSwitchClosedFor(m_impl, ref state);
            return state != 0;
        }

        public bool IsReverseLimitSwitchClosed()
        {
            int state = 0;
            C_TalonSRX_GetLimitSwitchClosedFor(m_impl, ref state);
            return state != 0;
        }

        public bool IsBreakEnabledduringNeutral()
        {
            int state = 0;
            C_TalonSRX_GetBrakeIsEnabled(m_impl, ref state);
            return state != 0;
        }

        public double GetTemperature()
        {
            double temp = 0.0;
            C_TalonSRX_GetTemp(m_impl, ref temp);
            return temp;
        }

        public double GetOutputCurrent()
        {
            double current = 0.0;
            C_TalonSRX_GetCurrent(m_impl, ref current);
            return current;
        }

        public double GetOutputVoltage()
        {
            int throttle = 0;
            C_TalonSRX_GetAppliedThrottle(m_impl, ref throttle);
            return GetBusVoltage()*(throttle/1023.0);
        }

        public double GetBusVoltage()
        {
            double voltage = 0.0;
            C_TalonSRX_GetBatteryV(m_impl, ref voltage);
            return voltage;
        }

        public double GetPosition()
        {
            int pos = 0;
            C_TalonSRX_GetSensorPosition(m_impl, ref pos);
            return pos;
        }

        public void SetPosition(double pos)
        {
            SetParam(ParamID.eSensorPosition, pos);
        }

        public double GetSpeed()
        {
            int vel = 0;
            C_TalonSRX_GetSensorVelocity(m_impl, ref vel);
            return vel;
        }

        public bool GetForwardLimitOK()
        {
            int limSwitch = FaultForwardLimit;
            int softLim = FaultForwardSoftLimit;
            return (softLim == 0 && limSwitch == 0);
        }

        public bool GetReverseLimitOK()
        {
            int limSwitch = FaultReverseLimit;
            int softLim = FaultReverseSoftLimit;
            return (softLim == 0 && limSwitch == 0);
        }

        public Faults GetFaults()
        {
            Faults retVal = 0;

            //Temp
            var val = 0;
            var status = C_TalonSRX_GetFault_OverTemp(m_impl, ref val);

            if (status != CTR_Code.CTR_OKAY)
            {
                CheckStatus((int) status);
            }

            retVal |= (val != 0) ? Faults.TemperatureFault : 0;

            //Voltage
            val = 0;
            status = C_TalonSRX_GetFault_UnderVoltage(m_impl, ref val);

            if (status != CTR_Code.CTR_OKAY)
            {
                CheckStatus((int) status);
            }

            retVal |= (val != 0) ? Faults.BusVoltageFault : 0;

            //Fwd Limit Switch
            val = 0;
            status = C_TalonSRX_GetFault_ForLim(m_impl, ref val);

            if (status != CTR_Code.CTR_OKAY)
            {
                CheckStatus((int) status);
            }

            retVal |= (val != 0) ? Faults.FwdLimitSwitch : 0;

            //Rev Limit Switch
            val = 0;
            status = C_TalonSRX_GetFault_RevLim(m_impl, ref val);

            if (status != CTR_Code.CTR_OKAY)
            {
                CheckStatus((int) status);
            }

            retVal |= (val != 0) ? Faults.RevLimitSwitch : 0;

            //Fwd Soft Limit
            val = 0;
            status = C_TalonSRX_GetFault_ForSoftLim(m_impl, ref val);

            if (status != CTR_Code.CTR_OKAY)
            {
                CheckStatus((int) status);
            }

            retVal |= (val != 0) ? Faults.FwdSoftLimit : 0;

            //Rev Soft Limit
            val = 0;
            status = C_TalonSRX_GetFault_RevSoftLim(m_impl, ref val);

            if (status != CTR_Code.CTR_OKAY)
            {
                CheckStatus((int) status);
            }

            retVal |= (val != 0) ? Faults.RevSoftLimit : 0;

            return retVal;
        }

        private void ApplyControlMode(ControlMode value)
        {
            m_controlMode = value;
            if (value == ControlMode.Disabled)
                m_controlEnabled = false;
            C_TalonSRX_SetModeSelect(m_impl, (int)ControlMode.Disabled);
        }

        [Obsolete("Use MotorControlMode property.")]
        public ControlMode GetControlMode() { return MotorControlMode; }

        [Obsolete("Use MotorControlMode property.")]
        public void SetControlMode(ControlMode mode) { MotorControlMode = mode; }

        public ControlMode MotorControlMode
        {
            get { return m_controlMode; }
            set
            {
                if (m_controlMode == value) return;
                ApplyControlMode(value);
            }
        }

        [Obsolete("Use FeedBackDevice property instead.")]
        public FeedbackDevice GetFeedbackDevice()
        {
            return FeedBackDevice;
        }

        [Obsolete("Use FeedBackDevice property instead.")]
        public void SetFeedbackDevice(FeedbackDevice device)
        {
            FeedBackDevice = device;
        }

        public FeedbackDevice FeedBackDevice
        {
            get
            {
                int device = 0;
                C_TalonSRX_GetFeedbackDeviceSelect(m_impl, ref device);
                return (FeedbackDevice)device;
            }
            set
            {
                C_TalonSRX_SetFeedbackDeviceSelect(m_impl, (int)value);
            }
        }

        [Obsolete("Use ControlEnabled property instead.")]
        public bool IsControlEnabled()
        {
            return ControlEnabled;
        }

        [Obsolete("Set ControlEnabled property to true instead.")]
        public void EnableControl()
        {
            ControlEnabled = true;
        }

        public void Enable() => ControlEnabled = true;

        [Obsolete("Set ControlEnabled property to false instead.")]
        public void DisableControl()
        {
            ControlEnabled = false;
        }

        public bool ControlEnabled
        {
            get
            {
                return m_controlEnabled;
            }
            set
            {
                if (m_controlEnabled == value) return;
                if (m_controlEnabled && !value)
                {
                    C_TalonSRX_SetModeSelect(m_impl, (int)ControlMode.Disabled);
                    m_controlEnabled = false;
                }
                else
                {
                    m_controlEnabled = true;
                }
            }
        }

        private void EnsureInPIDMode()
        {
            if (!(MotorControlMode == ControlMode.Position || MotorControlMode == ControlMode.Speed))
            {
                throw new InvalidOperationException("PID mode only applies to Position and Speed modes.");
            }
        }

        public double P
        {
            get
            {
                EnsureInPIDMode();
                if (m_profile == 0)
                    return GetParam(ParamID.eProfileParamSlot0_P);
                else
                    return GetParam(ParamID.eProfileParamSlot1_P);
            }
            set { SetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_P : ParamID.eProfileParamSlot1_P, value); }
        }

        public double I
        {
            get
            {
                EnsureInPIDMode();
                return GetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_I : ParamID.eProfileParamSlot1_I);
            }
            set { SetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_I : ParamID.eProfileParamSlot1_I, value); }
        }

        public double D
        {
            get
            {
                EnsureInPIDMode();
                return GetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_D : ParamID.eProfileParamSlot1_D);
            }
            set { SetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_D : ParamID.eProfileParamSlot1_D, value); }
        }

        [Obsolete("Use F property instead.")]
        public double GetF() { return F; }
        [Obsolete("Use F property instead.")]
        public void SetF(double f) { F = f; }

        public double F
        {
            get
            {
                EnsureInPIDMode();
                return GetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_F : ParamID.eProfileParamSlot1_F);
            }
            set { SetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_F : ParamID.eProfileParamSlot1_F, value); }
        }

        [Obsolete("Use IZone property instead.")]
        public double GetIZone() { return IZone; }
        [Obsolete("Use IZone property instead.")]
        public void SetIZone(double iZone) { IZone = iZone; }

        public double IZone
        {
            get
            {
                EnsureInPIDMode();
                return GetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_IZone : ParamID.eProfileParamSlot1_IZone);
            }
            set {
                SetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_IZone : ParamID.eProfileParamSlot1_IZone, value);
            }
        }

        public double GetIaccum()
        {
            EnsureInPIDMode();
            return GetParamInt32(ParamID.ePidIaccum);
        }

        public void ClearIAccum()
        {
            EnsureInPIDMode();
            SetParam(ParamID.ePidIaccum, 0.0);
        }

        [Obsolete("Use CloseLoopRampRate property instead.")]
        public double GetCloseLoopRampRate() { return CloseLoopRampRate; }
        [Obsolete("Use CloseLoopRampRate property instead.")]
        public void SetCloseLoopRampRate(double rate) { CloseLoopRampRate = rate; }

        public double CloseLoopRampRate
        {
            get
            {
                EnsureInPIDMode();
                return GetParam(m_profile == 0 ? ParamID.eProfileParamSlot0_CloseLoopRampRate : ParamID.eProfileParamSlot1_CloseLoopRampRate);
            }
            set {
                SetParam(m_profile == 0
                    ? ParamID.eProfileParamSlot0_CloseLoopRampRate
                    : ParamID.eProfileParamSlot1_CloseLoopRampRate, value);
            }
        }

        public void SetPID(double p, double i, double value, double f, int izone, double closeLoopRampRate, int profile)
        {
            if (profile != 0 && profile != 1)
                throw new ArgumentOutOfRangeException(nameof(profile), "Talon PID profile must be 0 or 1.");
            m_profile = profile;
            P = p;
            I = i;
            D = value;
            F = f;
            IZone = izone;
            CloseLoopRampRate = closeLoopRampRate;
        }

        public void SetPID(double p, double i, double d)
        {
            SetPID(p, i, d, 0, 0, 0, m_profile);
        }

        [Obsolete("Use Setpoint property instead.")]
        public double GetSetpoint() { return Setpoint; }

        public double Setpoint
        {
            get
            {
                return m_setPoint;
            }
            set
            {
                Set(value);
            }
        }

        [Obsolete("Use Profile property instead.")]
        public int GetProfile() { return Profile; }
        [Obsolete("Use Profile property instead.")]
        public void SetProfile(int profile) { Profile = profile; }

        public int Profile
        {
            get { return m_profile; }
            set
            {
                if (value != 0 && value != 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "Talon PID profile must be 0 or 1.");
                m_profile = value;
                C_TalonSRX_SetProfileSlotSelect(m_impl, m_profile);
            }
        }



        [Obsolete("Use the VoltageRampRate property instead.")]
        public double GetVoltageRampRate() { return VoltageRampRate; }
        [Obsolete("Use VoltageRampRate property instead.")]
        public void SetVoltageRampRate(double rate) { VoltageRampRate = rate; }

        public NeutralMode ConfigNeutralMode
        {
            set
            {
                CTR_Code status;
                switch (value)
                {
                    default:
                    case NeutralMode.Jumper:
                        status = C_TalonSRX_SetOverrideBrakeType(m_impl,
                            kBrakeOverride_UseDefaultsFromFlash);
                        break;
                    case NeutralMode.Brake:
                        status = C_TalonSRX_SetOverrideBrakeType(m_impl,
                            kBrakeOverride_OverrideBrake);
                        break;
                    case NeutralMode.Coast:
                        status = C_TalonSRX_SetOverrideBrakeType(m_impl,
                            kBrakeOverride_OverrideCoast);
                        break;
                }

                if (status != CTR_Code.CTR_OKAY)
                    CheckStatus((int)status);
            }
        }

        public int EncoderCodesPerRev
        {
            set { }
        }

        public int PotentiometerTurns
        {
            set { }
        }

        public void ConfigSoftPositionLimits(double forwardLimitPosition, double reverseLimitPosition)
        {
            LimitMode = LimitMode.SoftPositionLimits;
            ForwardLimit = forwardLimitPosition;
            ReverseLimit = reverseLimitPosition;
        }

        public void DisableSoftPositionLimits()
        {
            LimitMode = LimitMode.SwitchInputsOnly;
        }
        

        public LimitMode LimitMode
        {
            set
            {
                switch (value)
                {
                    case LimitMode.SwitchInputsOnly:
                        ForwardSoftLimitEnabled = false;
                        ReverseSoftLimitEnabled = false;
                        EnableLimitSwitches(true, true);
                        break;
                    case LimitMode.SoftPositionLimits:
                        ForwardSoftLimitEnabled = true;
                        ReverseSoftLimitEnabled = true;
                        EnableLimitSwitches(true, true);
                        break;
                    case LimitMode.SrxDisableSwitchInputs:
                        ForwardSoftLimitEnabled = false;
                        ReverseSoftLimitEnabled = false;
                        EnableLimitSwitches(false, false);
                        break;
                }
            }
        }

        public double ForwardLimit
        {
            set { ForwardSoftLimit = (value); }
        }

        public double ReverseLimit
        {
            set { ReverseSoftLimit = (value); }
        }

        public double MaxOutputVoltage
        {
            set { CheckStatus(-9); }
        }

        public float FaultTime
        {
            set { CheckStatus(-9); }
        }

        public double VoltageRampRate
        {
            get
            {
                return GetParamInt32(ParamID.eRampThrottle);
            }
            set
            {
                int rate = (int)(value * 1023.0 / 12.0 / 100.0);
                C_TalonSRX_SetParam(m_impl, (int)ParamID.eRampThrottle, rate);
            }
        }

        public uint FirmwareVersion
        {
            get
            {
                int version = 0;
                C_TalonSRX_GetFirmVers(m_impl, ref version);
                return (uint)version;
            }
        }

        [Obsolete("Use DeviceID property instead.")]
        public int GetDeviceID() { return DeviceID; }

        public int DeviceID { get; }

        [Obsolete("Use ForwardSoftLimit property instead.")]
        public double GetForwardSoftLimit() { return ForwardSoftLimit; }
        [Obsolete("Use ForwardSoftLimit poperty instead.")]
        public void SetForwardSoftLimit(double value) { ForwardSoftLimit = value; }

        public double ForwardSoftLimit
        {
            get
            {
                return GetParam(ParamID.eProfileParamSoftLimitForThreshold);
            }
            set
            {

                SetParam(ParamID.eProfileParamSoftLimitForThreshold, value);
            }
        }

        [Obsolete("Use ForwardSoftLimitEnabled property instead.")]
        public bool GetForwardSoftLimitEnabled() { return ForwardSoftLimitEnabled; }
        [Obsolete("Use ForwardSoftLimitEnabled poperty instead.")]
        public void SetForwardSoftLimitEnabled(bool value)
        {
            ForwardSoftLimitEnabled = value;
        }
        public bool ForwardSoftLimitEnabled
        {
            get
            {
                return GetParamInt32(ParamID.eProfileParamSoftLimitForEnable) != 0;
            }
            set
            {

                SetParam(ParamID.eProfileParamSoftLimitForEnable, value ? 1 : 0);
            }
        }

        [Obsolete("Use ReverseSoftLimit property instead.")]
        public double GetReverseSoftLimit() { return ReverseSoftLimit; }
        [Obsolete("Use ReverseSoftLimit poperty instead.")]
        public void SetReverseSoftLimit(double value) { ReverseSoftLimit = value; }

        public double ReverseSoftLimit
        {
            get
            {
                return GetParam(ParamID.eProfileParamSoftLimitRevThreshold);
            }
            set
            {

                SetParam(ParamID.eProfileParamSoftLimitRevThreshold, value);
            }
        }

        [Obsolete("Use ReverseSoftLimitEnabled property instead.")]
        public bool GetReverseSoftLimitEnabled() { return ReverseSoftLimitEnabled; }
        [Obsolete("Use ReverseSoftLimitEnabled poperty instead.")]
        public void SetReverseSoftLimitEnabled(bool value)
        {
            ReverseSoftLimitEnabled = value;
        }
        public bool ReverseSoftLimitEnabled
        {
            get
            {
                return GetParamInt32(ParamID.eProfileParamSoftLimitRevEnable) != 0;
            }
            set
            {

                SetParam(ParamID.eProfileParamSoftLimitRevEnable, value ? 1 : 0);
            }
        }
        public void ClearStickyFaults()
        {
            C_TalonSRX_ClearStickyFaults(m_impl);
        }

        public void EnableLimitSwitches(bool forward, bool reverse)
        {
            int mask = 1 << 2 | (forward ? 1 : 0) << 1 | (reverse ? 1 : 0);
            CTR_Code status = C_TalonSRX_SetOverrideLimitSwitchEn(m_impl, mask);
            if (status != CTR_Code.CTR_OKAY)
                CheckStatus((int)status);
        }

        [Obsolete("Use ForwardLimitSwitchNormallyOpen property instead.")]
        public void ConfigFwdLimitSwitchNormallyOpen(bool value) { ForwardLimitSwitchNormallyOpen = value; }

        public bool ForwardLimitSwitchNormallyOpen
        {
            get
            {
                return GetParamInt32(ParamID.eOnBoot_LimitSwitch_Forward_NormallyClosed) != 0;
            }
            set
            {
                SetParam(ParamID.eOnBoot_LimitSwitch_Forward_NormallyClosed, value ? 0 : 1);
            }
        }

        [Obsolete("Use ReverseLimitSwitchNormallyOpen property instead.")]
        public void ConfigRevLimitSwitchNormallyOpen(bool value) { ReverseLimitSwitchNormallyOpen = value; }

        public bool ReverseLimitSwitchNormallyOpen
        {
            get
            {
                return GetParamInt32(ParamID.eOnBoot_LimitSwitch_Reverse_NormallyClosed) != 0;
            }
            set
            {
                SetParam(ParamID.eOnBoot_LimitSwitch_Reverse_NormallyClosed, value ? 0 : 1);
            }
        }


        [Obsolete("Use ConfigNeutralMode instead")]
        public void EnableBrakeMode(bool brake)
        {
            C_TalonSRX_SetOverrideBrakeType(m_impl, brake ? 2 : 1);
        }

        public int FaultOverTemp
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_OverTemp(m_impl, ref val);
                return val;
            }
        }

        public int FaultUnderVoltage
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_UnderVoltage(m_impl, ref val);
                return val;
            }
        }

        public int FaultForwardLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_ForLim(m_impl, ref val);
                return val;
            }
        }

        public int FaultReverseLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_RevLim(m_impl, ref val);
                return val;
            }
        }

        public int FaultHardwareFailure
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_HardwareFailure(m_impl, ref val);
                return val;
            }
        }

        public int FaultForwardSoftLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_ForSoftLim(m_impl, ref val);
                return val;
            }
        }

        public int FaultReverseSoftLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetFault_RevSoftLim(m_impl, ref val);
                return val;
            }
        }

        public int StickyFaultOverTemp
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetStckyFault_OverTemp(m_impl, ref val);
                return val;
            }
        }

        public int StickyFaultUnderVoltage
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetStckyFault_UnderVoltage(m_impl, ref val);
                return val;
            }
        }

        public int StickyFaultForwardLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetStckyFault_ForLim(m_impl, ref val);
                return val;
            }
        }

        public int StickyFaultReverseLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetStckyFault_RevLim(m_impl, ref val);
                return val;
            }
        }

        public int StickyFaultForwardSoftLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetStckyFault_ForSoftLim(m_impl, ref val);
                return val;
            }
        }

        public int StickyFaultReverseSoftLimit
        {
            get
            {
                int val = 0;
                C_TalonSRX_GetStckyFault_RevSoftLim(m_impl, ref val);
                return val;
            }
        }

        public double Expiration
        {
            set { m_safetyHelper.Expiration = value; }
            get { return m_safetyHelper.Expiration; }
        }

        public bool Alive => m_safetyHelper.Alive;

        [Obsolete("Set the ControlEnabled to false.")]
        public void StopMotor()
        {
            ControlEnabled = false;
        }

        public bool SafetyEnabled
        {
            set { m_safetyHelper.SafetyEnabled = value; }
            get { return m_safetyHelper.SafetyEnabled; }
        }

        public string Description => $"CAN TalonSRX ID {DeviceID}";

        public void PidWrite(double value)
        {
            if (m_controlMode == ControlMode.PercentVbus)
            {
                Set(value);
            }
            else
            {
                throw new InvalidOperationException("PID on RoboRIO only supported in Voltage Bus (PWM-like) mode");
            }
        }

        public void Set(double value)
        {
            m_safetyHelper.Feed();
            if (m_controlEnabled)
            {
                m_setPoint = value;
                CTR_Code status = CTR_Code.CTR_OKAY;
                switch (m_controlMode)
                {
                    case ControlMode.PercentVbus:
                        C_TalonSRX_SetDemand(m_impl, Inverted ? -((int) (value*1023)) : ((int)(value * 1023)));
                        status = CTR_Code.CTR_OKAY;
                        break;
                    case ControlMode.Voltage:
                        int volts = (int) (value*256);
                        status = C_TalonSRX_SetDemand(m_impl, Inverted ? -volts : volts);
                        break;
                    case ControlMode.Position:
                    case ControlMode.Speed:
                    case ControlMode.Follower:
                        status = C_TalonSRX_SetDemand(m_impl, Inverted ? (int) -value : (int) value);
                        break;
                    case ControlMode.Current:
                    default:
                        status = CTR_Code.CTR_OKAY;
                        break;
                }
                CheckStatus((int)status);
                status = C_TalonSRX_SetModeSelect(m_impl, (int) MotorControlMode);
                CheckStatus((int)status);

            }
        }

        public double Get()
        {
            int value = 0;
            switch (m_controlMode)
            {
                case ControlMode.Voltage:
                    return GetOutputVoltage();
                case ControlMode.Position:
                    C_TalonSRX_GetSensorPosition(m_impl, ref value);
                    return value;
                case ControlMode.Speed:
                    C_TalonSRX_GetSensorVelocity(m_impl, ref value);
                    return value;
                case ControlMode.Current:
                    return GetOutputCurrent();
                case ControlMode.Follower:
                case ControlMode.PercentVbus:
                default:
                    C_TalonSRX_GetAppliedThrottle(m_impl, ref value);
                    return value/1023.0;
            }
        }

        [Obsolete("This is only here to make CAN Jaguars happy")]
        public void Set(double value, byte syncGroup)
        {
            Set(value);
        }

        public void Reset()
        {
            Disable();
            ClearIAccum();
        }

        public bool Enabled => ControlEnabled;

        public double GetError() => GetClosedLoopError();

        public void SelectProfileSlot(int slotIdx)
        {
            m_profile = (slotIdx == 0) ? 0 : 1;
            CTR_Code status = C_TalonSRX_SetProfileSlotSelect(m_impl, m_profile);
            if (status != CTR_Code.CTR_OKAY)
                CheckStatus((int)status);
        }

        public void Disable()
        {
            ControlEnabled = false;
        }

        ///<inheritdoc/>
        public void UpdateTable()
        {
            Table?.PutNumber("Value", Get());
        }

        ///<inheritdoc/>
        public void StartLiveWindowMode()
        {
            Set(0.0);
            Table.AddTableListener("Value", this, true);
        }

        ///<inheritdoc/>
        public void StopLiveWindowMode()
        {
            Set(0.0);
            Table.RemoveTableListener(this);
        }

        ///<inheritdoc/>
        public string SmartDashboardType => "Speed Controller";

        ///<inheritdoc/>
        public void ValueChanged(ITable source, string key, object value, bool isNew)
        {
            Set((double)value);
        }

        ///<inheritdoc/>
        public void InitTable(ITable subtable)
        {
            Table = subtable;
            UpdateTable();
        }

        ///<inheritdoc/>
        public ITable Table { get; private set; }
    }
}
