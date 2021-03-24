using System;
using System.Linq;

namespace NReplayGain
{
    class GainData
    {
        public int[] Accum { get; private set; }
        public double PeakSample { get; set; }

        public GainData()
        {
            this.Accum = new int[ReplayGain.STEPS_PER_DB * ReplayGain.MAX_DB];
        }
    }
}
