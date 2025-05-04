using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using CSkies.Content.Items.Armor.Starsteel;
using CSkies.Content.NPCs.Bosses.Novacore;
using CSkies.Content.NPCs.Other;
using CSkies.Content.NPCs.Bosses.Void;
using Void = CSkies.Content.NPCs.Bosses.Void.Void;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Content.NPCs.Bosses.ObserverVoid;
using CSkies.Content.Buffs;
using CSkies.Content.Projectiles.Star;
using CSkies.Content.Projectiles.Heart;
using CSkies.Content.Projectiles.Void;

namespace CSkies.Utilities
{
    public partial class CPlayer : ModPlayer
    {
        public bool Watcher = false;
        public bool Gazer = false;
        public bool Drone = false;
        public bool Rune = false;

        public bool ZoneComet = false;
        public bool ZoneVoid = false;
        public bool ZoneObservatory = false;

        public bool VoidEye = false;
        public bool VoidCD = false;
        public bool StarShield = false;
        public bool HeartShield = false;

        public float HeartringScale = 0;
        public float HeartringRot = 0;

        public bool CometSet = false;

        public bool Heartburn = false;
        public bool Cometspark = false;

        public bool Starsteel = false;
        public int StarsteelBonus = 0;

        public override void ResetEffects()
        {
            Reset();
        }

        public override void Initialize()
        {
            Reset();
        }

        public void Reset()
        {
            Watcher = false;
            Gazer = false;
            Drone = false;
            Rune = false;

            VoidEye = false;
            VoidCD = false;

            ZoneVoid = false;
            ZoneComet = false;
            ZoneObservatory = false;

            Heartburn = false;
            Cometspark = false;

            Starsteel = false;
            StarsteelBonus = 0;
        }

        /*public override void UpdateBiomes()
        {
            ZoneVoid = NearVoid() || CWorld.AbyssTiles > 50;
            ZoneComet = CWorld.CometTiles > 30;
        }

        public override void UpdateBiomeVisuals()
        {
            bool useVoid = ZoneVoid || NearVoidBoss();
            bool useNova = NearNova() ;

            player.ManageSpecialBiomeVisuals("CSkies:AbyssSky", useVoid);
            player.ManageSpecialBiomeVisuals("CSkies:NovaSky", useNova);
        }*/

