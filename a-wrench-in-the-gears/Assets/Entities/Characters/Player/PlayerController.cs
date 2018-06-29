using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Controller2D))]
public class PlayerController : MonoBehaviour {

  public float maxJumpHeight = 3f;
  public float minJumpHeight = 1f;
  public float distanceToJumpApex = 3f;
  public float gravityFallFactor = 1.2f;
  public int extrajumps = 1;
  public float extrajumpHeight = 2f;
  public float accelerationTimeAirborne = .2f;
  public float accelerationTimeGrounded = .1f;
  public float moveSpeed = 8f;
  public float wallSlideSpeedMax = 3f;
  public float wallStickTime = .25f;
  public Vector2 wallJumpClimb = new Vector2(7.5f, 14f);
  public Vector2 wallJumpOff = new Vector2(8.5f, 10f);
  public Vector2 wallLeap = new Vector2(18f, 17f);
  public float deathTimerDuration = .6f;

  private float deathTimer;
  private float gravity;
  private float maxJumpVelocity;
  private float minJumpVelocity;
  private float extraJumpVelocity;
  private Vector3 velocity;
  private float velocityXSmoothing;
  private float timeToWallUnstick;
  private Controller2D controller;
  private SpriteController spriteController;
  private Vector2 directionalInput;
  private bool wallSliding;
  private int wallDirX;
  private int currentJump;
  private bool jumpRequested = false;
  private bool actionRequested = false;
  private bool cancelJump = false;
  private SpriteController.PlayerState playerState;
  private UnityAction<RaycastHit2D> collisionsActions;
  private UnityAction<RaycastHit2D> triggerActions;
  private CheckpointController respawnPoint;
  private LevelController levelController;

  private void Start() {
    this.controller = GetComponent<Controller2D>();
    this.spriteController = GetComponentInChildren<SpriteController>();
    this.levelController = FindObjectOfType<LevelController>();

    this.collisionsActions += OnCollision;
    this.triggerActions += OnTrigger;
    this.controller.subscribeToColliderEvent(collisionsActions);
    this.controller.subscribeToTriggerEvent(triggerActions);

    this.deathTimer = this.deathTimerDuration;
    this.gravity = ComputeGravity();
    this.maxJumpVelocity = (2 * this.maxJumpHeight * this.moveSpeed) / this.distanceToJumpApex;
    this.minJumpVelocity = (2 * this.minJumpHeight * this.moveSpeed) / this.distanceToJumpApex;
    this.extraJumpVelocity = (2 * this.extrajumpHeight * this.moveSpeed) / this.distanceToJumpApex;
  }

  private void Update() {
    if (this.playerState.dying) {
      if (this.deathTimer <= 0) {
        this.Respawn();
      } else {
        this.deathTimer -= Time.deltaTime;
      }
    }
  }

  private void FixedUpdate() {
    if (this.playerState.dying) {
      this.velocity = new Vector2(0, 0);
      this.playerState.velocity = this.velocity;
      this.playerState.wallSliding = false;
      this.playerState.input = new Vector2(0, 0);
      this.controller.collisions.Reset();
    } else {
      CalculateVelocity();
      HandleWallSliding();

      if (this.controller.collisions.bellow || this.wallSliding) {
        this.currentJump = 0;
      }

      if (this.jumpRequested) {
        Jump();
      }

      if (this.cancelJump) {
        CancelJump();
      }

      this.controller.Move(this.velocity * Time.deltaTime, this.directionalInput);

      if (this.controller.collisions.above || this.controller.collisions.bellow) {
        if (this.controller.collisions.slidingDownMaxSlope) {
          this.velocity.y += this.controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
        } else {
          this.velocity.y = 0;
        }
      }

      this.playerState.collisions = this.controller.collisions;
      this.playerState.velocity = this.velocity;
      this.playerState.wallSliding = this.wallSliding;
      this.playerState.input = this.directionalInput;
    }
    this.jumpRequested = false;
    this.cancelJump = false;
    this.actionRequested = false;
    this.spriteController.updateSprite(this.playerState);
  }

  public void SetDirectionalInput(Vector2 input) {
    this.directionalInput = input;
  }

  public void OnJumpInputDown() {
    this.jumpRequested = true;
  }

  public void OnActionInputDown() {
    this.actionRequested = true;
  }

  public void OnJumpInputUp() {
    this.cancelJump = true;
  }

  public void Respawn() {
    this.deathTimer = this.deathTimerDuration;
    this.playerState.dying = false;
    transform.position = this.respawnPoint.transform.position;
  }

