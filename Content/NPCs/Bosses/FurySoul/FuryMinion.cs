using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.Heartcore;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Utilities;
using CSkies.Content.NPCs.Bosses.ObserverVoid;

namespace CSkies.Content.NPCs.Bosses.FurySoul
{
	public class FuryMinion : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Fury Spirit");
            Main.npcFrameCount[NPC.type] = 4;
		}		

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 38;
            NPC.value = BaseUtility.CalcValue(0, 0, 2, 0);
            NPC.npcSlots = 1;
            NPC.aiStyle = -1;
            NPC.lifeMax = 100;
            NPC.defense = 5;
            NPC.damage = 30;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath51;
            NPC.knockBackResist = 0.7f;
            NPC.alpha = 255;
            NPC.noTileCollide = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<CPlayer>().ZoneVoid)
            {
                return .05f;
            }
            return 0;
        }

        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			if (NPC.life <= 0)
			{
				for (int m = 0; m < 20; m++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, Color.White, 2f);
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
				}
			}
            else
			{
				for (int m = 0; m < 5; m++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SolarFlare, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, Color.White, 1.1f);
				}
			}
		}

        int frame = 0;
        public override void FindFrame(int frameHeight)
		{
            if (NPC.frameCounter++ > 7)
            {
                frame++;
                NPC.frameCounter = 0;
                if (frame > 3)
                {
                    frame = 0;
                }
            }
			NPC.frame = BaseDrawing.GetFrame(frame, 32, 42, 0, 0);
		}
        int shootTimer = 0;
        public override void AI()
		{
            if (NPC.alpha > 0)
            {
                NPC.alpha -= 5;
            }
            else
            {
                NPC.alpha = 0;
            }
			NPC.noGravity = true;
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			for (int m = NPC.oldPos.Length - 1; m > 0; m--)
			{
				NPC.oldPos[m] = NPC.oldPos[m - 1];
			}
			NPC.oldPos[0] = NPC.position;
            BaseAI.AISkull(NPC, ref NPC.ai, true, 10, 350, .03f, .04f);
            if (shootTimer >= 120)
            {
                Vector2 direction = player.Center - NPC.Center;
                direction.Normalize();
                direction *= 10f;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction, ModContent.ProjectileType<Fireshot>(), 90, 1f, Main.myPlayer);
                shootTimer = 0;
            }
            NPC.rotation = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            SpriteBatch sb = Main.spriteBatch;

            Texture2D Tex = TextureAssets.Npc[NPC.type].Value;

            Rectangle f = BaseDrawing.GetFrame(frame, TextureAssets.Npc[NPC.type].Value.Width, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type], 0, 0);

            BaseDrawing.DrawTexture(sb, Tex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 4, f, NPC.GetAlpha(Color.White * 0.8f), true);
            return false;
		}
	}
}