using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("movement data")]
    public float normalMovementSpeed = 10;
    public float rollMovementSpeed = 20;

    [Header("References")]
    [SerializeField] Transform gunTransform;
    InputActions inputActions;
    Rigidbody2D rigidbody;
    Animator animator;
    [SerializeField] SpriteRenderer playerSprite;

    Camera mainCam;



    Vector2 moveDir = Vector2.zero;
    float moveSpeed;

    #region Monobehaviour

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            inputActions = new InputActions();
            moveSpeed = normalMovementSpeed;
            mainCam = Camera.main;            
        }
        void OnEnable()
        {
            inputActions.Player.movement.performed += PlayerMovementHandler;
            inputActions.Player.movement.canceled += PlayerMovementStoppedHandler;
            inputActions.Player.roll.performed += PlayerRolledHandler;
            inputActions.Player.Aim.performed += PlayerAimHandler;
            inputActions.Player.Enable();
        }


        void OnDisable()
        {
            inputActions.Player.movement.performed -= PlayerMovementHandler;
            inputActions.Player.movement.canceled -= PlayerMovementStoppedHandler;
            inputActions.Player.roll.performed -= PlayerRolledHandler;
            inputActions.Player.Aim.performed -= PlayerAimHandler; 
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

        private void PlayerRolledHandler(InputAction.CallbackContext context)
        {
           animator.SetTrigger("roll");
           moveSpeed = rollMovementSpeed;
        }

        //called by animation event in roll clip
        public void PlayerRollCompleteHandler(){
            moveSpeed = normalMovementSpeed;
        }

        private void PlayerMovementHandler(InputAction.CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>().normalized;
        }

        private void PlayerAimHandler(InputAction.CallbackContext context)
        {
            Vector2 mousePos = context.ReadValue<Vector2>();
            Vector3 screenPos = mainCam.ScreenToWorldPoint(mousePos);
            Vector2 aimDirection = (screenPos-transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y,aimDirection.x)*Mathf.Rad2Deg;
            if(aimDirection.x >= 0){
                playerSprite.transform.localEulerAngles =  new Vector3(0,0,0);
                gunTransform.eulerAngles = new Vector3(0,0,angle);
            }
            else{
                playerSprite.transform.localEulerAngles =  new Vector3(0,180,0);
                gunTransform.eulerAngles = new Vector3(180,0,-angle);
            }
            
        }


        private void PlayerMovementStoppedHandler(InputAction.CallbackContext context)
        {
            moveDir = Vector2.zero;
        }


    #endregion

    #region Animation
        void HandleAnimations(){
            
            animator.SetFloat("speed",moveSpeed*moveDir.magnitude);
        }
    #endregion
}
