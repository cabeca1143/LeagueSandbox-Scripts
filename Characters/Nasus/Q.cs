using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class NasusQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
		public void OnLevelUp (ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("NasusQ", 6.0f, 1, spell, owner, owner);
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
    }
    public class NasusQAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			TriggersSpellCasts = true,
            // TODO
        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            //Packets dont have any manual cast to the animation, so we're probably missing something.
            //Maybe something broke along the way?
            //Note: Idk if using the attack speed as animation speed is correct.
            spell.CastInfo.Owner.PlayAnimation("Spell1", spell.CastInfo.Owner.Stats.GetTotalAttackSpeed());
        }
        public void OnSpellCast(ISpell spell)
        {
        }
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            var stackCount = GetStackCount(owner);

            target.TakeDamage(owner, owner.Stats.AttackDamage.Total + stackCount, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);

            if (target.IsDead)
            {
                //We still gotta fix this buff counter thing
                AddBuff("NasusQStacks", 25000, 1, spell, owner, owner);
                AddBuff("NasusQStacks", 25000, 1, spell, owner, owner);
                AddBuff("NasusQStacks", 25000, 1, spell, owner, owner);

                //Ideally we'd do this within the buff script, but it seems counter buffs never get their script activated.
                //Essentially all this does is update the buff's tooltip to show the proper stack count
                SetBuffToolTipVar(owner.GetBuffWithName("NasusQStacks"), 0, GetStackCount(owner));
            }
        }

        public int GetStackCount(IObjAiBase owner)
        {
            if (owner.HasBuff("NasusQStacks"))
            {
                return owner.GetBuffWithName("NasusQStacks").StackCount;
            }
            return 0;
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
