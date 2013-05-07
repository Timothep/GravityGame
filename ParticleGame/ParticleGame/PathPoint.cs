using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ParticleGame
{
	/// <summary>
	/// Represents a single dot from a planet's projected path
	/// </summary>
    class PathPoint
    {
        public bool IsOld
        {
            get
            {
                return (ttl <= 0);
            }
            
        }

        private int x;
        public int X
        {
            get
            {
                return x;
            }
        }
        private int y;
        public int Y
        {
            get
            {
                return y;
            }
        }
        private int ttl;
        public int TTL
        {
            get
            {
                return ttl;
            }
        }
        const int fadePoint = 240;

        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
        }

        public PathPoint(int x, int y, int ttl, Color color)
        {
            this.x = x;
            this.y = y;
            this.ttl = ttl;
            this.color = color;
        }
        public void Update()
        {
            ttl--;
            if (ttl <= fadePoint)
            {
                color.A = (byte)((float)ttl / (float)fadePoint * 255);
                //color.A = 10;
            }
        }
        
    }
}
