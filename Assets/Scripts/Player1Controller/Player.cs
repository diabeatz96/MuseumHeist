using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public enum PlayerRole
    {
        Hacker,
        Thief
    }

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

    private AnimationState _predictedAnimationState; // Predicted animation state on the client side

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _animator = GetComponentInChildren<Animator>(); // Get the Animator component from the child object
        _forward = transform.forward;
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
    }

    private void Update()
    {
        // Apply the predicted animation state to the animator for the local client
        // and the networked animation state for all other clients
        AnimationState animationState = HasInputAuthority ? _predictedAnimationState : CurrentAnimationState;

        switch (animationState)
        {
            case AnimationState.Idle:
                _animator.SetBool("IsWalking", false);
                break;
            case AnimationState.Walking:
                _animator.SetBool("IsWalking", true);
                break;
        }
    }
}