using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;


namespace Spells
{
    public class PoppyHeroicCharge : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,


        };
        IAttackableUnit Target;

        public void OnActivate(IObjAIBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }

        public ISpellSector DamageSector;

        public void OnDeactivate(IObjAIBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAIBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var to = Vector2.Normalize(Target.Position - owner.Position);
            owner.SetTargetUnit(null);
            ForceMovement(owner, "Spell1", new Vector2(Target.Position.X - to.X * 100f, Target.Position.Y - to.Y * 100f), 2000f, 500f, 0f, 0f);
            SpellCast(owner, 2, SpellSlotType.SpellSlots, true, Target, Target.Position);

        }

        public void OnSpellPostCast(ISpell spell)
        {


        }

        public void OnSpellChannel(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var to = Vector2.Normalize(Target.Position - owner.Position);
            ForceMovement(target, "Spell1", new Vector2(Target.Position.X - to.X * 100f, Target.Position.Y - to.Y * 100f), 2000f, 500f, 0f, 0f);
        }
        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}