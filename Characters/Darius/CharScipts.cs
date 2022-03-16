using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;



namespace CharScripts
{
     
    public class  CharScriptDarius : ICharScript

    {
        ISpell Spell;
		IAttackableUnit Target;
        IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);

        }


        public void OnHitUnit(IDamageData damageData)      
        {
            Target = damageData.Target;
            var owner = Owner;
			AddBuff("DariusHemo", 5.100006f, 1, Spell, Target, owner);

        }


        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}