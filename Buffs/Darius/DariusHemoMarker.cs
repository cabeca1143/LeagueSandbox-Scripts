using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;

namespace Buffs
{
    internal class DariusHemoMarker : IBuffGameScript
    {

        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5,
            IsHidden = true
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public IStatsModifier InternalStatsModifier { get; private set; } = new StatsModifier();

        public List<IChampion> UnitsApplied = new List<IChampion>();
        IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
            InternalStatsModifier.MoveSpeed.PercentBonus = (5f / 100f);
            unit.AddStatModifier(InternalStatsModifier);
        }

        public void RemoveMoveSpeed()
        {
            Unit.RemoveStatModifier(InternalStatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}