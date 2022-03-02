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
    public class DariusExecute : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,


        };

        IAttackableUnit Target;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            //ApiEventManager.OnSpellCast.AddListener(this, spell, CastSpell);
        }


        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;

        }

        public void OnSpellCast(ISpell spell)
        {


        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var AD = owner.Stats.AttackDamage.FlatBonus * 0.75f;
            var damage = 70f + (90f * spell.CastInfo.SpellLevel) + AD;
            if (!Target.HasBuff("DariusHemo"))
            {

                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);

            }
            if (Target.HasBuff("DariusHemo"))
            {
                int stackCount = Target.GetBuffWithName("DariusHemo").StackCount;
                float enemystacks = stackCount;
                var percbonusdamage = enemystacks * 0.20f;
                float damagebonus = damage * percbonusdamage;
                Target.TakeDamage(owner, damage + damagebonus, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);

            }
            AddBuff("DariusHemo", 5.100006f, 1, spell, Target, owner);

        }

        public void OnSpellChannel(ISpell spell)
        {
        }
        public void CastSpell(ISpell spell)
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