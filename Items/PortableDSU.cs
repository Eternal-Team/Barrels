using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Barrels.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TheOneLibrary.Base.Items;
using TheOneLibrary.Base.UI;
using TheOneLibrary.Storage;
using TheOneLibrary.Utility;

namespace Barrels.Items
{
	public class PortableDSU : BaseItem, IContainerItem
	{
		public Guid guid = Guid.NewGuid();
		public IList<Item> Items = new List<Item> {new Item()};

		public override bool CloneNewInstances => true;

		public override ModItem Clone(Item item)
		{
			PortableDSU clone = (PortableDSU)base.Clone();
			clone.Items = Items;
			clone.guid = guid;
			return clone;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portable Deep Storage Unit");
			Tooltip.SetDefault($"Stores {int.MaxValue.ToString("N0", CultureInfo.InvariantCulture)} items\nOn the go!");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 34;
			item.useTime = 5;
			item.useAnimation = 5;
			item.noUseGraphic = true;
			item.useStyle = 1;
			item.value = 10000;
			item.rare = 1;
		}

		public void HandleUI()
		{
			if (!Barrels.Instance.DSUUI.ContainsKey(guid))
			{
				PDSUUI ui = new PDSUUI();
				UserInterface userInterface = new UserInterface();
				ui.Activate();
				userInterface.SetState(ui);
				ui.visible = true;
				ui.Load(this);
				Barrels.Instance.DSUUI.Add(guid, new GUI(ui, userInterface));
			}
			else Barrels.Instance.DSUUI.Remove(guid);
		}

		public override bool UseItem(Player player)
		{
			HandleUI();

			return true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			item.stack++;
			Main.PlaySound(SoundID.DD2_EtherianPortalOpen.WithVolume(0.5f));

			HandleUI();
		}

		public override TagCompound Save() => new TagCompound
		{
			["Items"] = Items.Save(),
			["GUID"] = guid.ToString()
		};

		public override void Load(TagCompound tag)
		{
			Items = Utility.Load(tag);
			guid = tag.ContainsKey("GUID") && !string.IsNullOrEmpty((string)tag["GUID"]) ? Guid.Parse(tag.GetString("GUID")) : Guid.NewGuid();
		}

		public override void NetSend(BinaryWriter writer) => TagIO.Write(Save(), writer);

		public override void NetRecieve(BinaryReader reader) => Load(TagIO.Read(reader));

		public override void AddRecipes()
		{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Obsidian, 75);
			//recipe.AddIngredient(ItemID.Amethyst, 5);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		}

		public IList<Item> GetItems() => Items;
	}
}