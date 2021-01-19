using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Regret.Items;
using Regret.Buffs;

namespace Regret.Projectiles
{
	public class RegretProj : ModProjectile          ///222 230 =230/4=60
	{
		public const int MaxDist = 700;
		public const int NormalDist = 100;
		int Dir = 0;
		float DistSet = 0;
		float RotSet = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Regret");
			DisplayName.AddTranslation(GameCulture.Chinese, "悔恨");
		}

		public override void SetDefaults()
		{
			projectile.width = 60;
			projectile.height = 60;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.knockBack = 6f;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 50;
		}

        public override void AI()
        {
			projectile.timeLeft = 9999;
			Player owner = Main.player[projectile.owner];
			//SolemnLamentPlayer modowner = owner.GetModPlayer<SolemnLamentPlayer>();
			if (!owner.active || owner.dead || owner.ghost)
			{
				projectile.Kill();
				return;
			}
			if (owner.HeldItem.type != ModContent.ItemType<RegretItem>())
			{
				projectile.Kill();
				return;
			}
			owner.heldProj = projectile.whoAmI;
			int dir = owner.direction;
			projectile.rotation = (projectile.Center - owner.Center).ToRotation();
			owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);

			//Main.NewText(projectile.ai[0]);
			if (projectile.ai[0] == 0)
			{
				if (projectile.Distance(owner.Center) > (NormalDist + 50) && !Collision.CanHit(owner.Center, 1, 1, projectile.Center, 1, 1))
				{
					projectile.tileCollide = false;
				}
				else
				{
					projectile.tileCollide = true;
				}
				projectile.velocity.Y += 1f;
				if (projectile.Distance(owner.Center) >= NormalDist)
				{
					projectile.velocity += Vector2.Normalize(owner.Center - projectile.Center) * 1;
					projectile.Center = owner.Center + Vector2.Normalize(projectile.Center - owner.Center) * (NormalDist - 1);
					projectile.velocity *= 0.9f;
				}
                if (owner.channel)
                {
					//projectile.velocity = ((Main.MouseWorld - projectile.Center).ToRotation() + MathHelper.Pi / 3).ToRotationVector2() * 30f;
					projectile.ai[0] = 1;
					Dir = Math.Sign(Main.MouseWorld.X - owner.Center.X + 0.01f);
					DistSet = MathHelper.Clamp(owner.Distance(Main.MouseWorld), 0, MaxDist);
					RotSet = (Main.MouseWorld - owner.Center).ToRotation();
				}
			}
            if (projectile.ai[0] == 1)
            {
				projectile.velocity = Vector2.Zero;
				if (projectile.Distance(owner.Center) <= 10)
				{
					projectile.ai[0] = 2;
				}
				projectile.position += Vector2.Normalize(owner.Center - projectile.Center) * 10 + owner.velocity;
				projectile.tileCollide = false;

			}
            if (projectile.ai[0] == 2)
			{
				projectile.tileCollide = false;
				projectile.ai[1]++;
				float k = projectile.ai[1] / 20;
				float rotation = RotSet - MathHelper.Pi / 3 * 2 * Dir;
				rotation += MathHelper.Pi / 3 * 4 * Dir * k;
				float d = DistSet * (float)Math.Pow(Math.Sin(k * MathHelper.Pi), 4);
				projectile.Center = owner.Center + rotation.ToRotationVector2() * d;
				owner.itemTime = 20;
				owner.itemAnimation = 20;
				if (projectile.ai[1] >= 20)
                {
					projectile.ai[1] = 0;
					projectile.ai[0] = 0;
					owner.channel = false;
					owner.GetModPlayer<RegretPlayer>().RegretCD = 50;
					projectile.velocity = (MathHelper.TwoPi * Main.rand.NextFloat()).ToRotationVector2() * 10;
				}
			}
            if (projectile.ai[0] == 3)
            {
				projectile.velocity = Vector2.Zero;
				owner.itemTime = 20;
				owner.itemAnimation = 20;
				projectile.tileCollide = false;
				if (projectile.Distance(owner.Center) <= 50)
				{
					projectile.ai[0] = 0;
					projectile.ai[1] = 0;
					owner.channel = false;
					owner.GetModPlayer<RegretPlayer>().RegretCD = 50;
					projectile.velocity = (MathHelper.TwoPi * Main.rand.NextFloat()).ToRotationVector2() * 25;
				}
				projectile.position += Vector2.Normalize(owner.Center - projectile.Center) * 25 + owner.velocity;
			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.Y == 0) 
			{
				projectile.velocity.X *= 0.95f;
			}
			return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D tex2 = mod.GetTexture("Projectiles/Chain");
			Player owner = Main.player[projectile.owner];
			Vector2 Center = owner.mount.Active ? owner.MountedCenter : owner.Center;
			Vector2 Unit = Vector2.Normalize(projectile.Center - Center) * tex2.Width;
			int dist = (int)projectile.Distance(Center);
			if (dist > tex2.Width / 2) 
            {
				for (int i = 0; i * tex2.Width <= dist; i++) 
                {
					spriteBatch.Draw(tex2, Center + Unit * (i + 0.5f) - Main.screenPosition, null, Color.White, Unit.ToRotation(), tex2.Size() / 2, projectile.scale, SpriteEffects.None, 0);
                }
            }
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, tex.Size() / 2, projectile.scale * 0.25f, SpriteEffects.None, 0);
			
			return false;
		}

        public override bool CanDamage()
        {
            if (projectile.ai[0] == 2)
            {
				return true;
            }
			return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<EndBeginEnd>()))
            {
                switch (Main.player[projectile.owner].GetModPlayer<RegretPlayer>().AttackProgress)
                {
					case 1:
						damage *= 2;
						break;
					case 2:
						damage *= 3;
						break;
					case 3:
						damage *= 5;
						break;
					default:
						break;
                }
				crit = true;
            }
			else if (RegretPlayer.HasDebuff(projectile.owner))
			{
				damage = (int)(damage * 0.66f);
			}

		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.ai[0] == 2)
			{
				Player owner = Main.player[projectile.owner];
				int time = 20;
				if (RegretPlayer.HasDebuff(projectile.owner))
				{
					time = 14;
				}
				if (owner.HasBuff(ModContent.BuffType<EndBeginEnd>()))
				{
                    switch (owner.GetModPlayer<RegretPlayer>().AttackProgress)
                    {
						case -1:
						case 0:
							break;
						case 1:
							time = 60;
							break;
						case 2:
							time = 120;
							break;
						case 3:
							time = 360;
							break;
						default:
							break;
                    }
                }
				target.buffImmune[ModContent.BuffType<StunnedBuff>()] = false;
				target.AddBuff(ModContent.BuffType<StunnedBuff>(), time);
				if (target.realLife >= 0)
				{
					if (Main.npc[target.realLife].active)
					{
						Main.npc[target.realLife].buffImmune[ModContent.BuffType<StunnedBuff>()] = false;
						Main.npc[target.realLife].AddBuff(ModContent.BuffType<StunnedBuff>(), time);
						
						foreach(NPC npc in Main.npc)
                        {
                            if (npc.active)
                            {
								if (npc.active && npc.realLife == target.realLife)
                                {
									if (npc.whoAmI != target.whoAmI && npc.whoAmI != target.realLife)
                                    {
										npc.buffImmune[ModContent.BuffType<StunnedBuff>()] = false;
										npc.AddBuff(ModContent.BuffType<StunnedBuff>(), time);
									}
                                }
                            }
                        }
						
					}
				}

				Vector2 dir = Vector2.Normalize(projectile.Center - owner.Center);
				Projectile.NewProjectile(projectile.Center - dir * 10, dir, ModContent.ProjectileType<RegretHit>(), projectile.damage, projectile.knockBack, owner.whoAmI, owner.GetModPlayer<RegretPlayer>().AttackProgress);
				projectile.ai[0] = 3;

				if (owner.HasBuff(ModContent.BuffType<EndBeginEnd>()))
				{
					owner.GetModPlayer<RegretPlayer>().AttackProgress++;
					if (owner.GetModPlayer<RegretPlayer>().AttackProgress > 3)
					{
						owner.ClearBuff(ModContent.BuffType<EndBeginEnd>());
					}
				}
				if (owner.GetModPlayer<RegretPlayer>().AttackProgress == 3)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/StrongAtk2"), owner.Center);
				}
				if (owner.GetModPlayer<RegretPlayer>().AttackProgress == -1)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/NormalAtk"), owner.Center);
				}
				else
                {
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/StrongAtk1"), owner.Center);
				}
			}
		}
    }
}
