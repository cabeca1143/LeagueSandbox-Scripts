using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class AhriSoulCrusherCounter : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 9;
        public bool IsHidden => false;

        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = 9,
            IsHidden = false
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //AddBuff("ShenKiAttack", float.MaxValue, 1, ownerSpell, unit, unit as IObjAiBase, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //unit.RemoveBuffsWithName("ShenKiAttack");
            //AddBuff("ShenWayOfTheNinjaMarker", float.MaxValue, 1, ownerSpell, unit, unit as IObjAiBase, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}