using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Utilities;
using CSkies.Content.NPCs.Bosses.Void;
using Terraria.GameContent.ItemDropRules;
using CSkies.Content.Items.Boss.Void;

namespace CSkies.Content.NPCs.Bosses.ObserverVoid
{
    [AutoloadBossHead]
    public class ObserverVoid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Observer Void");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 136;
            NPC.value = BaseUtility.CalcValue(0, 10, 0, 0);
            NPC.npcSlots = 1000;
            NPC.aiStyle = -1;
            NPC.lifeMax = 100000;
            NPC.defense = 50;
            NPC.damage = 120;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/ObserverVoid");
            NPC.alpha = 255;
            NPC.noTileCollide = true;
            NPC.value = Item.sellPrice(0, 10, 0, 0);
        }
        public override void OnKill()
        {
            CSystem._ObserverVoid = true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * bossAdjustment);
            NPC.defense = (int)(NPC.defense * 1.2f);
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

        public int StarCount = Main.expertMode ? 10 : 8;
        public Vector2 pos;

        bool title = false;

        public override void AI()
        {
            if (!title)
            {
                CSkies.ShowTitle(NPC, 3);
                title = true;
            }

            Lighting.AddLight(NPC.Center, 0, 0f, .15f);
            NPC.TargetClosest();
            if (!Main.dayTime)
            {
                if (NPC.alpha <= 0)
                {
                    NPC.alpha = 0;
                }
                else
                {
                    NPC.alpha -= 5;
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
                    NPC.alpha += 5;
                }
            }

            if (internalAI[3] == 0 && NPC.ai[3] < 1000)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int m = 0; m < StarCount; m++)
                    {
                        int projectileID = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BlackHole>(), NPC.damage / 4, 4, Main.myPlayer, m);
                        Main.projectile[projectileID].Center = NPC.Center;
                        Main.projectile[projectileID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                        Main.projectile[projectileID].velocity *= 8f;
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

            BaseAI.AISkull(NPC, ref NPC.ai, true, 11, 350, .05f, .07f);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (internalAI[2]++ > 200)
                {
                    FireLaser(NPC);
                    internalAI[2] = 0;
                    NPC.netUpdate = true;
                }
                if (NPC.ai[3] == 1200f)
                {
                    NPC.netUpdate = true;
                }
                else if (NPC.ai[3] < 1200)
                {
                    NPC.ai[3]++;
                    NPC.netUpdate = true;
                    if (NPC.ai[2]++ == (Main.expertMode ? 400 : 500))
                    {
                        internalAI[0] += 1;
                        NPC.netUpdate = true;
                    }
                }
                if (!CUtils.AnyProjectiles(ModContent.ProjectileType<BlackHole>()))
                {
                    NPC.ai[2] = 0;
                    internalAI[0] = 0;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    internalAI[3] = 0;
                    NPC.netUpdate = true;
                }
            }

            if (NPC.ai[3] >= 1200f)
            {
                if (NPC.ai[3] > (Main.expertMode ? 1500 : 1400))
                {
                    NPC.ai[3] = 0;
                    NPC.netUpdate = true;
                }
                else
                {
                    NPC.ai[3]++;
                }

                NPC.ai[2] = 0;
                NPC.velocity *= .98f;
                if (VortexScale < 1f)
                {
                    VortexScale += .01f;
                }
                VortexRotation += .3f;

                for (int u = 0; u < Main.maxPlayers; u++)
                {
                    Player target = Main.player[u];

                    if (target.active && Vector2.Distance(NPC.Center, target.Center) < 260 * VortexScale)
                    {
                        float num3 = 6f;
                        Vector2 vector = new Vector2(target.position.X + target.width / 4, target.position.Y + target.height / 4);
                        float num4 = NPC.Center.X - vector.X;
                        float num5 = NPC.Center.Y - vector.Y;
                        float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        int num7 = 4;
                        target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                        target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                    }
                }
            }
            else
            {
                if (VortexScale > 0f)
                {
                    VortexScale -= .05f;
                }
                VortexRotation += .05f;
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
            int projType = ModContent.ProjectileType<ShadowBlast>();
            if (internalAI[0] == 0)
            {
                if (Main.expertMode)
                {
                    float shots = Main.rand.Next(1, 5);
                    float spread = 45f * 0.0174f;
                    Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                    dir *= 14f;
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / (shots * 2);
                    for (int i = 0; i < shots; i++)
                    {
                        double offsetAngle = startAngle + (deltaAngle * i);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 4, 5, Main.myPlayer);
                    }
                }
                else
                {
                    //BaseAI.FireProjectile(player.Center, npc, projType, npc.damage / 2, 4, 12, 0, Main.myPlayer);
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ObserverVoidBag>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverVoidTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverVoidMask>(), 7));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 8, 12));
            npcLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Singularity>(),
               ModContent.ItemType<VoidFan>(),
               ModContent.ItemType<Items.Boss.Void.VoidShot>(),
               ModContent.ItemType<VoidJavelin>(),
               ModContent.ItemType<VoidWings>(),
               ModContent.ItemType<VoidPortal>()
           ));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.expertMode)
            {
                if (NPC.life < NPC.lifeMax / 2)
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<VoidTransition1>());
                    Main.npc[n].Center = NPC.Center;
                    Main.npc[n].velocity = NPC.velocity;
                    NPC.active = false;
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
            potionType = ItemID.SuperHealingPotion;
        }

        float VortexScale = 0f;
        float VortexRotation = 0f;
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            Texture2D Cyclone = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/DarkVortex").Value;
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            if (VortexScale > 0)
            {
                Rectangle frame = BaseDrawing.GetFrame(0, Cyclone.Width, Cyclone.Height, 0, 0);
                BaseDrawing.DrawTexture(sb, Cyclone, 0, NPC.position, NPC.width, NPC.height, VortexScale, VortexRotation, NPC.direction, 1, frame, Color.White, true);
            }
            BaseDrawing.DrawAura(sb, tex, 0, NPC, auraPercent, 2f, 0f, 0f, NPC.GetAlpha(Color.White));
            BaseDrawing.DrawTexture(sb, tex, 0, NPC, NPC.GetAlpha(Color.White));
            return false;
        }
    }
}