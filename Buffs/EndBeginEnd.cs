using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Regret.Buffs
{
    public class EndBeginEnd : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ends, Begins, Ends");
            DisplayName.AddTranslation(GameCulture.Chinese, "结束，开始，结束");
            Description.SetDefault("You are immune to most debuffs\nAttack effect of Regret is greatly enhanced");
            Description.AddTranslation(GameCulture.Chinese, "你将免疫大多数负面效果\n悔恨的攻击效果会大幅增强");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
            longerExpertDebuff = false;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Gray;
        }
    }

    
}