using Assets.PixelHeroes.Scripts.CharacterScrips;
using Unity.VisualScripting;
using UnityEngine;
using AnimationState = Assets.PixelHeroes.Scripts.CharacterScrips.AnimationState;

namespace Assets.PixelHeroes.Scripts.ExampleScripts
{
    public class CharacterControls : MonoBehaviour
    {
        public Character Character;
        public float RunSpeed = 1f;
        public float JumpSpeed = 3f;
        public float CrawlSpeed = 0.25f;
        public float Gravity = -0.2f;
        public ParticleSystem MoveDust;
        public ParticleSystem JumpDust;

        private Vector3 _motion = Vector3.zero;
        private int _inputX, _inputY;
        private float _activityTime;

        private bool canMove = true;
        private bool canRun = true;
        private Rigidbody2D r2d;
        private Animator _animator;
        public float WalkSpeed = 1.0f;
        
        public void Start()
        {
            _animator = GetComponent<Animator>();
            r2d = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            HandleMovement();
        }

        public void HandleMovement()
        {
          
            if (canMove)
            {
                Vector2 input = Vector2.zero;

               
             
                    // Handle keyboard input (for PC)
                    input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
                    input.y = UnityEngine.Input.GetAxisRaw("Vertical");
                

                if (input != Vector2.zero)
                {
                   
                    // Normalize input for smooth movement
                    input.Normalize();

                    // Turn the character based on input direction
                    if (input.x < 0) // Moving left
                    {
                        Turn(-1); // Turn left (face left)
                    }
                    else if (input.x > 0) // Moving right
                    {
                        Turn(1); // Turn right (face right)
                    }

                    // Calculate and apply velocity based on input
                    Vector2 velocity = input * WalkSpeed;
                    r2d.linearVelocity = velocity;

                    Move(input);

                    // Set movement animations
                    _animator.SetBool("Idle", false);
                    _animator.SetBool("Walking", true);
                    _animator.SetBool("Running", false);
                }
                else
                {
                    // Stop movement
                    r2d.linearVelocity = Vector2.zero;

                    // Set idle animation
                    _animator.SetBool("Idle", true);
                    _animator.SetBool("Walking", false);
                    _animator.SetBool("Running", false);

                    // Stop walking sound
                }

                // Running check (if LeftShift is pressed for PC, or joystick for mobile)
                if (UnityEngine.Input.GetKey(KeyCode.LeftShift)  && input != Vector2.zero)
                {
                    if (canRun == true)
                    {
                        // Set running animation
                        _animator.SetBool("Running", true);
                        _animator.SetBool("Walking", false);

                        // Calculate running speed
                        Vector2 velocity = input * (WalkSpeed + RunSpeed);
                        r2d.linearVelocity = velocity;

                    
                    }
                }
                
            }
        }
        public Transform CharacterBody;
        private void Turn(int direction)
        {
            // Get the current scale of the character
            Vector3 scale = CharacterBody.localScale;

            // Set the scale's x component based on the direction
            if (direction > 0)
            {
                // Facing right
                scale.x = Mathf.Abs(scale.x); // Ensure positive value
            }
            else if (direction < 0)
            {
                // Facing left
                scale.x = -Mathf.Abs(scale.x); // Ensure negative value
            }

            // Apply the new scale to the character
            CharacterBody.localScale = scale;
        }

        private void Move(Vector2 movement)
        {
            // Calculate velocity vector based on input and speed
            Vector2 velocity = movement * WalkSpeed;

            // Apply velocity to Rigidbody2D
            r2d.linearVelocity = velocity;

            _animator.SetBool("Idle", false);
            _animator.SetBool("Walking", true);
            _animator.SetBool("Running", false); ;


        }


    }




}