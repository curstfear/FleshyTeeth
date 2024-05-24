using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float _characterMaxHealth = 100f;
    public float _characterHealth = 100f;
    
    public CharacterHealthUI _characterHealthUI;

    private void Start()
    {
        _characterHealthUI.UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        _characterHealth -= damage;
        if (_characterHealth <= 0) Die();
        _characterHealthUI.UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        _characterHealth += amount;
        if(_characterHealth > _characterMaxHealth) _characterHealth = _characterMaxHealth;
        _characterHealthUI.UpdateHealthBar();
    }
    
    private void Die()
    {
        Debug.Log("Die");
    }

    public float GetCurrentHealth()
    {
        return _characterHealth;
    }
}
