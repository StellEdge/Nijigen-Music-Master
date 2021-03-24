using System;
using System.Linq;

namespace NReplayGain
{
    class FrequencyInfo
    {
        public uint SampleRate { get; private set; }
        public double[] BYule { get; private set; }
        public double[] AYule { get; private set; }
        public double[] BButter { get; private set; }
        public double[] AButter { get; private set; }

        public FrequencyInfo(uint sampleRate, double[] bYule, double[] aYule, double[] bButter, double[] aButter)
        {
            this.SampleRate = sampleRate;
            this.BYule = bYule;
            this.AYule = aYule;
            this.BButter = bButter;
            this.AButter = aButter;
        }
    }
}
