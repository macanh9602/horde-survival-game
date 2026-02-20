using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace DucDevGame
{
    [TaskCategory("Tactician")]
    public class MoveToPosition : Action
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;

        public Teemo teemo;




        public override void OnStart()
        {
            teemo.Walk();
        }


        public override TaskStatus OnUpdate()
        {
            Vector3 currentPos = transform.position;
            Vector3 destination = teemo.GetTargetPosition();
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
            teemo.Idle();

        }
    }
}