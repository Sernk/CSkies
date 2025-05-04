using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Boss.Void;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace CSkies.Content.NPCs.Bosses.Void
{
    [AutoloadBossHead]
    public class Void : ModNPC,ILocalizedModType
    {
        public string LocalizationCategory => "VoidNPC"; 

        public override void Load()
        {
            string VOIDSAY = this.GetLocalization("Chat.VOIDSAY").Value; // VOID's form begins to destabilize
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("VOID");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 198;
            NPC.height = 138;
            NPC.value = BaseUtility.CalcValue(0, 10, 0, 0);
            NPC.npcSlots = 1000;
            NPC.aiStyle = -1;
            NPC.lifeMax = 150000;
            NPC.defense = 70;
            NPC.damage = 130;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath51;
            NPC.boss = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Void");
            //bossBag = ModContent.ItemType<ObserverVoidBag>();
            NPC.dontTakeDamage = true;
            NPC.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * bossAdjustment);
            NPC.defense = (int)(NPC.defense * 1.2f);
            NPC.damage = (int)(NPC.damage * .8f);
        }

        float VortexScale = 0f;
        float VortexRotation = 0f;

        public float[] Vortex = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Vortex[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Vortex[0] = reader.ReadFloat();
            }
        }

        bool rage = false;

        readonly float Pi2 = (float)Math.PI * 2;

        public override void AI()
        {
            if (NPC.life < NPC.lifeMax / 4)
            {
                if (!rage)
                {
                    rage = true;
                    string VOIDSAY = this.GetLocalization("Chat.VOIDSAY").Value;
                    Main.NewText(VOIDSAY, Color.LightCyan);
                }
                Music = MusicLoader.GetMusicSlot("CSkies/Sounds/Music/Pinch");
            }

            Lighting.AddLight(NPC.Center, 0, .1f, .3f);
            isCharging = false;
            Player player = Main.player[NPC.target];
            Vector2 targetPos;

            NPC.TargetClosest();
            SuckPlayer();

            switch ((int)NPC.ai[0])
            {
                case 0: //fly to corner for dash

                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 430 * (NPC.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 430;
                    Movement(targetPos, .7f);
                    if (++NPC.ai[1] > 180 || Math.Abs(NPC.Center.Y - targetPos.Y) < 100) //initiate dash
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                        NPC.velocity = NPC.DirectionTo(player.Center) * 45;
                    }
                    break;

                case 1: //dashing
                    isCharging = true;
                    if (NPC.Center.Y > player.Center.Y + 500 || Math.Abs(NPC.Center.X - player.Center.X) > 1000)
                    {
                        NPC.velocity.Y *= 0.5f;
                        NPC.ai[1] = 0;
                        if (++NPC.ai[2] >= Repeats() - 1) //repeat three times
                        {
                            NPC.ai[0]++;
                            NPC.ai[2] = 0;
                        }
                        else
                            NPC.ai[0]--;
                        NPC.netUpdate = true;
                    }
                    break;
                case 2:
                    NPC.velocity *= 0;
                    if (NPC.ai[1] < 90)
                    {
                        if (NPC.ai[3] < Repeats() - 1)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient) { NPC.ai[2]++; }
                            int teleportRate = NPC.life < NPC.lifeMax / 4 ? 15 : 30;
                            if (NPC.ai[2] >= teleportRate) // + lasers
                            {
                                Teleport();
                                Starblast();
                                NPC.ai[3] += 1;
                                NPC.ai[2] = 0;
                                NPC.netUpdate = true;
                            }
                            break;
                        }
                        else
                        {
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.ai[3] = 0;
                        }
                    }
                    break;

                case 3: //prepare for queen bee dashes
                    if (!AliveCheck(player))
                        break;

                    if (++NPC.ai[1] > 60)
                    {
                        targetPos = player.Center;
                        targetPos.X += 400 * (NPC.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 1f);
                        if (NPC.ai[1] > 180 || Math.Abs(NPC.Center.Y - targetPos.Y) < 40) //initiate dash
                        {
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
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

                case 4:
                    isCharging = true;

                    if (++NPC.ai[1] > 240 || (Math.Sign(NPC.velocity.X) > 0 ? NPC.Center.X > player.Center.X + 900 : NPC.Center.X < player.Center.X - 900))
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        if (++NPC.ai[3] >= 3) //repeat dash three times
                        {
                            Teleport();
                            NPC.ai[0]++;
                            NPC.ai[3] = 0;
                        }
                        else
                            NPC.ai[0]--;
                        NPC.netUpdate = true;
                    }
                    
                    if (NPC.ai[1] % 20 == 0)
                    {
                        int a = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<VoidVortex>(), NPC.damage / 2, 3);
                        Main.projectile[a].Center = NPC.Center;
                    }
                    if (NPC.ai[1] % 30 == 0)
                    {
                        if (NPC.life < NPC.lifeMax / 3)
                        {
                            Starblast();
                        }
                    }
                    break;

                case 5: //Prep Deathray
                    if (!AliveCheck(Main.player[NPC.target]))
                        break;

                    NPC.velocity *= 0;

                    if (!CUtils.AnyProjectiles(ModContent.ProjectileType<VoidraySmall>()))
                    {
                        float dir = Pi2 / 8;

                        if (Main.rand.NextBool() || NPC.life < NPC.lifeMax / 2)
                        {
                            dir = 0;
                        }

                        int loops = (NPC.life < NPC.lifeMax / 2 ? 8 : 4);

                        for (int l = 0; l < loops; l++)
                        {
                            float LaserPos = Pi2 / loops;
                            float laserDir = LaserPos * l + dir;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (NPC.rotation + laserDir).ToRotationVector2(), ModContent.ProjectileType<VoidraySmall>(), NPC.damage / 4, 0f, Main.myPlayer, laserDir, NPC.whoAmI);
                        }
                    }

                    if (++NPC.ai[2] > 60)
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            NPC.TargetClosest(false);

                        if (NPC.life < NPC.lifeMax / 2 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int choice = Main.rand.Next(2);
                            if (choice == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Item73, NPC.position);
                                int a = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, -12f), ModContent.ProjectileType<VoidShot>(), NPC.damage / 2, 3);
                                Main.projectile[a].Center = NPC.Center;
                                int b = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0f, 12f), ModContent.ProjectileType<VoidShot>(), NPC.damage / 2, 3);
                                Main.projectile[b].Center = NPC.Center;
                                int c = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-12f, 0f), ModContent.ProjectileType<VoidShot>(), NPC.damage / 2, 3);
                                Main.projectile[c].Center = NPC.Center;
                                int d = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(12f, 0f), ModContent.ProjectileType<VoidShot>(), NPC.damage / 2, 3);
                                Main.projectile[d].Center = NPC.Center;
                            }
                            else
                            {
                                int a = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[a].Center = NPC.Center + new Vector2(0, 60);
                                int b = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[b].Center = NPC.Center + new Vector2(0, -60);
                                int c = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[c].Center = NPC.Center + new Vector2(-60, 0);
                                int d = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[d].Center = NPC.Center + new Vector2(-60, 0);
                            }
                            NPC.netUpdate = true;
                        }
                    }
                    break;

                case 6: //firing mega ray

                    NPC.velocity *= 0;

                    if (++NPC.ai[1] > 180)
                    {
                        NPC.velocity *= .98f;
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.ai[3] = 0;
                        NPC.netUpdate = true;
                    }
                    break;
                

                case 7: //prepare for fishron dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center + player.DirectionTo(NPC.Center) * 600;
                    Movement(targetPos, 0.6f);
                    if (++NPC.ai[1] > 20)
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.netUpdate = true;
                        NPC.velocity = NPC.DirectionTo(player.Center) * 40;
                    }
                    break;

                case 8: //dashing
                    isCharging = true;
                    if (++NPC.ai[1] > 40)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        if (++NPC.ai[3] >= Repeats())
                        {
                            NPC.ai[0]++;
                            NPC.ai[3] = 0;
                        }
                        else
                            NPC.ai[0]--;
                        NPC.netUpdate = true;
                    }
                    break;

                case 9: //hover nearby, shoot lightning
                    if (!AliveCheck(player))
                        break;

                    NPC.velocity *= 0;

                    if (++NPC.ai[2] > 60)
                    {
                        Teleport();
                        NPC.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient) //spawn lightning
                        {
                            if (NPC.life < NPC.lifeMax / 2)
                            {
                                float dir = Pi2 / 8;

                                if (Main.rand.NextBool())
                                {
                                    dir = 0;
                                }

                                for (int l = 0; l < 4; l++)
                                {
                                    float LaserPos = Pi2 / 4;
                                    float laserDir = LaserPos * l + dir;
                                    Vector2 velocity = (NPC.rotation + laserDir).ToRotationVector2();
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<VoidShock>(), NPC.damage / 4, 0f, Main.myPlayer, velocity.ToRotation(), 0f);
                                }
                            }
                            else
                            {
                                float dir = (float)Math.PI;

                                if (Main.rand.NextBool())
                                {
                                    dir = 0;
                                }

                                for (int l = 0; l < 2; l++)
                                {
                                    float LaserPos = Pi2 / 2;
                                    float laserDir = LaserPos * l + dir;
                                    Vector2 velocity = (NPC.rotation + laserDir).ToRotationVector2();
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<VoidShock>(), NPC.damage / 4, 0f, Main.myPlayer, velocity.ToRotation(), 0f);
                                }
                            }
                        }
                    }
                    if (++NPC.ai[1] > 360)
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        NPC.ai[3] = NPC.Distance(player.Center);
                        NPC.netUpdate = true;
                    }
                    break;

                case 30:
                    if (NPC.ai[1]++ > 80)
                    {
                        NPC.ai[0] = 0;
                        NPC.dontTakeDamage = false;
                        NPC.ai[1] = 0;
                    }
                    break;

                default:
                    NPC.ai[0] = 0;
                    goto case 0;
            }

            for (int m = NPC.oldPos.Length - 1; m > 0; m--)
            {
                NPC.oldPos[m] = NPC.oldPos[m - 1];
            }
            NPC.oldPos[0] = NPC.position;

            NPC.rotation = 0;

            if (NPC.dontTakeDamage)
            {
                NPC.ai[0] = 30;
            }
        }

        public void Starblast()
        {
            SoundEngine.PlaySound(SoundID.Item73, NPC.position);

            float dir = 0;

            if (Main.rand.NextBool() || NPC.life < NPC.lifeMax / 2)
            {
                dir = (float)Math.PI / 8; ;
            }

            int loops = (NPC.life < NPC.lifeMax / 2 ? 8 : 4);

            for (int i = 0; i < loops; i++)
            {
                Vector2 shotDir = dir.ToRotationVector2();

                int a = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, shotDir * 10, ModContent.ProjectileType<VoidBlast>(), NPC.damage / 2, 0f, Main.myPlayer, 0, NPC.whoAmI);
                Main.projectile[a].Center = NPC.Center;

                dir += (float)Math.PI * 2 / loops;
            }
        }

        public void Teleport()
        {
            Player player = Main.player[NPC.target];
            Vector2 targetPos = player.Center;
            int posX = Main.rand.Next(3);
            switch (posX)
            {
                case 0:
                    posX = -400;
                    break;
                case 1:
                    posX = 0;
                    break;
                case 2:
                    posX = 400;
                    break;
            }
            int posY = Main.rand.Next(posX == 0 ? 1 : 2);
            switch (posY)
            {
                case 0:
                    posY = -400;
                    break;
                case 1:
                    posY = 0;
                    break;
            }

            NPC.position = new Vector2(targetPos.X + posX, targetPos.Y + posY);

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
                int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = NPC.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += NPC.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 25, default, 1.5f);
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
                int num90 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = NPC.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += NPC.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = NPC.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += NPC.DirectionTo(Main.dust[num92].position) * 3f;
            }

        }

        public void SuckPlayer()
        {
            bool V = NPC.ai[0] == 4;
            Player target = Main.player[Main.myPlayer];

            if (Vector2.Distance(target.Center, NPC.Center) > 4000 && !target.dead && target.active && V)
            {
                target.AddBuff(ModContent.BuffType<Buffs.Sucked>(), 2);
            }

            if (V)
            {
                if (VortexScale < 1f)
                {
                    VortexScale += .01f;
                }
                VortexRotation += .01f;
            }
            else
            {
                if (VortexScale > 0f)
                {
                    VortexScale -= .05f;
                }
                VortexRotation += .05f;
            }
        }

        private int Repeats()
        {
            if (NPC.life < (int)(NPC.life * .66f))
            {
                return 5;
            }
            else if (NPC.life < (int)(NPC.life * .33f))
            {
                return 6;
            }
            else
            {
                return 4;
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
                        int dust = ModContent.DustType<Dusts.VoidDust>();
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

        public override void OnKill()
        {
            CSystem._Void = true;
            CWorld.downedObserverV = true;
            CWorld.downedVoid = true;
            int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<VoidDeath>(), 0, 0);
            Main.npc[n].Center = NPC.Center;
            Main.npc[n].velocity = NPC.velocity;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VOIDTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ObserverVoidBag>(), 1));
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 7)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > (frameHeight * 5))
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int hitDirection = hit.HitDirection;

            //HitEffect(hitDirection, NPC.damage / 2);
            for (int Loop = 0; Loop < 3; Loop++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0);
                Main.dust[dust].velocity.Y = hitDirection * 0.1f;
                Main.dust[dust].noGravity = false;
            }
            if (NPC.life <= 0)
            {
                for (int Loop = 0; Loop < 5; Loop++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0);
                    Main.dust[dust].noGravity = false;
                }
            }
        }

        bool isCharging = false;
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            Texture2D Cyclone = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Void/VoidCyclone").Value;
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            if (VortexScale > 0)
            {
                Rectangle frame = BaseDrawing.GetFrame(0, Cyclone.Width, Cyclone.Height, 0, 0);
                BaseDrawing.DrawTexture(spriteBatch, Cyclone, 0, NPC.position, NPC.width, NPC.height, VortexScale, VortexRotation, NPC.direction, 1, frame, Color.White, true);
            }
            if (isCharging)
            {
                BaseDrawing.DrawAfterimage(spriteBatch, tex, 0, NPC, .6f, 1, 6, true, 0, 0, Color.White, NPC.frame, 4);
            }
            BaseDrawing.DrawAura(spriteBatch, tex, 0, NPC, auraPercent, 2f, 0f, 0f, NPC.GetAlpha(Color.White));
            return false;
        }
    }

}
