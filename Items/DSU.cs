using System.Globalization;
using Terraria;
using TheOneLibrary.Base.Items;

namespace Barrels.Items
{
	public class DSU : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deep Storage Unit");
			Tooltip.SetDefault($"Stores {int.MaxValue.ToString("N0", CultureInfo.InvariantCulture)} items");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType<Tiles.DSU>();
			item.value = Item.sellPrice(0, 0, 5);
		}
	}
}