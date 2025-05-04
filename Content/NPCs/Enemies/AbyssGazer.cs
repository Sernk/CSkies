using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Content.NPCs.Bosses.ObserverVoid;
using CSkies.Utilities.Base.BaseMod;

namespace CSkies.Content.NPCs.Enemies
{
	public class AbyssGazer : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Abyss Gazer");
            Main.npcFrameCount[NPC.type] = 4;
		}		

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 38;
            NPC.value = BaseUtility.CalcValue(0, 0, 2, 0);
            NPC.npcSlots = 1;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1200;
            NPC.defense = 5;
            NPC.damage = 30;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath51;
            NPC.knockBackResist = 0.7f;
            NPC.alpha = 255;
        }

        /*public override void OnKill()
		{
            if (!NPC.AnyNPCs(ModContent.NPCType<Bosses.Void.Void>()))
            {
                BaseAI.DropItem(NPC, Mod.Find<ModItem>("VoidLens").Type, 1, 1, 50, true);
            }
        }*/

        /*public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<CPlayer>().ZoneVoid && NPC.downedMoonlord)
            {
                return .05f;
            }
            return 0;
        }*/

        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			if (NPC.life <= 0)
			{
				for (int m = 0; m < 20; m++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 17, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, Color.White, 1f);
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
				}
			}else
			{
				for (int m = 0; m < 5; m++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 17, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, Color.White, 1.1f);
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
            BaseAI.AISkull(NPC, ref NPC.ai, true, 6, 350, .015f, .02f);
            if (NPC.ai[1] <= 400f)
            {
                BaseAI.ShootPeriodic(NPC, player.position, player.width, player.height, ModContent.ProjectileType<BlackHoleProj>(), ref NPC.ai[2], 100, NPC.damage / 2, 7, true);
            }
            if (NPC.ai[0] < 200) { BaseAI.LookAt(player.Center, NPC, 1); } else { if (NPC.timeLeft > 10) { NPC.timeLeft = 10; } NPC.spriteDirection = -NPC.direction; }
            NPC.rotation = 0;
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            Texture2D Tex = TextureAssets.Npc[NPC.type].Value;

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawAura(spriteBatch, Tex, 0, NPC, auraPercent, 2f, 0f, 0f, NPC.GetAlpha(Color.White * 0.8f ));
			return false;
		}
	}
}