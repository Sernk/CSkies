using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.FurySoul;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;
using Terraria.GameContent.ItemDropRules;
using CSkies.Content.Items.Boss.Heartcore;

namespace CSkies.Content.NPCs.Bosses.Heartcore
{
    [AutoloadBossHead]
    public class Heartcore : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.npcSlots = 100;
            NPC.width = 50;
            NPC.height = 50;
            NPC.aiStyle = -1;
            NPC.damage = 90;
            NPC.defense = 60;
            NPC.lifeMax = 150000;
            NPC.value = Item.sellPrice(0, 12, 0, 0);
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Heartcore");
            NPC.HitSound = SoundID.Item21;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * 0.8f);
            NPC.defense = 70;
        }

        public float[] Shoot = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Shoot[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Shoot[0] = reader.ReadSingle();
            }
        }

        public override void OnKill()
        {
            CSystem._Heartcore = true;
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HeartcoreGore1").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HeartcoreGore2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HeartcoreGore3").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HeartcoreGore4").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HeartcoreGoreHalf1").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("HeartcoreGoreHalf2").Type, 1f);

            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.SolarFlare, -NPC.velocity.X * 0.2f,
                    -NPC.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }

            if (Main.expertMode)
            {
                int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<FurySoulTransition>());
                Main.npc[n].Center = NPC.Center;
                return;
            }
            else
            {
                string[] lootTableA = { "Sol", "MeteorShower", "BlazeBuster", "FlamingSoul" };
                int lootA = Main.rand.Next(lootTableA.Length);

                //NPC.DropLoot(Mod.Find<ModItem>(lootTableA[lootA]).Type);

                //NPC.DropLoot(ModContent.ItemType<Items.Boss.Heartcore.HeartSoul>(), Main.rand.Next(8, 12));
                int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HeartcoreDefeat>());
                Main.npc[n].Center = NPC.Center;
                CWorld.downedHeartcore = true;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartSoul>(), 1, 8, 12));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartcoreTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartcoreMask>(), 7));
            npcLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Sol>(),
               ModContent.ItemType<MeteorShower>(),
               ModContent.ItemType<BlazeBuster>(), 
               ModContent.ItemType<FlamingSoul>()
           ));
        }
        public static Color Flame => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Orange, Color.Red, Color.Orange);

        bool Rage = false;

        bool title = false;

        public override void AI()
        {
            if (!title)
            {
                CSkies.ShowTitle(NPC, 5);
                title = true;
            }

            int speed = 9;
            float interval = .02f;

            if (NPC.life < NPC.lifeMax / 3)
            {
                if (!Rage)
                {
                    Rage = true;
                }
                speed = 10;
                interval = .025f;
            }

            RingEffects();

            Lighting.AddLight(NPC.Center, Flame.R / 150, Flame.G / 150, Flame.B / 150);


            if (!NPC.HasPlayerTarget)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target];

            if (player.dead || !player.active || Main.dayTime)
            {
                NPC.TargetClosest();
                NPC.noTileCollide = true;

                if (NPC.timeLeft < 10)
                    NPC.timeLeft = 10;
                NPC.velocity.X *= 0.9f;

                if (NPC.ai[1]++ > 300)
                {
                    NPC.velocity.Y -= 0.2f;
                    if (NPC.velocity.Y > 15f) NPC.velocity.Y = 15f;
                    NPC.rotation = 0f;
                    if (NPC.position.Y + NPC.velocity.Y <= 0f && Main.netMode != NetmodeID.MultiplayerClient) { BaseAI.KillNPC(NPC); NPC.netUpdate = true; }
                }
            }

            BaseAI.AISkull(NPC, ref NPC.ai, true, speed, 350, interval, .025f);

            if (NPC.ai[2]++ > (Main.expertMode ? 150 : 220))
            {
                if (NPC.velocity.X > 0)
                {
                    NPC.rotation += .06f;
                }
                else if (NPC.velocity.X < 0)
                {
                    NPC.rotation -= .06f;
                }
                switch (Shoot[0])
                {
                    case 0:
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - NPC.Center);
                        dir *= 12f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        if (NPC.life < NPC.lifeMax / 3)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (NPC.ai[2] % 30 == 0)
                                {
                                    double offsetAngle = startAngle + (deltaAngle * i);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigHeartshot>(), NPC.damage / 2, 5, Main.myPlayer);
                                }
                            }
                            if (NPC.ai[2] > (Main.expertMode ? 271 : 331))
                            {
                                NPC.ai[2] = 0;
                                Shoot[0] = Main.rand.Next(4);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                double offsetAngle = startAngle + (deltaAngle * i);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigHeartshot>(), NPC.damage / 2, 5, Main.myPlayer);
                            }
                            NPC.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
                    case 1:
                        float spread1 = 12f * 0.0174f;
                        double startAngle1 = Math.Atan2(NPC.velocity.X, NPC.velocity.Y) - spread1 / 2;
                        double deltaAngle1 = spread1 / 10f;
                        SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                        for (int i = 0; i < 10; i++)
                        {
                            double offsetAngle1 = (startAngle1 + deltaAngle1 * (i + i * i) / 2f) + 32f * i;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle1) * 8f), (float)(Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), NPC.damage / 2, 6);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle1) * 8f), (float)(-Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), NPC.damage / 2, 6);
                        }
                        NPC.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 2:
                        SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(7, 7), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-7, 7), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(7, -7), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-7, -7), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(9, 0), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-9, 0), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -9), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 9), ModContent.ProjectileType<Fireball>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);

                        NPC.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 3:
                        if (NPC.ai[2] % 20 == 0)
                        {
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(9, 9), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-9, 9), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(9, -9), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-9, -9), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(9, 0), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-9, 0), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -9), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 9), ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        }
                        if (NPC.ai[2] > (Main.expertMode ? 271 : 331))
                        {
                            NPC.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
                }
            }
            else
            {
                if (NPC.velocity.X > 0)
                {
                    NPC.rotation += .03f;
                }
                else if (NPC.velocity.X < 0)
                {
                    NPC.rotation -= .03f;
                }
                if (NPC.life < NPC.lifeMax / 2)
                {
                    BaseAI.ShootPeriodic(NPC, player.position, player.width, player.height, ModContent.ProjectileType<Meteor>(), ref NPC.ai[3], Main.rand.Next(30, 50), NPC.damage / 4, 10, true);
                }
            }
        }

        float scale = 0;

        private void RingEffects()
        {
            if (NPC.life < NPC.lifeMax / 3)
            {
                if (scale >= 1f)
                {
                    scale = 1f;
                }
                else
                {
                    scale += .02f;
                }
            }
            else
            {
                if (scale > .1f)
                {
                    scale -= .02f;
                }
                else
                {
                    scale = 0;
                }
            }
        }

        int f = 0;

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 5)
            {
                NPC.frameCounter = 0;
                if (f >= 4)
                {
                    f = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2D13 = TextureAssets.Npc[NPC.type].Value;
            Texture2D BladeTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/HeartcoreBack").Value;
            Texture2D BladeGlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/HeartcoreBack_Glow").Value;
            Texture2D GlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/Heartcore_Glow").Value;

            Texture2D RingTex1 = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ring1").Value;
            Texture2D RingTex2 = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ring2").Value;
            Texture2D RitualTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ritual").Value;

            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Rectangle BackFrame = BaseDrawing.GetFrame(f, BladeTex.Width, BladeTex.Height / 4, 0, 0);
            Rectangle RingFrame = BaseDrawing.GetFrame(0, RingTex1.Width, RingTex1.Height, 0, 0);
            Rectangle RitualFrame = BaseDrawing.GetFrame(0, RitualTex.Width, RitualTex.Height, 0, 0);
            Rectangle Frame = BaseDrawing.GetFrame(0, texture2D13.Width, texture2D13.Height, 0, 0);

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(spriteBatch, RitualTex, r, NPC.position, NPC.width, NPC.height, scale, -NPC.rotation, 0, 1, RitualFrame, drawColor, true);
                BaseDrawing.DrawTexture(spriteBatch, RingTex1, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, RingFrame, drawColor, true);
                BaseDrawing.DrawTexture(spriteBatch, RingTex2, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, RingFrame, drawColor, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 4, BackFrame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, r, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 4, BackFrame, Color.White, true);

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 1, Frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, GlowTex, r, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 1, Frame, Color.White, true);

            return false;
        }
    }
}