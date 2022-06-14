using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;

namespace Buffs
{
    internal class AkaliShroudBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float ticks;
        ISpell originSpell;
        IAttackableUnit Owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetChampionsInRange(unit.Position, 50000, true);
            Owner = owner;
            originSpell = ownerSpell;
            foreach (IChampion player in Champs)
            {
                if (player.Team.Equals(owner.Team))
                {
                    //owner.SetInvisible((int)player.GetPlayerId(), owner, 0.5f, 0.1f);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    //owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                }
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetChampionsInRange(unit.Position, 50000, true);
            foreach (IChampion player in Champs)
            {
                //owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                owner.SetStatus(StatusFlags.Ghosted, false);
            }
            unit.SetStatus(StatusFlags.Targetable, true);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks >= 100f)
            {
                var spellPos = new Vector2(originSpell.CastInfo.TargetPositionEnd.X, originSpell.CastInfo.TargetPositionEnd.Z);
                if (GameServerCore.Extensions.IsVectorWithinRange(Owner.Position, spellPos, 400f))
                {
                    Owner.SetStatus(StatusFlags.Targetable, false);
                }
                else Owner.SetStatus(StatusFlags.Targetable, true);
                ticks = 0f;
            }
        }
    }
}