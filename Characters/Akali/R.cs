using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class AkaliShadowDance : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        IAttackableUnit datarget;
        ISpell Spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //ApiEventManager.OnSpellHit.AddListener(owner, spell, ApplyEffects, true);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            datarget = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            Spell = spell;
            var owner = spell.CastInfo.Owner;
            var target = datarget;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(target.Position.X, target.Position.Y) - current);
            var range = to * 800;

            var trueCoords = current + range;

            //TODO: Dash to the correct location (in front of the enemy IChampion) instead of far behind or inside them
            ForceMovement(owner, target, "Spell4", 2200, 0, 0, 0, 20000);
            ApplyEffects(target);
            //ForceMovement(spell.CastInfo.Owner, "Spell4", trueCoords, 2200, 0, 0, 0);
            AddParticleTarget(owner, target, "akali_shadowDance_tar", target);
        }

        public void ApplyEffects(IAttackableUnit target)
        {
            var owner = Spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 25f + Spell.CastInfo.SpellLevel * 75f + ap/2;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnSpellChannel(ISpell spell)
        {
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
