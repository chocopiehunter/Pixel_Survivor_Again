using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System.Linq;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterAnimation))]
    public class CharacterController2D : MonoBehaviour
    {
        public Vector2 Input;
        public bool IsGrounded;

        public float Acceleration;
        public float MaxSpeed;
        public float JumpForce;
        public float Gravity;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private CharacterAnimation _animation;
        
        private bool _jump;
        private bool _crouch;

        public void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animation = GetComponent<CharacterAnimation>();
        }

        public void FixedUpdate()
        {
            var state = _animation.GetState();

            if (state == CharacterState.Die || state == CharacterState.Block || state == CharacterState.Climb) return;

            var velocity = _rigidbody.linearVelocity;
            var maxSpeed = MaxSpeed;
            float accel = Acceleration;
            float decel = Acceleration * 4f;

            if (!IsGrounded)
            {
                accel *= 0.8f;
                decel *= 0.5f;
            }
            else if (_crouch)
            {
                accel /= 2;
                maxSpeed /= 4;
            }

            if (Input.x == 0)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, decel * Time.fixedDeltaTime);
            }
            else
            {
                bool isTurning = (Input.x > 0 && velocity.x < 0) || (Input.x < 0 && velocity.x > 0);
                float currentStrength = isTurning ? (accel + decel) : accel;

                velocity.x = Mathf.MoveTowards(velocity.x, Input.x * maxSpeed, currentStrength * Time.fixedDeltaTime);
                Turn(velocity.x);
            }

            if (IsGrounded)
            {
                _crouch = Input.y < 0;

                if (!_jump)
                {
                    if (Input.x == 0)
                    {
                        if (_crouch) _animation.Crouch();
                        else if (state != CharacterState.Idle) _animation.Ready();
                    }
                    else
                    {
                        if (_crouch) _animation.Crawl();
                        else _animation.Run();
                    }
                }

                if (Input.y > 0 && !_jump)
                {
                    _jump = true;
                    _rigidbody.AddForce(Vector2.up * JumpForce);
                    _animation.Jump();
                }
            }
            else
            {
                velocity.y -= Gravity * Time.fixedDeltaTime;

                if (velocity.y < 0)
                {
                    _jump = true;
                    _animation.Fall();
                }
            }

            _rigidbody.linearVelocity = velocity;
        }

        private void Turn(float direction)
        {
            var scale = transform.localScale;

            scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);

            transform.localScale = scale;
        }

        private Collider2D _ground;

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.All(i => i.point.y <= _collider.bounds.min.y))
            {
                IsGrounded = true;
                _ground = collision.collider;

                if (_jump)
                {
                    _jump = false;
                    _animation.Land(Input.y < 0 ? CharacterState.Crouch : CharacterState.Land);
                }
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (IsGrounded && collision.collider == _ground)
            {
                IsGrounded = false;
            }
        }
    }
}