        public bool NearVoid()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<AbyssVoid>()))
            {
                int v = BaseAI.GetNPC(Player.Center, ModContent.NPCType<AbyssVoid>(), 2500);
                if (v != -1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool NearNova()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Novacore>()))
            {
                int N = BaseAI.GetNPC(Player.Center, ModContent.NPCType<Novacore>(), 2500);
                if (N != -1)
                {
                    return true;
                }
            }
            if (NPC.AnyNPCs(ModContent.NPCType<NovacoreIntro>()))
            {
                int N = BaseAI.GetNPC(Player.Center, ModContent.NPCType<NovacoreIntro>(), 2500);
                if (N != -1)
                {
                    return true;
                }
            }
            return false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.Next(10) == 0 && CometSet)
            {
                target.AddBuff(ModContent.BuffType<Cometspark>(), 300);
            }
        }
        public bool NearVoidBoss()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Void>()) || NPC.AnyNPCs(ModContent.NPCType<VoidTransition1>()) || NPC.AnyNPCs(ModContent.NPCType<VoidTransition2>()) || NPC.AnyNPCs(ModContent.NPCType<ObserverVoid>()))
            {
                int v = BaseAI.GetNPC(Player.Center, ModContent.NPCType<Void>(), 2500);
                int vt1 = BaseAI.GetNPC(Player.Center, ModContent.NPCType<VoidTransition1>(), 2500);
                int vt2 = BaseAI.GetNPC(Player.Center, ModContent.NPCType<VoidTransition2>(), 2500);
                int ov = BaseAI.GetNPC(Player.Center, ModContent.NPCType<ObserverVoid>(), 2500);
                if (v != -1 || vt1 != -1 || vt2 != -1 || ov != -1)
                {
                    return true;
                }
            }
            return false;
        }

        public int VortexScale = 0;

        public Color StarsteelColor = Color.White;

        public override void PostUpdate()
        {
            if (Starsteel)
            {
                switch (StarsteelBonus)
                {
                    case 1:
                        StarsteelColor = Color.Red;
                        break;
                    case 2:
                        StarsteelColor = Color.LimeGreen;
                        break;
                    case 3:
                        StarsteelColor = Color.Violet;
                        break;
                    case 4:
                        StarsteelColor = Color.Cyan;
                        break;
                    default: StarsteelColor = BaseDrawing.GetLightColor(Player.Center); break;

                }
            }
            if (VoidEye)
            {
                
            }
            if (HeartShield)
            {
                if (Player.statLife < Player.statLifeMax2)
                {
                    Player.moveSpeed *= 1.1f;
                    Player.GetDamage(DamageClass.Generic) += .25f;
                    Player.statDefense -= 8;

                    HeartringRot += .02f;
                    if (HeartringScale >= 1f)
                    {
                        HeartringScale = 1f;
                    }
                    else
                    {
                        HeartringScale += .02f;
                    }
                }
                else
                {
                    Player.moveSpeed *= .9f;
                    Player.statDefense += 8;
                    HeartringRot -= .02f;
                    if (HeartringScale <= 0)
                    {
                        HeartringScale = 0f;
                    }
                    else
                    {
                        HeartringScale -= .02f;
                    }
                }
            }
            else
            {
                HeartringRot -= .02f;
                if (HeartringScale <= 0)
                {
                    HeartringScale = 0f;
                }
                else
                {
                    HeartringScale -= .02f;
                }
            }

            if (Cometspark)
            {
                if (Main.rand.Next(30) == 0)
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        if (Vector2.Distance(Main.player[p].Center, Player.Center) < 80)
                        {
                            Main.player[p].AddBuff(ModContent.BuffType<Cometspark>(), 120);
                        }
                    }
                }
            }
        }

        public override void UpdateDead()
        {
            Cometspark = false;
            Heartburn = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (Cometspark)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }

                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 5;
            }
            if (Heartburn)
            {
                if (Player.statLife > Player.statLifeMax2 * .8f)
                {
                    Player.lifeRegen -= 30;
                }
                else
                {
                    Player.lifeRegen = 0;
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Cometspark)
            {
                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, DustID.Electric, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 1.5f);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (StarShield)
            {
                for (int n = 0; n < 3; n++)
                {
                    float x = Player.position.X + Main.rand.Next(-400, 400);
                    float y = Player.position.Y - Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = Player.position.X + Player.width / 2 - vector.X;
                    float num14 = Player.position.Y + Player.height / 2 - vector.Y;
                    num13 += Main.rand.Next(-100, 101);
                    int num15 = 23;
                    float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
                    num16 = num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    int num17 = Projectile.NewProjectile(Player.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<ShieldStar>(), 30, 5f, Player.whoAmI, 0f, 0f);
                    Main.projectile[num17].ai[1] = Player.position.Y;
                }
            }
            if (HeartShield)
            {
                for (int n = 0; n < 6; n++)
                {
                    float x = Player.position.X + Main.rand.Next(-400, 400);
                    float y = Player.position.Y - Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = Player.position.X + Player.width / 2 - vector.X;
                    float num14 = Player.position.Y + Player.height / 2 - vector.Y;
                    num13 += Main.rand.Next(-100, 101);
                    int num15 = 23;
                    float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
                    num16 = num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    int num17 = Projectile.NewProjectile(Player.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<Meteor0>(), 30, 5f, Player.whoAmI, 0f, 0f);
                    Main.projectile[num17].ai[1] = Player.position.Y;
                }
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Starsteel && StarsteelBonus == 1 && hit.Crit)
            {
                for (int n = 0; n < 3; n++)
                {
                    float x = Player.position.X + Main.rand.Next(-400, 400);
                    float y = Player.position.Y - Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = Player.position.X + Player.width / 2 - vector.X;
                    float num14 = Player.position.Y + Player.height / 2 - vector.Y;
                    num13 += Main.rand.Next(-100, 101);
                    int num15 = 23;
                    float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
                    num16 = num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    int num17 = Projectile.NewProjectile(Player.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<ShieldStar>(), 30, 5f, Player.whoAmI, 0, Player.position.Y);
                    Main.projectile[num17].DamageType = DamageClass.Melee;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Starsteel && hit.Crit)
            {
                for (int n = 0; n < 3; n++)
                {
                    float x = Player.position.X + Main.rand.Next(-400, 400);
                    float y = Player.position.Y - Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = Player.position.X + Player.width / 2 - vector.X;
                    float num14 = Player.position.Y + Player.height / 2 - vector.Y;
                    num13 += Main.rand.Next(-100, 101);
                    int num15 = 23;
                    float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
                    num16 = num15 / num16;
                    num13 *= num16;
                    num14 *= num16;

                    if (StarsteelBonus == 1 && proj.CountsAsClass(DamageClass.Melee) == true)
                    {
                        int num17 = Projectile.NewProjectile(proj.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<FallingStarProj>(), 30, 5f, Player.whoAmI, 0, Player.position.Y);
                        Main.projectile[num17].DamageType = DamageClass.Melee;
                    }

                    if (StarsteelBonus == 2 && proj.CountsAsClass(DamageClass.Ranged) == true)
                    {
                        int num17 = Projectile.NewProjectile(proj.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<FallingStarProj>(), 30, 5f, Player.whoAmI, 1, Player.position.Y);
                        Main.projectile[num17].DamageType = DamageClass.Ranged;
                    }

                    if (StarsteelBonus == 3 && proj.CountsAsClass(DamageClass.Magic) == true)
                    {
                        int num17 = Projectile.NewProjectile(proj.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<FallingStarProj>(), 30, 5f, Player.whoAmI, 2, Player.position.Y);
                        Main.projectile[num17].DamageType = DamageClass.Magic;
                    }
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (VoidEye)
            {
                if (CSkies.AccessoryAbilityKey.JustPressed && !Player.HasBuff(ModContent.BuffType<VECooldown>()))
                {
                    Player.AddBuff(ModContent.BuffType<VECooldown>(), 3000);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<VoidEyeVortex>(), 60, 0, Main.myPlayer);
                }
            }
        } 
    }

    public partial class CPlayer : ModPlayer
    {

    }
}