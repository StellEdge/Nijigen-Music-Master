using System;
using System.Linq;

namespace NReplayGain
{
    /// <summary>
    /// Contains ReplayGain data for a track.
    /// </summary>
    public class TrackGain : IDisposable
    {
        private int sampleSize;
        internal GainData gainData;

        private double[] lInPreBuf;
        private CPtr<double> lInPre;
        private double[] lStepBuf;
        private CPtr<double> lStep;
        private double[] lOutBuf;
        private CPtr<double> lOut;

        private double[] rInPreBuf;
        private CPtr<double> rInPre;
        private double[] rStepBuf;
        private CPtr<double> rStep;
        private double[] rOutBuf;
        private CPtr<double> rOut;

        private long sampleWindow;
        private long totSamp;
        private double lSum;
        private double rSum;
        private int freqIndex;

        public TrackGain(int sampleRate, int sampleSize)
        {
            if(!ReplayGain.IsSupportedFormat(sampleRate, sampleSize)) {
                throw new NotSupportedException("Unsupported format. Supported sample sizes are 16, 24.");
            }

            this.freqIndex = ReplayGain.FreqInfos.IndexOf(i => i.SampleRate == sampleRate);

            this.sampleSize = sampleSize;
            this.gainData = new GainData();

            this.lInPreBuf = new double[ReplayGain.MAX_ORDER * 2];
            this.lStepBuf = new double[ReplayGain.MAX_SAMPLES_PER_WINDOW + ReplayGain.MAX_ORDER];
            this.lOutBuf = new double[ReplayGain.MAX_SAMPLES_PER_WINDOW + ReplayGain.MAX_ORDER];
            this.rInPreBuf = new double[ReplayGain.MAX_ORDER * 2];
            this.rStepBuf = new double[ReplayGain.MAX_SAMPLES_PER_WINDOW + ReplayGain.MAX_ORDER];
            this.rOutBuf = new double[ReplayGain.MAX_SAMPLES_PER_WINDOW + ReplayGain.MAX_ORDER];

            this.sampleWindow = (int)Math.Ceiling(sampleRate * ReplayGain.RMS_WINDOW_TIME);

            this.lInPre = new CPtr<double>(lInPreBuf, ReplayGain.MAX_ORDER);
            this.lStep = new CPtr<double>(lStepBuf, ReplayGain.MAX_ORDER);
            this.lOut = new CPtr<double>(lOutBuf, ReplayGain.MAX_ORDER);
            this.rInPre = new CPtr<double>(rInPreBuf, ReplayGain.MAX_ORDER);
            this.rStep = new CPtr<double>(rStepBuf, ReplayGain.MAX_ORDER);
            this.rOut = new CPtr<double>(rOutBuf, ReplayGain.MAX_ORDER);
        }

