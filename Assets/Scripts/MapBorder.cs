using UnityEngine.SceneManagement;
using UnityEngine;

public class MapBorder : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            SceneManager.LoadScene("Map");
    }
}
