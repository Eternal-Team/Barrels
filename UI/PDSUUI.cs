using Barrels.Items;
using Terraria;
using Terraria.GameContent.UI.Elements;
using TheOneLibrary.Base.UI;
using TheOneLibrary.UI.Elements;
using TheOneLibrary.Utility;

namespace Barrels.UI
{
	public class PDSUUI : BaseUI
	{
		public UIText textLabel = new UIText("Deep Storage Unit");
		public UIContainerSlot slot;

		public override void OnInitialize()
		{
			panelMain.Width.Pixels = Main.screenWidth / 7f;
			panelMain.Height.Pixels = 84;
			panelMain.Center();
			panelMain.SetPadding(0);
			panelMain.OnMouseDown += DragStart;
			panelMain.OnMouseUp += DragEnd;
			Append(panelMain);

			textLabel.HAlign = 0.5f;
			textLabel.Top.Pixels = 8;
			panelMain.Append(textLabel);
		}

		public void Load(PortableDSU dsu)
		{
			slot = new UIContainerSlot(dsu);
			slot.maxStack = int.MaxValue;
			slot.HAlign = 0.5f;
			slot.Top.Pixels = 36;
			panelMain.Append(slot);
		}
	}
}