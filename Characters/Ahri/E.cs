using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AhriSeduce : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

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
            var endPos = GetPointFromUnit(spell.CastInfo.Owner, 10);
            SpellCast(spell.CastInfo.Owner, 4, SpellSlotType.ExtraSlots, endPos, endPos, true, Vector2.Zero);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class AhriSeduceMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
        };
        IObjAiBase _owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
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
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (missile is ISpellCircleMissile skillshot)
            {
                var owner = spell.CastInfo.Owner;
                var hitobj = skillshot.ObjectsHit.Count;
                //target.TakeDamage(owner, 50, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_RAW, true);
                var ap = owner.Stats.AbilityPower.Total * 0.30;
                float damage = (float)(ap + 50 + (owner.Spells[2].CastInfo.SpellLevel * 25));
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                missile.SetToRemove();
                FaceDirection(owner.Position, target);
                var x = GetPointFromUnit(target, 300);
                var xy = target as IObjAiBase;
                xy.SetTargetUnit(null);
                ForceMovement(target, "run", x, 10, 0, 0, 0);
                AddBuff("VeigarEventHorizon", 2.0f, 1, spell, target, owner);
                AddBuff("AhriSoulCrusherCounter", float.MaxValue, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);

                if (spell.CastInfo.Owner.GetBuffWithName("AhriSoulCrusherCounter").StackCount == 9)
                {
                    PerformHeal(spell.CastInfo.Owner, spell, spell.CastInfo.Owner);
                    CreateTimer(0.1f, () => { RemoveStacks(9); });
                }

            }
        }

        public void RemoveStacks(int var)
        {
            int x = 0;
            foreach (var swag in _owner.GetBuffsWithName("AhriSoulCrusherCounter"))
            {
                if (x < var)
                {
                    x++;
                    swag.DeactivateBuff();
                }
            }
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * spell.SpellData.MagicDamageCoefficient;
            float healthGain = 15 + (spell.CastInfo.SpellLevel * 45) + ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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