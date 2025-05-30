using UnityEngine.SceneManagement;
using UnityEngine;

namespace Location
{
    public class Border : MonoBehaviour
    {
        [SerializeField] Vector3 _spawnPosition;

        private void Awake()
        {
            Instantiate(Resources.Load<GameObject>("Player"), _spawnPosition, Quaternion.identity);
            Instantiate(Resources.Load<GameObject>("Canvas"));
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
                SceneManager.LoadScene("Map");
        }
    }
}
