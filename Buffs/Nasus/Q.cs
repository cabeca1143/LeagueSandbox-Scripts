using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;


namespace Spells
{
    public class NasusQ : ISpellScript
    {
        IObjAiBase Owner;
        IAttackableUnit Target;
        IAttackableUnit owner2;
        ISpell spelll;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
           
         //   ApiEventManager.OnLevelUpSpell.AddListener(this, owner.GetSpell("NasusQ"), AddNasusQStacksBuff, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Owner = owner;
            spelll = spell;
        }


        public void AddNasusQStacksBuff(ISpell spell)
        {
          // AddBuff("NasusQStacks", 25000f, 1, spell, owner, owner, true);
        }

        public void OnSpellCast(ISpell spell)
        {
            //AddBuff("NasusQStacks", 25000f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner, true);
            AddBuff("NasusQAttack", 5f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
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


    /*public class NasusQAttack : ISpellScript
    {
        IObjAiBase Owner;
        IAttackableUnit Target;
        IAttackableUnit owner2;
        ISpell spelll;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            IAttackableUnit atunit;
            atunit = spell.CastInfo.Owner.TargetUnit;

            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            Owner = owner;
            spelll = spell;
        }


        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            //AddParticle(owner, null, "Leona_ShieldOfDaybreak_nova.troy", target.Position);
            //AddParticleTarget(owner, target, "Leona_ShieldOfDaybreak_tar.troy", target);

            var ap = owner.Stats.AbilityPower.Total * 0.3f;
            var damage = 40 + (30 * (spell.CastInfo.SpellLevel - 1)) + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnSpellCast(ISpell spell)
        {

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
    }*/

}