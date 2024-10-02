using UnityEngine.UI;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    [SerializeField] private Image _hpStripe;
    [SerializeField] private PlayerArmorScript _playerArmorScript;
    [SerializeField] private int _maxHealth; //ser
    [SerializeField] private int _nowHealth; //ser

    private void Update() => _hpStripe.fillAmount = Mathf.MoveTowards(_hpStripe.fillAmount, (float)_nowHealth / _maxHealth, Time.deltaTime);

    public void Heal(int heal)
    {
        _nowHealth += heal;
        _nowHealth = (int)Mathf.Clamp(_nowHealth, 0, (float)_maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (_playerArmorScript.ArmorValue == 0)
            _nowHealth -= damage;
        else
            _nowHealth -= (int)(damage * (((float)damage / _playerArmorScript.ArmorValue) / ((float)damage / _playerArmorScript.ArmorValue + 1)));

        _nowHealth = (int)Mathf.Clamp(_nowHealth, 0, (float)_maxHealth);
    }
}
