using DG.Tweening;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public event Action EndingTakeStartingPositionEvent;

    [SerializeField] private PlayerMovement _circleMovement;
    [SerializeField] private ArrowController _arrowController;
    [SerializeField] private BodyController _bodyController;

    [SerializeField] private Transform _startPoint;

    private PlayerPosition _position = PlayerPosition.Bottom;
    private bool _isOnWall = false;
    private bool _isDead = false;

    public void TakeStartingPosition()
    {
        _bodyController.SwitchToJump();
        _circleMovement.transform.DOMove(_startPoint.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _isOnWall = true;
            _bodyController.SwitchToIdle(_position);
            ChangeStartAngle();
            EndingTakeStartingPositionEvent?.Invoke();
        });
    }

    public void Death()
    {
        _isDead = true;
        _bodyController.SwitchToDeath();
        _arrowController.ChangeArrowVisibility(false);
        _circleMovement.StartCircleMovement(new Vector2(0f,-1f));
    }

    public void SwitchPositionTo(PlayerPosition position)
    {
        if(_isDead) return;

        _position = position;
        _isOnWall = true;
        _bodyController.SwitchToIdle(_position);
        ChangeStartAngle();
    }

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (_isOnWall && !_isDead && Input.GetKeyDown(KeyCode.Mouse0))
            Move();

        if (_isOnWall && !_isDead && Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                Move();
        }
    }

    private void ChangeStartAngle()
    {
        _arrowController.ChangeStartAngle(_position);
    }

    private void Move()
    {
        _isOnWall = false;
        _bodyController.SwitchToJump(_arrowController.GetArrowDirection());
        _arrowController.ChangeArrowVisibility(false);
        _circleMovement.StartCircleMovement(_arrowController.GetArrowLocalPosition());
    }

    ~PlayerManager()
    {
        if (EndingTakeStartingPositionEvent != null)
        {
            foreach (Delegate d in EndingTakeStartingPositionEvent.GetInvocationList())
                EndingTakeStartingPositionEvent -= (Action)d;
        }       
    }
}