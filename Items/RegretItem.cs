using Microsoft.Xna.Framework;
using Regret.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Regret.Items
{
	public class RegretItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Regret");
			DisplayName.AddTranslation(GameCulture.Chinese, "悔恨");
			Tooltip.SetDefault("\"Bearing unlimited possibility to change humanity's future, confidential research began in the underground.\n" +
			"The researchers laid down all sense of dignity, but they did not feel guilty as they had the greater good to pursue.\n" +
			"Even the merciful Carmen condoned the project.\n" +
			"Enemies crushed by this regret can never return to their normal life.\n" +
			"Before swinging this weapon, expressing one’s condolences for the demise of the inmate who couldn't even have a funeral would be nice.\"\n" +
            "[c/FFFF00:HE] E.G.O. Weapon\n" +
			"Swing a huge flail to hit the enemies and stun them temporarily\n" +
			"When receiving non-persistent debuffs, you will grant horror debuff, during which the damage is temporarily reduced.\n" +
			"When receiving horror debuff for 30 seconds, you will be temporarily immune to most non-persistent debuffs\n" +
			"At the same time, the attack damage and effect of Regret will be greatly increased, lasting three attacks\n" +
			"All effects will disappear after switching weapons");

		   Tooltip.AddTranslation(GameCulture.Chinese,
			"秘密研究是从一个地下室开始的，而那项研究有着改变人类未来的无限可能。\n" +
			"为了一个更远大的目标，他们放下了道德和尊严。\n" +
			"虽不人道，可没人会感到后悔...除了他们。\n" +
			"看上去连仁慈的Carmen都默许了这一切。\n" +
			"他们再也不能回到正常的生活当中。\n" +
			"对一个连葬礼都没有的人而言，唯有深深的悔恨才是最后的悼唁。\n" +
			"[c/FFFF00:HE]级E.G.O.武器\n" +
			"挥动巨大的链球重击敌人，并暂时的砸晕它们\n" +
			"受到非持续性负面效果时会获得恐惧debuff，伤害暂时降低\n" +
			"当累计受到30秒恐惧debuff时，会暂时免疫大多数非持续性debuff\n" +
			"与此同时，悔恨的攻击伤害和效果会得到大幅提升，持续三次攻击\n" +
			"所有效果会在切换武器时消失");
		}

		public override void SetDefaults() 
		{
			item.damage = 2000;
			item.melee = true;
			item.width = 174;
			item.height = 113;
			item.scale = 0.5f;
			item.useTime = 20;
			item.crit = 22;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 6;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(1, 50, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.channel = true;
			item.useTurn = true;
		}
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<RegretPlayer>().RegretCD > 0)
            {
				item.channel = false;
				return false;
            }
            else
            {
				item.channel = true;
            }
			return true;
        }
        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 99);
			recipe.AddIngredient(ItemID.Chain, 10);
			recipe.AddIngredient(ItemID.Ectoplasm, 20);
			recipe.AddIngredient(ItemID.BeetleHusk, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.needLava = true;
			recipe.SetResult(this);
			recipe.AddRecipe();


			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 99);
			recipe.AddIngredient(ItemID.Chain, 10);
			recipe.AddIngredient(ItemID.Ectoplasm, 20);
			recipe.AddIngredient(ItemID.BeetleHusk, 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.needLava = true;
			recipe.SetResult(this);
			recipe.AddRecipe();
		}


    }
}