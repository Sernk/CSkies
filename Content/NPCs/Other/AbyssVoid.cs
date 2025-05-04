using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Enemies;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Other
{
    [AutoloadBossHead]
	public class AbyssVoid : ModNPC
	{
        public override string Texture => "CSkies/Content/NPCs/Bosses/ObserverVoid/DarkVortex";
        public override string BossHeadTexture => "CSkies/Content/NPCs/Other/AbyssVoid_Head_Boss";
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Abyss Gate");
        }

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 20000;
			NPC.damage = 0;
			NPC.defense = 20;
			NPC.knockBackResist = 0f;
            NPC.width = 264;
            NPC.height = 264;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.alpha = 0;
			NPC.dontTakeDamage = true;
			NPC.boss = false;
            NPC.npcSlots = 0;
        }

		public override bool CheckActive()
		{
			return false;
		}		

		public override void AI()
		{
            NPC.rotation += .01f;
            NPC.timeLeft = 10;
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[1]++ > 450)
            {
                NPC.ai[1] = 0;
                for (int a = 0; a < Main.rand.Next(4); a++)
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AbyssGazer>());
                    Main.npc[n].Center = NPC.Center;
                }
            }

            if (NPC.collideX || NPC.collideY)
            {
                int VoidHeight = 140;
                int boundary = Main.maxTilesX / 15;
                Point spawnTilePos = new Point(Main.rand.Next(boundary, Main.maxTilesX - boundary), VoidHeight);
                Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);

                NPC.position = spawnPos;
            }
        }

        public Color GetGlowAlpha()
        {
            return Color.White * 0.8f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D T = TextureAssets.Npc[NPC.type].Value;
            BaseDrawing.DrawTexture(spriteBatch, T, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 1, NPC.frame, GetGlowAlpha(), true);
            return false;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0f;
            return;
        }
    }

    public class VoidHandler : ModPlayer
    {
        public override void PostUpdate()
        {
            bool anyVoidExist = NPC.AnyNPCs(ModContent.NPCType<AbyssVoid>());
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.downedMoonlord && !anyVoidExist)
            {
                SpawnVoid();
            }
        }

        public void SpawnVoid()
        {
            int VoidHeight = 140;
            int boundary = Main.maxTilesX / 15;

            Point spawnTilePos = new Point(Main.rand.Next(boundary, Main.maxTilesX - boundary), VoidHeight);				
			Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
			bool anyVoidExist = NPC.AnyNPCs(ModContent.NPCType<AbyssVoid>());			
			if (!anyVoidExist)
			{
                int whoAmI = NPC.NewNPC(Player.GetSource_FromThis(), (int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<AbyssVoid>());			
				if (Main.netMode == NetmodeID.Server && whoAmI != -1 && whoAmI < 200)
				{					
					NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
				}			
			}
        }
    }
}
