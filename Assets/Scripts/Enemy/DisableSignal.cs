using UnityEngine;

namespace Enemy
{
    public interface IEnemyLeft
    {
        public void EnemyLeft(int number);
    }

    public class DisableSignal : MonoBehaviour
    {
        private IEnemyLeft _iEnemyLeft;
        private int _number;

        private void OnDisable() => _iEnemyLeft.EnemyLeft(_number);

        public void SetSpawn(IEnemyLeft iEnemyleft, int number = 0)
        {
            _iEnemyLeft = iEnemyleft;
            _number = number;
        }
    }
}