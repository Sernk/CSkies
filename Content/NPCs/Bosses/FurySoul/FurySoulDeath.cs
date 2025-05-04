using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.FurySoul
{
    public class FurySoulDeath : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fury's End");
            Main.npcFrameCount[NPC.type] = 18;
        }

        public override void SetDefaults()
        {
            NPC.width = 230;
            NPC.height = 142;
            NPC.friendly = false;
            NPC.lifeMax = 1;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.timeLeft = 10;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        public override void AI()
        {
            if (scale > 0)
            {
                scale *= .96f;
            }
            if (++NPC.ai[0] >= 240)
            {
                NPC.active = false;
            }
            if (NPC.frame.Y == 18)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 12);
                Main.projectile[p].Center = NPC.Center;
            }
            if (NPC.frame.Y > 18)
            {
                NPC.alpha = 255;
            }
            NPC.velocity *= 0;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ >= 5)
            {
                NPC.frameCounter = 0;

                NPC.frame.Y += frameHeight;

                if (NPC.ai[0] < 120 && NPC.frame.Y > frameHeight * 6)
                {
                    NPC.frame.Y = frameHeight * 6;
                }
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;
        public float scale = 1;

        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Texture2D texture2D13 = TextureAssets.Npc[NPC.type].Value;
            Texture2D RingTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/FurySoul/FuryRing").Value;

            Texture2D RingTex1 = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ring1").Value;
            Texture2D RingTex2 = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ring2").Value;
            Texture2D RitualTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Heartcore/Ritual").Value;

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RitualTex, r, NPC.position, NPC.width, NPC.height, scale, -NPC.rotation, 0, 1, new Rectangle(0, 0, RitualTex.Width, RitualTex.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex1, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex2, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex, r, NPC.position, NPC.width, NPC.height, scale, NPC.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            }

            BaseDrawing.DrawAura(sb, texture2D13, 0, NPC.position, NPC.width, NPC.height, auraPercent, 1.5f, 1f, 0, NPC.direction, 1, NPC.frame, 0f, 0f, NPC.GetAlpha(Color.White));

            return false;
        }
    }
}