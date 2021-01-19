using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Regret.Buffs;
using Regret.Items;
using Regret.Projectiles;
using Regret.Sky;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Regret
{
    public class Regret : Mod
    {
        public static Regret Instance;
        public static int BrokenState = -1;
        public static Vector2 BrokenPos = Vector2.Zero;
        public override void Load()
        {
            Instance = this;
            /*
            GameShaders.Misc["Regret:BrokenEffect"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/BrokenEffect")), "BrokenEffect").UseImage("Images/Broken");
            */
            Filters.Scene["Regret:RegretSky"] = new Filter(new RegretSkyScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.9f, 0.9f).UseOpacity(0.2f), EffectPriority.VeryHigh);
            SkyManager.Instance["Regret:RegretSky"] = new RegretSky();

            Filters.Scene["Regret:BrokenEffect"] = new Filter(
    new BrokenShaderData(new Ref<Effect>(GetEffect("Effects/BrokenEffect")), "BrokenEffect"), EffectPriority.Medium);
            Filters.Scene["Regret:BrokenEffect"].Load();

        }

        public override void Unload()
        {
            SkyManager.Instance["Regret:RegretSky"].Deactivate();
            Filters.Scene["Regret:BrokenEffect"].Deactivate();
            Instance = null;
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EndBeginEnd>()))
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Angela");
                priority = MusicPriority.BossHigh;
            }
        }

        public override void PreUpdateEntities()
        {
            //BrokenState = 3;
            if (BrokenState >= 0)
            {
                if (!Filters.Scene["Regret:BrokenEffect"].IsActive())
                {
                    Vector2 Pos = BrokenPos - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
                    Pos.X /= Main.screenWidth;
                    Pos.Y /= Main.screenHeight;
                    Filters.Scene.Activate("Regret:BrokenEffect");
                }
            }
            else
            {
                if (Filters.Scene["Regret:BrokenEffect"].IsActive())
                {
                    Filters.Scene.Deactivate("Regret:BrokenEffect");
                }
            }


        }

        public override void MidUpdateGoreProjectile()
        {
            BrokenState = -1;
        }
    }


    public class RegretPlayer : ModPlayer
    {
        public int RegretCD = 80;
        public int DebuffTime = 0;
        public int AttackProgress = -1;

        public override void UpdateBiomeVisuals()
        {
            player.ManageSpecialBiomeVisuals("Regret:RegretSky", player.HasBuff(ModContent.BuffType<EndBeginEnd>()), default);
        }

        public override void PostUpdateMiscEffects()
        {
            //Main.NewText("DebuffTime: " + DebuffTime+ " AttackProgress: " + AttackProgress);
            if (player.HeldItem.type == ModContent.ItemType<RegretItem>())
            {
                if (HasDebuff(player.whoAmI))
                {
                    player.AddBuff(ModContent.BuffType<HorrorBuff>(), 2);
                    DebuffTime++;
                    if (DebuffTime > 30 * 60)
                    {
                        DebuffTime = 0;
                        player.AddBuff(ModContent.BuffType<EndBeginEnd>(), 999999);
                        AttackProgress = 1;
                    }
                }
            }
            else
            {
                DebuffTime = 0;
                player.ClearBuff(ModContent.BuffType<EndBeginEnd>());
            }
            if (player.HasBuff(ModContent.BuffType<EndBeginEnd>()))
            {
                for (int i = 0; i < player.buffImmune.Length; i++)
                {
                    if (Main.debuff[i] && !Main.buffNoTimeDisplay[i] &&
                        i != ModContent.BuffType<EndBeginEnd>() &&
                        i != BuffID.PotionSickness &&
                        i != BuffID.ManaSickness)
                    {
                        player.buffImmune[i] = true;
                    }
                }
                DebuffTime = 0;
            }
            else
            {
                AttackProgress = -1;
            }

            if (player.HeldItem.type == ModContent.ItemType<RegretItem>())
            {
                if (RegretCD > 0)
                {
                    RegretCD--;
                }
                if (!AnyRegret(player.whoAmI))
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<RegretProj>(), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem, 6f), player.whoAmI);
                }
            }
        }

        public static bool AnyRegret(int plr)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<RegretProj>() && proj.owner == plr)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasDebuff(int plr)
        {
            Player player = Main.player[plr];
            for (int i = 0; i < player.buffType.Length; i++)
            {
                if (player.buffTime[i] > 0 &&
                    Main.debuff[player.buffType[i]] &&
                    !Main.buffNoTimeDisplay[player.buffType[i]])
                {
                    if (player.buffType[i] != BuffID.MonsterBanner &&
                        player.buffType[i] != BuffID.Tipsy &&
                        player.buffType[i] != BuffID.Wet &&
                        player.buffType[i] != BuffID.DryadsWardDebuff &&
                        player.buffType[i] != BuffID.DryadsWard &&
                        player.buffType[i] != BuffID.Stinky &&
                        player.buffType[i] != ModContent.BuffType<EndBeginEnd>() &&
                        player.buffTime[i] != ModContent.BuffType<HorrorBuff>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}