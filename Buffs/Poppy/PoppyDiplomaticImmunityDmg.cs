using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;

namespace Buffs
{
    internal class PoppyDiplomaticImmunityDmg : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        private IObjAIBase Owner;
        private float damage2;
        ISpell Spell;
        IParticle p;
        IAttackableUnit target;
        IObjAIBase globowner;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var Owner = ownerSpell.CastInfo.Owner;
            globowner = Owner;
            Spell = ownerSpell;
            AddParticleTarget(Owner, Owner, "DiplomaticImmunity_buf.troy", Owner, buff.Duration);
            ApiEventManager.OnPreTakeDamage.AddListener(Owner, unit, VerifyDamage, false);
            //ApiEventManager.OnTakeDamage.AddListener();

        }

        public void VerifyDamage(IDamageData damage)
        {
            var attacker = damage.Attacker;
            var owner = damage.Target;

            if (!attacker.HasBuff("PoppyDITarget"))
            {
                owner.SetStatus(StatusFlags.Invulnerable, true);
            }
            else
            {
                owner.SetStatus(StatusFlags.Invulnerable, false);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            ApiEventManager.OnPreTakeDamage.RemoveListener(this, owner as IAttackableUnit);
            ApiEventManager.RemoveAllListenersForOwner(owner);
            globowner.SetStatus(StatusFlags.Invulnerable, false);

        }

        public void OnUpdate(float diff)
        {
        }
    }
}