using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaucheProjet
{
    public class FPSCounter
    {
        #region Vars

        public double lastFps;
        public double fps;
        public int drawInterval;
        public int lastDraw;
        public int globalTime;

        public List<double> fpsList;
        public double realFps;

        #endregion Vars


        public FPSCounter()
        {
            lastFps = 0;
            fps = 0;
            drawInterval = 0;
            lastDraw = 0;
            globalTime = 0;

            fpsList = new List<double>();
        }

        public void UpdateFPS(int time)
        {
            globalTime = time;
            drawInterval = time - lastDraw;
            lastDraw = time;
            lastFps = fps;
            fps = 1000 / (drawInterval==0?1:drawInterval);

            fpsList.Add(fps);
            if(fpsList.Count > 30) fpsList.RemoveAt(0);

            realFps = 0;
            foreach (double d in fpsList)
            {
                realFps += d;
            }

            realFps /= fpsList.Count;
            realFps = (int)realFps;
        }

        public string GetFPS()
        {
            return (fpsList.Count > 0) ? realFps.ToString() :"";
        }
    }
}
