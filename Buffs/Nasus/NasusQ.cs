using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using System.Numerics;
using System.Linq;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class NasusQ : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff Buff;
        IParticle p1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Buff = buff;
            if (unit is IObjAiBase obj)
            {
                SealSpellSlot(obj, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnPreAttack.AddListener(this, obj, OnPreAttack, true);
                p1 = AddParticleTarget(obj, obj, "Nasus_Base_Q_Buf", obj, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1", "weapon_b1");
                obj.CancelAutoAttack(true);
            }
        }

        public void OnPreAttack(ISpell spell)
        {
            //Theoretically, this would skip the character's next basic attack damage, but doesnt seem to be working anymore, has to be investigated.
            spell.CastInfo.Owner.SkipNextAutoAttack();

            SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Targets[0].Unit, Vector2.Zero);

            if(Buff != null)
            {
                Buff.DeactivateBuff();
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (unit is IObjAiBase obj)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this);
                SealSpellSlot(obj, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
                RemoveParticle(p1);
            }

        }

        public void OnUpdate(float diff)
        {
        }
    }
}
