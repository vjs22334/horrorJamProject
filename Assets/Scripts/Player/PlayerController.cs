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
    [SerializeField] Transform gunPivotTransform;
    InputActions inputActions;
    Rigidbody2D rigidbody;
    Animator animator;
    IGun equipedGun;
    [SerializeField] Transform playerSpriteParent;

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
            equipedGun = gunPivotTransform.GetComponentInChildren<IGun>();         
        }
        void OnEnable()
        {
            inputActions.Player.movement.performed += PlayerMovementHandler;
            inputActions.Player.movement.canceled += PlayerMovementStoppedHandler;
            inputActions.Player.roll.performed += PlayerRolledHandler;
            inputActions.Player.Aim.performed += PlayerAimHandler;
            inputActions.Player.Shoot.performed += PlayerShootHandler;
            inputActions.Player.reload.performed += PlayerReloadHandler;
            inputActions.Player.Enable();
        }


        void OnDisable()
        {
            inputActions.Player.movement.performed -= PlayerMovementHandler;
            inputActions.Player.movement.canceled -= PlayerMovementStoppedHandler;
            inputActions.Player.roll.performed -= PlayerRolledHandler;
            inputActions.Player.Aim.performed -= PlayerAimHandler;
            inputActions.Player.Shoot.performed -= PlayerShootHandler;
            inputActions.Player.reload.performed -= PlayerReloadHandler; 
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
           gunPivotTransform.gameObject.SetActive(false);
           inputActions.Player.Shoot.Disable();
        }

        //called by animation event in roll clip
        public void PlayerRollCompleteHandler(){
            moveSpeed = normalMovementSpeed;
            inputActions.Player.Shoot.Enable();
            gunPivotTransform.gameObject.SetActive(true);
        }

        private void PlayerMovementHandler(InputAction.CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>().normalized;
        }

        private void PlayerReloadHandler(InputAction.CallbackContext context)
        {
            if(equipedGun!=null){
                equipedGun.Reload();
            }
        }

        private void PlayerShootHandler(InputAction.CallbackContext context)
        {
            if(equipedGun!=null){
                equipedGun.Fire();
            }
        }

        private void PlayerAimHandler(InputAction.CallbackContext context)
        {
            Vector2 mousePos = context.ReadValue<Vector2>();
            Vector3 screenPos = mainCam.ScreenToWorldPoint(mousePos);
            Vector2 aimDirection = (screenPos-transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y,aimDirection.x)*Mathf.Rad2Deg;
            if(aimDirection.x >= 0){
                playerSpriteParent.transform.localEulerAngles =  new Vector3(0,0,0);
                gunPivotTransform.eulerAngles = new Vector3(0,0,angle);
            }
            else{
                playerSpriteParent.transform.localEulerAngles =  new Vector3(0,180,0);
                gunPivotTransform.eulerAngles = new Vector3(180,0,-angle);
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
