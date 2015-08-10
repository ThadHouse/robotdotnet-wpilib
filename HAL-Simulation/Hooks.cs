﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HAL_Simulator
{
    public static class Hooks
    {
        public static uint GetFPGATime()
        {//TODO: Implement
            return (uint)((DateTime.Now.Ticks - SimData.halData["time"]["program_start"]) / 10);
            //Testing
        }

        public static uint GetTime()
        {//TODO: Implement
            return (uint)(DateTime.Now.Ticks);
        }

        public static void DelayMillis(double ms)
        {
            Thread.Sleep((int)ms);
        }

        public static void DelaySeconds(double s)
        {
            Thread.Sleep((int)s * 1000);
        } 
    }
}
