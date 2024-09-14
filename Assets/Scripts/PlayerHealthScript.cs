using UnityEngine.UI;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    [SerializeField] private Image _hpStripe;
    [SerializeField] private PlayerArmorScript _playerArmorScript;
    [SerializeField] private int _maxHealth; //ser
    [SerializeField] private int _nowHealth; //ser

    public void TakeDamage(int damage)
    {
        if (_playerArmorScript.ArmorValue == 0)
            _nowHealth -= damage;
        else
            _nowHealth -= (int)(damage * (((float)damage / _playerArmorScript.ArmorValue) / ((float)damage / _playerArmorScript.ArmorValue + 1)));

        _hpStripe.fillAmount = (float)_nowHealth / _maxHealth;
    }
}
