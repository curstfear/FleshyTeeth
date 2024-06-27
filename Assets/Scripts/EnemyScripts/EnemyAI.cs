using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Vector3 _startPosition; // ��������� ���������
    [SerializeField] private float _roamingDistanceMin = 3f; // ����������� ���������, ������� ����� ���������� NPC
    [SerializeField] private float _roamingDistanceMax = 8f; // ������������ ���������, ������� ����� ���������� NPC
    private Vector3 _roamingPosition; //�����, � ������� ������ ������ NPC
    private float _roamingSpeed;
    private float _roamingTimer;
    [SerializeField] private float _roamingTimerMax = 2f; // ������������ ���-�� ������� ��� ����������� ��������� NPC (� ���.)

    [SerializeField] private State _startState; // ��������� ���������
    private State _currentState; // ������� ���������
    [SerializeField] private bool _isChasingEnemy = false; // ���� ������������ ��� ���
    [SerializeField] private float _chasingDistance = 8f; // ��������� �� NPC �� ������, ���� �������� �������������
    private float _chasingSpeed;
    [SerializeField] private float _chasingSpeedMultiplayer = 1.5f; //��������� NPC ��� �������������

    private float _nextCheckDirectionTime = 0f; //��������� �������� � ����� ����������� �������� NPC
    private float _checkDirectionDuration = 0.05f; //������� ��� ����� ���������� � ����� ����������� �������� NPC (0.05f = 5 ��� � �������)
    private Vector3 _lastPosition; // ��������� ��������� NPC;

    private SpriteRenderer _enemySpriteRenderer;
    private NavMeshAgent _navMeshAgent;

    private enum State
    {
        Roaming,
        Chasing,
        Attacking,
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
        _enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        StateHandler();
        MovementDirection();
    }

    //������ ���������
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



    //��������� �������� ���������
    private void CheckCurrentState()
    {
        float distanceToCharacter = Vector3.Distance(transform.position, Character.Instance.transform.position); // ��������� �� ������
        State newState = State.Roaming;

        if (_isChasingEnemy && distanceToCharacter <= _chasingDistance) // ���� NPC ������������ � ���������� �� ������ ������ ��� ����� ��������� ��� �������������, ��
        {
            newState = State.Chasing;
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
                if(gameObject.tag == "Ghost") ChangeColor("red");
            }
            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
                if (gameObject.tag == "Ghost") ChangeColor("white");
            }
            _currentState = newState;
        }
    }

    //������������� ������
    private void ChasingCharacter()
    {
        Vector3 _characterChaseTarget = Character.Instance.transform.position; // ��� ��������� �������� � ������������
        _navMeshAgent.SetDestination(_characterChaseTarget);
        ChangeFaceDirection(_startPosition, _characterChaseTarget);
    }

    //��������� 
    private void Roaming()
    {
        _startPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamingPosition);
    }

    //��������� �����, � ������� ���� ���� NPC ��� ��������� ���������
    private Vector3 GetRoamingPosition()
    {
        return _startPosition + GetRandomDirection() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void MovementDirection()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (_currentState == State.Roaming)
            {
                ChangeFaceDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFaceDirection(transform.position, Character.Instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
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
    //��������� ���������� �����������
    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    // ����� �����
    private void ChangeColor(string color)
    {
        Color red = new Color(1, 0, 0, 0.5f);
        Color white = new Color(1, 1, 1, 0.5f);
        if (_enemySpriteRenderer != null)
        {
            if (color == "red")
            {
                _enemySpriteRenderer.color = red;
            }
            else if (color == "white")
            {
                _enemySpriteRenderer.color = white;
            }
        }
    }
}
