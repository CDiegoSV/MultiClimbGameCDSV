using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks/*, IPunObservable*/
{
    #region Enum

    public enum PlayerStates { LOCKED, IDLE, MOVING }

    #endregion

    #region Knobs

    public PlayerStates currentPlayerState;
    
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Transform groundCheckRay;
    [SerializeField] float playerSpeed, jumpForce;
    [SerializeField] bool onLand, jumped = false;

    #endregion

    #region Runtime Variables

    private Vector2 movementVector;

    #endregion

    #region References


    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    public TextMeshProUGUI playerNameTextMesh;
    [SerializeField] GameObject hitCollider;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (photonView.IsMine)
        {
            InitializeAvatar();

        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if(PlayerStates.LOCKED == currentPlayerState)
            {
                if(VSGameManager.instance != null)
                {
                    if (VSGameManager.instance.CurrentGameState == VSGameManager.GameStates.GAME)
                    {
                        currentPlayerState = PlayerStates.IDLE;
                    }
                }
                else
                {
                    currentPlayerState = PlayerStates.IDLE;
                }

            }
            else
            {
                float m_movementX = Input.GetAxisRaw("Horizontal");

                movementVector = new Vector2(m_movementX, 0).normalized;

                onLand = IsOnGround();
                animator.SetBool("OnLand", onLand);
                animator.SetBool("Falling", IsFalling());
                animator.SetBool("Jumped", jumped);

                if (Input.GetKeyDown(KeyCode.Space) && IsOnGround())
                {
                    JumpImpulse();
                    jumped = true;
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            switch (currentPlayerState)
            {
                case PlayerStates.IDLE:
                    if (movementVector != Vector2.zero)
                    {
                        currentPlayerState = PlayerStates.MOVING;
                    }
                    if(IsOnGround() && rb2D.velocity.x != 0)
                    {
                        rb2D.velocity = Vector2.zero;
                    }
                    break;

                case PlayerStates.MOVING:
                    if (movementVector == Vector2.zero)
                    {
                        currentPlayerState = PlayerStates.IDLE;
                    }
                    if (IsOnGround() == true && jumped == false)
                    {
                        if (rb2D.velocity.x != 0)
                        {
                            rb2D.velocity = Vector2.zero;
                        }
                        rb2D.MovePosition(rb2D.position + playerSpeed * Time.deltaTime * movementVector);
                    }
                    else if(IsOnGround() == false)
                    {
                        rb2D.AddForce(movementVector * playerSpeed * 3, ForceMode2D.Force);
                        rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -5, 5), rb2D.velocity.y);
                    }
                    animator.SetInteger("MovementInt", (int)movementVector.x);
                    SpriteFlip();
                    
                    break;
            }
            
        }
    }

    #endregion

    #region Private Methods

    private bool IsOnGround()
    {
        Ray2D ray2D = new Ray2D(groundCheckRay.position, Vector2.down);

        RaycastHit2D hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, 0.05f, groundLayerMask);

        return hit.collider is not null;
    }

    private bool IsFalling()
    {
        return rb2D.velocity.y < 0;
    }

    private void JumpImpulse()
    {
        if(IsOnGround() == true)
        {
            rb2D.AddForce(movementVector + Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(SetJumpedFalseInSeconds(0.5f));
        }
    }

    private IEnumerator SetJumpedFalseInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        jumped = false;
    }

    private void InitializeAvatar()
    {
        playerNameTextMesh.text = photonView.Owner.NickName;
    }

    private void SpriteFlip()
    {
        
        if (movementVector.x > 0)
        {
            spriteRenderer.flipX = false;
            hitCollider.transform.localScale = new Vector3(1, 1, 1);
            photonView.RPC("UpdateFlipNScale", RpcTarget.Others, spriteRenderer.flipX, hitCollider.transform.localScale);
        }
        else if (movementVector.x < 0)
        {
            
            spriteRenderer.flipX = true;
            hitCollider.transform.localScale = new Vector3(-1, 1, 1);
            photonView.RPC("UpdateFlipNScale", RpcTarget.Others, spriteRenderer.flipX, hitCollider.transform.localScale);
        }
    }

    [PunRPC]
    void UpdateFlipNScale(bool flipX, Vector3 scale)
    {
        spriteRenderer.flipX = flipX;
        hitCollider.transform.localScale = scale;
    }

    #endregion

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(spriteRenderer.flipX);
    //        //stream.SendNext(hitCollider.transform.localScale);
    //    }
    //    else
    //    {
    //        spriteRenderer.flipX = (bool)stream.ReceiveNext();
    //        //hitCollider.transform.localScale = (Vector3)stream.ReceiveNext();
    //    }
    //}
}
