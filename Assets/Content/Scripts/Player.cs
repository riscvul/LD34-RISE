using UnityEngine;
using System.Collections;
using DarkTonic.MasterAudio;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    ParticleSystem footstepParticles;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    [HideInInspector]
    public bool carryingItem;
    bool pickedUpThisFrame = false;
    [HideInInspector]
    public GameObject item;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    public void GiveItem(GameObject pItem) {
        carryingItem = true;
        item = pItem;
        pickedUpThisFrame = true;
    }

    void Start() {
        controller = GetComponent<Controller2D>();
        footstepParticles = transform.FindChild("FootstepParticles").GetComponent<ParticleSystem>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void LateUpdate() {
        if (carryingItem && Input.GetButtonDown("Interact") && !pickedUpThisFrame) {
            carryingItem = false;
            float forceX = item.transform.parent == transform.FindChild("CarryLeft") ? -5 : 5;
            item.transform.SetParent(GameObject.Find("Level").transform);
            item.GetComponent<Rigidbody2D>().isKinematic = false;

            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX,10), ForceMode2D.Impulse);
        }
        pickedUpThisFrame = false;
    }

    void Update() {

        if (!UIManager.menuShown) {

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            int wallDirX = (controller.collisions.left) ? -1 : 1;

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            bool wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
                wallSliding = true;

                if (velocity.y < -wallSlideSpeedMax) {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0) {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (input.x != wallDirX && input.x != 0) {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else {
                    timeToWallUnstick = wallStickTime;
                }

            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (wallSliding) {
                    if (wallDirX == input.x) {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }
                    else if (input.x == 0) {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    else {
                        velocity.x = -wallDirX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }
                }
                if (controller.collisions.below) {
                    velocity.y = maxJumpVelocity;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                if (velocity.y > minJumpVelocity) {
                    velocity.y = minJumpVelocity;
                }
            }


            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);

            if (Mathf.Abs(velocity.x) > 0.5 && controller.collisions.below) {

                Transform rearFoot = velocity.x < 0 ? transform.FindChild("RightFoot") : transform.FindChild("LeftFoot");

                footstepParticles.transform.SetParent(rearFoot);
                footstepParticles.transform.localPosition = Vector3.zero;
                footstepParticles.transform.localRotation = Quaternion.identity;

                footstepParticles.enableEmission = true;
                footstepParticles.Play();
                MasterAudio.PlaySound3DAtTransform("SnowFootsteps", transform);
            }
            else if (footstepParticles.isPlaying) {
                footstepParticles.Stop();
                //MasterAudio.FadeOutAllOfSound("SnowFootsteps", 0.1f);
                MasterAudio.StopAllOfSound("SnowFootsteps");
            }

            if (carryingItem) {

                Transform carryPos = ((velocity.x <= 0 && wallDirX != -1) || (velocity.x > 0 && controller.collisions.right)) ? transform.FindChild("CarryLeft") : transform.FindChild("CarryRight");

                item.transform.SetParent(carryPos);
                item.transform.localPosition = Vector3.zero;
            }

            if (controller.collisions.above || controller.collisions.below) {
                velocity.y = 0;
            }
        }

    }

}