  public void UpdateRespawnPoint(CheckpointController newPoint) {
    this.respawnPoint = newPoint;
  }

  public void Die() {
    this.playerState.dying = true;
  }

  private void CalculateVelocity() {
    float gravityFactor = 1;
    if (this.velocity.y < 0) {
      gravityFactor = gravityFallFactor;
    }
    float targetVelocityX = this.directionalInput.x * this.moveSpeed;
    this.velocity.x = Mathf.SmoothDamp(this.velocity.x, targetVelocityX, ref this.velocityXSmoothing, this.controller.collisions.bellow ? this.accelerationTimeGrounded : this.accelerationTimeAirborne);
    this.velocity.y += this.gravity * gravityFactor * Time.deltaTime;
  }

  private void HandleWallSliding() {
    this.wallDirX = this.controller.collisions.left ? -1 : 1;
    this.wallSliding = false;
    if ((this.controller.collisions.left || this.controller.collisions.right) && !this.controller.collisions.bellow) {
      this.wallSliding = true;

      if (this.velocity.y < wallSlideSpeedMax) {
        this.velocity.y = -wallSlideSpeedMax;
      }

      if (this.timeToWallUnstick > 0) {
        this.velocityXSmoothing = 0;
        this.velocity.x = 0;

        if (this.directionalInput.x != this.wallDirX && this.directionalInput.x != 0) {
          this.timeToWallUnstick -= Time.deltaTime;
        } else {
          this.timeToWallUnstick = this.wallStickTime;
        }
      } else {
        this.timeToWallUnstick = this.wallStickTime;
      }
    }
  }

  private void Jump() {
    if (this.wallSliding) {
      if (this.wallDirX == this.directionalInput.x) {
        this.velocity.x = -this.wallDirX * this.wallJumpClimb.x;
        this.velocity.y = this.wallJumpClimb.y;
      } else if (this.directionalInput.x == 0) {
        this.velocity.x = -this.wallDirX * this.wallJumpOff.x;
        this.velocity.y = this.wallJumpOff.y;
      } else {
        this.velocity.x = -this.wallDirX * this.wallLeap.x;
        this.velocity.y = this.wallLeap.y;
      }
    } else {
      if (this.controller.collisions.bellow) {
        this.currentJump = 0;
        if (this.controller.collisions.slidingDownMaxSlope) {
          if (this.directionalInput.x != -Mathf.Sign(this.controller.collisions.slopeNormal.x)) {
            velocity.y = this.maxJumpVelocity * this.controller.collisions.slopeNormal.y;
            velocity.x = this.maxJumpVelocity * this.controller.collisions.slopeNormal.x;
          }
        } else {
          this.velocity.y = this.maxJumpVelocity;
        }
      } else {
        if (this.currentJump < this.extrajumps) {
          this.currentJump++;
          this.velocity.y = this.extraJumpVelocity;
        }
      }
    }
  }

  private void CancelJump() {
    if (this.velocity.y > this.minJumpVelocity) {
      this.velocity.y = this.minJumpVelocity;
    }
  }

  private float ComputeGravity()	{
		return (-2 * this.maxJumpHeight * Mathf.Pow(this.moveSpeed, 2)) / Mathf.Pow(this.distanceToJumpApex, 2);
	}

  private void OnTrigger(RaycastHit2D hit) {
    // Debug.Log("Trigger: " + hit.collider.tag);
    if (hit.collider.tag == "Checkpoint") {
      CheckpointController checkpoint = hit.collider.gameObject.GetComponent<CheckpointController>();
      if (checkpoint.active) {
        checkpoint.Disable();
        this.UpdateRespawnPoint(checkpoint);
      }
    }
    if (hit.collider.tag == "Pearl") {
      GameObject pearl = hit.collider.gameObject;
      pearl.SetActive(false);
    }
    if (hit.collider.tag == "End") {
      CheckpointController checkpoint = hit.collider.gameObject.GetComponent<CheckpointController>();
      this.levelController.LoadNextLevel();
      // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Dangers")) {
      DangerController danger = hit.collider.gameObject.GetComponent<DangerController>();
      if (!danger || danger.active) {
        Die();
      }
    }
    if (hit.collider.tag == "Lever" && this.actionRequested) {
      hit.collider.gameObject.GetComponent<LeverController>().Toggle();
    }
  }

  private void OnCollision(RaycastHit2D hit) {
    // Debug.Log("collision: " + hit.collider.tag);
  }
}
