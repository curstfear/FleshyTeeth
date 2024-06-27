using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeatSticks : MonoBehaviour
{
    [SerializeField] private int MeatHeal = 15; //сколько восстанавливает здоровья мясо
    private bool _isNear; //логическая переменная - рядом или нет

    public ParticleSystem _useParicle; // партикл использования предмета
    private GameObject Character;
    public CharacterHealth _characterHealth; //ссылка на класс CharacterHealth
    public InputActionReference inputActionValue; 
    public CharacterInputActions _inputActions; //ссылка на пакет InputManager
    public GameObject _hint; //объект "подсказка"
    public TMP_Text _hintText; // текст подсказки

    private void Awake()
    {
        Character = GameObject.Find("Character");
        _characterHealth = Character.GetComponent<CharacterHealth>();
        _inputActions = new CharacterInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Character.Enable();
        _inputActions.Character.UseItem.performed += UseMeat;
        string inputName = inputActionValue.action.GetBindingDisplayString(0); //получение значения клавиши, указанного в InputMap
        _hintText.text = inputName.ToString(); //передача этого значения в текст
    }

    //когда персонаж входит в триггерную зону предмета (мясо)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            _isNear = true;
            _hint.SetActive(true); // включается подсказка
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            _isNear = false;
            _hint.SetActive(false);
        }
    }

    // использование мясной колбы или красной мясной палочки
    void UseMeat(InputAction.CallbackContext context)
    {
        if (_isNear)
        {
            //при условии, что здоровье персонажа меньше максимального здоровья
            if (_characterHealth._characterHealth < _characterHealth._characterMaxHealth)
            {
                ParticleSystem particles = Instantiate(_useParicle, transform.position, Quaternion.identity);
                particles.Play();
                _characterHealth.Heal(MeatHeal);
                Destroy(gameObject);
            }
        }
    }
}
