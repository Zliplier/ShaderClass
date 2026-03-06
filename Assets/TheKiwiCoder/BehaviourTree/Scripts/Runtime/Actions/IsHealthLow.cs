using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsHealthLow : ActionNode
{
    public float threshold = 40f;
    private Health health;

    protected override void OnStart() {
        if (health == null)
            health = context.gameObject.GetComponent<Health>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (health == null) return State.Failure;

        if (health.currentHealth <= threshold)
        {
            return State.Success;
        }

        return State.Failure;
    }
}
