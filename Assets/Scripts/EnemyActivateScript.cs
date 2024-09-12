using UnityEngine;

public class EnemyActivateScript : MonoBehaviour
{
    public Transform EnemyTransform;

    public bool HaveEnemy => EnemyTransform != null;

    private void OnTriggerEnter(Collider col)
    {
        if (EnemyTransform != null)
            EnemyTransform.gameObject.GetComponent<OutlineScript>().OutlineWidth = 0;

        EnemyTransform = col.gameObject.GetComponent<Transform>();
        col.gameObject.GetComponent<OutlineScript>().OutlineWidth = 5;
    }

    private void FixedUpdate()
    {
        if (HaveEnemy && Vector3.Distance(transform.position, EnemyTransform.position) > 11)
        {
            EnemyTransform.gameObject.GetComponent<OutlineScript>().OutlineWidth = 0;
            EnemyTransform = null;
        }
    }
}
