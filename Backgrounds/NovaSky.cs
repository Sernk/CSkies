﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CSkies.Backgrounds
{
    public class NovaSky : CustomSky
    {
        public bool Active;
        public float Intensity; 
        
        private struct Star
        {
            public Vector2 Position;

            public float Depth;

            public int TextureIndex;

            public float SinOffset;

            public float AlphaFrequency;

            public float AlphaAmplitude;
        }

        private readonly UnifiedRandom _random = new UnifiedRandom();

        private bool _isActive;

        private Star[] _stars;

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Intensity = Math.Min(1f, 0.01f + Intensity);
            }
            else
            {
                Intensity = Math.Max(0f, Intensity - 0.01f);
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            Vector4 value = inColor.ToVector4();
            return new Color(Vector4.Lerp(value, Vector4.One, Intensity * 0.5f));
        }

        readonly CSkies mod = CSkies.inst;
        float Rotation = 0;
        readonly Texture2D[] StarTex = new Texture2D[2];

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D Vortex = ModContent.Request<Texture2D>("CSkies/Backgrounds/NovaVortex").Value;
            Texture2D SkyTex = ModContent.Request<Texture2D>("CSkies/Backgrounds/SkyTex").Value;
            for (int i = 0; i < StarTex.Length; i++)
            {
                StarTex[i] = ModContent.Request<Texture2D>("CSkies/Backgrounds/NovaStar" + i).Value;
            }

            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {

                var planetPos = new Vector2(Main.screenWidth / 2, Main.screenHeight/ 3);
                Rotation -= .0008f;

                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * Intensity);
                spriteBatch.Draw(SkyTex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Violet);
                spriteBatch.Draw(Vortex, planetPos, null, Color.White * 0.9f * Intensity, Rotation, new Vector2(Vortex.Width >> 1, Vortex.Height >> 1), 1f, SpriteEffects.None, 1f);
            }

            int num = -1;
            int num2 = 0;
            for (int i = 0; i < _stars.Length; i++)
            {
                float depth = _stars[i].Depth;
                if (num == -1 && depth < maxDepth)
                {
                    num = i;
                }
                if (depth <= minDepth)
                {
                    break;
                }
                num2 = i;
            }
            if (num == -1)
            {
                return;
            }
            float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
            Vector2 value3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);

            for (int j = num; j < num2; j++)
            {
                Vector2 value4 = new Vector2(1f / _stars[j].Depth, 1.1f / _stars[j].Depth);
                Vector2 position = (_stars[j].Position - value3) * value4 + value3 - Main.screenPosition;
                if (rectangle.Contains((int)position.X, (int)position.Y))
                {
                    float value5 = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly + _stars[j].SinOffset) * _stars[j].AlphaAmplitude + _stars[j].AlphaAmplitude;
                    float num3 = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly * 5f + _stars[j].SinOffset) * 0.1f - 0.1f;
                    value5 = MathHelper.Clamp(value5, 0f, 1f);
                    Texture2D value6 = StarTex[_stars[j].TextureIndex];
                    spriteBatch.Draw(value6, position, null, Color.White * scale * value5 * 0.8f * (1f - num3) * Intensity, 0f, new Vector2(value6.Width >> 1, value6.Height >> 1), (value4.X * 0.5f + 0.5f) * (value5 * 0.3f + 0.7f), SpriteEffects.None, 0f);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f - Intensity;
        }

        public Color GetAlpha(Color newColor, float alph)
        {
            int alpha = 255 - (int)(255 * alph);
            float alphaDiff = (255 - alpha) / 255f;
            int newR = (int)(newColor.R * alphaDiff);
            int newG = (int)(newColor.G * alphaDiff);
            int newB = (int)(newColor.B * alphaDiff);
            int newA = newColor.A - alpha;
            if (newA < 0) newA = 0;
            if (newA > 255) newA = 255;
            return new Color(newR, newG, newB, newA);
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            Intensity = 0.002f;
            Active = true; 
            int num = 200;
            int num2 = 10;
            _stars = new Star[num * num2];
            int num3 = 0;
            for (int i = 0; i < num; i++)
            {
                float num4 = i / (float)num;
                for (int j = 0; j < num2; j++)
                {
                    float num5 = j / (float)num2;
                    _stars[num3].Position.X = num4 * Main.maxTilesX * 16f;
                    _stars[num3].Position.Y = num5 * ((float)Main.worldSurface * 16f + 2000f) - 1000f;
                    _stars[num3].Depth = _random.NextFloat() * 8f + 1.5f;
                    _stars[num3].TextureIndex = _random.Next(StarTex.Length);
                    _stars[num3].SinOffset = _random.NextFloat() * 6.28f;
                    _stars[num3].AlphaAmplitude = _random.NextFloat() * 5f;
                    _stars[num3].AlphaFrequency = _random.NextFloat() + 1f;
                    num3++;
                }
            }
            Array.Sort(_stars, new Comparison<Star>(this.SortMethod));
        }

        private int SortMethod(Star meteor1, Star meteor2)
        {
            return meteor2.Depth.CompareTo(meteor1.Depth);
        }

        public override void Deactivate(params object[] args)
        {
            Active = false;
        }

        public override void Reset()
        {
            Active = false;
        }

        public override bool IsActive()
        {
            return Active || Intensity > 0.001f;
        }
    }

    public class NovaSkyData : ScreenShaderData
    {
        private int NovaIndex;

        public NovaSkyData(string passName) : base(passName)
        {
        }

        private void UpdateNovaIndex()
        {
            int NovaType = CSkies.inst.Find<ModNPC>("Novacore").Type;
            if (NovaIndex >= 0 && Main.npc[NovaIndex].active && Main.npc[NovaIndex].type == NovaType)
            {
                return;
            }
            NovaIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NovaType)
                {
                    NovaIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdateNovaIndex();
            if (NovaIndex != -1)
            {
                UseTargetPosition(Main.npc[NovaIndex].Center);
            }
            base.Apply();
        }
    }
}