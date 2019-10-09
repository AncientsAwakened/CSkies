using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace CSkies.Backgrounds
{
    public class AbyssSky : CustomSky
    {
        private readonly UnifiedRandom random = new UnifiedRandom();

        private struct Bolt
        {
            public Vector2 Position;

            public float Depth;

            public int Life;

            public bool IsAlive;
        }

        public static Texture2D boltTexture;
        public static Texture2D flashTexture;

        private Bolt[] bolts;
        public bool Active;
        public int ticksUntilNextBolt;
        public float Intensity;

        public override void OnLoad()
        {
            boltTexture = TextureManager.Load("Backgrounds/VoidBolt");
            flashTexture = TextureManager.Load("Backgrounds/VoidFlash");
        }

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

            if (NPC.AnyNPCs(CSkies.inst.NPCType<NPCs.Bosses.Void.Void>()))
            {
                if (ticksUntilNextBolt <= 0)
                {
                    ticksUntilNextBolt = random.Next(5, 20);
                    int num = 0;
                    while (bolts[num].IsAlive && num != bolts.Length - 1)
                    {
                        num++;
                    }
                    bolts[num].IsAlive = true;
                    bolts[num].Position.X = random.NextFloat() * (Main.maxTilesX * 16f + 4000f) - 2000f;
                    bolts[num].Position.Y = random.NextFloat() * 500f;
                    bolts[num].Depth = random.NextFloat() * 8f + 2f;
                    bolts[num].Life = 30;
                }
                ticksUntilNextBolt--;
                for (int i = 0; i < bolts.Length; i++)
                {
                    if (bolts[i].IsAlive)
                    {
                        Bolt[] expr168cp0 = bolts;
                        int expr168cp1 = i;
                        expr168cp0[expr168cp1].Life = expr168cp0[expr168cp1].Life - 1;
                        if (bolts[i].Life <= 0)
                        {
                            bolts[i].IsAlive = false;
                        }
                    }
                }
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            Vector4 value = inColor.ToVector4();
            return new Color(Vector4.Lerp(value, Vector4.One, Intensity * 0.5f));
        }

        

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * Intensity);
            }
            if (NPC.AnyNPCs(CSkies.inst.NPCType<NPCs.Bosses.Void.Void>()))
            {
                float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
                Vector2 value3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
                Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
                for (int i = 0; i < bolts.Length; i++)
                {
                    if (bolts[i].IsAlive && bolts[i].Depth > minDepth && bolts[i].Depth < maxDepth)
                    {
                        Vector2 value4 = new Vector2(1f / bolts[i].Depth, 0.9f / bolts[i].Depth);
                        Vector2 position = (bolts[i].Position - value3) * value4 + value3 - Main.screenPosition;
                        if (rectangle.Contains((int)position.X, (int)position.Y))
                        {
                            Texture2D texture = boltTexture;
                            int life = bolts[i].Life;
                            if (life > 26 && life % 2 == 0)
                            {
                                texture = flashTexture;
                            }
                            float scale2 = life / 30f;
                            spriteBatch.Draw(texture, position, null, Color.White * scale * scale2 * Intensity, 0f, Vector2.Zero, value4.X * 5f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f - Intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            Intensity = 0.002f;
            Active = true;
            bolts = new Bolt[500];
            for (int i = 0; i < bolts.Length; i++)
            {
                bolts[i].IsAlive = false;
            }
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

    public class AbyssSkyData : ScreenShaderData
    {
        public AbyssSkyData(string passName) : base(passName)
        {
        }

        private void UpdateVoidSky()
        {
            CPlayer modPlayer = Main.LocalPlayer.GetModPlayer<CPlayer>();
            if (modPlayer.NearVoid())
            {
                return;
            }
        }

        public override void Apply()
        {
            UpdateVoidSky();
            base.Apply();
        }
    }
}