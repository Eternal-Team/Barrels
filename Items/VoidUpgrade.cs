using Terraria;

namespace Barrels.Items
{
	public class VoidUpgrade : BarrelUpgrade
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Upgrade");
			Tooltip.SetDefault("Voids access items");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 16;
			item.maxStack = 999;
			item.value = Item.sellPrice(0, 0, 10);
		}
	}
}