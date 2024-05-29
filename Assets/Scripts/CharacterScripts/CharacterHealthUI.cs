using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    public CharacterHealth _characterHealth;
    [SerializeField] private Image _characterHealthBar;
    public Animator heartBeats;


    //инициализация хп бара
    private void Start()
    {
        heartBeats = GetComponent<Animator>();
        UpdateHealthBar();
    }

    private void Update()
    {
        HeartBeatsControl();
    }

    // вычисление соотношения текущего здоровья персонажа к максимальному и вывод в UI
    public void UpdateHealthBar()
    {
        float characterHealthRatio = (float)_characterHealth.GetCurrentHealth() / _characterHealth._characterMaxHealth;
        _characterHealthBar.fillAmount = characterHealthRatio;
        Debug.Log(_characterHealth._characterHealth.ToString());
    }

    private void HeartBeatsControl()
    {
        //если у игрока 25% здоровья
        if (_characterHealth.GetCurrentHealth() <= (_characterHealth._characterMaxHealth * 0.25f))
        {
            heartBeats.SetBool("isBeatsSlow", false);
            heartBeats.SetBool("isBeatsFast", true);
        }

        //если у игрока 50% здоровья
        else if (_characterHealth.GetCurrentHealth() <= (_characterHealth._characterMaxHealth * 0.5f))
        {
            heartBeats.SetBool("isBeatsFast", false);
            heartBeats.SetBool("isBeatsSlow", true);
        }

        //если у игрока больше 50%
        else
        {
            heartBeats.SetBool("isBeatsSlow", false);
            heartBeats.SetBool("isBeatsFast", false);
        }
    }

}
