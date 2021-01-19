using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Regret.Buffs
{
    public class StunnedBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stunned");
            DisplayName.AddTranslation(GameCulture.Chinese, "击晕");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            canBeCleared = false;
            longerExpertDebuff = false;
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EndBeginEnd>()))
            {
                npc.buffTime[buffIndex] += time;
                if (npc.buffTime[buffIndex] > 20 * 60)
                {
                    npc.buffTime[buffIndex] = 20 * 60;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class StunnedNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Vector2 OldVel = Vector2.Zero;
        public bool Stunned = false;
        public override bool PreAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<StunnedBuff>()))
            {
                if (!Stunned)
                {
                    OldVel = npc.velocity;
                }
                Stunned = true;
                npc.velocity = Vector2.Zero;
                return false;
            }
            else
            {
                if (Stunned)
                {
                    npc.velocity = OldVel;
                }
                Stunned = false;
            }
            return true;
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<StunnedBuff>()))
            {
                damage += defense / 4;
                if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EndBeginEnd>()))
                {
                    damage += defense / 4;
                }
            }
            return true;
        }
    }
}