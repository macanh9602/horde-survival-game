using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

[TaskCategory("Tactician")]
public class MoveToPosition : Action
{
    public SharedVector3 targetPosition;
    public SharedBool isMoving;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public Teemo teemo;




    public override void OnStart()
    {
        Debug.Log($"<color=green>[DA]</color> start");
        teemo.Walk();
    }


    public override TaskStatus OnUpdate()
    {
        Debug.Log($"<color=green>[DA]</color> update");
        Vector3 currentPos = transform.position;
        Vector3 destination = targetPosition.Value;
        destination.y = currentPos.y; // Đảm bảo linh thú không bay lên

        float distance = Vector3.Distance(currentPos, destination);
        if (distance < 0.1f) return TaskStatus.Success;

        // Xoay mặt
        Vector3 direction = (destination - currentPos).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Di chuyển
        transform.position = Vector3.MoveTowards(currentPos, destination, moveSpeed * Time.deltaTime);
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        Debug.Log($"<color=green>[DA]</color> end");
        teemo.Idle();
        isMoving.Value = false;
    }
}