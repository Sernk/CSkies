using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Content.Buffs;

namespace CSkies.Content.Projectiles.Minions
{
    public class HeartRune : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Heart Rune");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
    	
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 28;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
        }

        public float maxDistToAttack = 360f;
        public Entity target = null;

        public override void AI()
        {
            Projectile.rotation += .1f;
            if (scale < 1)
            {
                scale += .05f;
            }
            else
            {
                scale = 1;
            }
            Player player = Main.player[Projectile.owner];
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
            if (player.dead || !player.HasBuff(ModContent.BuffType<Rune>()))
            {
                Projectile.Kill();
                modPlayer.Rune = false;
            }
            if (modPlayer.Rune)
            {
                Projectile.timeLeft = 2;
            }

            Target();
            BaseAI.AIMinionFlier(Projectile, ref Projectile.ai, player, false, false, false, 70, 70, 400, 800, .2f, 6f, 6f, !CanShoot(target), false, (proj, owner) => { return (target == player ? null : target); }, Shoot);
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
                            int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Heart.FirePro>(), Projectile.damage, 0f, Projectile.owner);
                            Main.projectile[projID].minion = true;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].netUpdate = true;
                        }
                        else
                        {
                            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(8, 8), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-8, 8), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(8, -8), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-8, -8), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(8, 0), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-8, 0), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -8), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 8), ModContent.ProjectileType<RuneWave>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.whoAmI);
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

        float scale = 0;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D RingTex = ModContent.Request<Texture2D>("CSkies/Content/Projectiles/Minions/HeartRune_Ring").Value;

            BaseDrawing.DrawTexture(sb, texture2D13, 0, Projectile.position, Projectile.width, Projectile.height, 1f, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Projectile.GetAlpha(Color.White), true);

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RingTex, r, Projectile.position, Projectile.width, Projectile.height, scale, Projectile.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            }

            return false;
        }
    }
}