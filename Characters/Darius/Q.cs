using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore;
using System;
using System.Linq;
using GameServerCore.Enums;

namespace Spells
{
    public class DariusCleave : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
        };

        IObjAiBase Owner;
        public ISpellSector DamageSector;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;

            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1f
            });
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            PlayAnimation(owner, "Spell1", 0.7f);
            AddParticleTarget(owner, owner, "darius_Base_Q_aoe_cast.troy", owner);
            AddParticleTarget(owner, owner, "darius_Base_Q_aoe_cast_mist.troy", owner);



            var AD = owner.Stats.AttackDamage.Total;
            var damage = 35 + (35 * spell.CastInfo.SpellLevel) + AD;

            var champs = GetUnitsInRange(owner.Position, 400f, true).OrderBy(enemy => Vector2.DistanceSquared(enemy.Position, owner.Position)).ToList();
            foreach (var enemy in champs
                 .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                var distance = Vector2.Distance(enemy.Position, owner.Position);
                if (distance < 200f)
                {
                    AddParticleTarget(owner, enemy, "darius_Base_Q_tar_inner.troy", enemy);
                    enemy.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
                if (distance >= 200f) 
                {
                    enemy.TakeDamage(owner, damage * 1.5f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(owner, enemy, "darius_Base_Q_tar.troy", enemy);
                    AddBuff("DariusHemo", 5.100006f, 1, spell, enemy, owner);
                } 
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
            //var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            //SpellCast(Owner, 1, SpellSlotType.ExtraSlots, spellpos, spellpos, false, Vector2.Zero);
            //AddParticle(Owner, null, "Pantheon_Base_R_indicator_green.troy", spellpos, 2.7f);
        }

        public void OnUpdate(float diff)
        {
        }
    }

}