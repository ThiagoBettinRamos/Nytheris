using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;


namespace NytherisMod.Items
{
    public class NytherisSummonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Definindo nome e descrição via Localization
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 20;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            // Permite uso apenas se Nytheris não estiver ativo
            return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Nytheris>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // Invoca o boss no local do jogador
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Nytheris>());
                Main.NewText("Nytheris emerge do vazio!", 175, 75, 255);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentNebula, 10)
                .AddIngredient(ItemID.FragmentVortex, 10)
                .AddIngredient(ItemID.FragmentStardust, 10)
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
