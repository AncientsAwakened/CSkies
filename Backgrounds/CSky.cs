using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace CSkies.Backgrounds
{
	public class CSky : CustomSky
    {

        private struct Star
		{
			public Vector2 Position;

			public float Depth;

			public int TextureIndex;

			public float SinOffset;

			public float AlphaFrequency;

			public float AlphaAmplitude;
		}

		private readonly UnifiedRandom random = new UnifiedRandom();

        public static Texture2D SkyTex;

        public static Texture2D[] starTextures;

		private bool isActive;

		private Star[] stars;

		private float fadeOpacity;

		public override void OnLoad()
        {
            SkyTex = TextureManager.Load("Backgrounds/CSky");
            starTextures = new Texture2D[2];
			for (int i = 0; i < starTextures.Length; i++)
			{
				starTextures[i] = TextureManager.Load("Backgrounds/CStar" + i);
            }
        }

		public override void Update(GameTime gameTime)
		{
			if (isActive)
			{
				fadeOpacity = Math.Min(1f, 0.01f + fadeOpacity);
				return;
			}
			fadeOpacity = Math.Max(0f, fadeOpacity - 0.01f);
		}

		public override Color OnTileColor(Color inColor)
		{
			Vector4 value = inColor.ToVector4();
			return new Color(Vector4.Lerp(value, Vector4.One, fadeOpacity * 0.5f));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
			{
                spriteBatch.Draw(SkyTex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            }
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < stars.Length; i++)
			{
				float depth = stars[i].Depth;
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
				Vector2 value4 = new Vector2(1f / stars[j].Depth, 1.1f / stars[j].Depth);
				Vector2 position = (stars[j].Position - value3) * value4 + value3 - Main.screenPosition;
				if (rectangle.Contains((int)position.X, (int)position.Y))
				{
					float num3 = (float)Math.Sin(stars[j].AlphaFrequency * Main.GlobalTime + stars[j].SinOffset) * stars[j].AlphaAmplitude + stars[j].AlphaAmplitude;
					float num4 = (float)Math.Sin(stars[j].AlphaFrequency * Main.GlobalTime * 5f + stars[j].SinOffset) * 0.1f - 0.1f;
					num3 = MathHelper.Clamp(num3, 0f, 1f);
					Texture2D texture2D = starTextures[stars[j].TextureIndex];
					spriteBatch.Draw(texture2D, position, null, Color.White * scale * num3 * 0.8f * (1f - num4) * fadeOpacity, 0f, new Vector2(texture2D.Width >> 1, texture2D.Height >> 1), (value4.X * 0.5f + 0.5f) * (num3 * 0.3f + 0.7f), SpriteEffects.None, 0f);
				}
			}
		}

		public override float GetCloudAlpha()
		{
			return (1f - fadeOpacity) * 0.3f + 0.7f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			fadeOpacity = 0.002f;
			isActive = true;
			int num = 200;
			int num2 = 10;
			stars = new Star[num * num2];
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				float num4 = i / (float)num;
				for (int j = 0; j < num2; j++)
				{
					float num5 = j / (float)num2;
					stars[num3].Position.X = num4 * Main.maxTilesX * 16f;
					stars[num3].Position.Y = num5 * ((float)Main.worldSurface * 16f + 2000f) - 1000f;
					stars[num3].Depth = random.NextFloat() * 8f + 1.5f;
					stars[num3].TextureIndex = random.Next(starTextures.Length);
					stars[num3].SinOffset = random.NextFloat() * 6.28f;
					stars[num3].AlphaAmplitude = random.NextFloat() * 5f;
					stars[num3].AlphaFrequency = random.NextFloat() + 1f;
					num3++;
				}
			}
			Array.Sort(stars, new Comparison<Star>(SortMethod));
		}

		private int SortMethod(Star meteor1, Star meteor2)
		{
			return meteor2.Depth.CompareTo(meteor1.Depth);
		}

		public override void Deactivate(params object[] args)
		{
			isActive = false;
		}

		public override void Reset()
		{
			isActive = false;
		}

		public override bool IsActive()
		{
			return isActive || fadeOpacity > 0.001f;
		}
	}

    public class CSkyData : ScreenShaderData
    {
        public CSkyData(string passName) : base(passName)
        {

        }

        private void UpdateCSky()
        {
            CPlayer modPlayer = Main.LocalPlayer.GetModPlayer<CPlayer>();
            if (modPlayer.ZoneCSky)
            {
                return;
            }
        }

        public override void Apply()
        {
            UpdateCSky();
            base.Apply();
        }
    }
}
