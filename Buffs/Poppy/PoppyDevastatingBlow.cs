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
    internal class PoppyDevastatingBlow : IBuffGameScript
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

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var Owner = ownerSpell.CastInfo.Owner;
            Owner.CancelAutoAttack(true);
            Spell = ownerSpell;
            AddParticleTarget(Owner, Owner, "Poppy_DevastatingBlow_buf.troy", Owner, 5f);

            //AddParticleTarget(Owner, Owner, "Fizz_SeastoneTrident.troy", Owner, 5f, bone: "BUFFBONE_GLB_WEAPON_1");
            //AddParticleTarget(Owner, Owner, "Fizz_SeastonePassive_Weapon.troy", Owner, bone: "BUFFBONE_GLB_WEAPON_1");

            ApiEventManager.OnHitUnit.AddListener(Owner, ownerSpell.CastInfo.Owner, TargetTakeDamage, true);
            Owner.SkipNextAutoAttack();
        }

        public void TargetTakeDamage(IDamageData damage)
        {
            var Owner = damage.Attacker;
            target = damage.Target;
            var ap = Owner.Stats.AbilityPower.Total * 0.60f;
            var ad = Owner.Stats.AttackDamage.Total;
            var targetHealth = target.Stats.HealthPoints.Total * 0.08f;
            var basedmg = Spell.CastInfo.SpellLevel * 20;
            var basepluspercent = targetHealth + basedmg;
            if ((basepluspercent) > (75 * Spell.CastInfo.SpellLevel)) basepluspercent = Spell.CastInfo.SpellLevel * 75;
            damage2 = basepluspercent + ap + ad;
            target.TakeDamage(Owner, damage2, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(Owner, target, "Poppy_DevastatingBlow_tar.troy", target, 0.5f);
            Owner.RemoveBuffsWithName("PoppyDevastatingBlow");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var Owner = ownerSpell.CastInfo.Owner;
            //ApiEventManager.OnHitUnit.RemoveListener(this);
            SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}