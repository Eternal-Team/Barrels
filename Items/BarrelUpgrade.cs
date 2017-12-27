using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheOneLibrary.Base.Items;

namespace Barrels.Items
{
	public class BarrelUpgrade : BaseItem
	{
		public override bool CloneNewInstances => false;

		public override ModItem Clone(Item item)
		{
			BarrelUpgrade clone = (BarrelUpgrade)base.Clone(item);
			clone.data = data;
			return clone;
		}

		public TagCompound data = new TagCompound();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Upgrade");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 16;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
		}

		public override TagCompound Save() => new TagCompound
		{
			["Data"] = data
		};

		public override void Load(TagCompound tag)
		{
			data = tag.GetCompound("Data");
		}

		public override void NetSend(BinaryWriter writer) => TagIO.Write(Save(), writer);

		public override void NetRecieve(BinaryReader reader) => Load(TagIO.Read(reader));
	}
}