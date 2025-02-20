using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace NytherisMod.NPCs
{
    [AutoloadBossHead]
    public class Nytheris : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            
            // Configuração do Bestiário
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 1.4f,                           // Tamanho do sprite no Bestiário
                PortraitScale = 1.2f,                    // Tamanho do retrato
                PortraitPositionYOverride = -20f,        // Posição do retrato
                SpriteDirection = 1                      // Direção do sprite
            };
            value.Position.X += 26f;
            value.Position.Y -= 14f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 200;
            NPC.height = 300;
            NPC.damage = 150;
            NPC.defense = 70;
            NPC.lifeMax = 700000;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 50, 0, 0);
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            Music = MusicID.Boss2;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, -10f);
                    if (NPC.timeLeft > 10)
                        NPC.timeLeft = 10;
                    return;
                }
            }

            float speed = 10f;
            Vector2 direction = player.Center - NPC.Center;
            NPC.velocity = Vector2.Normalize(direction) * speed;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] >= 120)
                {
                    NPC.ai[0] = 0;
                    Vector2 shootDir = Vector2.Normalize(player.Center - NPC.Center) * 15f;
                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        shootDir,
                        ProjectileID.NebulaBlaze1,
                        120,
                        3f,
                        Main.myPlayer
                    );
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemID.MoonLordBossBag));
            npcLoot.Add(ItemDropRule.Common(ItemID.LunarOre, 1, 20, 50));
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Nytheris foi derrotado! O vazio se acalma...", 200, 50, 50);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.VortexDebuff, 300);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Nytheris, o Conquistador do Vazio, desafia até os mais fortes. Dizem que seu poder rivaliza o próprio Moon Lord.")
            });
        }
    }
}
