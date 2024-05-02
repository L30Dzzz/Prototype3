using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isOnGround = true;
    public float jumpForce;
    public float gravityModifier;
    private Rigidbody playerRb;
    public bool gameOver = false;
    private Animator playerAnim;

    public float doubleJumpForce;
    private bool doubleJump = false;
    
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound; 
    public AudioClip crashSound;
    public AudioSource cameraAudio;
    private AudioSource playerAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            isOnGround = false;
            doubleJump = true;
            playerRb.AddForce(new Vector3(playerRb.velocity.x, jumpForce));
        }

        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && doubleJump)
        {
            playerRb.AddForce(new Vector3(playerRb.velocity.x, doubleJumpForce));
            doubleJump = false;
        }
    }
        private void OnCollisionEnter(Collision collision)
        {
            isOnGround = true;
            doubleJump = false;

            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
                dirtParticle.Play();
            }
            
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                gameOver = true;
                Debug.Log("Game Over!");
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                explosionParticle.Play();
                dirtParticle.Stop();
                playerAudio.PlayOneShot(crashSound, 1.0f);
                cameraAudio.Stop();
            }
        }
}
