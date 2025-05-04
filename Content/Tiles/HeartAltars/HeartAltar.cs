using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CSkies.Content.NPCs.Bosses.Heartcore;
using CSkies.Utilities;
using CSkies.Content.Items.Boss.Void;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Tiles.HeartAltars
{
    public class HeartAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            DustType = DustID.t_Meteor;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(199, 74, 161), CreateMapEntryName());
            AnimationFrameHeight = 54;
            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            Color color = BaseUtility.ColorMult(Heartcore.Flame, 0.7f);
            r = color.R / 255f; g = color.G / 255f; b = color.B / 255f;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 10)
            {
                frameCounter = 0;
                if (++frame >= 4) frame = 0;
            }
        }

        public override bool RightClick(int i, int j)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Heartcore>()))
            {
                return false;
            }
            Player player = Main.LocalPlayer;
            int type = ModContent.ItemType<VoidFragment>();
            if (BasePlayer.HasItem(player, type, 1))
            {
                for (int m = 0; m < 50; m++)
                {
                    Item item = player.inventory[m];
                    if (item != null && item.type == type && item.stack >= 1)
                    {
                        item.stack--;
                        NPC.NewNPC(player.GetSource_FromThis(), i * 16, (j * 16) - 72, ModContent.NPCType<Heartcore>());
                    }
                }
                CWorld.Altar = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Color GetColor(Color color)
        {
            Color glowColor = Color.White * 0.8f;
            return glowColor;
        }

        public Color HeartColor(Color color)
        {
            Color glowColor = Color.White;
            return glowColor;
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Tile tile = Main.tile[x, y];
            Texture2D glowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/HeartAltar_Glow").Value;
            Texture2D heart = ModContent.Request<Texture2D>("CSkies/Glowmasks/HeartAltar_Heart").Value;
            int frameX = tile != null && tile.HasTile ? tile.TileFrameX : 0;
            int frameY = tile != null && tile.HasTile ? tile.TileFrameY + (Main.tileFrame[Type] * 54) : 0;
            BaseDrawing.DrawTileTexture(sb, glowTex, x, y, 16, 16, frameX, frameY, false, false, false, null, GetColor);
            if (!CWorld.Altar1)
            {
                BaseDrawing.DrawTileTexture(sb, heart, x, y, 16, 16, frameX, frameY, false, false, false, null, HeartColor);
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            player.cursorItemIconID = ModContent.ItemType<VoidFragment>();

            player.cursorItemIconText = "";
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }
    }
}