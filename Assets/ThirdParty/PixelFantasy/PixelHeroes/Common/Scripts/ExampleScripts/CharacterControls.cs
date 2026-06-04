using Assets.PixelFantasy.Common.Scripts;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using UnityEngine;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts
{
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(CharacterAnimation))]
    public class CharacterControls : MonoBehaviour
    {
        private Creature _character;
        private CharacterController2D _controller;
        private CharacterAnimation _animation;

        public void Start()
        {
            _character = GetComponent<Character>();
            _controller = GetComponent<CharacterController2D>();
            _animation = GetComponent<CharacterAnimation>();
        }

        public void Update()
        {
            Move();
            //Attack();

            // Play other animations, just for example.
            //if (Input.GetKeyDown(KeyCode.I)) _animation.Idle();
            //if (Input.GetKeyDown(KeyCode.R)) _animation.Ready();
            //if (Input.GetKeyDown(KeyCode.B)) _animation.Block();
            //if (Input.GetKeyDown(KeyCode.C)) _animation.Climb();
            //if (Input.GetKeyDown(KeyCode.D)) _animation.Die();
            //if (Input.GetKeyDown(KeyCode.N)) _animation.Roll();
            //if (Input.GetKeyDown(KeyCode.H)) _animation.Hit();
            //if (Input.GetKeyUp(KeyCode.L)) EffectManager.Instance.Blink(_character);
        }

        private void Move()
        {
            _controller.Input = Vector2.zero;

            // 왼쪽 화살표 키 또는 A 키를 눌렀을 때
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _controller.Input.x = -1;
            }
            // 오른쪽 화살표 키 또는 D 키를 눌렀을 때
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _controller.Input.x = 1;
            }

            // 위쪽 화살표 키 또는 W 키를 눌렀을 때
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                _controller.Input.y = 1;
            }
            // 아래쪽 화살표 키 또는 S 키를 눌렀을 때
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                _controller.Input.y = -1;
            }
        }

        //private void Attack()
        //{
        //    if (Input.GetKeyDown(KeyCode.J)) _animation.Jab();
        //    if (Input.GetKeyDown(KeyCode.S)) _animation.Slash();
        //    if (Input.GetKeyDown(KeyCode.P)) _animation.Push();
        //    if (Input.GetKeyDown(KeyCode.O)) _animation.Shot();
        //    if (Input.GetKey(KeyCode.F)) Fire();
        //    if (Input.GetKey(KeyCode.Q)) Fire(power: true);
        //}

        private float _fireTime;

        public void Fire(bool power = false)
        {
            if (Time.time - _fireTime < 0.15f) return;

            if (_animation.GetState() == CharacterState.Idle)
            {
                _animation.Ready();
            }

            _fireTime = Time.time;

            var firearm = _character.GetComponent<Character>().Firearm;

            if (firearm.Detached)
            {
                firearm.Transform.gameObject.SetActive(true);
                firearm.Animator.SetTrigger(power ? "PowerFire" : "Fire");
            }
            else
            {
                _character.Animator.SetTrigger("Attack2");
            }

            _character.AudioSource.pitch = Random.Range(0.9f, 1.1f);
            _character.AudioSource.PlayOneShot(EffectManager.Instance.FireAudioClip);
            EffectManager.Instance.CreateSpriteEffect(_character, power ? "FireMuzzleM" : "FireMuzzleS", direction: 1, parent: firearm.FireMuzzle);
        }
    }
}