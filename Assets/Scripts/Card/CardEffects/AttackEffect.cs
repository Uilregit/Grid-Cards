using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : Effect
{
    public override void Process(GameObject caster, CardEffectsController effectController, GameObject target, Card card, int effectIndex)
    {
        HealthController targetHealthController = target.GetComponent<HealthController>();
        targetHealthController.TakeVitDamage(caster.GetComponent<HealthController>().GetAttack());
    }

    public override SimHealthController SimulateProcess(GameObject caster, CardEffectsController effectController, Vector2 location, int value, int duration, SimHealthController simH)
    {
        HealthController target = GridController.gridController.GetObjectAtLocation(location).GetComponent<HealthController>();
        return target.SimulateTakeVitDamage(simH, target.GetAttack());
    }
}
