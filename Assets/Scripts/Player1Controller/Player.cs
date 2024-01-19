using Fusion;
using UnityEngine;

    public enum PlayerRole
    {
        Hacker,
        Thief
    }


public class Player : NetworkBehaviour
{

    public enum AnimationState
    {
        Idle,
        Walking
    }


    [SerializeField] private Ball _prefabBall;
    [SerializeField] private Animator _animator; // Reference to the Animator component

    [Networked] private TickTimer delay { get; set; }
    [Networked] public PlayerRole Role { get; set; }
    [Networked] public AnimationState CurrentAnimationState { get; set; } // Networked animation state

    private NetworkCharacterController _cc;
    private Vector3 _forward;
    [SerializeField] private GameObject _hackerModel;
    [SerializeField] private GameObject _thiefModel;
    [SerializeField] private Animator _hackerAnimator;
    [SerializeField] private Animator _thiefAnimator;
    private GameObject _activeModel;
    private Animator _activeAnimator;
    private AnimationState _predictedAnimationState; // Predicted animation state on the client side
    [Networked] public bool IsHost { get; set; }

private void Awake()
{
    _cc = GetComponent<NetworkCharacterController>();
    _forward = transform.forward;

}

private void UpdateActiveModelAndAnimator()
{

    if (Role == PlayerRole.Hacker)
    {
        _activeModel = _hackerModel;
        _activeAnimator = _hackerAnimator;
    }
    else if (Role == PlayerRole.Thief)
    {
        _activeModel = _thiefModel;
        _activeAnimator = _thiefAnimator;
    }

    // Ensure only the active model is enabled
    _hackerModel.SetActive(Role == PlayerRole.Hacker);
    _thiefModel.SetActive(Role == PlayerRole.Thief);
}

public override void Spawned()
{
    base.Spawned();

    // Set the player's role based on whether the player has state authority
    
    Role = PlayerRole.Hacker;

    // Debug.Log("HasStateAuthority: " + HasStateAuthority);
    // Debug.Log("Role: " + Role);
    // IsHost = Runner.IsServer && HasStateAuthority;
    // Update the active model and animator based on the player's role
    UpdateActiveModelAndAnimator();
}

public override void FixedUpdateNetwork()
{
    if (GetInput(out NetworkInputData data))
    {
        data.direction.Normalize();

        // Update the animation state based on the player's movement
        CurrentAnimationState = data.direction.sqrMagnitude > 0 ? AnimationState.Walking : AnimationState.Idle;

        // Predict the animation state on the client side
        _predictedAnimationState = CurrentAnimationState;

        // Different control schemes for Hacker and Thief
        if (Role == PlayerRole.Hacker)
        {
            // Hacker control scheme
        }
        else if (Role == PlayerRole.Thief)
        {
            // Thief control scheme
        }

        _cc.Move(5 * data.direction * Runner.DeltaTime);

        if (data.direction.sqrMagnitude > 0)
            _forward = data.direction;

        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
        {
            if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
            {
                delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                Runner.Spawn(_prefabBall,
                transform.position + _forward, Quaternion.LookRotation(_forward),
                Object.InputAuthority, (runner, o) =>
                {
                    // Initialize the Ball before synchronizing it
                    o.GetComponent<Ball>().Init();
                });
            }
        }
    }

    UpdateActiveModelAndAnimator();
}

private void Update()
{
    // Apply the networked animation state to the animator for all clients
    AnimationState animationState = CurrentAnimationState;

    switch (animationState)
    {
        case AnimationState.Idle:
            _activeAnimator.SetBool("IsWalking", false);
            break;
        case AnimationState.Walking:
            _activeAnimator.SetBool("IsWalking", true);
            break;
    }
}

}