using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleGame
{
    public class ParticleSmoke : Particle
    {
        public float ResizeSpeed { get; set; }
        public float Transparency { get; set; }
        private float nextHorVelocity;
        private int timeToNextVelocity;
        private int stepsToNextVelocity;

        public ParticleSmoke(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            FadePoint = 40;
            ResizeSpeed = (float)random.NextDouble()/20f;
            stepsToNextVelocity = random.Next(50);
            timeToNextVelocity = stepsToNextVelocity;
            nextHorVelocity = (NextFloat * 2 - 1) / 9;
        }
        public override void Update()
        {
            TTL--;

            if (Velocity.Y > -1.5f)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y - 0.1f);
            }
            else if(Velocity.Y < -1.5f)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y+0.1f);
            }
            stepsToNextVelocity--;
            if (stepsToNextVelocity == 0)
            {
                stepsToNextVelocity = random.Next(70);
                timeToNextVelocity = stepsToNextVelocity;
                float newVel = (NextFloat * 2 - 1) / 9;
                nextHorVelocity = newVel;
            }
            else
            {
                velocity.X += nextHorVelocity / (float)stepsToNextVelocity;
            }

            Position += Velocity;
            Angle += AngularVelocity;
            Size += ResizeSpeed;
            if (TTL < FadePoint)
            {
                //Color = new Color(1, 0, 0);
                //Transparency = (float)TTL / FadePoint;

            }
            else
            {
                //Transparency =0.6f;
            }
        }
    }
}
