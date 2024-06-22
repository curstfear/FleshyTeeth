using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startState;
    [SerializeField] private float _roamingDistanceMax = 8f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    private float _roamingTime;
    private NavMeshAgent _navMeshAgent;
    private State _state;
    private Vector3 _roamingPosition;
    private Vector3 _startPosition;

    private enum State
    {
        Idle,
        Roaming,
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _state = startState;
    }

    private void Update()
    {
        switch (_state)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                _roamingTime -= Time.deltaTime;
                if(_roamingTime <= 0)
                {
                    Roaming();
                    _roamingTime = _roamingTimerMax;
                }
                break;
        }
    }

    private void Roaming()
    {
        _roamingPosition = GetRoamingPosition();
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

}
