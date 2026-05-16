using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Responsible ONLY for gathering input from the PlayerInput component.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpStarted { get; private set; }
    public bool JumpCanceled { get; private set; }

    private void OnEnable()
    {
        GetComponent<PlayerInput>().onActionTriggered += HandleAction;
    }

    private void OnDisable()
    {
        GetComponent<PlayerInput>().onActionTriggered -= HandleAction;
    }

    private void HandleAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                MoveInput = context.ReadValue<Vector2>();
                break;
            case "Jump":
                if (context.started) JumpStarted = true;
                else if (context.canceled) JumpCanceled = true;
                break;
        }
    }

    private void LateUpdate()
    {
        // Reset single-frame triggers
        JumpStarted = false;
        JumpCanceled = false;
    }
}
