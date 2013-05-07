using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleGame
{
    public class ParticleExplosion : Particle
    {
        public ParticleExplosion(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            FadePoint = 2;
        }
        public override void Update()
        {
            TTL--;
            Velocity = new Vector2(Velocity.X *0.9f, Velocity.Y*0.9f);
            Position += Velocity;

            Angle += AngularVelocity;

            Color = new Color(Color.R, Color.G - 5, Color.B);
            if (TTL < FadePoint)
            {

                Size = (float)TTL / (float)FadePoint / 2;

            }
        }
    }
}
