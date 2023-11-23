using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrainLine : MonoBehaviour
{
    public Transform target;
    public float speed;
    float stopDistance = 0.2f;
    public float speedRotation;

    // Update is called once per frame
    void Update()
    {

        // Version sans : la rotation de l’ennemi se fait de manière progressive, et pas d’un seul coup
        /*
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float distance = MathHelper.VectorDistance(transform.position, target.position);

            if (distance > stopDistance)
            {
                transform.position += direction.normalized * speed * Time.deltaTime;
            }

            transform.LookAt(target);
        }*/

        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 enemyPosition = transform.position;

            float distance = MathHelper.VectorDistance(enemyPosition, targetPosition);

            if (distance > stopDistance)
            {
                Vector3 direction = targetPosition - enemyPosition;
                Vector3 movement = direction.normalized * speed * Time.deltaTime;
                transform.position += movement;

                // trouvé à l'aide d'un livre sur le c# avec la partie sur les Quaternions et autocomplétion
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - enemyPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
            }

        }  

        else { return; }
    }
}
