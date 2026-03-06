using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FindHealingItem : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        
        HealingItem[] items = Object.FindObjectsOfType<HealingItem>();
        if (items.Length == 0)
        {
            return State.Failure;
        }

        HealingItem closestItem = null;
        float closestDist = Mathf.Infinity;

        foreach (var item in items)
        {
            float dist = Vector3.Distance(context.transform.position, item.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestItem = item;
            }
        }

        if (closestItem != null)
        {
            blackboard.healingItemTarget = closestItem.gameObject;
            blackboard.moveToPosition = closestItem.transform.position;
            return State.Success;
        }

        return State.Failure;
    }
}
