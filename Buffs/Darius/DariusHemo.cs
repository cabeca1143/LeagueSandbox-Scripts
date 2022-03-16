using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore;
using System.Linq;
using System.Collections.Generic;

namespace Buffs
{
    internal class DariusHemo : IBuffGameScript
    {

        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff Buff;
        private float timeSinceLastTick;

        IParticle p1;
        IParticle p2;

        Dictionary<int, KeyValuePair<string, string>> Particles = new Dictionary<int, KeyValuePair<string, string>>
        {
            { 1, new KeyValuePair<string, string>("darius_Base_hemo_counter_01", "darius_Base_hemo_bleed_trail_only1")},
            { 2, new KeyValuePair<string, string>("darius_Base_hemo_counter_02", "darius_Base_hemo_bleed_trail_only2") },
            { 3, new KeyValuePair<string, string>("darius_Base_hemo_counter_03", "darius_Base_hemo_bleed_trail_only3") },
            { 4, new KeyValuePair<string, string>("darius_Base_hemo_counter_04", "darius_Base_hemo_bleed_trail_only4") },
            { 5, new KeyValuePair<string, string>("darius_Base_hemo_counter_05", "darius_Base_hemo_bleed_trail_only5") }
        };
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Buff = buff;
            var owner = buff.SourceUnit;

            RemoveParticle(p1);
            RemoveParticle(p2);

            p1 = AddParticleTarget(owner, unit, Particles[buff.StackCount].Value, unit, buff.Duration);
            p2 = AddParticleTarget(owner, unit, Particles[buff.StackCount].Key, unit, buff.Duration);



            if (unit is IChampion champion)
            {
                if (!owner.HasBuff("DariusHemoMarker")) //If the person who applied this buff doesn't have the speed buff)
                {
                    AddMoveSpeedBuff(owner, champion);
                }
                else
                {
                    var speedBuff = owner.GetBuffWithName("DariusHemoMarker");
                    var speedBuffScript = speedBuff.BuffScript as DariusHemoMarker;
                    if (!speedBuffScript.UnitsApplied.Contains(champion))
                    {
                        AddMoveSpeedBuff(owner, champion);
                    }
                }
            }
        }

        public void AddMoveSpeedBuff(IObjAiBase owmer, IChampion target)
        {
            ISpell spell = null;
            if (owmer.Spells.ContainsKey((short)SpellSlotType.PassiveSpellSlot))
            {
                spell = owmer.Spells[(short)SpellSlotType.PassiveSpellSlot];
            }

            //We essentially just add it forever here
            AddBuff("DariusHemoMarker", 25000.0f, 1, spell, owmer, owmer);
            (owmer.GetBuffWithName("DariusHemoMarker").BuffScript as DariusHemoMarker).UnitsApplied.Add(target);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //We then remove it manually down here when this buff is over
            if (unit is IChampion champion)
            {
                var speedBuff = buff.SourceUnit.GetBuffWithName("DariusHemoMarker");
                var speedBuffScript = speedBuff.BuffScript as DariusHemoMarker;
                speedBuffScript.RemoveMoveSpeed();
                speedBuffScript.UnitsApplied.Remove(champion);
                speedBuff.DecrementStackCount();
                if (speedBuff.StackCount <= 0)
                {
                    speedBuff.DeactivateBuff();
                }
            }
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f)
            {
                var AD = Buff.SourceUnit.Stats.AttackDamage.FlatBonus * 0.3f;
                var damage = (12f + AD);

                Buff.TargetUnit.TakeDamage(Buff.SourceUnit, damage * Buff.StackCount, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLPERSIST, false);
                timeSinceLastTick = 0;
            }
        }
    }
}