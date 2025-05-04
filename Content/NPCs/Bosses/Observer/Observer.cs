using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using CSkies.Content.Items.Boss.Observer;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;
using Terraria.GameContent.ItemDropRules;

namespace CSkies.Content.NPCs.Bosses.Observer
{
    [AutoloadBossHead]
    public class Observer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Observer");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 136;
            NPC.value = BaseUtility.CalcValue(0, 10, 0, 0);
            NPC.npcSlots = 1000;
            NPC.aiStyle = -1;
            NPC.lifeMax = 6000;
            NPC.defense = 10;
            NPC.damage = 30;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Observer");
            NPC.alpha = 255;
            NPC.noTileCollide = true;         
            NPC.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.6f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public float[] internalAI = new float[4];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }

        public int StarCount = Main.expertMode ? 6 : 4;

        bool title = false;

        public override void AI()
        {
            if (!title)
            {
                CSkies.ShowTitle(NPC, 1);
                title = true;
            }

            NPC.TargetClosest();
            if (!Main.dayTime)
            {
                if (NPC.alpha <= 0)
                {
                    NPC.alpha = 0;
                }
                else
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 17, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, DustID.Electric, Color.White, 1f);
                    NPC.alpha -= 3;
                }
            }
            else
            {
                if (NPC.alpha >= 255)
                {
                    NPC.active = false;
                }
                else
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 17, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, DustID.Electric, Color.White, 1f);
                    NPC.alpha += 3;
                }
            }

            if (internalAI[3] == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int m = 0; m < StarCount; m++)
                    {
                        int projectileID = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Mod.Find<ModProjectile>("Star").Type, NPC.damage, 4, Main.myPlayer);
                        Main.projectile[projectileID].Center = NPC.Center;
                        Main.projectile[projectileID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                        Main.projectile[projectileID].velocity *= 8f;
                        Main.projectile[projectileID].ai[0] = m;
                    }
                    internalAI[3] = 1;
                    NPC.netUpdate = true;
                }
            }
            if (internalAI[1] < 120)
            {
                internalAI[1] += 2;
            }
            else
            {
                internalAI[1] = 120;
                NPC.netUpdate = true;
            }

            BaseAI.AISpaceOctopus(NPC, ref NPC.ai, .2f, 6, 270, 70, null);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (internalAI[2]++ > 100)
                {
                    FireLaser(NPC);
                    internalAI[2] = 0;
                    NPC.netUpdate = true;
                }
                if (NPC.ai[2]++ == (Main.expertMode ? 501 : 701))
                {
                    internalAI[0] += 1;
                    NPC.netUpdate = true;
                }
                if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Star>()))
                {
                    NPC.ai[2] = 0;
                    internalAI[0] = 0;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    internalAI[3] = 0;
                    NPC.netUpdate = true;
                }
            }
            NPC.rotation = 0;

            for (int m = NPC.oldPos.Length - 1; m > 0; m--)
            {
                NPC.oldPos[m] = NPC.oldPos[m - 1];
            }
            NPC.oldPos[0] = NPC.position;

        }

        public void FireLaser(NPC npc)
        {
            Player player = Main.player[npc.target];
            int projType = ModContent.ProjectileType<Starbeam>();
            if (internalAI[0] == 0)
            {
                if (Main.expertMode)
                {
                    float spread = 45f * 0.0174f;
                    Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                    dir *= 14f;
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / 6f;
                    for (int i = 0; i < 3; i++)
                    {
                        double offsetAngle = startAngle + (deltaAngle * i);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 2, 5, Main.myPlayer);
                    }
                }
                else
                {
                    //BaseAI.FireProjectile(player.position, NPC, projType, npc.damage/2, 4, 12, 0, Main.myPlayer);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ >= 6)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        public override void OnKill()
        {
            CSystem._Observer = true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ObserverBag>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverMask>(), 7));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CometFragment>(), 1, 8, 12));

            npcLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Comet>(),
               ModContent.ItemType<CometDagger>(),
               ModContent.ItemType<CometFan>(), 
               ModContent.ItemType<CometJavelin>(),
               ModContent.ItemType<CometPortal>(),
               ModContent.ItemType<Skyshot>()
           ));
        }
        
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server) { return; }
            if (NPC.life <= 0)
            {
                for (int m = 0; m < 20; m++)
                {
                    int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 17, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 10, Color.White, 1f);
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                }
            }
            else
            {
                for (int m = 0; m < 5; m++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 17, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 0, Color.White, 1.1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            BaseDrawing.DrawAfterimage(sb, tex, 0, NPC, 2.5f, 1, 3, true, 0f, 0f, NPC.GetAlpha(Color.White * 0.8f));
            BaseDrawing.DrawTexture(sb, tex, 0, NPC, NPC.GetAlpha(Color.White * 0.8f));
            return false;
        }
    }
}