using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace GazeLaser.Processor
{
    [Serializable]
    public class HeadCorrector
    {
        #region Members

        private readonly List<string> iSourcesOfEyePos = new List<string>()
        {
            "Tobii T/X-series",
            "The EyeTribe ET-1000",
            "Dikablis",
            "Mouse"
        };

        private PointF iReference = PointF.Empty;
        private PointF iOffset = PointF.Empty;
        private bool iHasValidEyePos = false;

        #endregion

        #region Properties

        public bool Enabled { get; set; } = false;
        public int Factor { get; set; } = 500;

        [System.Xml.Serialization.XmlIgnore]
        public bool IsTuning { get; private set; } = false;

        public PointF LinearCorrection { get { return new PointF(-iOffset.X, iOffset.Y); } }

        #endregion

        #region Public methods

        public void start(string aSource)
        {
            iHasValidEyePos = iSourcesOfEyePos.Contains(aSource);
        }

        public void stop()
        {
            IsTuning = false;
            iHasValidEyePos = false;
            iOffset = new PointF(0, 0);
        }

        public void feed(ETUDriver.SiETUDSample aSample)
        {
            if (Enabled && iHasValidEyePos)
            {
                PointF eyePos = GetEyePos(aSample);

                if (!IsTuning)
                {
                    iReference = eyePos;
                    IsTuning = true;
                }
                else
                {
                    iOffset = new PointF(
                        (eyePos.X - iReference.X) * Factor,
                        (eyePos.Y - iReference.Y) * Factor);
                }
            }
        }

        public PointF correct(PointF aPoint)
        {
            PointF result = aPoint;
            if (IsTuning)
            {
                result.X -= iOffset.X;
                result.Y += iOffset.Y;
            }
            return result;
        }

        #endregion

        private unsafe PointF GetEyePos(ETUDriver.SiETUDSample aSample)
        {
            PointF result = PointF.Empty;
            try
            {
                UInt32 camX = *((UInt32*)&(aSample.Offset.X));
                UInt32 camY = *((UInt32*)&(aSample.Offset.Y));

                float ecxlRel = (float)(camX & 0xFFFF) / 0xFFFF;
                float ecylRel = (float)(camY & 0xFFFF) / 0xFFFF;
                //float ecxrRel = (float)((camX & 0xFFFF0000) >> 16) / 0xFFFF;
                //float ecyrRel = (float)((camY & 0xFFFF0000) >> 16) / 0xFFFF;

                float ecxl = Screen.PrimaryScreen.Bounds.Width * ecxlRel;
                float ecyl = Screen.PrimaryScreen.Bounds.Height * ecylRel;
                //float ecxr = Screen.PrimaryScreen.Bounds.Width * ecxrRel;
                //float ecyr = Screen.PrimaryScreen.Bounds.Height * ecyrRel;

                result = new PointF(ecxl, ecyl);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return result;
        }
    }
}
