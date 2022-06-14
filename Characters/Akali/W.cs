using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class AkaliSmokeBomb : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public ISpellSector SlowSector;
        ISpell originSpell;
        IAttackableUnit Owner;
        float ticks; 

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            originSpell = spell;
            var owner = spell.CastInfo.Owner;
            var smokeBomb = AddParticle(owner, null, "akali_smoke_bomb_tar", spellPos, lifetime: 8f);
            /*
             * TODO: Display green border (akali_smoke_bomb_tar_team_green.troy) for the own team,
             * display red border (akali_smoke_bomb_tar_team_red.troy) for the enemy team
             * Currently only displaying the green border for everyone
            */
            var smokeBombBorder = AddParticle(owner, null, "akali_smoke_bomb_tar_team_green", spellPos, lifetime: 8f);

            AddBuff("AkaliShroudBuff", 8f, 1, spell, owner, owner);

            //TODO: Add invisibility

            SlowSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                Tickrate = 4,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });

        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
