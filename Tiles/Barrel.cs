using System;
using System.Linq;
using Barrels.Items;
using Barrels.TileEntities;
using Barrels.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
using TheOneLibrary.Base;
using TheOneLibrary.Utility;

namespace Barrels.Tiles
{
	public class Barrel : BaseTile
	{
		public override string Texture => Barrels.TileTexturePath + "Barrel";

		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidBottom, 0, 0);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new[] {16, 16};
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TEBarrel>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Barrel");
			AddMapEntry(Color.Brown, name);
			mineResist = 10f;
		}

		public override void LeftClick(int i, int j)
		{
			int ID = mod.GetID<TEBarrel>(i, j);
			if (ID == -1) return;

			Main.LocalPlayer.noThrow = 2;

			TEBarrel barrel = (TEBarrel)TileEntity.ByID[ID];

			if (!barrel.Items[0].IsAir)
			{
				if (Main.keyState.IsKeyDown(Keys.RightShift))
				{
					int count = Math.Min(barrel.Items[0].stack, barrel.Items[0].maxStack);
					Item.NewItem(barrel.Position.ToVector2() * 16, new Vector2(32), barrel.Items[0].type, count);
					barrel.Items[0].stack -= count;
					if (barrel.Items[0].stack <= 0) barrel.Items[0].TurnToAir();
				}
				else
				{
					Item.NewItem(barrel.Position.ToVector2() * 16, new Vector2(32), barrel.Items[0].type);
					barrel.Items[0].stack--;
					if (barrel.Items[0].stack <= 0) barrel.Items[0].TurnToAir();
				}

				barrel.SendUpdate();
			}
		}

		public override void RightClick(int i, int j)
		{
			int ID = mod.GetID<TEBarrel>(i, j);
			if (ID == -1) return;

			Main.LocalPlayer.noThrow = 2;

			TEBarrel barrel = (TEBarrel)TileEntity.ByID[ID];

			if (Main.inputText.IsKeyDown(Keys.RightShift))
			{
				if (Main.LocalPlayer.HasItem(barrel.Items[0].type))
				{
					int count = Math.Min(barrel.maxStoredItems - barrel.Items[0].stack, Main.LocalPlayer.CountItem(barrel.Items[0].type));
					Main.LocalPlayer.ConsumeItem(barrel.Items[0].type, count);
					barrel.Items[0].stack += count;
				}
				else mod.HandleUI<BarrelUI>(ID);
			}
			else
			{
				Item item = Utility.HeldItem;
				if (!item.IsAir)
				{
					if (barrel.Items[0].IsAir)
					{
						barrel.Items[0] = item.Clone();
						int count = Math.Min(item.stack, barrel.maxStoredItems);
						if (barrel.Items.Where((x, index) => index > 0 && index < barrel.Items.Count).Any(x => x.type == mod.ItemType<VoidUpgrade>())) item.TurnToAir();
						else
						{
							item.stack -= count;
							if (item.stack <= 0) item.TurnToAir();
						}

						barrel.Items[0].stack = count;
					}
					else
					{
						if (barrel.Items[0].type == item.type)
						{
							int count = Math.Min(item.stack, barrel.maxStoredItems - barrel.Items[0].stack);
							if (barrel.Items.Where((x, index) => index > 0 && index < barrel.Items.Count).Any(x => x.type == mod.ItemType<VoidUpgrade>())) item.TurnToAir();
							else
							{
								item.stack -= count;
								if (item.stack <= 0) item.TurnToAir();
							}

							barrel.Items[0].stack += count;
						}
					}
				}
				else mod.HandleUI<BarrelUI>(ID);
			}

			barrel.SendUpdate();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int ID = mod.GetID<TEBarrel>(i, j);
			if (ID != -1) mod.CloseUI(ID);

			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType<Items.Barrel>());
			mod.GetTileEntity<TEBarrel>().Kill(i, j);
		}

		public override void MouseOver(int i, int j) => mod.DrawInfo<TEBarrel>(i, j);

		public override void MouseOverFar(int i, int j) => mod.DrawInfo<TEBarrel>(i, j);

		public override bool AutoSelect(int i, int j, Item item)
		{
			int ID = mod.GetID<TEBarrel>(i, j);
			if (ID != -1)
			{
				TEBarrel barrel = (TEBarrel)TileEntity.ByID[ID];
				return item.type == barrel.Items[0].type;
			}
			return false;
		}

		public override void DrawEffects(int i, int j, SpriteBatch batch, ref Color color, ref int nextSpecialDrawIndex)
		{
			Main.specX[nextSpecialDrawIndex] = i;
			Main.specY[nextSpecialDrawIndex] = j;
			nextSpecialDrawIndex++;
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			int ID = mod.GetID<TEBarrel>(i, j);
			if (ID == -1) return;

			Tile tile = Main.tile[i, j];
			if (tile.TopLeft())
			{
				TEBarrel barrel = (TEBarrel)TileEntity.ByID[ID];
				Item item = barrel.Items[0];

				if (!item.IsAir)
				{
					Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
					if (Main.drawToScreen) zero = Vector2.Zero;
					Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X + 16, j * 16 - (int)Main.screenPosition.Y + 16) + zero;

					Rectangle rectangle = item.GetRectangle();

					Texture2D texture = Main.itemTexture[item.type];
					Color color = Color.White;
					float pulseScale = 1f;
					ItemSlot.GetItemLight(ref color, ref pulseScale, item);
					float scale = 1f;
					if (rectangle.Width > 24f || rectangle.Height > 24f) scale = Math.Min(24f / rectangle.Width, 24f / rectangle.Height);
					scale *= 0.85f;
					Vector2 itemPosition = position - rectangle.Size() * scale / 2f;
					Vector2 origin = rectangle.Size() * (pulseScale / 2f - 0.5f);

					if (ItemLoader.PreDrawInInventory(item, spriteBatch, itemPosition, rectangle, item.GetAlpha(color), item.GetColor(Color.White), origin, scale * pulseScale))
					{
						spriteBatch.Draw(texture, itemPosition, rectangle, item.GetAlpha(color), 0f, origin, scale * pulseScale, SpriteEffects.None, 0f);
						if (item.color != Color.Transparent) spriteBatch.Draw(texture, itemPosition, rectangle, item.GetColor(Color.White), 0f, origin, scale * pulseScale, SpriteEffects.None, 0f);
					}
					ItemLoader.PostDrawInInventory(item, spriteBatch, itemPosition, rectangle, item.GetAlpha(color), item.GetColor(Color.White), origin, scale * pulseScale);
					if (ItemID.Sets.TrapSigned[item.type])
					{
						spriteBatch.Draw(Main.wireTexture, position + new Vector2(20f, 20f) * scale, new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
					}

					string stack = $"{item.stack}/{barrel.maxStoredItems}";
					if (barrel.drawInfo) Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, stack, position.X, position.Y - 20, Color.White, Color.Black, new Vector2(Main.fontMouseText.MeasureString(stack).X * 0.5f, Main.fontMouseText.MeasureString(stack).Y * 0.7f), 0.7f);
					barrel.drawInfo = false;
				}
			}
		}
	}
}