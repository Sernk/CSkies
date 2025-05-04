using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using CSkies.Content.NPCs.Bosses.Observer;
using CSkies.Content.Items.Summons;
using CSkies.Content.Items.Boss.Observer;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.Starcore;
using CSkies.Content.Items.Boss.Starcore;
using CSkies.Content.NPCs.Bosses.ObserverVoid;
using CSkies.Content.Items.Boss.Void;
using CSkies.Content.NPCs.Bosses.Heartcore;
using CSkies.Content.Items.Boss.Heartcore;
using CSkies.Content.NPCs.Bosses.FurySoul;

namespace CSkies.Utilities
{
    public class BossChecklistSuport : ModSystem
    {
        public override void PostSetupContent()
        {
            DoBossChecklistIntegration();
        }

        private void DoBossChecklistIntegration()
        {

            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }
            string internalName = "ObserverB";

            float weight = 4f;

            Func<bool> downed = () => CSystem._Observer;

            int bossType = ModContent.NPCType<Observer>();

            int spawnItem = ModContent.ItemType<CosmicEye>();

            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("CSkies/BossChecklist/Observer").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<ObserverBag>(),
                ModContent.ItemType<ObserverEye>(),
                ModContent.ItemType<Comet>(),
                ModContent.ItemType<CometDagger>(),
                ModContent.ItemType<CometFan>(),
                ModContent.ItemType<CometJavelin>(),
                ModContent.ItemType<CometPortal>(),
                ModContent.ItemType<Skyshot>(),
                ModContent.ItemType<CometFragment>()
            };
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               internalName,
               weight,
               downed,
               bossType,
               new Dictionary<string, object>()
               {
                   ["spawnItems"] = spawnItem,
                   ["collectibles"] = collectibles,
                   ["customPortrait"] = customPortrait,
               }
            );

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            string internalNameStarcore = "StarcoreB";

            float weightStarcore = 7.5f;

            Func<bool> downedStarcore = () => CSystem._Starcore;

            int bossTypeStarcore = ModContent.NPCType<Starcore>();

            int spawnItemStarcore = ModContent.ItemType<Transmitter>();

            var customPortraitStarcore = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("CSkies/BossChecklist/Starcore").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            List<int> collectiblesStarcore = new List<int>()
            {
                ModContent.ItemType<StarcoreBag>(),
                ModContent.ItemType<StarcoreShield>(),
                ModContent.ItemType<Starsaber>(),
                ModContent.ItemType<Railscope>(),
                ModContent.ItemType<StormStaff>(),
                ModContent.ItemType<StarDroneUnit>(),
                ModContent.ItemType<Stelarite>()
            };
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               internalNameStarcore,
               weightStarcore,
               downedStarcore,
               bossTypeStarcore,
               new Dictionary<string, object>()
               {
                   ["spawnItems"] = spawnItemStarcore,
                   ["collectibles"] = collectiblesStarcore,
                   ["customPortrait"] = customPortraitStarcore,
               }
            );

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            string internalNameObserverVoid = "ObserverVoidB";

            float weightObserverVoid = 18f;

            Func<bool> downedObserverVoid = () => CSystem._ObserverVoid;

            int bossTypeObserverVoid = ModContent.NPCType<ObserverVoid>();

            int spawnItemObserverVoid = ModContent.ItemType<VoidEye>();

            var customPortraitObserverVoid = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("CSkies/BossChecklist/ObserverVoid").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            List<int> collectiblesObserverVoid = new List<int>()
            {
                ModContent.ItemType<ObserverVoidMask>(),
                ModContent.ItemType<ObserverVoidTrophy>(),
                ModContent.ItemType<Singularity>(),
                ModContent.ItemType<VoidJavelin>(),
                ModContent.ItemType<VoidFan>(),
                ModContent.ItemType<VoidShot>(),
                ModContent.ItemType<VoidWings>(),
                ModContent.ItemType<VoidFragment>(),
                ModContent.ItemType<VoidPortal>()
            };
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               internalNameObserverVoid,
               weightObserverVoid,
               downedObserverVoid,
               bossTypeObserverVoid,
               new Dictionary<string, object>()
               {
                   ["spawnItems"] = spawnItemObserverVoid,
                   ["collectibles"] = collectiblesObserverVoid,
                   ["customPortrait"] = customPortraitObserverVoid,
               }
            );

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            string internalNameVoid = "VoidB";

            float weightVoid = 18f;

            Func<bool> downedVoid = () => CSystem._Void;

            int bossTypeVoid = ModContent.NPCType<Content.NPCs.Bosses.Void.Void>();

            int spawnItemVoid = ModContent.ItemType<VoidEye>();

            var customPortraitVoid = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("CSkies/BossChecklist/VOID").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            List<int> collectiblesVoid = new List<int>()
            {
                ModContent.ItemType<VOIDMask>(),
                ModContent.ItemType<VOIDTrophy>(),
                ModContent.ItemType<ObserverVoidBag>(),
                ModContent.ItemType<ObserverVoidEye>()
            };
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               internalNameVoid,
               weightVoid,
               downedVoid,
               bossTypeVoid,
               new Dictionary<string, object>()
               {
                   ["spawnItems"] = spawnItemVoid,
                   ["collectibles"] = collectiblesVoid,
                   ["customPortrait"] = customPortraitVoid,
               }
            );

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            string internalNameHeartcore = "HeartcoreB";

            float weightHeartcore = 19f;

            Func<bool> downedHeartcore = () => CSystem._Heartcore;

            int bossTypeHeartcore = ModContent.NPCType<Heartcore>();

            int spawnItemHeartcore = ModContent.ItemType<PassionRune>();

            var customPortraitHeartcore = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("CSkies/BossChecklist/Heartcore").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            List<int> collectiblesHeartcore = new List<int>()
            {
                ModContent.ItemType<HeartcoreMask>(),
                ModContent.ItemType<HeartcoreTrophy>(),
                ModContent.ItemType<Sol>(),
                ModContent.ItemType<BlazeBuster>(),
                ModContent.ItemType<MeteorShower>(),
                ModContent.ItemType<FlamingSoul>()
            };
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               internalNameHeartcore,
               weightHeartcore,
               downedHeartcore,
               bossTypeHeartcore,
               new Dictionary<string, object>()
               {
                   ["spawnItems"] = spawnItemHeartcore,
                   ["collectibles"] = collectiblesHeartcore,
                   ["customPortrait"] = customPortraitHeartcore,
               }
            );

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            string internalNameFurySoul = "FurySoulB";

            float weightFurySoul = 19.5f;

            Func<bool> downedFurySoul = () => CSystem._FurySoul;

            int bossTypeFurySoul = ModContent.NPCType<FurySoul>();

            int spawnItemFurySoul = ModContent.ItemType<PassionRune>();

            var customPortraitFurySoul = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("CSkies/BossChecklist/FurySoul").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            List<int> collectiblesFurySoul = new List<int>()
            {
                ModContent.ItemType<FurySoulTrophy>(),
                ModContent.ItemType<FurySoulMask>(),
                ModContent.ItemType<HeartcoreBag>(),
                ModContent.ItemType<HeartcoreShield>()
            };
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               internalNameFurySoul,
               weightFurySoul,
               downedFurySoul,
               bossTypeFurySoul,
               new Dictionary<string, object>()
               {
                   ["spawnItems"] = spawnItemFurySoul,
                   ["collectibles"] = collectiblesFurySoul,
                   ["customPortrait"] = customPortraitFurySoul,
               }
            );
        }
    }
}