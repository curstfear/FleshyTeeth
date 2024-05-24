using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    public CharacterHealth _characterHealth;
    [SerializeField] private Image _characterHealthBar;

    private void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float characterHealthRatio = (float)_characterHealth.GetCurrentHealth() / _characterHealth._characterMaxHealth;
        _characterHealthBar.fillAmount = characterHealthRatio;
    }

}
