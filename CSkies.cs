using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Globals;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Utilities.Base.BaseMod;
using CSkies.Utilities.Base.NPCs;
using CSkies.Utilities.Base.Projectiles;
using System.IO;
using Terraria.ID;

namespace CSkies
{
	public class CSkies : Mod
	{
        public static CSkies inst;
        public static CSkies Instance;

        public static ModKeybind AccessoryAbilityKey;

        public CSkies()
        {

            Instance = this;
            inst = this;
        }
        public static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }

        public override void Load()
        {
            Instance = this;
            inst = this;
            AccessoryAbilityKey = KeybindLoader.RegisterKeybind(this, "Celestial Accessory Ability", "V");
            //WeakReferences.PerformModSupport();
            if (!Main.dedServ)
            {
                LoadClient();
            }
        }
        public void LoadClient()
        {
            /*PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Observer/Star").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Observer/StarProj").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex1").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/DarkVortex").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Void/VoidCyclone").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Backgrounds/VoidBolt").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Backgrounds/VoidFlash").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Backgrounds/NovaVortex").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Backgrounds/NovaStar0").Value);
            PremultiplyTexture(ModContent.Request<Texture2D>("CSkies/Backgrounds/NovaStar1").Value);*/

            /*Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("CSkies/Effects/Shockwave").Value);
            Ref<Effect> screenRef2 = new Ref<Effect>(ModContent.Request<Effect>("CSkies/Effects/WhiteFlash").Value);
            Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
            Filters.Scene["Shockwave"].Load();

            Filters.Scene["WhiteFlash"] = new Filter(new ScreenShaderData(screenRef2, "WhiteFlash"), EffectPriority.VeryHigh);
            Filters.Scene["WhiteFlash"].Load();

            Filters.Scene["CSkies:AbyssSky"] = new Filter(new AbyssSkyData("FilterMiniTower").UseColor(.2f, .2f, .2f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CSkies:AbyssSky"] = new AbyssSky();

            Filters.Scene["CSkies:NovaSky"] = new Filter(new AbyssSkyData("FilterMiniTower").UseColor(.3f, 0f, .3f).UseOpacity(0.4f), EffectPriority.VeryHigh);
            SkyManager.Instance["CSkies:NovaSky"] = new NovaSky();*/
        }
        public override void Unload()
        {
            inst = null;
            AccessoryAbilityKey = null;
        }

        public override void HandlePacket(BinaryReader bb, int whoAmI)
        {
            MsgType msg = (MsgType)bb.ReadByte();
            if (msg == MsgType.ProjectileHostility) 
            {
                int owner = bb.ReadInt32();
                int projID = bb.ReadInt32();
                bool friendly = bb.ReadBoolean();
                bool hostile = bb.ReadBoolean();
                if (Main.projectile[projID] != null)
                {
                    Main.projectile[projID].owner = owner;
                    Main.projectile[projID].friendly = friendly;
                    Main.projectile[projID].hostile = hostile;
                }
                if (Main.netMode == NetmodeID.Server) MNet.SendBaseNetMessage(0, owner, projID, friendly, hostile);
            }
            else
            if (msg == MsgType.SyncAI) 
            {
                int classID = bb.ReadByte();
                int id = bb.ReadInt16();
                int aitype = bb.ReadByte();
                int arrayLength = bb.ReadByte();
                float[] newAI = new float[arrayLength];
                for (int m = 0; m < arrayLength; m++)
                {
                    newAI[m] = bb.ReadSingle();
                }
                if (classID == 0 && Main.npc[id] != null && Main.npc[id].active && Main.npc[id].ModNPC != null && Main.npc[id].ModNPC is ParentNPC)
                {
                    ((ParentNPC)Main.npc[id].ModNPC).SetAI(newAI, aitype);
                }
                else
                if (classID == 1 && Main.projectile[id] != null && Main.projectile[id].active && Main.projectile[id].ModProjectile != null && Main.projectile[id].ModProjectile is ParentProjectile)
                {
                    ((ParentProjectile)Main.projectile[id].ModProjectile).SetAI(newAI, aitype);
                }
                if (Main.netMode == NetmodeID.Server) BaseNet.SyncAI(classID, id, newAI, aitype);
            }
        }
        public static void ShowTitle(NPC npc, int ID)
        {
            if (CConfigClient.Instance.BossIntroText)
            {
                Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Title>(), 0, 0, Main.myPlayer, ID, 0);
            }
        }

        public static void ShowTitle(Player player, int ID)
        {
            if (CConfigClient.Instance.BossIntroText)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<Title>(), 0, 0, Main.myPlayer, ID, 0);
            }
        }
    }
    enum MsgType : byte
    {
        ProjectileHostility,
        SyncAI
    }
}