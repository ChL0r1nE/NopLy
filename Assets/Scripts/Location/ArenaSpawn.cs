using UnityEngine;

namespace Location
{
    public class ArenaSpawn : MonoBehaviour, Enemy.IEnemyLeft
    {
        public void Open() => _animator.SetBool("IsOpen", true);

        [SerializeField] private Interact.ArenaEnter _arenaEnter;
        [SerializeField] private GameObject _enemy;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _enemyPositions;
        private BoxCollider _collider;
        private Vector3 _insidePosition = new(2f, 1f, -17.5f);
        private Vector3 _insideSize = new(10f, 1f, 8f);
        private Vector3 _outsidePosition = new(2f, 1f, -23.5f);
        private Vector3 _outsideSize = new(10f, 1f, 2f);
        private int _enemyCount = 0;
        private bool _isInside = false;

        private void Start() => _collider = GetComponent<BoxCollider>();

        private void OnTriggerExit(Collider col)
        {
            if (!col.CompareTag("Player")) return;

            _isInside = !_isInside;
            _collider.center = _isInside ? _outsidePosition : _insidePosition;
            _collider.size = _isInside ? _outsideSize : _insideSize;
            _animator.SetBool("IsOpen", false);

            if (!_isInside) return;

            _arenaEnter.SetClose();
            Instantiate(_enemy, _enemyPositions, Quaternion.identity).AddComponent<Enemy.Health>().SetSpawn(this);
            _enemyCount++;
        }

        public void EnemyLeft(int number)
        {
            if (--_enemyCount == 0)
                _animator.SetBool("IsOpen", true);
        }
    }
}
