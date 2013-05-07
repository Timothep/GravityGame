using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleGame
{
    public class ParticleFire : Particle
    {
        public ParticleFire(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            FadePoint = 20;
        }
        public override void Update()
        {
            ttl--;
            if (Velocity.Y > -5f) Velocity = new Vector2(Velocity.X, Velocity.Y - 0.09f);
            Velocity = new Vector2(Velocity.X * 0.98f, Velocity.Y);
            Position += Velocity;
            Angle += AngularVelocity;

            Color = new Color(Color.R, Color.G - 5, Color.B);
            if (TTL < FadePoint)
            {
                Size = TTL / FadePoint;
            }
        }
    }
}
