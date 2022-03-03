using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class NasusR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase owner;
        IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner as IChampion;

            var HealthBuff = 150f + 150f * ownerSpell.CastInfo.SpellLevel;

            p = AddParticleTarget(owner, unit, "Nasus_Base_R_Aura.troy", unit, buff.Duration);
            p = AddParticleTarget(owner, unit, "Nasus_Base_R_Avatar.troy", unit, buff.Duration);
            StatsModifier.Armor.PercentBonus = 25 + 15 * ownerSpell.CastInfo.SpellLevel;
            StatsModifier.MagicResist.PercentBonus = 25 + 15 * ownerSpell.CastInfo.SpellLevel;
            StatsModifier.Size.BaseBonus = StatsModifier.Size.BaseBonus + 0.4f;
            StatsModifier.HealthPoints.BaseBonus += HealthBuff;

            unit.AddStatModifier(StatsModifier);
            unit.Stats.CurrentHealth += HealthBuff;
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            //StatsModifier.Size.BaseBonus = StatsModifier.Size.BaseBonus - 0.1f;

        }

        public void OnUpdate(float diff)
        {
        }
    }
}