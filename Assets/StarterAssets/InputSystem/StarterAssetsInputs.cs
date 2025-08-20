using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		private Vector2 _move;
		private Vector2 _look;
		private bool _jump;
		private bool _sprint;
		public Vector2 move => CanGetInput ? _move : Vector2.zero;
		public Vector2 look => CanGetInput ? _look : Vector2.zero;
		public bool jump { get => CanGetInput ? _jump : false; set => _jump &= value; }
		public bool sprint => CanGetInput ? _sprint : false;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public static bool CanGetInput { get; set; } = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			_move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			_look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			_jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			_sprint = newSprintState;
		}

#if false
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
#endif
	}

}