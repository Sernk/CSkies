using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.UI;
using ReLogic.Graphics;
using CSkies.Utilities.Globals;

namespace CSkies.Utilities;

public class CUI : ModSystem
{
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        Titles modPlayer = Main.player[Main.myPlayer].GetModPlayer<Titles>();
        if (modPlayer.text)
        {
            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            var computerState = new LegacyGameInterfaceLayer("CSkies: UI",
                delegate
                {
                    BossTitle(modPlayer.BossID);
                    return true;
                },
                InterfaceScaleType.UI);
            layers.Insert(textLayer, computerState);
        }
    }
    private void BossTitle(int BossID)
    {
        string BossName = "";
        string BossTitle = "";
        Color titleColor = Color.White;

        switch (BossID)
        {
            case 1:
                BossName = "The Observer";
                BossTitle = "All-Seeing Eye";
                titleColor = Color.SkyBlue;
                break;
            case 2:
                BossName = "Starcore";
                BossTitle = "Cosmic Construct";
                titleColor = Color.LimeGreen;
                break;
            case 3:
                BossName = "Observer Void";
                BossTitle = "Abyssal Gazer";
                titleColor = new Color(75, 68, 124);
                break;
            case 4:
                BossName = "V O I D";
                BossTitle = "All-Seeing Evil";
                titleColor = new Color(143, 204, 204);
                break;
            case 5:
                BossName = "Heartcore";
                BossTitle = "Sealed Fury";
                titleColor = Color.HotPink;
                break;
            case 6:
                BossName = "Fury Soul";
                BossTitle = "Hellish Wrath Incarnate";
                titleColor = new Color(254, 121, 2);
                break;
            case 7:
                BossName = "Novacore";
                BossTitle = "Astral Artifact";
                titleColor = Color.Magenta;
                break;
            case 8:
                BossName = "Enigma";
                BossTitle = "Mechanical Madman";
                titleColor = Color.DarkBlue;
                break;
            case 9:
                BossName = "Enigma Prime";
                BossTitle = "Supreme Galactic Genius";
                titleColor = Color.LimeGreen;
                break;
            case 10:
                BossName = "Artemis Luminoth";
                BossTitle = "Mechanical Masterpiece";
                titleColor = Color.LimeGreen;
                break;
        }

        Titles modPlayer2 = Main.player[Main.myPlayer].GetModPlayer<Titles>();
        float alpha = modPlayer2.alphaText;
        float alpha2 = modPlayer2.alphaText2;

        Vector2 textSize = FontAssets.DeathText.Value.MeasureString("~ " + BossName + " ~");
        Vector2 textSize2 = FontAssets.DeathText.Value.MeasureString(BossTitle) * .6f; ;
        float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
        float text2PositionLeft = Main.screenWidth / 2 - textSize2.X / 2;

        Main.spriteBatch.DrawString(FontAssets.DeathText.Value, BossTitle, new Vector2(text2PositionLeft, (Main.screenHeight / 2) - 350), titleColor * ((255 - alpha2) / 255f), 0f, Vector2.Zero, .6f, SpriteEffects.None, 0f);
        Main.spriteBatch.DrawString(FontAssets.DeathText.Value, "~ " + BossName + " ~", new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), titleColor * ((255 - alpha) / 255f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

    }
}