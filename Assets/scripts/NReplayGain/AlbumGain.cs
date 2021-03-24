using System;
using System.Linq;

namespace NReplayGain
{
    /// <summary>
    /// Contains ReplayGain data for an album.
    /// </summary>
    public class AlbumGain
    {
        private GainData albumData;

        public AlbumGain()
        {
            this.albumData = new GainData();
        }

        /// <summary>
        /// After calculating the ReplayGain data for an album, call this to append the data to the album.
        /// </summary>
        public void AppendTrackData(TrackGain trackGain)
        {
            int[] sourceAccum = trackGain.gainData.Accum;
            for (int i = 0; i < sourceAccum.Length; ++i)
            {
                this.albumData.Accum[i] += sourceAccum[i];
            }
            this.albumData.PeakSample = Math.Max(this.albumData.PeakSample, trackGain.gainData.PeakSample);
        }

        /// <summary>
        /// Returns the normalization gain for the entire album in decibels.
        /// </summary>
        public double GetGain()
        {
            return ReplayGain.AnalyzeResult(this.albumData.Accum);
        }

        /// <summary>
        /// Returns the peak album value, normalized to the [0,1] interval.
        /// </summary>
        public double GetPeak()
        {
            return this.albumData.PeakSample / ReplayGain.MAX_SAMPLE_VALUE;
        }
    }
}
