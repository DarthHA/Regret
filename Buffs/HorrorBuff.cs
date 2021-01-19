using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Regret.Buffs
{
    public class HorrorBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Horror");
            DisplayName.AddTranslation(GameCulture.Chinese, "恐惧");
            Description.SetDefault("Damage and effects of Regret have decreased temporarily.");
            Description.AddTranslation(GameCulture.Chinese, "悔恨的伤害和效果暂时降低了。");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
            longerExpertDebuff = false;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (Main.LocalPlayer.GetModPlayer<RegretPlayer>().DebuffTime > 15 * 60)
            {
                rare = ItemRarityID.Gray;
            }
            else
            {
                rare = ItemRarityID.White;
            }
        }
    }

}