using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleGame
{
    public abstract class Particle
    {
        #region Fields & Accessors
        protected Random random = new Random();

        protected Texture2D texture;
        public Texture2D Texture 
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
        protected Vector2 position;
        public Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        protected Vector2 velocity;
        public Vector2 Velocity 
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }
        protected float angle;
        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }
        protected float angularVelocity;
        public float AngularVelocity
        {
            get
            {
                return angularVelocity;
            }
            set
            {
                angularVelocity = value;
            }
        }
        protected Color color;
        public Color Color 
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        protected float size;
        public float Size 
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }
        protected int ttl;
        public int TTL 
        {
            get
            {
                return ttl;
            }
            set
            {
                ttl = value;
            }
        }
        protected int fadePoint;
        public int FadePoint
        {
            get
            {
                return fadePoint;
            }
            set
            {
                fadePoint = value;
            }
        }
        public bool Old
        {
            get
            {
                return (TTL == FadePoint);
            }
        } 
        #endregion

        public abstract void Update();

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
        }
        protected float NextFloat
        {
            get
            {
                return (float)random.NextDouble();
            }
        }
    }
}
