using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Regret.Items;

namespace Regret.Projectiles
{
	public class RegretHit : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Regret");
			DisplayName.AddTranslation(GameCulture.Chinese, "悔恨");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.timeLeft = 51;
		}

		public override void AI()
		{
			if (projectile.ai[0] >= 0)
			{
				Regret.BrokenState = (int)projectile.ai[0];
				Regret.BrokenPos = projectile.Center;
                switch (projectile.ai[0])
                {
					case 0:
						Main.LocalPlayer.GetModPlayer<ShakeScreenPlayer>().shakeQuake = true;
						break;
					case 1:
						Main.LocalPlayer.GetModPlayer<ShakeScreenPlayer>().shakeSubtle = true;
						break;
					case 2:
						Main.LocalPlayer.GetModPlayer<ShakeScreenPlayer>().shake = true;
						break;
					case 3:
						Main.LocalPlayer.GetModPlayer<ShakeScreenPlayer>().shakeMega = true;
						break;
					default:
						break;
                }
			}

			projectile.rotation = projectile.velocity.ToRotation();
			projectile.ai[1]++;
            if (projectile.ai[1] <= 5)
            {
				projectile.Opacity = projectile.ai[1] / 5;
            }
            if (projectile.ai[1] > 5)
            {
				projectile.Opacity = (50 - projectile.ai[1]) / 45;
                if (projectile.ai[1] >= 50)
                {
					projectile.Kill();
                }
            }
		}

		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			Texture2D tex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.15f, SpriteEffects.None, 0);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
			return false;
		}

        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
