using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleGame
{
    abstract class ParticleEmitter
    {
        protected static Random random;
        public Vector2 EmitterLocation { get; set; }
        protected List<Particle> particles;
        protected List<Texture2D> textures;

        protected const float PI = (float)Math.PI;

        public int AmountOfParticles
        {
            get
            {
                return particles.Count;
            }
        }

        protected static float NextFloat
        {
            get
            {
                return (float)random.NextDouble();
            }
        }

        public ParticleEmitter(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }
        protected abstract Particle GenerateNewParticle();

        public abstract void Update();

        public void Emit(int total)
        {

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(/*SpriteSortMode.Immediate, BlendState.Additive*/);
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
