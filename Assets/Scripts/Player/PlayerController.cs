using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("movement data")]
    public float normalMovementSpeed = 10;
    public float rollMovementSpeed = 20;

    [Header("References")]
    [SerializeField] Transform gunPivotTransform;
    public Text BulletCountText;
    public SpriteRenderer CrossHair;
    InputActions inputActions;
    Rigidbody2D rigidbody;
    Animator animator;
    Gun equipedGun;
    [SerializeField] Transform playerSpriteParent;

    Camera mainCam;

    bool fireButtonHeld = false;


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
            equipedGun = gunPivotTransform.GetComponentInChildren<Gun>();
        }
        void OnEnable()
        {
            inputActions.Player.movement.performed += PlayerMovementHandler;
            inputActions.Player.movement.canceled += PlayerMovementStoppedHandler;
            inputActions.Player.roll.performed += PlayerRolledHandler;
            inputActions.Player.Aim.performed += PlayerAimHandler;
            inputActions.Player.Shoot.performed += PlayerShootHandler;
            inputActions.Player.Shoot.canceled += PlayerShootReleaseHandler;
            inputActions.Player.reload.performed += PlayerReloadHandler;
            inputActions.Player.Enable();
            BulletCountText.text = equipedGun.BulletsInClip.ToString();
        }


        void OnDisable()
        {
            inputActions.Player.movement.performed -= PlayerMovementHandler;
            inputActions.Player.movement.canceled -= PlayerMovementStoppedHandler;
            inputActions.Player.roll.performed -= PlayerRolledHandler;
            inputActions.Player.Aim.performed -= PlayerAimHandler;
            inputActions.Player.Shoot.performed -= PlayerShootHandler;
            inputActions.Player.Shoot.canceled -= PlayerShootReleaseHandler;
            inputActions.Player.reload.performed -= PlayerReloadHandler; 
            inputActions.Player.Disable();
        }

   
        void Update()
        {
            HandleAnimations();
            if(fireButtonHeld){
                if(equipedGun!=null){
                    equipedGun.Fire();
                    BulletCountText.text = equipedGun.BulletsInClip.ToString();
                }
            }
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
            gunPivotTransform.gameObject.SetActive(true);
            inputActions.Player.Shoot.Enable();
        }

        private void PlayerMovementHandler(InputAction.CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>().normalized;
        }

        private void PlayerReloadHandler(InputAction.CallbackContext context)
        {
            if(equipedGun!=null){
                equipedGun.Reload();
                BulletCountText.text = "Reloading";
                StartCoroutine(FinishReloading());
            }
        }

        IEnumerator FinishReloading(){
            while(equipedGun.Reloading){
                yield return null;
            }
            BulletCountText.text = equipedGun.BulletsInClip.ToString();
        }

        private void PlayerShootHandler(InputAction.CallbackContext context)
        {
            fireButtonHeld = true;
        }
        private void PlayerShootReleaseHandler(InputAction.CallbackContext context)
        {
            fireButtonHeld = false;
        }


        private void PlayerAimHandler(InputAction.CallbackContext context)
        {
            if(mainCam == null){
                mainCam = Camera.main;
            }
            Vector2 mousePos = context.ReadValue<Vector2>();
            Vector3 screenPos = mainCam.ScreenToWorldPoint(mousePos);
            CrossHair.transform.position = new Vector3(screenPos.x,screenPos.y,0);
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
