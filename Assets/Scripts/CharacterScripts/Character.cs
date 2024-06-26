using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance { get; private set; }
    [SerializeField] private float _speed = 7f;
    private CharacterInputActions _characterInputActions;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _characterInputActions = new CharacterInputActions();
        _characterInputActions.Enable();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one Character instance found!");
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    //передвижение
    private void Movement()
    {
        Vector2 inputVector = GetMovementVector();
        _rigidBody.MovePosition(_rigidBody.position + inputVector * (_speed * Time.fixedDeltaTime));
        if(inputVector.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(inputVector.x), 1);
        }
    }

    //получение вектора движения
    private Vector2 GetMovementVector()
    {
        Vector2 inputVector = _characterInputActions.Character.Movement.ReadValue<Vector2>();
        return inputVector;
    }
}
