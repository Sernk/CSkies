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
using CSkies.Content.NPCs.Bosses.Heartcore;
using Terraria.Graphics.Effects;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Content.Buffs;
using Terraria.GameContent.ItemDropRules;
using CSkies.Content.Items.Boss.Heartcore;

namespace CSkies.Content.NPCs.Bosses.FurySoul
{
    [AutoloadBossHead]
    public class FurySoul : ModNPC, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fury Soul");
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void Load()
        {
            string FurySoulSay = this.GetLocalization("Chat.FurySoulSay").Value; // The soul begins to lash out in a fit of fiery rage.
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 100;
            NPC.width = 82;
            NPC.height = 82;
            NPC.aiStyle = -1;
            NPC.damage = 120;
            NPC.defense = 60;
            NPC.lifeMax = 150000;
            NPC.value = Item.sellPrice(0, 45, 0, 0);
            NPC.HitSound = SoundID.Item20;
            NPC.DeathSound = SoundID.NPCDeath14; 
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/FurySoul");
            //bossBag = mod.ItemType("HeartcoreBag");
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * 0.5f);
        }

        public float[] Movement = new float[2];
        public float TeleportTimer = 0;
        public bool IsTeleporting = false;
        public float[] InternalAI = new float[4];
        float scale = 0;

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Movement[0]);
                writer.Write(Movement[1]);
                writer.Write(TeleportTimer);
                writer.Write(IsTeleporting);
                writer.Write(InternalAI[0]);
                writer.Write(InternalAI[1]);
                writer.Write(InternalAI[2]);
                writer.Write(InternalAI[3]);
                writer.Write(scale);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Movement[0] = reader.ReadSingle();
                Movement[1] = reader.ReadSingle();
                TeleportTimer = reader.ReadSingle();
                IsTeleporting = reader.ReadBoolean();
                InternalAI[0] = reader.ReadSingle();
                InternalAI[1] = reader.ReadSingle();
                InternalAI[2] = reader.ReadSingle();
                InternalAI[3] = reader.ReadSingle();
                scale = reader.ReadSingle();
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FurySoulTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<HeartcoreBag>(), 1));
        }
        public override void OnKill()
        {
            CSystem._FurySoul = true;
            CWorld.downedSoul = true;
            CWorld.downedHeartcore = true;
            int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<FurySoulDeath>());
            Main.npc[n].Center = NPC.Center;
            Main.npc[n].rotation = NPC.rotation;
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.SolarFlare, -NPC.velocity.X * 0.2f,
                    -NPC.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public static Color Flame => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Orange, Color.Red, Color.Orange);

        float Changerate;

        bool rage = false;
        float rotAmt = 0;

        public override void AI()
        {
            if (NPC.life < NPC.lifeMax / 4)
            {
                if (!rage)
                {
                    rage = true;
                    string FurySoulSay = this.GetLocalization("Chat.FurySoulSay").Value;
                    Main.NewText(FurySoulSay, new Color(253, 62, 3));
                }
                Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Pinch");
            }
            
            if (NPC.life < NPC.lifeMax / 2 && NPC.CountNPCS(ModContent.NPCType<FuryMinion>()) < 2)
            {
                InternalAI[1]++;
                if (InternalAI[1] > 360)
                {
                    InternalAI[1] = 0;
                    int a = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[a].Center = NPC.Center + new Vector2(200, 0);
                    SoundEngine.PlaySound(SoundID.Item14, Main.npc[a].Center);
                    int num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[a].position, Main.npc[a].width, Main.npc[a].height, DustID.Torch, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                    int b = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[b].Center = NPC.Center + new Vector2(-200, 0);
                    SoundEngine.PlaySound(SoundID.Item14, Main.npc[b].Center);
                    num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[b].position, Main.npc[b].width, Main.npc[b].height, DustID.Torch, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                    int c = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[c].Center = NPC.Center + new Vector2(0, 200);
                    SoundEngine.PlaySound(SoundID.Item14, Main.npc[c].Center);
                    num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[c].position, Main.npc[c].width, Main.npc[c].height, DustID.Torch, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                    int d = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[d].Center = NPC.Center + new Vector2(0, -200);
                    SoundEngine.PlaySound(SoundID.Item14, Main.npc[d].Center);
                    num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[d].position, Main.npc[d].width, Main.npc[d].height, DustID.Torch, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                }
            }

            Changerate = NPC.life < NPC.lifeMax / 2 ? 150 : 120;
            Lighting.AddLight(NPC.Center, Flame.R / 150, Flame.G / 150, Flame.B / 150);

            if (!NPC.HasPlayerTarget)
            {
                NPC.TargetClosest();
            }

            if (NPC.ai[0] == 10)
            {
                NPC.velocity *= 0;
                if (InternalAI[0]++ > 60)
                {
                    AIChange();
                }
            }

            Player player = Main.player[NPC.target];

            if (player.dead || !player.active || Main.dayTime)
            {
                NPC.TargetClosest();

                NPC.velocity *= .95f;

                if (NPC.alpha > 255)
                {
                    NPC.active = false;
                    NPC.netUpdate = true;
                }
                else
                {
                    NPC.alpha += 4;
                }
                
                if (scale < 0)
                {
                    scale = 0f;
                }
                else
                {
                    scale -= .02f;
                }
                return;
            }
            else
            {
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                else
                {
                    NPC.alpha -= 4;
                }
                if (!IsTeleporting)
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
            }

            if (Vector2.Distance(player.Center, NPC.Center) < 204 && !player.dead && player.active)
            {
                player.AddBuff(ModContent.BuffType<Heartburn>(), 10);
            }

            if (!IsTeleporting)
            {
                if (NPC.ai[0] == 2 || NPC.ai[0] == 4)
                {
                    NPC.velocity *= .0f;
                }
                else
                {
                    BaseAI.AISkull(NPC, ref Movement, true, 14, 350, .04f, .05f);
                }
            }

            if (NPC.ai[2]++ > Changerate)
            {
                if (NPC.ai[0] != 2)
                {
                    NPC.rotation += .06f;
                    rotAmt = .03f;
                }

                switch (NPC.ai[0])
                {
                    case 0:
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - NPC.Center);
                        dir *= 12f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 3; i++)
                        {
                            if (NPC.ai[2] % Main.rand.Next(10) == 0 && Main.rand.Next(2) == 0)
                            {
                                double offsetAngle = startAngle + (deltaAngle * i);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<Fireshot>(), NPC.damage / 4, 5, Main.myPlayer);
                            }
                        }
                        if (NPC.ai[2] > 271)
                        {
                            AIChange();
                        }
                        break;
                    case 1:
                        if (NPC.ai[2] >= Changerate + 30)
                        {
                            if (NPC.ai[2] > 260)
                            {
                                AIChange();
                            }
                            else
                            {
                                float spread1 = 12f * 0.0174f;
                                double startAngle1 = Math.Atan2(NPC.velocity.X, NPC.velocity.Y) - spread1 / 2;
                                double deltaAngle1 = spread1 / 10f;
                                if (NPC.ai[2] % 30 == 0)
                                {
                                    SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                                    for (int i = 0; i < 10; i++)
                                    {
                                        double offsetAngle1 = (startAngle1 + deltaAngle1 * (i + i * i) / 2f) + 32f * i;
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle1) * 8f), (float)(Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), NPC.damage / 2, 6);
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle1) * 8f), (float)(-Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), NPC.damage / 2, 6);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int a = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                                Main.dust[a].noGravity = false;
                            }
                        }
                        break;
                    case 2:
                        LaserAttack();
                        break;
                    case 3:

                        if (NPC.ai[2] == Changerate + 30)
                        {
                            int loops = (NPC.life < NPC.lifeMax / 4) ? 10 : 6;
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);

                            for (int a = 0; a < loops; a++)
                            {
                                float shotDir = Pi2 / loops * a;

                                Vector2 Direction = shotDir.ToRotationVector2();

                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Direction * 12, ModContent.ProjectileType<Flamewave>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            }
                        }
                        else if(NPC.ai[2] > Changerate + 30)
                        {
                            AIChange();
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int a = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                                Main.dust[a].noGravity = true;
                            }
                        }

                        break;
                    case 4:
                        if (NPC.ai[2] <= Changerate + 20)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                int a = Dust.NewDust(NPC.Center + new Vector2(32, 32), 0, 0, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                                Main.dust[a].noGravity = true;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                int a = Dust.NewDust(NPC.Center + new Vector2(-32, 32), 0, 0, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                                Main.dust[a].noGravity = true;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                int a = Dust.NewDust(NPC.Center + new Vector2(32, -32), 0, 0, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                                Main.dust[a].noGravity = true;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                int a = Dust.NewDust(NPC.Center + new Vector2(-32, -32), 0, 0, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                                Main.dust[a].noGravity = true;
                            }
                            return;
                        }
                        if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()) && InternalAI[3] == 0)
                        {
                            InternalAI[3] += 1;
                            SetTeleportLocation();
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        }
                        if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()) && InternalAI[3] == 1)
                        {
                            FuryrangTeleport(); InternalAI[3] += 1;
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, 0), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, 0), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        }
                        if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()) && InternalAI[3] == 2)
                        {
                            FuryrangTeleport(); InternalAI[3] += 1;
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        }
                        if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()) && InternalAI[3] == 3)
                        {
                            FuryrangTeleport(); InternalAI[3] += 1;
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, 0), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, 0), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        }
                        if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()) && InternalAI[3] == 4)
                        {
                            FuryrangTeleport(); InternalAI[3] += 1;
                            SoundEngine.PlaySound(new SoundStyle("CSkies/Sounds/Sounds/FireCast"), NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(14, 0), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-14, 0), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, 14), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -14), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, 12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12, -12), ModContent.ProjectileType<Furyrang>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                        }

                        if (InternalAI[3] == 5 && !CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()))
                        {
                            AIChange();
                        }
                        break;
                    default:
                        AIChange();
                        break;
                }
            }
            else
            {
                NPC.rotation -= .03f;
                if (NPC.ai[0] != 10 && NPC.ai[2]++ < Changerate)
                {
                    int Frequency = Main.rand.Next(30, 50);
                    if (NPC.life < NPC.lifeMax / 2)
                    {
                        Frequency = Main.rand.Next(20, 50);
                    }
                    if (NPC.life < NPC.lifeMax / 4)
                    {
                        Frequency = Main.rand.Next(10, 40);
                    }
                    if (Main.rand.NextBool(2))
                    {
                        BaseAI.ShootPeriodic(NPC, player.position, player.width, player.height, ModContent.ProjectileType<BigHeartshot>(), ref NPC.ai[3], Frequency, NPC.damage / 2, 10, true);
                    }
                    else
                    {
                        BaseAI.ShootPeriodic(NPC, player.position, player.width, player.height, ModContent.ProjectileType<Meteor>(), ref NPC.ai[3], Frequency, NPC.damage / 2, 10, true);
                    }
                }
            }
        }

        private void SetTeleportLocation()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<TeleportLocation>()))
            {
                Player player = Main.player[NPC.target];
                Vector2 targetPos = player.Center;
                int posX = Main.rand.Next(5);

                int posY = 0;
                switch (posX)
                {
                    case 0:
                        posX = -300;
                        posY = 0;
                        break;
                    case 1:
                        posX = -300;
                        posY = -300;
                        break;
                    case 2:
                        posX = 0;
                        posY = -300;
                        break;
                    case 3:
                        posX = 300;
                        posY = -300;
                        break;
                    case 4:
                        posX = 300;
                        posY = 0;
                        break;
                }

                Vector2 position = new Vector2(targetPos.X + posX, targetPos.Y + posY);

                NPC.NewNPC(NPC.GetSource_FromThis(), (int)position.X, (int)position.Y, ModContent.NPCType<TeleportLocation>(), 0, NPC.whoAmI);
            }
        }

        public void Teleport()
        {
            TeleportTimer++;
            if (scale > 0)
            {
                scale -= .05f;
            }

            SetTeleportLocation();

            if (scale <= 0 || NPC.ai[0] == 4)
            {
                scale = 0;
                TeleportTimer = 0;

                TPDust();

                int Reticle = BaseAI.GetNPC(NPC.Center, ModContent.NPCType<TeleportLocation>(), -1);

                NPC.Center = Main.npc[Reticle].Center;
                Main.npc[Reticle].active = false;
                Main.npc[Reticle].netUpdate = true;

                IsTeleporting = false;
                NPC.netUpdate = true;

                TPDust();
            }
            else
            {
                IsTeleporting = true;
                return;
            }
        }

        public void FuryrangTeleport()
        {
            scale = 0;;

            TPDust();

            int Reticle = BaseAI.GetNPC(NPC.Center, ModContent.NPCType<TeleportLocation>(), -1);

            NPC.Center = Main.npc[Reticle].Center;
            Main.npc[Reticle].active = false;
            Main.npc[Reticle].netUpdate = true;

            if (InternalAI[3] != 4)
            {
                SetTeleportLocation();
            }

            IsTeleporting = false;
            NPC.netUpdate = true;

            TPDust();
        }

        private void AIChange()
        {
            NPC.velocity *= .98f;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] == 2 || NPC.ai[0] == 4)
                {
                    Teleport();
                }
                else if ((NPC.life < (int)(NPC.lifeMax * .75f)) && Main.rand.Next(3) == 0)
                {
                    Teleport();
                }
                else if ((NPC.life < NPC.lifeMax / 2) && Main.rand.Next(2) == 0)
                {
                    Teleport();
                }
                if (NPC.life < NPC.lifeMax / 4)
                {
                    Teleport();
                }
                if (!IsTeleporting)
                {
                    float oldAI = NPC.ai[0];
                    for (int i = 0; i < 5; i++) //Gives it 5 chances to choose an ai that wasn't the last one, making it rare to do the same attack again but not impossible
                    {
                        NPC.ai[0] = Main.rand.Next(5);
                        if (NPC.ai[0] != oldAI)
                        {
                            break;
                        }
                    }
                    NPC.ai[0] = Main.rand.Next(5);
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    InternalAI[2] = 0;
                    InternalAI[3] = 0;
                    rotAmt = 0;
                    NPC.netUpdate = true;
                }
            }
        }

        readonly float Pi2 = (float)Math.PI * 2;

        private void LaserAttack()
        {
            rotAmt += .0005f;
            NPC.rotation += rotAmt;
            int LaserCount;
            if (NPC.life < NPC.lifeMax / 4)
            {
                if (rotAmt > .028f)
                {
                    rotAmt = .028f;
                }
                LaserCount = 4;
            }
            else if (NPC.life < NPC.lifeMax / 2)
            {
                if (rotAmt > .024f)
                {
                    rotAmt = .024f;
                }
                LaserCount = 3;
            }
            else
            {
                if (rotAmt > .02f)
                {
                    rotAmt = .02f;
                }
                LaserCount = 2;
            }
            if ((!CUtils.AnyProjectiles(ModContent.ProjectileType<Flameray>()) || !CUtils.AnyProjectiles(ModContent.ProjectileType<FlameraySmall>())) && InternalAI[2] == 0)
            {
                for (int l = 0; l < LaserCount; l++)
                {
                    float LaserPos = Pi2 / LaserCount;
                    float laserDir = LaserPos * l;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (NPC.rotation + laserDir).ToRotationVector2(), ModContent.ProjectileType<FlameraySmall>(), NPC.damage / 4, 0f, Main.myPlayer, laserDir, NPC.whoAmI);
                }
            }
            InternalAI[2]++;
            if (InternalAI[2] > 240 && (!CUtils.AnyProjectiles(ModContent.ProjectileType<Flameray>()) || !CUtils.AnyProjectiles(ModContent.ProjectileType<FlameraySmall>())))
            {
                AIChange();
            }
        }

        public void TPDust()
        {
            Vector2 position = NPC.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = NPC.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, DustID.Torch, 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = NPC.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += NPC.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, DustID.Torch, 0f, 0f, 25, default, 1.5f);
                Main.dust[num88].position = NPC.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += NPC.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, DustID.Torch, 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = NPC.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += NPC.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, DustID.Torch, 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = NPC.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += NPC.DirectionTo(Main.dust[num92].position) * 3f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter > 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * 4)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Texture2D texture2D13 = TextureAssets.Npc[NPC.type].Value;
            Texture2D GlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/FurySoul_Glow").Value;
            Texture2D RingTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/FurySoul/FuryRing").Value;

            Texture2D RingTex1 = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ring1").Value;
            Texture2D RingTex2 = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ring2").Value;
            Texture2D RitualTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ritual").Value;

            Rectangle ring1 = new Rectangle(0, 0, RingTex.Width, RingTex.Height);
            Rectangle ring2 = new Rectangle(0, 0, RingTex2.Width, RingTex2.Height);
            Rectangle ritual = new Rectangle(0, 0, RitualTex.Width, RitualTex.Height);

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RitualTex, r, NPC.position, NPC.width, NPC.height, scale, -NPC.rotation, 0, 1, ritual, drawColor, true);
                BaseDrawing.DrawTexture(sb, RingTex1, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, ring2, drawColor, true);
                BaseDrawing.DrawTexture(sb, RingTex2, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, ring2, drawColor, true);
                BaseDrawing.DrawTexture(sb, RingTex, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, ring1, Color.White, true);
            }

            BaseDrawing.DrawTexture(sb, texture2D13, 0, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 4, NPC.frame, Color.White, true);
            BaseDrawing.DrawTexture(sb, GlowTex, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 4, NPC.frame, Color.White, true);

            return false;
        }
    }
}