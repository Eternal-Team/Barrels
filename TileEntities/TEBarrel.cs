using Barrels.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheOneLibrary.Base;
using TheOneLibrary.Storage;
using TheOneLibrary.Utility;
using Barrel = Barrels.Tiles.Barrel;

namespace Barrels.TileEntities
{
	public class TEBarrel : BaseTE, IContainer
	{
		public const int BaseMax = 2048;
		public int maxStoredItems = BaseMax;

		public IList<Item> Items = new List<Item>();

		public int StackInc
		{
			get { return Items.Where((x, i) => i > 0 && i < Items.Count).Sum(x => (x.modItem as StackUpgrade)?.data.GetInt("StackIncrease") ?? 0); }
		}

		public override bool ValidTile(Tile tile) => tile.type == mod.TileType<Barrel>() && tile.TopLeft();

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient) Place(i, j - 1);

			NetMessage.SendTileSquare(Main.myPlayer, i, j - 1, 2);
			NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j - 1, Type);
			return -1;
		}

		public override void OnPlace()
		{
			for (int i = 0; i < 6; i++) Items.Add(new Item());
		}

		public override void OnNetPlace() => OnPlace();

		public override void OnKill()
		{
			this.DropItems(new Rectangle(Position.X * 16, Position.Y * 16, 32, 32));
		}

		public override void Update()
		{
			maxStoredItems = BaseMax + BaseMax * StackInc;

			this.HandleUIFar();
		}

		public override TagCompound Save() => new TagCompound
		{
			["Items"] = Items.Save()
		};

		public override void Load(TagCompound tag)
		{
			Items = Utility.Load(tag);
		}

		public override void NetSend(BinaryWriter writer, bool lightSend) => TagIO.Write(Save(), writer);

		public override void NetReceive(BinaryReader reader, bool lightReceive) => Load(TagIO.Read(reader));

		public IList<Item> GetItems() => Items.Where((x, i) => i == 0).ToList();

		public ModTileEntity GetTileEntity() => this;
	}
}