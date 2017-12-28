using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;
using TheOneLibrary.Base;
using TheOneLibrary.Base.UI;
using TheOneLibrary.Utility;

namespace Barrels
{
	public class Barrels : Mod
	{
		[Null] public static Barrels Instance;

		public const string TexturePath = "Barrels/Textures/";
		public const string TileTexturePath = "Barrels/Textures/Tiles/";
		public const string ItemTexturePath = "Barrels/Textures/Items/";

		[UI("TileEntity")] public Dictionary<ModTileEntity, GUI> TEUI = new Dictionary<ModTileEntity, GUI>();
		public Dictionary<Guid, GUI> DSUUI = new Dictionary<Guid, GUI>();

		public Barrels()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadBackgrounds = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void PreSaveAndQuit()
		{
			TEUI.Clear();
		}

		public override void Load()
		{
			Instance = this;
		}

		public override void Unload()
		{
			this.UnloadNullableTypes();

			GC.Collect();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

			if (InventoryIndex != -1)
			{
				layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
					"Barrels: UI",
					delegate
					{
						TEUI.Values.Draw();
						DSUUI.Values.Draw();

						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}