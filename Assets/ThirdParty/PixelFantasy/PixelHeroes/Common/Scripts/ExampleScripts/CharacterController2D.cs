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
        public Vector2 Input; // 외부 입력 스크립트 등에서 넣어주는 키보드 입력값 (X, Y)

        public float Acceleration; // 가속도
        public float MaxSpeed;     // 최대 이동 속도

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private CharacterAnimation _animation;

        public void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animation = GetComponent<CharacterAnimation>();
        }

        public void FixedUpdate()
        {
            var state = _animation.GetState();

            // 사망, 방어, 등반 상태일 때는 이동하지 못하도록 차단
            if (state == CharacterState.Die || state == CharacterState.Block || state == CharacterState.Climb) return;

            var velocity = _rigidbody.linearVelocity;
            var maxSpeed = MaxSpeed;
            float accel = Acceleration;
            float decel = Acceleration * 4f; // 정지할 때의 감속도

            // --- [X축 이동 처리] ---
            if (Input.x == 0)
            {
                // 입력이 없으면 부드럽게 멈춤
                velocity.x = Mathf.MoveTowards(velocity.x, 0, decel * Time.fixedDeltaTime);
            }
            else
            {
                // 방향을 전환하는 중인지 체크하여 가속도 조절
                bool isTurning = (Input.x > 0 && velocity.x < 0) || (Input.x < 0 && velocity.x > 0);
                float currentStrength = isTurning ? (accel + decel) : accel;

                velocity.x = Mathf.MoveTowards(velocity.x, Input.x * maxSpeed, currentStrength * Time.fixedDeltaTime);
                Turn(velocity.x); // 이동 방향에 맞춰 스프라이트 좌우 반전
            }

            // --- [Y축 이동 처리 (★탑다운 뷰 추가)] ---
            if (Input.y == 0)
            {
                // 입력이 없으면 부드럽게 멈춤
                velocity.y = Mathf.MoveTowards(velocity.y, 0, decel * Time.fixedDeltaTime);
            }
            else
            {
                // 방향을 전환하는 중인지 체크하여 가속도 조절
                bool isTurning = (Input.y > 0 && velocity.y < 0) || (Input.y < 0 && velocity.y > 0);
                float currentStrength = isTurning ? (accel + decel) : accel;

                velocity.y = Mathf.MoveTowards(velocity.y, Input.y * maxSpeed, currentStrength * Time.fixedDeltaTime);
            }

            // --- [탑다운 뷰 애니메이션 제어] ---
            if (Input.x == 0 && Input.y == 0)
            {
                // 상하좌우 아무 입력도 없으면 대기(Idle) 상태로 변경
                if (state != CharacterState.Idle) _animation.Ready();
            }
            else
            {
                // 움직임이 있다면 달리기(Run) 애니메이션 재생
                _animation.Run();
            }

            // 최종 계산된 속도를 리지드바디에 적용
            _rigidbody.linearVelocity = velocity;
        }

        // 스프라이트 좌우 반전 함수
        private void Turn(float direction)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}