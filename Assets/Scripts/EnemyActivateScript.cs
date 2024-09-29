using UnityEngine;

public class EnemyActivateScript : MonoBehaviour
{
    public Transform EnemyTransform;

    private void OnTriggerEnter(Collider col)
    {
        if (EnemyTransform)
            EnemyTransform.GetComponent<OutlineScript>().OutlineWidth = 0;

        EnemyTransform = col.GetComponent<Transform>();
        col.GetComponent<OutlineScript>().OutlineWidth = 5;
    }

    private void FixedUpdate()
    {
        if (!EnemyTransform || Vector3.Distance(transform.position, EnemyTransform.position) < 15) return;

        EnemyTransform.GetComponent<OutlineScript>().OutlineWidth = 0;
        EnemyTransform = null;
    }
}
