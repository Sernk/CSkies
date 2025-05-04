using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Dusts;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Utilities;

namespace CSkies.Content.NPCs.Bosses.Novacore
{
    [AutoloadBossHead]
    public class Novacore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 200;
            NPC.width = 46;
            NPC.height = 46;
            NPC.aiStyle = -1;
            NPC.damage = 130;
            NPC.defense = 50;
            NPC.lifeMax = 1000000;
            NPC.value = Item.sellPrice(0, 12, 0, 0);
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.boss = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Novacore1");
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * bossAdjustment);
            NPC.damage = (int)(NPC.damage * 0.8f);
            NPC.defense = 100;
        }

        public float Shoot = 0;
        public float[] internalAI = new float[4];
        public bool LaserShot = false;
		public float OrbitterDist = 0;
		public float closestPlayer = 0;
        public bool Phase2 = false;
        public int PhaseTimer = 0;
        public bool MeleeMode = false;

		public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Shoot);

                writer.Write(internalAI[0]); //Projectile Attack Choice
                writer.Write(internalAI[1]); //Melee Attack Choice
                writer.Write(internalAI[2]); //Melee Attack Repeats
                writer.Write(internalAI[3]); //Melee Attack Timer

                writer.Write(LaserShot);
                writer.Write(OrbitterDist);
				writer.Write(closestPlayer);

                writer.Write(Phase2);
                writer.Write(PhaseTimer);

                writer.Write(MeleeMode); //If performing a melee attack
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Shoot = reader.ReadSingle();

                internalAI[0] = reader.ReadSingle();
                internalAI[1] = reader.ReadSingle();
                internalAI[2] = reader.ReadSingle();
                internalAI[3] = reader.ReadSingle();

                LaserShot = reader.ReadBoolean(); 
                OrbitterDist = reader.ReadSingle();
                closestPlayer = reader.ReadSingle();

                Phase2 = reader.ReadBoolean();
                PhaseTimer = reader.ReadInt32();

                MeleeMode = reader.ReadBoolean();
            }
        }

        readonly int AIRate = (Main.expertMode ? 150 : 220);
        readonly float Pi2 = (float)Math.PI * 2;
        readonly bool AnyOrbitters = CUtils.AnyProjectiles(ModContent.ProjectileType<NovaTurretProj>()) || CUtils.AnyProjectiles(ModContent.ProjectileType<NovaTurret>());

        public static Color Warning => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Purple, Color.Red, Color.Purple);

        bool WarningText = false;

        public override void AI()
        {
          
            Player player = Main.player[NPC.target];

            Vector2 targetPos;

            if (!NPC.HasPlayerTarget)
            {
                NPC.TargetClosest();
            }

            int speed = 16;
            float interval = .025f;

            if (NPC.life < NPC.lifeMax / 2)
            {
                if (!Phase2)
                {
                    NPC.velocity *= .92f;

                    PhaseTimer++;

                    if (PhaseTimer == 120)
                    {
                        CombatText.NewText(NPC.Hitbox, Color.Purple, "Target threat level underestimated. Engaging melee protocols.", true);
                    }
                    if (PhaseTimer >= 180)
                    {
                        Phase2 = true;
                        internalAI[0] = 6;
                    }

                    return;
                }
                speed = 18;
                interval = .03f;
                //music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Novacore2");
            }

            if (NPC.life < NPC.lifeMax / 4)
            {
                Lighting.AddLight(NPC.Center, Warning.R / 150, Warning.G / 150, Warning.B / 150);
                if (!WarningText)
                {
                    CombatText.NewText(NPC.Hitbox, Color.Red, "ERROR. DRASTIC PHYSICAL DAMAGE TO CHASSIS! ASTRAL ENERGY LEAKS IMMINENT!", true);
                    WarningText = true;
                }
                //music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Pinch");
                speed = 20;
                interval = .03f;

                if (Main.rand.Next(30) == 0)
                {
                    Lightning();
                }
            }
            else
            {
                Lighting.AddLight(NPC.Center, Color.Purple.R / 150, Color.Purple.G / 150, Color.Purple.B / 150);
            }


            if (!MeleeMode)
            {
                BaseAI.AISkull(NPC, ref NPC.ai, true, speed, 280, interval, .05f);
            }

            OrbitterDist += 2f;
            if (OrbitterDist > 300)
            {
                OrbitterDist = 300;
            }

            CWorld.NovacoreCounter = NPC.ai[2];

            if (MeleeMode)
            {
                internalAI[3]++;
                switch (internalAI[1])
                {
                    #region Diagonal Dashes
                    case 0:
                        if (!AliveCheck(player))
                            break;
                        targetPos = player.Center;
                        targetPos.X += 430 * (NPC.Center.X < targetPos.X ? -1 : 1);
                        targetPos.Y -= 430;
                        Movement(targetPos, .7f);
                        if (internalAI[3] > 180 || Math.Abs(NPC.Center.Y - targetPos.Y) < 100) //initiate dash
                        {
                            internalAI[1]++;
                            internalAI[3] = 0;
                            NPC.netUpdate = true;
                            NPC.velocity = NPC.DirectionTo(player.Center) * 45;
                        }
                        break;

                    case 1: //dashing
                        if (NPC.Center.Y > player.Center.Y + 500 || Math.Abs(NPC.Center.X - player.Center.X) > 1000)
                        {
                            NPC.velocity.Y *= 0.5f;
                            internalAI[3] = 0;
                            if (++internalAI[2] >= Repeats() - 1) //repeat three times
                            {
                                internalAI[1] = 0;
                                internalAI[2] = 0;
                                internalAI[3] = 0;
                                MeleeMode = false;
                            }
                            else
                                internalAI[1]--;
                            NPC.netUpdate = true;
                        }
                        break;
                    #endregion

                    #region Horizontal Dash
                    case 2: //prepare for queen bee dashes
                        if (!AliveCheck(player))
                            break;

                        if (internalAI[3] > 60)
                        {
                            targetPos = player.Center;
                            targetPos.X += 400 * (NPC.Center.X < targetPos.X ? -1 : 1);
                            Movement(targetPos, 1f);
                            if (internalAI[3] > 180 || Math.Abs(NPC.Center.Y - targetPos.Y) < 40) //initiate dash
                            {
                                internalAI[1]++;
                                internalAI[3] = 0;
                                NPC.netUpdate = true;
                                NPC.velocity.X = -40 * (NPC.Center.X < player.Center.X ? -1 : 1);
                                NPC.velocity.Y *= 0.1f;
                            }
                        }
                        else
                        {
                            NPC.velocity *= 0.9f; //decelerate briefly
                        }
                        break;

                    case 3:

                        if (internalAI[3] > 240 || (Math.Sign(NPC.velocity.X) > 0 ? NPC.Center.X > player.Center.X + 900 : NPC.Center.X < player.Center.X - 900))
                        {
                            internalAI[3] = 0;

                            if (++internalAI[2] >= Repeats()) //repeat dash three times
                            {
                                internalAI[1] = 0;
                                internalAI[2] = 0;
                                internalAI[3] = 0;
                                MeleeMode = false;
                            }
                            else
                                internalAI[1]--;
                            NPC.netUpdate = true;
                        }
                        break;
                    default:
                        internalAI[1] = 0;
                        goto case 0;
                        #endregion
                }
            }
            else
            {
                if (NPC.ai[2]++ > AIRate)
                {
                    if (internalAI[0] != 5)
                    {
                        if (NPC.velocity.X >= 0)
                        {
                            NPC.rotation += .06f;
                            NPC.spriteDirection = 1;
                        }
                        else if (NPC.velocity.X < 0)
                        {
                            NPC.rotation -= .06f;
                            NPC.spriteDirection = -1;
                        }
                    }
                    else
                    {
                        NPC.rotation += .03f;
                    }
                    switch (internalAI[0])
                    {
                        case 0:
                        case 1:
                        case 2:

                        #region Lightning
                        case 3:
                            if (!AliveCheck(Main.player[NPC.target]))
                                break;

                            if (NPC.localAI[0] == 0f)
                            {
                                NPC.localAI[0] = 1f;
                                int num833 = Player.FindClosest(NPC.Center, 0, 0);
                                Vector2 vector62 = Main.player[num833].Center - NPC.Center;
                                if (vector62 == Vector2.Zero)
                                {
                                    vector62 = Vector2.UnitY;
                                }
                                closestPlayer = vector62.ToRotation();
                                NPC.netUpdate = true;
                            }

                            if (NPC.ai[2] % 45 == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                                Vector2 vector71 = closestPlayer.ToRotationVector2();
                                Vector2 value56 = vector71.RotatedBy(1.5707963705062866) * (Main.rand.Next(2) == 0).ToDirectionInt() * Main.rand.Next(10, 21);
                                vector71 *= Main.rand.Next(-80, 81);
                                Vector2 velocity3 = (vector71 - value56) / 10f;
                                Dust dust25 = Main.dust[Dust.NewDust(NPC.Center, 0, 0, ModContent.DustType<CDust>())];
                                dust25.noGravity = true;
                                dust25.position = NPC.Center + value56;
                                dust25.velocity = velocity3;
                                dust25.scale = 0.5f + Main.rand.NextFloat();
                                dust25.fadeIn = 0.5f;
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    for (int a = 0; a < Repeats() + 1; a++)
                                    {
                                        Vector2 vector72 = closestPlayer.ToRotationVector2() * 8f;
                                        float ai2 = Main.rand.Next(80);
                                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X - vector72.X, NPC.Center.Y - vector72.Y, vector72.X, vector72.Y, ModContent.ProjectileType<Novashock>(), 15, 1f, Main.myPlayer, closestPlayer, ai2);
                                    }
                                }
                                NPC.localAI[0] = 0;
                            }

                            if (NPC.ai[2] > AIRate + 140 && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                AIChange();
                            }
                            break;
                        #endregion

                        #region Turrets
                        case 4:
                            if (!AliveCheck(Main.player[NPC.target]))
                                break;
                            if (!AnyOrbitters)
                            {
                                OrbitterDist = 0;
                                for (int m = 0; m < TurretCount(); m++)
                                {
                                    int projectileID = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<NovaTurretProj>(), NPC.damage, 4, Main.myPlayer);
                                    Main.projectile[projectileID].Center = NPC.Center;
                                    Main.projectile[projectileID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                                    Main.projectile[projectileID].velocity *= 8f;
                                    Main.projectile[projectileID].ai[0] = m;
                                }
                            }
                            AIChange();
                            break;
                        #endregion

                        #region Lasers
                        case 5: //Nova Beams
                            if (!AliveCheck(Main.player[NPC.target]))
                                break;

                            NPC.velocity *= .94f;

                            LaserAttack();

                            if (!CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeam>()) || !CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeamSmall>()) && !LaserShot)
                            {
                                AIChange();
                            }
                            break;
                            #endregion
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
                        if (Shoot > 30)
                        {
                            if (Shoot % 10 == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 spinningpoint2 = 0 * Vector2.UnitX;
                                    spinningpoint2 = spinningpoint2.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.78539818525314331);
                                    spinningpoint2 *= 8f;
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, spinningpoint2.X, spinningpoint2.Y, ModContent.ProjectileType<NovaRocket>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, 20f);
                                    if (Shoot > 60)
                                    {
                                        Shoot = 0;
                                        NPC.netUpdate = true;
                                    }
                                }
                                SoundEngine.PlaySound(SoundID.Item39, NPC.Center);
                            }
                        }
                    }
                }
            }
        }

        private void Movement(Vector2 targetPos, float speedModifier)
        {
            if (NPC.Center.X < targetPos.X)
            {
                NPC.velocity.X += speedModifier;
                if (NPC.velocity.X < 0)
                    NPC.velocity.X += speedModifier * 2;
            }
            else
            {
                NPC.velocity.X -= speedModifier;
                if (NPC.velocity.X > 0)
                    NPC.velocity.X -= speedModifier * 2;
            }
            if (NPC.Center.Y < targetPos.Y)
            {
                NPC.velocity.Y += speedModifier;
                if (NPC.velocity.Y < 0)
                    NPC.velocity.Y += speedModifier * 2;
            }
            else
            {
                NPC.velocity.Y -= speedModifier;
                if (NPC.velocity.Y > 0)
                    NPC.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(NPC.velocity.X) > 30)
                NPC.velocity.X = 30 * Math.Sign(NPC.velocity.X);
            if (Math.Abs(NPC.velocity.Y) > 30)
                NPC.velocity.Y = 30 * Math.Sign(NPC.velocity.Y);
        }

        private void AIChange()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[2] = 0;
                LaserShot = false;
                NPC.localAI[0] = 0;

                if (NPC.life < NPC.lifeMax / 2 && Main.rand.Next(3) == 0)
                {
                    MeleeMode = true;
                    internalAI[1] = (Main.rand.Next(2) == 0 ? 0 : 2);
                }

                int Selection = Main.rand.Next(6);
                internalAI[0] = Selection;

                CWorld.NovacoreAI = internalAI[0];

                NPC.netUpdate = true;
            }
        }

        private int Repeats()
        {
            if (NPC.life < (int)(NPC.lifeMax * .25f))
            {
                return 4;
            }
            else if (NPC.life < (int)(NPC.lifeMax * .5f))
            {
                return 3;
            }
            else if (NPC.life < (int)(NPC.lifeMax * .75f))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public int TurretCount()
        {
            if (NPC.life < (int)(NPC.lifeMax * .25f))
            {
                return 8;
            }
            else if (NPC.life < (int)(NPC.lifeMax * .5f))
            {
                return 7;
            }
            else if (NPC.life < (int)(NPC.lifeMax * .75f))
            {
                return 6;
            }
            else
            {
                return 5;
            }
        }

        private bool AliveCheck(Player player)
        {
            if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 10000)
            {
                NPC.TargetClosest();

                if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 10000)
                {
                    NPC.alpha += 3;
                    if (NPC.alpha > 255)
                    {
                        NPC.active = false;
                        NPC.netUpdate = true;
                    }
                }
            }
            else
            {
                if (NPC.alpha > 0)
                {
                    for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                    {
                        int dust = ModContent.DustType<VoidDust>();
                        int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, dust, 0f, 0f, 100, default, 2f);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].noLight = true;
                    }
                    NPC.alpha -= 4;
                }
                else
                {
                    NPC.alpha = 0;
                }
            }
            return true;
        }

        public void Lightning()
        {
            if (NPC.localAI[0] == 0f)
            {
                NPC.localAI[0] = 1f;
                int num833 = Player.FindClosest(NPC.Center, 0, 0);
                Vector2 vector62 = Main.player[num833].Center - NPC.Center;
                if (vector62 == Vector2.Zero)
                {
                    vector62 = Vector2.UnitY;
                }
                closestPlayer = vector62.ToRotation();
                NPC.netUpdate = true;
            }

            SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            Vector2 vector71 = closestPlayer.ToRotationVector2();
            Vector2 value56 = vector71.RotatedBy(1.5707963705062866) * (Main.rand.Next(2) == 0).ToDirectionInt() * Main.rand.Next(10, 21);
            vector71 *= Main.rand.Next(-80, 81);
            Vector2 velocity3 = (vector71 - value56) / 10f;
            Dust dust25 = Main.dust[Dust.NewDust(NPC.Center, 0, 0, ModContent.DustType<Dusts.CDust>())];
            dust25.noGravity = true;
            dust25.position = NPC.Center + value56;
            dust25.velocity = velocity3;
            dust25.scale = 0.5f + Main.rand.NextFloat();
            dust25.fadeIn = 0.5f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 vector72 = closestPlayer.ToRotationVector2() * 8f;
                    float ai2 = Main.rand.Next(80);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X - vector72.X, NPC.Center.Y - vector72.Y, vector72.X, vector72.Y, ProjectileID.VortexLightning, 15, 1f, Main.myPlayer, closestPlayer, ai2);
                }
            }
            NPC.localAI[0] = 0;
        }

        float rotAmt;

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
            if ((!CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeam>()) || !CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeamSmall>())) && !LaserShot)
            {
                LaserShot = true;
                for (int l = 0; l < LaserCount; l++)
                {
                    float LaserPos = Pi2 / LaserCount;
                    float laserDir = LaserPos * l; // FlameraySmall
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (NPC.rotation + laserDir).ToRotationVector2(), ModContent.ProjectileType<NovaBeamSmall>(), NPC.damage / 4, 0f, Main.myPlayer, laserDir, NPC.whoAmI);
                }
            }
        }

        public override void OnKill()
        {
            /*Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore1"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore2"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore3"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore4"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGoreHalf1"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGoreHalf2"), 1f);

            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.SolarFlare, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }

            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("HeartcoreTrophy"));
            }
            if (Main.rand.Next(7) == 0)
            {
                npc.DropLoot(mod.ItemType("HeartcoreMask"));
            }

            if (Main.expertMode)
            {
                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<FurySoulTransition>());
                Main.npc[n].Center = npc.Center;
                return;
            }
            else
            {
                string[] lootTableA = { "Sol", "MeteorShower", "BlazeBuster", "FlamingSoul" };
                int lootA = Main.rand.Next(lootTableA.Length);

                npc.DropLoot(mod.ItemType(lootTableA[lootA]));

                npc.DropLoot(ModContent.ItemType<Items.Boss.Heartcore.HeartSoul>(), Main.rand.Next(8, 12));
                //int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HeartcoreDefeat>());
                //Main.npc[n].Center = npc.Center;
                CWorld.downedHeartcore = true;
            }*/
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 5)
            {
                NPC.frameCounter = 0;
                Frame++;
                if (Frame > 7)
                {
                    Frame = 0;
                }
            }
            NPC.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D BladeTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Novacore/NovacoreBack").Value;
            Texture2D BladeGlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/NovacoreBack_Glow").Value;
            Texture2D BladeWarning = ModContent.Request<Texture2D>("CSkies/Glowmasks/NovacoreBack_Glow").Value;

            Texture2D texture2D13 = TextureAssets.Npc[NPC.type].Value;
            Texture2D Glow = ModContent.Request<Texture2D>("CSkies/Glowmasks/Novacore_Glow").Value;
            Texture2D Warning = ModContent.Request<Texture2D>("CSkies/Glowmasks/Novacore_Warning").Value;

            Rectangle Bladeframe = BaseDrawing.GetFrame(Frame, BladeTex.Width, BladeTex.Height / 8, 0, 0);

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.spriteDirection, 8, Bladeframe, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, 0, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.spriteDirection, 8, Bladeframe, Color.White, true);

            if (NPC.life < NPC.lifeMax / 4)
            {
                BaseDrawing.DrawTexture(spriteBatch, BladeWarning, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 8, Bladeframe, Color.Red, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 8, NPC.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, Glow, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 8, NPC.frame, Color.White * 0.8f, true);

            if (NPC.life < NPC.lifeMax / 4)
            {
                BaseDrawing.DrawTexture(spriteBatch, Warning, 0, NPC.position, NPC.width, NPC.height, NPC.scale, 0, 0, 8, Bladeframe, Color.Red, true);
            }

            return false;
        }
    }
}
