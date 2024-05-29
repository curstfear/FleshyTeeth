using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeatSticks : MonoBehaviour
{
    [SerializeField] private int MeatHeal = 15;
    private bool _isNear;

    private GameObject Character;
    public CharacterHealth _characterHealth;
    public InputActionReference inputActionValue;
    public CharacterInputActions _inputActions;
    public GameObject _hint;
    public TMP_Text _hintText;

    private void Awake()
    {
        Character = GameObject.Find("Character");
        _characterHealth = Character.GetComponent<CharacterHealth>();
        _inputActions = new CharacterInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Character.Enable();
        _inputActions.Character.UseItem.performed += Use;
        string inputName = inputActionValue.action.GetBindingDisplayString(0);
        _hintText.text = inputName.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            _isNear = true;
            _hint.SetActive(true);
       
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

    void Use(InputAction.CallbackContext context)
    {
        if (_isNear)
        {
            if (_characterHealth._characterHealth < 100)
            {
                _characterHealth.Heal(MeatHeal);
                Destroy(gameObject);
            }
        }
    }
}
