using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;



namespace CharScripts
{

    public class CharScriptAhri : ICharScript

    {
        ISpell Spell;
        IAttackableUnit Target;
        IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            AddParticleTarget(owner, owner, "Ahri_Orb.troy", owner, lifetime: 9999999f, bone: "r_hand");
        }


        public void OnHitUnit(IDamageData damageData)
        {

        }


        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}