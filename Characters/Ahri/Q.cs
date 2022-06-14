using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AhriOrbofDeception : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }
        static internal Vector2 endpos;
        static internal int addpassive = 0;
        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var targetPos = GetPointFromUnit(owner, 800f, 0);
            endpos = targetPos;
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class AhriOrbMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle,
                CanHitSameTargetConsecutively = true,
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true,

            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        IObjAiBase _owner;
        private bool comeBack = false;


        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }


        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = _owner;
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, owner, AhriOrbofDeception.endpos);
            //AddParticle(owner, null, "Ahri_Orb_cas.troy", AhriOrbofDeception.endpos);
        }



        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }




    public class AhriOrbReturn : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle,
                CanHitSameTargetConsecutively = true,
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true,

            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        IObjAiBase _owner;
        private bool comeBack = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }



        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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