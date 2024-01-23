using System;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float turnSpeed = 5f; // The speed at which the player turns

    [SerializeField] private GameObject _hackerModel;
    [SerializeField] private GameObject _thiefModel;
    [SerializeField] private Animator _hackerAnimator;
    [SerializeField] private Animator _thiefAnimator;
    public GameObject gameController;
    private GameObject _activeModel;
    private Animator _activeAnimator;
    public GameObject trophy;
    public GameObject trophyBack;
    public Light spotlight; // The spotlight attached to the player
    public GameObject ThiefLight; // The spotlight attached to the Hacker
    private AnimationState _predictedAnimationState; // Predicted animation state on the client side
    [Networked] public bool IsHost { get; set; }
    bool hasSpawned = false;
public SwitchCameras switchCameras; // The SwitchCameras instance
private void Awake()
{
    _cc = GetComponent<NetworkCharacterController>();
    _forward = transform.forward;
}

public void Start() {
     // This script is assumed to be attached to the player prefab
    NetworkObject networkObject = GetComponent<NetworkObject>();

    Debug.Log("NetworkObject: " + networkObject);
    // Check if this client has input authority over the network object
    if (networkObject.HasInputAuthority)
    {
        // Find the player camera in the scene
        // This assumes you have a script named PlayerCamera with a SetCameraTarget method
        PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
        Debug.Log("PlayerCamera: " + playerCamera);
        // Set the camera target
        playerCamera.SetCameraTarget(transform);
    }

    if(spotlight == null) {
        Debug.Log("Spotlight is null");
        spotlight = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();
    }

    if (gameController == null) {
        Debug.Log("GameController is null");
        gameController = GameObject.Find("GameManager");
    }

    if(trophy == null) {
        Debug.Log("Trophy is null");
        trophy = GameObject.FindGameObjectWithTag("Trophy");
    }

    
    ThiefLight = GameObject.FindGameObjectWithTag("ThiefLight");
   
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
    
    Debug.Log("Spawned");
    Role = PlayerRole.Hacker;
    hasSpawned = true;
    Debug.Log("Role: " + Role);

    // Debug.Log("HasStateAuthority: " + HasStateAuthority);
    // Debug.Log("Role: " + Role);
    // IsHost = Runner.IsServer && HasStateAuthority;
    // Update the active model and animator based on the player's role
    UpdateActiveModelAndAnimator();
}


public override void FixedUpdateNetwork()
{
    if (!hasSpawned) {
        Debug.Log("Has not spawned");
        return;
    }

    if (GetInput(out NetworkInputData data))
    {
        data.direction.Normalize();

        if (data.direction.x != 0 || data.direction.z != 0 || data.direction.y != 0)
        {
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }

        // // Move this outside of the above if condition
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     _cc.Jump();
        // }

        if (data.direction.sqrMagnitude > 0)
            _forward = data.direction;

        CurrentAnimationState = data.direction.sqrMagnitude > 0 ? AnimationState.Walking : AnimationState.Idle;
        _predictedAnimationState = CurrentAnimationState;

        UpdateActiveModelAndAnimator();   
    }
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

     // Different control schemes for Hacker and Thief
    
        if(gameController.GetComponent<GameController>().opStat == OperationStatus.Finished) {
            
            return;
        }

        else if (Role == PlayerRole.Thief)
        {
         // Set the position of the Thief's light to be above the Thief player's head
            if(ThiefLight == null) {
                Debug.Log("ThiefLight is null");
                ThiefLight = GameObject.FindGameObjectWithTag("ThiefLight");
            }

            ThiefLight.transform.position = transform.position + new Vector3(0, 10, 0); // Adjust the y value to position the light above the player's head
        }



        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(Runner.IsSceneAuthority) {
             Runner.LoadScene(SceneRef.FromIndex(1), LoadSceneMode.Additive);
             Runner.UnloadScene(SceneRef.FromIndex(0));
        }
    }
}

private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Artifact")
        {

            trophy.SetActive(false);
            trophyBack.SetActive(true);
            gameController.GetComponent<GameController>().hasArtifact = true;
        }
        else if (other.gameObject.tag == "Escape")
        {
            Debug.Log("NO ARTIFACT");
            if (gameController.GetComponent<GameController>().hasArtifact)
            {
                transform.position = gameController.GetComponent<GameController>().baseSpawner.transform.position;
                Debug.Log("Escaped");
                Debug.Log("TELEPORT");
                gameController.GetComponent<GameController>().hasArtifact = false;
                gameController.GetComponent<GameController>().hasEscaped = true;
                gameController.GetComponent<GameController>().Escaped(); 
            }
        }
    }

}