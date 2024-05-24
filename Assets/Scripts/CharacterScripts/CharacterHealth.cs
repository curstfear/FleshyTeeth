using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float _characterMaxHealth = 100f;
    public float _characterHealth = 100f;
    
    public CharacterHealthUI _characterHealthUI;

    // метод получения урона
    public void TakeDamage(float damage)
    {
        _characterHealth -= damage;
        if (_characterHealth <= 0) Die();
        _characterHealthUI.UpdateHealthBar();
    }

    // метод лечения
    public void Heal(float amount)
    {
        _characterHealth += amount;
        if(_characterHealth > _characterMaxHealth) _characterHealth = _characterMaxHealth;
        _characterHealthUI.UpdateHealthBar();
    }
    
    // смерть игрока
    private void Die()
    {
        Debug.Log("Die");
    }

    //получение текущего здоровья
    public float GetCurrentHealth()
    {
        return _characterHealth;
    }
}
