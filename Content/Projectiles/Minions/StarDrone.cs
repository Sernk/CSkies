using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Projectiles.Minions
{
    public class StarDrone : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.aiStyle = -1;
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Star Drone");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;
        }

        public float maxDistToAttack = 360f;
        public Entity target = null;

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }

            Player player = Main.player[Projectile.owner];
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
            if (player.dead)
            {
                modPlayer.Drone = false;
            }
            if (modPlayer.Drone)
            {
                Projectile.timeLeft = 2;
            }

            Target();

            BaseAI.AIMinionFlier(Projectile, ref Projectile.ai, player, false, false, false, 40, 40, 400, 800, .15f, 4.5f, 5f, !CanShoot(target), false, (proj, owner) => { return (target == player ? null : target); }, Shoot);
            Projectile.position -= (player.oldPosition - player.position);
        }

        public bool CanShoot(Entity target)
        {
            return target != null && target is NPC && BaseUtility.CanHit(Projectile.Hitbox, new Rectangle((int)target.Center.X, (int)target.Center.Y, 1, 1)) && Vector2.Distance(Projectile.Center, target.Center) < 350;
        }

        public bool Shoot(Entity proj, Entity owner, Entity target)
        {
            if (CanShoot(target))
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.localAI[0]--;
                    if (Projectile.localAI[0] <= 0)
                    {
                        Projectile.localAI[0] = 30;
                        if (Main.rand.Next(4) != 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                            Vector2 velocity = BaseUtility.RotateVector(default, new Vector2(5f, 0f), BaseUtility.RotationTo(Projectile.Center, target.Center));
                            int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<MinionStar>(), Projectile.damage, 0f, Projectile.owner);
                            Main.projectile[projID].minion = true;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].netUpdate = true;
                        }
                        else
                        {
                            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(8, 8), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-8, 8), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(8, -8), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-8, -8), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(8, 0), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-8, 0), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -8), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 8), ModContent.ProjectileType<Static>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                        }
                    }
                }
                return true;
            }
            Projectile.localAI[0] = 0;
            return false;
        }

        public void Target()
        {
            Vector2 startPos = Main.player[Projectile.owner].Center;
            if (target != null && target != Main.player[Projectile.owner] && !CanTarget(target, startPos))
            {
                target = null;
            }
            if (target == null || target == Main.player[Projectile.owner])
            {
                int[] npcs = BaseAI.GetNPCs(startPos, -1, default, maxDistToAttack);
                float prevDist = maxDistToAttack;
                foreach (int i in npcs)
                {
                    NPC npc = Main.npc[i];
                    float dist = Vector2.Distance(startPos, npc.Center);
                    if (CanTarget(npc, startPos) && dist < prevDist) { target = npc; prevDist = dist; }
                }
            }
            if (target == null) { target = Main.player[Projectile.owner]; }
        }

        public bool CanTarget(Entity codable, Vector2 startPos)
        {
            if (codable is NPC npc)
            {
                return npc.active && npc.life > 0 && !npc.friendly && !npc.dontTakeDamage && npc.lifeMax > 5 && Vector2.Distance(startPos, npc.Center) < maxDistToAttack && BaseUtility.CanHit(Projectile.Hitbox, npc.Hitbox);
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D Tex = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D Glow = ModContent.Request<Texture2D>("CSkies/Glowmasks/StarDrone_Glow").Value;

            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 4, 0, 0);
            BaseDrawing.DrawTexture(sb, Tex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, Projectile.direction, 4, frame, lightColor, true);
            BaseDrawing.DrawTexture(sb, Glow, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, Projectile.direction, 4, frame, Color.White, true);
            return false;
        }
    }
}