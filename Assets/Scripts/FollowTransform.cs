using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTransform;
    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }
    public Transform GetTargetTransform()
    {
        return targetTransform;
    }
    private void LateUpdate() 
    {
        if (targetTransform == null)
        {
            return;
        }
        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
    }
}
