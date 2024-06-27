using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [SerializeField] private float _ghostDamage = 20f;
    public ParticleSystem _deathParticle;

    private CharacterHealth _characterHealth;

    private void Awake()
    {
        _characterHealth = FindObjectOfType<CharacterHealth>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            Destroy(gameObject);
            ParticleSystem particles = Instantiate(_deathParticle, transform.position, Quaternion.identity);
            particles.Play();
            _characterHealth.TakeDamage(_ghostDamage);
        }
    }
}
