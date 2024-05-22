using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    [SerializeField] private float _speed = 0.15f;

    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveInputX, moveInputY) * _speed;
        transform.position += movement;
    }
}
