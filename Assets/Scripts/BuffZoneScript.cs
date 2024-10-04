using UnityEngine;

public class BuffZoneScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerArmorScript>().ArmorBuff += 5;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerArmorScript>().ArmorBuff -= 5;
    }
}
