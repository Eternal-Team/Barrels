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
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"Barrels: UI",
					delegate
					{
						TEUI.Values.Draw();

						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}