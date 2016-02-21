using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterMovement : MonoBehaviour {
	[SerializeField] private LayerMask groundFlags;
	[SerializeField] private float jumpForce = 1000;
	[SerializeField] private float moveSpeed = 5;

	private bool isGrounded = false;
	private Transform groundFlag;
	private Rigidbody2D rigid2D;
	private SpriteRenderer spriteRender;
	private bool jumping = false;
	private Animator animator;

	// Use this for initialization
	void Start () {
		groundFlag = transform.Find("GroundedFlag");
		rigid2D = GetComponent<Rigidbody2D>();
		spriteRender = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Jump") && !jumping) {
			jumping = true;
		}

		if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
			animator.SetTrigger ("Smack");
		}
	}

	void FixedUpdate () {
		isGrounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundFlag.position, .2f, groundFlags);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				isGrounded = true;
		}

		float moveX = CrossPlatformInputManager.GetAxis("Horizontal");
		if (moveX < 0 && !spriteRender.flipX) {
			spriteRender.flipX = true;
		} else if (moveX > 0 && spriteRender.flipX) {
			spriteRender.flipX = false;
		}

		rigid2D.velocity = new Vector2(moveX*moveSpeed, rigid2D.velocity.y);

		if (jumping && isGrounded) {
			rigid2D.AddForce(new Vector2(0f, jumpForce));
			jumping = false;
			isGrounded = false;
		}
	}
}
