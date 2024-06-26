using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Vector3 _startPosition; // начальное положение
    [SerializeField] private float _roamingDistanceMin = 3f; // минимальная дистанция, которую может преодолеть NPC
    [SerializeField] private float _roamingDistanceMax = 8f; // максимальная дистанция, которую может преодолеть NPC
    private Vector3 _roamingPosition; //точка, в которую должен прийти NPC
    private float _roamingSpeed;
    private float _roamingTimer;
    [SerializeField] private float _roamingTimerMax = 2f; // максимальное кол-во времени для преодоления дистанции NPC (в сек.)

    [SerializeField] private State _startState; // начальное состояние
    private State _currentState; // текущее состояние
    [SerializeField] private bool _isChasingEnemy = false; // враг преследующий или нет
    [SerializeField] private float _chasingDistance = 8f; // дистанция от NPC до игрока, чтоб началось преследование
    private float _chasingSpeed;
    [SerializeField] private float _chasingSpeedMultiplayer = 1.5f; //ускорение NPC при преследовании

    private NavMeshAgent _navMeshAgent;

    private enum State
    {
        Roaming,
        Chasing,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplayer;
    }

    private void Update()
    {
        StateHandler();
    }
    
    //машина состояний
    private void StateHandler()
    {
        switch (_currentState)
        {
            default:
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer <= 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChasingCharacter();
                CheckCurrentState();
                break;

            case State.Death:
                break;
        }
    }

    private void CheckCurrentState()
    {
        float distanceToCharacter = Vector3.Distance(transform.position, Character.Instance.transform.position); // дистанция до игрока
        Debug.Log($"Distance to Character: {distanceToCharacter}");
        State newState = State.Roaming;

        if (_isChasingEnemy && distanceToCharacter <= _chasingDistance) // если NPC преследующий и расстояние до игрока меньше или равно дистанции для преследования, то
        {
            newState = State.Chasing;
            Debug.Log("Switching to Chasing State");
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            _currentState = newState;
        }
    }

    private void ChasingCharacter()
    {
        Vector3 _characterChaseTarget = Character.Instance.transform.position; // где находится персонаж в пространстве
        _navMeshAgent.SetDestination(_characterChaseTarget);
        ChangeFaceDirection(_startPosition, _characterChaseTarget);
    }

    private void Roaming()
    {
        _startPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        ChangeFaceDirection(_startPosition, _roamingPosition);
        _navMeshAgent.SetDestination(_roamingPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startPosition + GetRandomDirection() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void ChangeFaceDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x < targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
