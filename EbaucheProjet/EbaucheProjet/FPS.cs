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
        
        #endregion Vars


        public FPSCounter()
        {
            lastFps = 0;
            fps = 0;
            drawInterval = 0;
            lastDraw = 0;
            globalTime = 0;
        }

        public void UpdateFPS(int time)
        {
            globalTime = time;
            drawInterval = time - lastDraw;
            lastDraw = time;
            lastFps = fps;
            fps = 1000 / (drawInterval==0?1:drawInterval);
        }

        public void ShowFPS()
        {
            int x, y;
            x = Console.CursorLeft; y = Console.CursorTop;
            Console.SetCursorPosition(78, 0);
            Console.WriteLine(fps);
            Console.SetCursorPosition(x, y);
        }
    }
}
