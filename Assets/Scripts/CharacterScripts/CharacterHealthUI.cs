using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    public CharacterHealth _characterHealth;
    [SerializeField] private Image _characterHealthBar;

    //инициализация хп бара
    private void Start()
    {
        UpdateHealthBar();
    }

    // вычисление соотношения текущего здоровья персонажа к максимальному и вывод в UI
    public void UpdateHealthBar()
    {
        float characterHealthRatio = (float)_characterHealth.GetCurrentHealth() / _characterHealth._characterMaxHealth;
        _characterHealthBar.fillAmount = characterHealthRatio;
    }

}
