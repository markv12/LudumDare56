using System;
using UnityEngine;
using UnityStandardAssets.Utility;

#pragma warning disable 618, 649
namespace UnityStandardAssets.Characters.FirstPerson {
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour {
        private bool isWalking = true;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        private bool isCrouching = false;
        [SerializeField] private float crouchSpeed;
        private float normalHeight;
        [SerializeField] private float crouchHeight;
        private float CurrentSpeed => CrouchLevel >= 0.99f ? crouchSpeed : (isWalking ? walkSpeed : runSpeed);

        [SerializeField][Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        private float jumpsRemaining;
        private float wallJumpsRemaining;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private FPSSounds fpsSounds;
        [SerializeField] private Transform mainT;
        [SerializeField] private Transform cameraT;
        [SerializeField] private TouchScreenMoveAndLook moveAndLook;
        public WallHitManager wallHitManager;

        private bool hasJumpInput;
        private Vector3 moveDir = Vector3.zero;
        private Coroutine wallJumpRoutine = null;
        private Vector3 wallJumpMoveDir = Vector3.zero;
        private Coroutine slideRoutine = null;
        private Vector3 slideMoveDir = Vector3.zero;
        private CharacterController charController;
        private CollisionFlags lastCollisionFlag;
        private bool prevGrounded = true;
        private float m_StepCycle;
        private float m_NextStep;
        private bool isJumping;

        private void Start() {
            charController = GetComponent<CharacterController>();
            normalHeight = charController.height;
            m_HeadBob.Setup(cameraT, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            isJumping = false;
            ResetMouseLook();
        }

        public void ResetMouseLook() {
            m_MouseLook.Init(mainT, cameraT);
        }

        private void Update() {
            if (InputUtil.IsMobile) {
                if (moveAndLook.HasMoveInput && moveAndLook.moveInput.sqrMagnitude > 3) {
                    ApplyMove(CurrentSpeed, moveAndLook.moveInput.normalized);
                }
            } else {
                ApplyMove(CurrentSpeed, GetInputDirection());
                m_MouseLook.UpdateCursorLock();
            }

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            if (InputUtil.ToggleRun.WasPressed) {
                isWalking = !isWalking;
            }
#endif
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!hasJumpInput && InputUtil.Jump.WasPressed) {
                if (!charController.isGrounded) {
                    if (wallHitManager.TryGetWallHit(out Vector3 wallHitNormal) && wallJumpsRemaining > 0) {
                        wallJumpsRemaining--;
                        DoWallJump(wallHitNormal);
                    } else if (jumpsRemaining > 0) {
                        hasJumpInput = true;
                    }
                } else {
                    hasJumpInput = true;
                }
            }
            isCrouching = InputUtil.Crouch.IsPressed;
            CrouchLevel += (isCrouching ? 5f : -5f) * Time.deltaTime;

            if (!prevGrounded && charController.isGrounded) {
                HandleLanding();
            }
            if (!charController.isGrounded && !isJumping && prevGrounded) {
                moveDir.y = 0f;
            }

            prevGrounded = charController.isGrounded;
        }

        private void HandleLanding() {
            StartCoroutine(m_JumpBob.DoBobCycle());
            if (Time.timeSinceLevelLoad > 0.5f) {
                fpsSounds.PlayLandingSound();
                m_NextStep = m_StepCycle + .5f;
            }
            moveDir.y = 0f;
            isJumping = false;
        }

        private void DoWallJump(Vector3 wallHitNormal) {
            moveDir.y = Mathf.Max(0, moveDir.y);
            this.EnsureCoroutineStopped(ref wallJumpRoutine);
            fpsSounds.PlayJumpSound();
            Vector3 jumpDir = Vector3.Lerp(wallHitNormal, Vector3.up, 0.5f) * 25;
            wallJumpRoutine = this.CreateWorldAnimRoutine(1f, (float progress) => {
                wallJumpMoveDir = Vector3.Lerp(jumpDir, Vector3.zero, progress);
            });
        }

        private void ApplyMove(float speed, Vector3 direction) {
            Vector3 groundMove = GetMoveRelativeToGround(direction);
            moveDir.x = groundMove.x * speed;
            moveDir.z = groundMove.z * speed;

            if (charController.isGrounded) {
                moveDir.y = -10;
                jumpsRemaining = 2;
                wallJumpsRemaining = 3;
            } else {
                moveDir += m_GravityMultiplier * Time.deltaTime * Physics.gravity;
            }
            if (hasJumpInput) {
                jumpsRemaining--;
                moveDir.y = m_JumpSpeed;
                fpsSounds.PlayJumpSound();
                hasJumpInput = false;
                isJumping = true;
            }
            if (charController.enabled) {
                lastCollisionFlag = charController.Move((moveDir + wallJumpMoveDir + slideMoveDir) * Time.deltaTime);
            }

            ProgressStepCycle(groundMove, speed);
            UpdateCameraPosition(speed);
        }

        private Vector3 GetMoveRelativeToGround(Vector3 direction) {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 result = mainT.forward * direction.y + mainT.right * direction.x;
            // get a normal for the surface that is being touched to move along it
            Physics.SphereCast(mainT.position, charController.radius, Vector3.down, out RaycastHit hitInfo,
                               charController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            result = Vector3.ProjectOnPlane(result, hitInfo.normal).normalized;
            return result;
        }

        private void ProgressStepCycle(Vector2 direction, float speed) {
            if (charController.velocity.sqrMagnitude > 0 && direction.sqrMagnitude > 0) {
                m_StepCycle += (charController.velocity.magnitude + (speed * (isWalking ? 1f : m_RunstepLenghten))) *
                             Time.deltaTime;
            }
            if (m_StepCycle > m_NextStep) {
                m_NextStep = m_StepCycle + m_StepInterval;
                if (charController.isGrounded) {
                    fpsSounds.PlayFootStepAudio();
                }
            }
        }

        private void UpdateCameraPosition(float speed) {
            Vector3 newCameraPosition;
            if (charController.velocity.magnitude > 0 && charController.isGrounded) {
                cameraT.localPosition =
                    m_HeadBob.DoHeadBob(charController.velocity.magnitude +
                                        (speed * (isWalking ? 1f : m_RunstepLenghten)), CameraPosition);
                newCameraPosition = cameraT.localPosition;
                newCameraPosition.y = cameraT.localPosition.y - m_JumpBob.Offset();
            } else {
                newCameraPosition = cameraT.localPosition;
                newCameraPosition.y = CameraPosition.y - m_JumpBob.Offset();
            }
            cameraT.localPosition = newCameraPosition;
        }

        private Vector2 GetInputDirection() {
            Vector2 direction = new Vector2(InputUtil.GetHorizontal(), InputUtil.GetVertical());
            if (direction.sqrMagnitude > 1) {
                direction.Normalize();
            }
            return direction;
        }

        private void RotateView() {
            if (Time.timeSinceLevelLoad > 0.5f) {
                m_MouseLook.LookRotation();
            }
        }

        private float crouchLevel = 0;
        private float CrouchLevel {
            get { return crouchLevel; }
            set {
                value = Mathf.Max(0, Mathf.Min(1, value));
                if (value != crouchLevel) {
                    crouchLevel = value;
                    float height = Mathf.Lerp(normalHeight, crouchHeight, crouchLevel);
                    charController.height = height;
                    charController.center = new Vector3(0, height * 0.5f, 0);
                    charController.radius = Mathf.Min(0.5f, height * 0.5f);
                    cameraT.localPosition = CameraPosition;
                }
            }
        }
        Vector3 CameraPosition => new Vector3(0, charController.height - 0.15f, 0);

        private void OnControllerColliderHit(ControllerColliderHit hit) {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (lastCollisionFlag != CollisionFlags.Below && body != null && !body.isKinematic) {
                body.AddForceAtPosition(charController.velocity * 0.1f, hit.point, ForceMode.Impulse);
            }
        }

        public void ResetCameraRotation() {
            cameraT.localRotation = Quaternion.identity;
            m_MouseLook.ResetTargetRotations();
        }

        private void OnDisable() {
            moveAndLook.CancelDrag();
        }
    }
}
