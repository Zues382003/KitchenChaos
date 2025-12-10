using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private bool _isWalking;
    private Vector3 lastInteractionDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of Player cannot have more than one Player!");
        }
        Instance = this;
    }
    
    private void Start()
    {
        _gameInput.OnInteractAction += GameInputOn_onInteractAction;
        _gameInput.OnInteractAlternateAction += GameInputOn_InteractAlternateAction;
        
    }

    private void GameInputOn_InteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }    
    }

    void GameInputOn_onInteractAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    
    private void Update()
    {
        handleMovement();
        handleInteractions();
    }
    
    public bool IsWalking()
    {
        return _isWalking;
    }

    private void handleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMomentVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }
        
        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit rayCastHit, interactionDistance, countersLayerMask))
        {
            if (rayCastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }else
            {
                SetSelectedCounter(null);
            }
        } else
        {
            SetSelectedCounter(null);

        }
    }

    private void handleMovement()
    {
        Vector2 inputVector = _gameInput.GetMomentVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 rotateDir = moveDir;


        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerHigh = 2f;
        float playerRadius = .7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh,
            playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //cannot move to toward Dir
            //Attempt only X moment
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh,
                playerRadius, moveDirX, moveDistance);

            if (canMove)
            { 
                //Can move only on X
                moveDir = moveDirX;
            }
            else
            {
                //cannot move to X Dir
                //Attempt only Z moment
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh,
                    playerRadius, moveDirZ, moveDistance);
                
                if (canMove)
                { 
                    //Can move only on Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move any in Dir
                }
            }
        }
        
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        
        _isWalking = inputVector != Vector2.zero;

        float _rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, rotateDir, _rotateSpeed * Time.deltaTime);
    }
    
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        { 
            selectedCounter =  selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetkitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void RemoveKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
