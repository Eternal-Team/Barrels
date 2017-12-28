using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Barrels.TileEntities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Barrels.Items
{
	public class StackUpgrade : BaseUpgrade
	{
		public override bool CloneNewInstances => false;

		public override ModItem Clone(Item item)
		{
			StackUpgrade clone = (StackUpgrade)base.Clone(item);
			clone.stackInc = stackInc;
			return clone;
		}

		public int stackInc = 1;

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
			TooltipLine line = tooltips.FirstOrDefault(x => x.mod == "Terraria" && x.Name == "ItemName");
			if (line != null) line.text += $" (x{stackInc})";
			line = tooltips.FirstOrDefault(x => x.mod == "Terraria" && x.Name == "Tooltip0");
			if (line != null) line.text = $"Increases barrel's storage {stackInc} times ({(TEBarrel.BaseMax * stackInc).ToString("N0", CultureInfo.InvariantCulture)} items)";
		}

		public override TagCompound Save() => new TagCompound
		{
			["StackInc"] = stackInc
		};

		public override void Load(TagCompound tag)
		{
			stackInc = tag.GetInt("StackInc");
		}

		public override void NetSend(BinaryWriter writer) => TagIO.Write(Save(), writer);

		public override void NetRecieve(BinaryReader reader) => Load(TagIO.Read(reader));

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnCraft(Recipe recipe)
		{
			stackInc = 7;
		}
	}
}