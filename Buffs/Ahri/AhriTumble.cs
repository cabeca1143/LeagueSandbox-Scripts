using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore;
using System.Linq;

namespace Buffs
{
    internal class AhriTumble : IBuffGameScript
    {

        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_CONTINUE,
            MaxStacks = 3,
            IsHidden = false
        };

        ISpell Spell;
        IAttackableUnit datarget;
        float ticks;
        bool cancastmissile = false;
        IObjAiBase Owner;
        IBuff thisbuff;


        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            thisbuff = buff;
            ApiEventManager.OnSpellPostCast.AddListener(owner, owner.GetSpell(3), DoTheThing);
            DoTheThing(ownerSpell);
        }

        public void DoTheThing(ISpell spell)
        {
            Spell = spell;
            var target = spell.CastInfo.Targets[0] as IAttackableUnit;
            datarget = target;
            var owner = spell.CastInfo.Owner;
            var current2 = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current2, spellPos);

            if (dist > 425.0f)
            {
                dist = 425.0f;
            }

            FaceDirection(spellPos, owner, true);
            var trueCoords2 = GetPointFromUnit(owner, dist);
            spell.CastInfo.Owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell4", trueCoords2, 2200, 0, 0, 0);
            cancastmissile = true;

        }
    

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;

            if (ticks >=350f && ticks <= 400f)
            {
                if (cancastmissile == true)
                {
                    var spell = Spell;
                    var current = new Vector2(spell.CastInfo.Owner.Position.X, spell.CastInfo.Owner.Position.Y);
                    var trueCoords = GetPointFromUnit(spell.CastInfo.Owner, spell.SpellData.CastRangeDisplayOverride);
                    var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 425f, true).Where(x => x.Team == CustomConvert.GetEnemyTeam(spell.CastInfo.Owner.Team));
                    units.OrderBy(allyTarget => Vector2.Distance(Owner.Position, allyTarget.Position));
                    var i = 0;
                    foreach (var allyTarget in units)
                    {
                        if (allyTarget is IAttackableUnit && spell.CastInfo.Owner != allyTarget)
                        {
                            if (i < 1)
                            {
                                SpellCast(spell.CastInfo.Owner, 5, SpellSlotType.ExtraSlots, true, allyTarget, Vector2.Zero);
                                i++;
                            }
                        }
                    }
                    cancastmissile = false;
                }
                Owner.GetBuffWithName("AhriTumble").DecrementStackCount();
                ticks = 0;
            }
        }
    }
}