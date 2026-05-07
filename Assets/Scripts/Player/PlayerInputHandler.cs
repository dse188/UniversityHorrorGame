using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{

    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }


        // InputSystem
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }
    }

}