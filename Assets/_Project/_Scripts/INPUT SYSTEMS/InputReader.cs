using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour {
    [SerializeField] LayerMask layerMask;
    Camera mainCamera;
    PlayerInput playerInput;
    bool isPressed;

    void Awake() {
        mainCamera = Camera.main;
        playerInput = new PlayerInput();
    }

    void OnEnable() {
        playerInput.Player.Enable();
        playerInput.Player.Tap.started += OnTapStarted;
    }

    void OnTapStarted( InputAction.CallbackContext context ) {
        Vector2 tapPosition = Pointer.current.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3( tapPosition.x, tapPosition.y, mainCamera.nearClipPlane ) );

        CheckForInteractiveObjects( worldPosition );
    }

    private void CheckForInteractiveObjects( Vector3 worldPosition ) {
        RaycastHit2D[] hits = Physics2D.RaycastAll( worldPosition, Vector2.zero, Mathf.Infinity, layerMask );
        int maxInteractions = Mathf.Min( 2, hits.Length );
        for ( int i = 0 ; i < maxInteractions ; i++ ) {
            InteractiveObject interactiveObject = hits[ i ].collider.GetComponent<InteractiveObject>();
            if ( interactiveObject != null ) {
                interactiveObject.Interact();
            }
        }
        //RaycastHit2D hit = Physics2D.Raycast( worldPosition, Vector2.zero );
        //if ( hit.collider != null ) {
        //    InteractiveObject interactiveObject = hit.collider.GetComponent<InteractiveObject>();
        //    if ( interactiveObject != null ) {
        //        interactiveObject.Interact();
        //    }
        //}
    }

    void OnDisable() {
        playerInput.Player.Disable();
        playerInput.Player.Tap.started -= OnTapStarted;
    }
}