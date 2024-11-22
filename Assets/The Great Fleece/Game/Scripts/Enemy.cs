using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Animator _enemyAnimator;

    [SerializeField]
    private Player _playertodetect;

    public Vector3 _startPosition;
    public Vector3 _endPosition;

    [SerializeField]
    private float _xOffSet;
    [SerializeField]
    private float _zOffSet;

    private NavMeshAgent _guard;
    private bool _stillInRange = false;







    // Start is called before the first frameupdate
    void Start()
    {
        _guard = GetComponent<NavMeshAgent>();
        _guard.SetDestination(_endPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Activation();
    }

    private void Movement()
    {
        if (this.transform.position == _startPosition)
        {
            _enemyAnimator.SetBool("Walk", false);
            StartCoroutine(waitAndMove(_endPosition));
        }
        if (this.transform.position == _endPosition)
        {
            _enemyAnimator.SetBool("Walk", false);
            StartCoroutine(waitAndMove(_startPosition));
        }
    }

    private IEnumerator waitAndMove(Vector3 _position)
    {
        yield return new WaitForSeconds(Random.Range(3.0f,7.0f));
        _enemyAnimator.SetBool("Walk", true);
        _guard.SetDestination(_position);
    }

    private void Activation()
    {
        Vector3 _xOfEnemy = new Vector3(transform.position.x, 0f, 0f);
        Vector3 _zOfEnemy = new Vector3(0f, 0f, transform.position.z);
        Vector3 _xOfPlayer = new Vector3(_playertodetect.transform.position.x, 0f, 0f);
        Vector3 _zOfPlayer = new Vector3(0, 0f, _playertodetect.transform.position.z);

        float _xDetection = Vector3.Distance(_xOfEnemy, _xOfPlayer);
        float _zDetection = Vector3.Distance(_zOfEnemy, _zOfPlayer);

        if (_xDetection <= _xOffSet && _zDetection <= _zOffSet)
        {
            _enemyAnimator.SetBool("InRange", true);
            _guard.isStopped = true;
            StartCoroutine(waitBeforeCapture(_xOfEnemy,_zOfEnemy,_playertodetect));
            if (_stillInRange)
            {
                _enemyAnimator.SetBool("Catch", true);
                _guard.isStopped = false;
                _guard.speed = 4.0f;
                _guard.SetDestination(_playertodetect.transform.position);
            }
        }
        else
        {
            _guard.isStopped = false;
            _enemyAnimator.SetBool("InRange", false);
        }
    }

    IEnumerator waitBeforeCapture(Vector3 x,Vector3 y,Player _playerDetected)
    {
        yield return new WaitForSeconds(5.0f);
        Vector3 _xOfEnemy = new Vector3(transform.position.x, 0f, 0f);
        Vector3 _zOfEnemy = new Vector3(0f, 0f, transform.position.z);
        Vector3 _xOfPlayer = new Vector3(_playerDetected.transform.position.x, 0f, 0f);
        Vector3 _zOfPlayer = new Vector3(0, 0f, _playerDetected.transform.position.z);

        float _xDetection = Vector3.Distance(_xOfEnemy, _xOfPlayer);
        float _zDetection = Vector3.Distance(_zOfEnemy, _zOfPlayer);

        if (_xDetection <= _xOffSet && _zDetection <= _zOffSet) {
            _stillInRange = true;
        }
        else
        {
            _stillInRange = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _enemyAnimator.SetBool("Catch", false);
            other.transform.position = new Vector3(0f, 0f, 0f);
            _guard.isStopped = true;
            
        }
    }


}
