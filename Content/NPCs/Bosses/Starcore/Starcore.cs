using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;
using Terraria.GameContent.ItemDropRules;
using CSkies.Content.Items.Boss.Starcore;

namespace CSkies.Content.NPCs.Bosses.Starcore
{
    [AutoloadBossHead]
    public class Starcore : ModNPC, ILocalizedModType
    {
        public override void Load()
        {
            string StarCore = this.GetLocalization("Chat.StarCore").Value; // BODY IN CRITICAL STATE. ENGAGE PANIC MODE
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starcore");
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 100;
            NPC.width = 50;
            NPC.height = 50;
            NPC.aiStyle = -1;
            NPC.damage = 45;
            NPC.defense = 25;
            NPC.lifeMax = 12000;
            NPC.value = Item.sellPrice(0, 0, 50, 0);
            NPC.HitSound = SoundID.NPCHit4; 
            NPC.DeathSound = SoundID.NPCDeath14; 
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Starcore");
            //bossBag = mod.ItemType("StarcoreBag");
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
                Shoot[0] = reader.ReadFloat();
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<StarcoreBag>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarcoreMask>(), 7));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarcoreTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Stelarite>(), 1, 8, 12));

            npcLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Starsaber>(),
               ModContent.ItemType<StormStaff>(),
               ModContent.ItemType<StarDroneUnit>(), 
               ModContent.ItemType<Railscope>()
           ));
        }
        public override void OnKill()
        {
            CSystem._Starcore = true;
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.StarDust>(), -NPC.velocity.X * 0.2f,
                    -NPC.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        bool warning = false;

        public static Color Warning => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.LimeGreen, Color.Red, Color.LimeGreen);

        bool title = false;

        public override void AI()
        {
            if (!title)
            {
                CSkies.ShowTitle(NPC, 2);
                title = true;
            }

            int speed = 10;
            float interval = .015f;
            if (NPC.life < NPC.lifeMax / 4)
            {
                speed = 14;
                interval = .02f;
                if (!warning)
                {
                    warning = true;
                    string StarCore = this.GetLocalization("Chat.StarCore").Value;
                    CombatText.NewText(NPC.getRect(), Color.Lime, StarCore, true);
                }
                Lighting.AddLight(NPC.Center, Warning.R / 150, Warning.G / 150,  Warning.B / 150);
            }
            else
            {
                Lighting.AddLight(NPC.Center, Color.LimeGreen.R / 150, Color.LimeGreen.G / 150, Color.LimeGreen.B / 150);
            }


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
                    NPC.rotation += .09f;
                }
                else if (NPC.velocity.X < 0)
                {
                    NPC.rotation -= .09f;
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
                        if (NPC.life < (int)(NPC.lifeMax * .5f))
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (NPC.ai[2] % 30 == 0)
                                {
                                    double offsetAngle = startAngle + (deltaAngle * i);
                                    if (NPC.life < (int)(NPC.lifeMax * .4f) && Main.rand.Next(2) == 0)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigStarProj>(), NPC.damage / 4, 5, Main.myPlayer);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<MiniStar>(), NPC.damage / 4, 5, Main.myPlayer);
                                    }
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
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<MiniStar>(), NPC.damage / 4, 12, Main.myPlayer);
                            }
                            NPC.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
                    case 1:
                        float spread1 = 12f * 0.0174f;
                        double startAngle1 = Math.Atan2(NPC.velocity.X, NPC.velocity.Y) - spread1 / 2;
                        double deltaAngle1 = spread1 / 20f;
                        for (int i = 0; i < 10; i++)
                        {
                            double offsetAngle = startAngle1 + deltaAngle1 * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 10f), (float)(Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<Starstatic>(), NPC.damage / 4, 5);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 10f), (float)(-Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<Starstatic>(), NPC.damage / 4, 5);
                        }
                        NPC.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 2:
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(7, 7), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-7, 7), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(7, -7), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-7, -7), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(9, 0), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-9, 0), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -9), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 9), Mod.Find<ModProjectile>("Starsphere").Type, NPC.damage / 4, 0f, Main.myPlayer, 0, NPC.whoAmI);

                        NPC.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 3:
                        if (NPC.ai[2] % 20 == 0)
                        {
                            BaseAI.FireProjectile(player.position, NPC, ModContent.ProjectileType<Starblast>(), NPC.damage / 4, 5, 12, 0, Main.myPlayer);
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
                    BaseAI.ShootPeriodic(NPC, player.position, player.width, player.height, ModContent.ProjectileType<Starshot>(), ref NPC.ai[3], Main.rand.Next(30, 50), NPC.damage / 4, 10, true);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2D13 = TextureAssets.Npc[NPC.type].Value;
            Texture2D BladeTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Starcore/StarcoreBack").Value;
            Texture2D GlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/Starcore_Glow").Value;
            Texture2D BladeGlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/StarcoreBack_Glow").Value;
            Texture2D Warning = ModContent.Request<Texture2D>("CSkies/Glowmasks/StarcoreWarning").Value;
            Texture2D WarningBack = ModContent.Request<Texture2D>("CSkies/Glowmasks/StarcoreBackWarning").Value;

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), Color.White * 0.8f, true);

            if (NPC.life < NPC.lifeMax / 4)
            {
                BaseDrawing.DrawTexture(spriteBatch, WarningBack, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, 0, 1, new Rectangle(0, 0, WarningBack.Width, WarningBack.Height), Color.White * 0.8f, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, GlowTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Color.White * 0.8f, true);

            if (NPC.life < NPC.lifeMax / 4)
            {
                BaseDrawing.DrawTexture(spriteBatch, Warning, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Color.White * 0.8f, true);
            }
            return false;
        }
    }
}