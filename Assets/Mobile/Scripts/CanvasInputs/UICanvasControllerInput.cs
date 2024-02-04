using UnityEngine;

    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public PlayerInputManager playerInput;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            playerInput.move = virtualMoveDirection;
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            playerInput.look = virtualLookDirection;
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            playerInput.jump = virtualJumpState;
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            playerInput.sprint = virtualSprintState;
        }

        public void VirtualArrestInput(bool virtualArrestState)
        {
            playerInput.arrest = virtualArrestState;
        }

}

