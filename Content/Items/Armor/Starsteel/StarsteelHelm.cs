using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using CSkies.Utilities;
using CSkies.Content.Items.Boss.Starcore;

namespace CSkies.Content.Items.Armor.Starsteel
{
	[AutoloadEquip(EquipType.Head)]
	public class StarsteelHelm : ModItem
	{
        public static LocalizedText SetBonusText { get; private set; }
        public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Starsteel Helmet");
			// Tooltip.SetDefault(@"Changes stats based on which Starsteel Augment is equipped");
            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs("ERROR: AUGMENT SLOT EMPTY. \nPLEASE EQUIP AUGMENT TO ACTIVATE ABILITY SYSTEM");
        }

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("StarsteelPlate").Type && legs.type == Mod.Find<ModItem>("StarsteelBoots").Type;
		
		}

		public override void UpdateArmorSet(Player player)
		{
            player.setBonus = SetBonusText.Value;
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			modPlayer.Starsteel = true;

			if (modPlayer.StarsteelBonus == 1)
			{
				player.setBonus = @"+9% Melee Speed
Melee critical hits will cause a piercing star to fall";
				Item.defense = 23;
				player.GetAttackSpeed(DamageClass.Melee) += .09f;
				return;
			}
			if (modPlayer.StarsteelBonus == 2)
			{
				player.setBonus = @"20% reduced ammo consumption
Ranged critical hits will cause a piercing star to fall";
				player.ammoCost80 = true;
				Item.defense = 5;
				return;
			}
			if (modPlayer.StarsteelBonus == 3)
			{
				player.setBonus = @"Increases maximum mana by 90
Magic critical hits will cause a piercing star to fall";
				Item.defense = 9;
				player.statManaMax2 += 90;
				return;
			}
			if (modPlayer.StarsteelBonus == 4)
			{
				player.setBonus = @"A guardian star watches over you by firing mini stars at enemies.";
				Item.defense = 2; 
				if (player.whoAmI == Main.myPlayer)
				{
					if (player.ownedProjectileCounts[ModContent.ProjectileType<StarGuardian>()] < 1)
					{
                        int baseDamage = 60;
                        float modifiedDamage = player.GetDamage(DamageClass.Summon).ApplyTo(baseDamage);

                        Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<StarGuardian>(), (int)modifiedDamage, 0f, Main.myPlayer, 0f, 0f);
					}
				}
				return;
			}

			player.setBonus = @"ERROR: AUGMENT SLOT EMPTY. 
PLEASE EQUIP AUGMENT TO ACTIVATE ABILITY SYSTEM";
			modPlayer.StarsteelBonus = 0;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Stelarite>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}