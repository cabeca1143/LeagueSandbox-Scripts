using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore;
using System.Linq;

namespace Spells
{
    public class LuluR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
        };

        public ISpellSector DamageSector;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            // Thanks Cosmin <3
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 350f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1f
            });

            AddBuff("LuluR", 7.0f, 1, spell, target, owner);
        }

        public void OnSpellCast(ISpell spell)
        {

            var owner = spell.CastInfo.Owner;
            var champs = GetChampionsInRange(owner.Position, 400f, true).OrderBy(enemy => Vector2.DistanceSquared(enemy.Position, owner.Position)).ToList();
            foreach (var enemy in champs
                 .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                AddBuff("LuluRSlow", 5f, 1, spell, enemy, owner);
            }


        }

        public void OnSpellPostCast(ISpell spell)
        {
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
