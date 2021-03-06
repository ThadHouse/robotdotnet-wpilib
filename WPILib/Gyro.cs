﻿using System;
using System.CodeDom;
using HAL_Base;
using NetworkTables.Tables;
using WPILib.Exceptions;
using WPILib.Interfaces;
using WPILib.LiveWindows;

namespace WPILib
{
    /// <summary>
    /// Class for interfacing with an analog gyro to get robot heading.
    /// </summary>
    public class Gyro : SensorBase, IPIDSource, ILiveWindowSendable
    {
        private static int kOversampleBits = 10;
        private static int kAverageBits = 0;
        private static double kSamplesPerSecond = 50.0;
        private static double kCalibrationSampleTime = 5.0;
        private static double kDefaultVoltsPerDegreePerSecond = 0.007;

        protected AnalogInput m_analog;
        private double m_offset;
        private int m_center;
        bool m_channelAllocated = false;
        private PIDSourceParameter m_pidSource;

        public void InitGyro()
        {
            if (m_analog == null)
            {
                Console.WriteLine("Null m_analog");
                throw new NullReferenceException("m_analog");
            }
            if (!m_analog.IsAccumulatorChannel)
            {
                throw new InvalidValueException("channel must be an accumulator channel");
            }
            Sensitivity = kDefaultVoltsPerDegreePerSecond;
            m_analog.AverageBits = kAverageBits;
            m_analog.OversampleBits = kOversampleBits;
            double sampleRate = kSamplesPerSecond
                    * (1 << (kAverageBits + kOversampleBits));
            AnalogInput.GlobalSampleRate = sampleRate;
            Timer.Delay(1.0);

            m_analog.InitAccumulator();
            m_analog.ResetAccumulator();

            Timer.Delay(kCalibrationSampleTime);

            long value = 0;
            uint count = 0;
            m_analog.GetAccumulatorOutput(ref value, ref count);

            m_center = (int)((double)value / (double)count + .5);

            m_offset = ((double)value / (double)count)
                    - m_center;

            
            m_analog.AccumulatorCenter = m_center;
            m_analog.ResetAccumulator();

            Deadband = 0.0;

            PIDSourceParameter = PIDSourceParameter.Angle;

            HAL.Report(ResourceType.kResourceType_Gyro, (byte)m_analog.Channel);
            LiveWindow.AddSensor("Gyro", m_analog.Channel, this);

        }

        public Gyro(int channel) : this(new AnalogInput(channel))
        {
            m_channelAllocated = true;
        }

        public Gyro(AnalogInput channel)
        {
            m_analog = channel;
            if (m_analog == null)
            {
                throw new NullReferenceException("AnalogInput supplied to Gyro constructor is null");
            }
            InitGyro();
        }

        public void Reset() => m_analog?.ResetAccumulator();

        ///<inheritdoc/>
        public override void Dispose()
        {
            if (m_analog != null && m_channelAllocated)
            {
                m_analog.Dispose();
            }
            m_analog = null;
            //base.Dispose();
        }

        public double GetAngle()
        {
            if (m_analog == null)
            {
                return 0.0;
            }
            else
            {
                long rawValue = 0;
                uint count = 0;
                m_analog.GetAccumulatorOutput(ref rawValue, ref count);

                long value = rawValue - (long) (count*m_offset);

                double scaledValue = value
                                     *1e-9
                                     *m_analog.LSBWeight
                                     *(1 << m_analog.AverageBits)
                                     /(AnalogInput.GlobalSampleRate*Sensitivity);

                return scaledValue;
            }
        }

        public double GetRate()
        {
            if (m_analog == null)
            {
                return 0.0;
            }
            else
            {
                return (m_analog.GetAverageValue() - (m_center + m_offset))
                       *1e-9
                       *m_analog.LSBWeight
                       /((1 << m_analog.OversampleBits)*Sensitivity);
            }
        }

        public double Sensitivity { get; set; }

        private double Deadband
        {
            set
            {
                int deadband = (int) (value*1e9/m_analog.LSBWeight*(1 << m_analog.OversampleBits));
                m_analog.AccumulatorDeadband = deadband;
            }
        }

        public PIDSourceParameter PIDSourceParameter
        {
            set
            {
                BoundaryException.AssertWithinBounds((int) value, 1, 2);
                m_pidSource = value;
            }
        }

        public double PidGet()
        {
            switch (m_pidSource)
            {
                case PIDSourceParameter.Rate:
                    return GetRate();
                case PIDSourceParameter.Angle:
                    return GetAngle();
                default:
                    return 0.0;
            }
        }
        ///<inheritdoc />
        public void InitTable(ITable subtable)
        {
            Table = subtable;
            UpdateTable();
        }
        ///<inheritdoc />
        public ITable Table { get; private set; }

        ///<inheritdoc />
        public string SmartDashboardType => "Gyro";
        ///<inheritdoc />
        public void UpdateTable()
        {
            Table?.PutNumber("Value", GetAngle());
        }
        ///<inheritdoc />
        public void StartLiveWindowMode()
        {
        }
        ///<inheritdoc />
        public void StopLiveWindowMode()
        {
        }
    }
}
