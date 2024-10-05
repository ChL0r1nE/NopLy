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
        _nowHealth = Mathf.Clamp(_nowHealth, 0, _maxHealth);
    }

    public void TakeDamage(int damage)
    {
        _nowHealth -= (int)Mathf.Round(damage / _playerArmorScript.ArmorModifier);
        _nowHealth = Mathf.Clamp(_nowHealth, 0, _maxHealth);
    }
}
