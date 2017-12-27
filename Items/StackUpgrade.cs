using Barrels.TileEntities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Barrels.Items
{
	public class StackUpgrade : BarrelUpgrade
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stack Upgrade");
			Tooltip.SetDefault("TOOLTIP");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 16;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int stackIncrease = data.GetInt("StackIncrease");
			TooltipLine line = tooltips.FirstOrDefault(x => x.mod == "Terraria" && x.Name == "ItemName");
			if (line != null) line.text += $" (x{stackIncrease})";
			line = tooltips.FirstOrDefault(x => x.mod == "Terraria" && x.Name == "Tooltip0");
			if (line != null) line.text = $"Increases barrel's storage {stackIncrease} times ({(TEBarrel.BaseMax * stackIncrease).ToString("N0", CultureInfo.InvariantCulture)} items)";
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnCraft(Recipe recipe)
		{
			((BarrelUpgrade)recipe.createItem.modItem).data = new TagCompound();
			((BarrelUpgrade)recipe.createItem.modItem).data.Set("StackIncrease", 7);
		}
	}
}