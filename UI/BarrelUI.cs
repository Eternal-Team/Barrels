using Barrels.Items;
using Barrels.TileEntities;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using TheOneLibrary.Base.UI;
using TheOneLibrary.UI.Elements;
using TheOneLibrary.Utility;

namespace Barrels.UI
{
	public class BarrelUI : BaseUI, ITileEntityUI
	{
		public TEBarrel barrel;

		public UIText textLabel = new UIText("Barrel");

		public UIContainerSlot display;
		public UIGrid items = new UIGrid(5);

		public override void OnInitialize()
		{
			panelMain.Width.Pixels = 232;
			panelMain.Height.Pixels = 132;
			panelMain.Center();
			panelMain.Top.Pixels -= panelMain.Height.Pixels / 3f * 2f;
			panelMain.SetPadding(0);
			panelMain.OnMouseDown += DragStart;
			panelMain.OnMouseUp += DragEnd;
			panelMain.BackgroundColor = panelColor;
			Append(panelMain);

			textLabel.HAlign = 0.5f;
			textLabel.Top.Pixels = 8;
			panelMain.Append(textLabel);

			display = new UIContainerSlot(barrel);
			display.HAlign = 0.5f;
			display.Top.Pixels = 36;
			display.CanInteract += (item, mouseItem) => false;
			panelMain.Append(display);

			items.Width.Set(-16, 1);
			items.Height.Pixels = 40;
			items.Left.Pixels = 8;
			items.Top.Pixels = 84;
			items.ListPadding = 4;
			items.OverflowHidden = true;
			panelMain.Append(items);
		}

		public override void Load()
		{
			for (int i = 1; i < barrel.GetItems().Count; i++)
			{
				UIContainerSlot slot = new UIContainerSlot(barrel, i);
				slot.CanInteract += (item, mouseItem) => (mouseItem.IsAir || mouseItem.modItem is BarrelUpgrade) && (!(item.modItem is StackUpgrade) || barrel.Items[0].stack <= barrel.maxStoredItems - ((StackUpgrade)item.modItem).data.GetInt("StackIncrease") * TEBarrel.BaseMax);
				items.Add(slot);
			}
		}

		public void SetTileEntity(ModTileEntity tileEntity) => barrel = (TEBarrel)tileEntity;
	}
}