using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class PoppyParagonSpeed : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float timeSinceLastTick;
        private IAttackableUnit Unit;
        private float TickingDamage;
        private IObjAIBase Owner;
        private ISpell spell;
        private bool limiter = false;
        float perlevelbonus;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            perlevelbonus = 15f + ownerSpell.CastInfo.SpellLevel * 2;
            owner.Stats.MoveSpeed.PercentBonus += 0.01f * perlevelbonus;
        }



        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner.Stats.MoveSpeed.PercentBonus -= 0.01f * perlevelbonus;
            Owner.RemoveBuffsWithName("PoppyParagonSpeed");
        }

        public void OnUpdate(float diff)
        {
        }
    }
}