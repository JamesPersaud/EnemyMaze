/*
 * Note to LD - this timer is something I've written before but the code is freely available at http://cplus.about.com/library/downloads/general/timing/csharp/hires.cs (that version not written my me)
 * 
 * I've added some stuff.
 */

using System;
using System.Runtime.InteropServices;

namespace ld18_EnemyDefence
{
    public class GameTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long start = 0;
        private long stop = 0;        
       
        private static double frequency = getFrequency();       
        private static double getFrequency()
        {
            long tempfrequency;
            QueryPerformanceFrequency(out tempfrequency);
            return tempfrequency; // implicit casting to double from long
        }

        public void Start()
        {
            QueryPerformanceCounter(out start);
        }

        public void Stop()
        {
            QueryPerformanceCounter(out stop);          
        }

        public double Elapsed
        {
            get
            {
                return (double)(stop - start) / frequency;
            }
        }

        public float FPS
        {
            get
            {
                Stop();
                float f = 1.0f/(float)Elapsed;
                Start();
                return f;
            }
        }

        public float GetTicks(float ticklen)
        {
            return (float)Elapsed / ticklen;
        }
    }
}
