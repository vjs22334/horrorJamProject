using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("movement data")]
    public float normalMovementSpeed = 10; // normal movement, noisy
    public float quietMovementSpeed = 2.5f; // for sneaking

    [Header("References")]
    InputActions inputActions;
    Rigidbody2D rigidbody;
    Animator animator;

    Vector2 moveDir = Vector2.zero;
    float moveSpeed;

    #region Monobehaviour

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            inputActions = new InputActions();
            moveSpeed = normalMovementSpeed;            
        }
        void OnEnable()
        {
            inputActions.Player.movement.performed += PlayerMovementHandler;
            inputActions.Player.movement.canceled += PlayerMovementStoppedHandler;
            inputActions.Player.Sneak.performed += SneakHandler;
            inputActions.Player.Sneak.canceled += SneakCanceledHandler;
            inputActions.Player.Enable();
        }


        void OnDisable()
        {
            inputActions.Player.movement.performed -= PlayerMovementHandler;
            inputActions.Player.movement.canceled -= PlayerMovementStoppedHandler;
            inputActions.Player.Sneak.performed -= SneakHandler;
            inputActions.Player.Sneak.canceled -= SneakCanceledHandler;
            inputActions.Player.Disable();
        }

        void Update()
        {
            HandleAnimations();
        }

    
        void FixedUpdate()
        {
            rigidbody.velocity = moveDir*moveSpeed;
        }
    #endregion

    #region InputHandlers
        private void PlayerMovementHandler(InputAction.CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>().normalized;
        }

        private void PlayerMovementStoppedHandler(InputAction.CallbackContext context)
        {
            moveDir = Vector2.zero;
        }

        private void SneakCanceledHandler(InputAction.CallbackContext context)
        {
            moveSpeed = normalMovementSpeed;
        }

        private void SneakHandler(InputAction.CallbackContext context)
        {
            moveSpeed = quietMovementSpeed;
        }

    #endregion

    #region Animation
        void HandleAnimations(){
            if(moveDir.magnitude!=0){
                animator.SetFloat("moveX",moveDir.x);
                animator.SetFloat("moveY",moveDir.y);
            }
            animator.SetFloat("speed",moveSpeed*moveDir.magnitude);
        }
    #endregion
}
