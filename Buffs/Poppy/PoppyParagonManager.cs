using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class PoppyParagonManager : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 10
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float timeSinceLastTick;
        private IAttackableUnit Unit;
        private float TickingDamage;
        private IObjAIBase Owner;
        private ISpell spell;
        private bool limiter = false;
        float stackdamage;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            var stackdamage1 = (1f + ownerSpell.CastInfo.SpellLevel * 0.5f) * owner.GetBuffWithName("PoppyParagonManager").StackCount;
            stackdamage = stackdamage1;
            owner.Stats.AttackDamage.FlatBonus = stackdamage;
            owner.Stats.Armor.FlatBonus = stackdamage;
        }



        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner.Stats.AttackDamage.FlatBonus -= stackdamage;
            Owner.Stats.Armor.FlatBonus -= stackdamage;
            //Owner.RemoveBuffsWithName("PoppyParagonManager");
        }

        public void OnUpdate(float diff)
        {
        }
    }
}