        public void AnalyzeSamples(int[] leftSamples, int[] rightSamples)
        {
            if (leftSamples.Length != rightSamples.Length)
            {
                throw new ArgumentException("leftSamples must be as big as rightSamples");
            }

            int numSamples = leftSamples.Length;

            double[] leftDouble = new double[numSamples];
            double[] rightDouble = new double[numSamples];

            if (this.sampleSize == 16)
            {
                for (int i = 0; i < numSamples; ++i)
                {
                    leftDouble[i] = leftSamples[i];
                    rightDouble[i] = rightSamples[i];
                }
            }
            else if (this.sampleSize == 24)
            {
                for (int i = 0; i < numSamples; ++i)
                {
                    leftDouble[i] = leftSamples[i] * ReplayGain.FACTOR_24BIT;
                    rightDouble[i] = rightSamples[i] * ReplayGain.FACTOR_24BIT;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            double tmpPeak;
            for (int i = 0; i < numSamples; ++i)
            {
                tmpPeak = leftDouble[i] >= 0 ? leftDouble[i] : -leftDouble[i];
                if (tmpPeak > this.gainData.PeakSample) this.gainData.PeakSample = tmpPeak;

                tmpPeak = rightDouble[i] >= 0 ? rightDouble[i] : -rightDouble[i];
                if (tmpPeak > this.gainData.PeakSample) this.gainData.PeakSample = tmpPeak;
            }

            this.AnalyzeSamples(new CPtr<double>(leftDouble), new CPtr<double>(rightDouble));
        }

        private static double Sqr(double d)
        {
            return d * d;
        }

        private void FilterYule(CPtr<double> input, CPtr<double> output, long nSamples, double[] aKernel, double[] bKernel)
        {
            while (nSamples-- != 0)
            {
                output[0] = 1e-10  /* 1e-10 is a hack to avoid slowdown because of denormals */
                  + input[0] * bKernel[0]
                  - output[-1] * aKernel[1]
                  + input[-1] * bKernel[1]
                  - output[-2] * aKernel[2]
                  + input[-2] * bKernel[2]
                  - output[-3] * aKernel[3]
                  + input[-3] * bKernel[3]
                  - output[-4] * aKernel[4]
                  + input[-4] * bKernel[4]
                  - output[-5] * aKernel[5]
                  + input[-5] * bKernel[5]
                  - output[-6] * aKernel[6]
                  + input[-6] * bKernel[6]
                  - output[-7] * aKernel[7]
                  + input[-7] * bKernel[7]
                  - output[-8] * aKernel[8]
                  + input[-8] * bKernel[8]
                  - output[-9] * aKernel[9]
                  + input[-9] * bKernel[9]
                  - output[-10] * aKernel[10]
                  + input[-10] * bKernel[10];
                ++output;
                ++input;
            }
        }

        private void FilterButter(CPtr<double> input, CPtr<double> output, long nSamples, double[] aKernel, double[] bKernel)
        {
            while (nSamples-- != 0)
            {
                output[0] =
                   input[0] * bKernel[0]
                 - output[-1] * aKernel[1]
                 + input[-1] * bKernel[1]
                 - output[-2] * aKernel[2]
                 + input[-2] * bKernel[2];
                ++output;
                ++input;
            }
        }

        private void AnalyzeSamples(CPtr<double> leftSamples, CPtr<double> rightSamples)
        {
            int numSamples = leftSamples.Length;

            CPtr<double> curLeft;
            CPtr<double> curRight;
            long batchSamples = numSamples;
            long curSamples;
            long curSamplePos = 0;

            if (numSamples < ReplayGain.MAX_ORDER)
            {
                Array.Copy(leftSamples.Array, 0, this.lInPreBuf, ReplayGain.MAX_ORDER, numSamples);
                Array.Copy(rightSamples.Array, 0, this.rInPreBuf, ReplayGain.MAX_ORDER, numSamples);
            }
            else
            {
                Array.Copy(leftSamples.Array, 0, this.lInPreBuf, ReplayGain.MAX_ORDER, ReplayGain.MAX_ORDER);
                Array.Copy(rightSamples.Array, 0, this.rInPreBuf, ReplayGain.MAX_ORDER, ReplayGain.MAX_ORDER);
            }

            while (batchSamples > 0)
            {
                curSamples = batchSamples > this.sampleWindow - this.totSamp ? this.sampleWindow - this.totSamp : batchSamples;
                if (curSamplePos < ReplayGain.MAX_ORDER)
                {
                    curLeft = this.lInPre + curSamplePos;
                    curRight = this.rInPre + curSamplePos;
                    if (curSamples > ReplayGain.MAX_ORDER - curSamplePos)
                        curSamples = ReplayGain.MAX_ORDER - curSamplePos;
                }
                else
                {
                    curLeft = leftSamples + curSamplePos;
                    curRight = rightSamples + curSamplePos;
                }

                FilterYule(curLeft, this.lStep + this.totSamp, curSamples, ReplayGain.FreqInfos[this.freqIndex].AYule, ReplayGain.FreqInfos[this.freqIndex].BYule);
                FilterYule(curRight, this.rStep + this.totSamp, curSamples, ReplayGain.FreqInfos[this.freqIndex].AYule, ReplayGain.FreqInfos[this.freqIndex].BYule);

                FilterButter(this.lStep + this.totSamp, this.lOut + this.totSamp, curSamples, ReplayGain.FreqInfos[this.freqIndex].AButter, ReplayGain.FreqInfos[this.freqIndex].BButter);
                FilterButter(this.rStep + this.totSamp, this.rOut + this.totSamp, curSamples, ReplayGain.FreqInfos[this.freqIndex].AButter, ReplayGain.FreqInfos[this.freqIndex].BButter);

                curLeft = this.lOut + this.totSamp;                   // Get the squared values
                curRight = this.rOut + this.totSamp;

                for (long i = curSamples % 16; i-- != 0; )
                {
                    this.lSum += Sqr(curLeft[0]);
                    ++curLeft;
                    this.rSum += Sqr(curRight[0]);
                    ++curRight;
                }

                for (long i = curSamples / 16; i-- != 0; )
                {
                    this.lSum += Sqr(curLeft[0])
                          + Sqr(curLeft[1])
                          + Sqr(curLeft[2])
                          + Sqr(curLeft[3])
                          + Sqr(curLeft[4])
                          + Sqr(curLeft[5])
                          + Sqr(curLeft[6])
                          + Sqr(curLeft[7])
                          + Sqr(curLeft[8])
                          + Sqr(curLeft[9])
                          + Sqr(curLeft[10])
                          + Sqr(curLeft[11])
                          + Sqr(curLeft[12])
                          + Sqr(curLeft[13])
                          + Sqr(curLeft[14])
                          + Sqr(curLeft[15]);
                    curLeft += 16;
                    this.rSum += Sqr(curRight[0])
                          + Sqr(curRight[1])
                          + Sqr(curRight[2])
                          + Sqr(curRight[3])
                          + Sqr(curRight[4])
                          + Sqr(curRight[5])
                          + Sqr(curRight[6])
                          + Sqr(curRight[7])
                          + Sqr(curRight[8])
                          + Sqr(curRight[9])
                          + Sqr(curRight[10])
                          + Sqr(curRight[11])
                          + Sqr(curRight[12])
                          + Sqr(curRight[13])
                          + Sqr(curRight[14])
                          + Sqr(curRight[15]);
                    curRight += 16;
                }

                batchSamples -= curSamples;
                curSamplePos += curSamples;
                this.totSamp += curSamples;
                if (this.totSamp == this.sampleWindow)
                {
                    // Get the Root Mean Square (RMS) for this set of samples
                    double val = ReplayGain.STEPS_PER_DB * 10.0 * Math.Log10((this.lSum + this.rSum) / this.totSamp * 0.5 + 1.0e-37);
                    int ival = (int)val;
                    if (ival < 0) ival = 0;
                    if (ival >= this.gainData.Accum.Length) ival = this.gainData.Accum.Length - 1;
                    this.gainData.Accum[ival]++;
                    this.lSum = this.rSum = 0.0;

                    if (this.totSamp > int.MaxValue)
                    {
                        throw new OverflowException("Too many samples! Change to long and recompile!");
                    }

                    Array.Copy(this.lOutBuf, (int)this.totSamp, this.lOutBuf, 0, ReplayGain.MAX_ORDER);
                    Array.Copy(this.rOutBuf, (int)this.totSamp, this.rOutBuf, 0, ReplayGain.MAX_ORDER);
                    Array.Copy(this.lStepBuf, (int)this.totSamp, this.lStepBuf, 0, ReplayGain.MAX_ORDER);
                    Array.Copy(this.rStepBuf, (int)this.totSamp, this.rStepBuf, 0, ReplayGain.MAX_ORDER);

                    this.totSamp = 0;
                }
                if (this.totSamp > this.sampleWindow)
                {
                    // somehow I really screwed up: Error in programming! Contact author about totsamp > sampleWindow
                    throw new Exception("Gain analysis error!");
                }
            }

            if (numSamples < ReplayGain.MAX_ORDER)
            {
                Array.Copy(this.lInPreBuf, numSamples, this.lInPreBuf, 0, ReplayGain.MAX_ORDER - numSamples);
                Array.Copy(this.rInPreBuf, numSamples, this.rInPreBuf, 0, ReplayGain.MAX_ORDER - numSamples);
                Array.Copy(leftSamples.Array, leftSamples.Index, this.lInPreBuf, ReplayGain.MAX_ORDER - numSamples, numSamples);
                Array.Copy(rightSamples.Array, rightSamples.Index, this.rInPreBuf, ReplayGain.MAX_ORDER - numSamples, numSamples);
            }
            else
            {
                Array.Copy(leftSamples.Array, leftSamples.Index + numSamples - ReplayGain.MAX_ORDER, this.lInPreBuf, 0, ReplayGain.MAX_ORDER);
                Array.Copy(rightSamples.Array, rightSamples.Index + numSamples - ReplayGain.MAX_ORDER, this.rInPreBuf, 0, ReplayGain.MAX_ORDER);
            }
        }

        /// <summary>
        /// Returns the normalization gain for the track in decibels.
        /// </summary>
        public double GetGain()
        {
            return ReplayGain.AnalyzeResult(this.gainData.Accum);
        }

        /// <summary>
        /// Returns the peak title value, normalized to the [0,1] interval.
        /// </summary>
        public double GetPeak()
        {
            return this.gainData.PeakSample / ReplayGain.MAX_SAMPLE_VALUE;
        }

        /// <summary>
        /// Disposes the resources used to calculate ReplayGain, but doesn't clear the result of the analysis.
        /// </summary>
        public void Dispose()
        {
            this.lInPreBuf = null;
            this.lStepBuf = null;
            this.lOutBuf = null;
            this.rInPreBuf = null;
            this.rStepBuf = null;
            this.rOutBuf = null;

            this.lInPre = new CPtr<double>();
            this.lStep = new CPtr<double>();
            this.lOut = new CPtr<double>();
            this.rInPre = new CPtr<double>();
            this.rStep = new CPtr<double>();
            this.rOut = new CPtr<double>();
        }
    }
}