﻿using WPILib.Exceptions;

namespace WPILib
{
    /// <summary>
    /// High level robot state reporting.
    /// </summary>
    public static class RobotState
    {
        public static Interface Implementation { private get; set; }

        public static bool Disabled
        {
            get
            {
                if (Implementation != null)
                {
                    return Implementation.Disabled;
                }
                throw new BaseSystemNotInitializedException(Implementation, typeof (RobotBase));
            }
        }

        public static bool Enabled
        {
            get
            {
                if (Implementation != null)
                {
                    return Implementation.Enabled;
                }
                throw new BaseSystemNotInitializedException(Implementation, typeof (RobotBase));
            }
        }

        public static bool OperatorControl
        {
            get
            {
                if (Implementation != null)
                {
                    return Implementation.OperatorControl;
                }
                throw new BaseSystemNotInitializedException(Implementation, typeof (RobotBase));
            }
        }

        public static bool Autonomous
        {
            get
            {
                if (Implementation != null)
                {
                    return Implementation.Autonomous;
                }
                throw new BaseSystemNotInitializedException(Implementation, typeof (RobotBase));
            }
        }

        public static bool Test
        {
            get
            {
                if (Implementation != null)
                {
                    return Implementation.Test;
                }
                throw new BaseSystemNotInitializedException(Implementation, typeof (RobotBase));
            }
        }

        /// <summary>
        /// Interface for robot state reporters.
        /// </summary>
        public interface Interface
        {
            bool Disabled { get; }
            bool Enabled { get; }
            bool OperatorControl { get; }
            bool Autonomous { get; }
            bool Test{ get; }
        }
    }
}
