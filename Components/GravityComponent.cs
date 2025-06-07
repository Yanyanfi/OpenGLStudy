using OpenGLStudy.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLStudy.Components.Cameras;
using Assimp.Configs;


namespace OpenGLStudy.Components
{
    internal class GravityComponent : Component
    {
        public float GravityForce { get; set; } = -9.81f;
        public float VerticalVelocity { get; set; } = 0f;
        public float MaxFallSpeed { get; set; } = -20f;
        public bool IsGrounded { get; set; } = false;
        public float GroundLevel { get; set; } = 0f;

        public event System.Action OnGrounded;

        public event System.Action OnLeftGround;

        private bool wasGrounded = false;

        public override void Start()
        {
            // 初始化时检查是否在地面
            CheckGrounded();
        }

        public override void Update(float deltaTime)
        {
            ApplyGravity(deltaTime);
            UpdatePosition(deltaTime);
            CheckGrounded();
            HandleGroundEvents();
        }

        private void ApplyGravity(float deltaTime)
        {
            if (!IsGrounded)
            {
                // 应用重力
                VerticalVelocity += GravityForce * deltaTime;
            }
            else
            {
                if (VerticalVelocity < 0)
                    VerticalVelocity = 0f;
            }
        }

        private void UpdatePosition(float deltaTime)
        {
            // 更新垂直位置
            Vector3 currentPos = Owner.Transform.Position;
            currentPos.Y += VerticalVelocity * deltaTime;
            Owner.Transform.Position = currentPos;
        }

        private void CheckGrounded()
        {
            Vector3 currentPos = Owner.Transform.Position;

            if (currentPos.Y <= GroundLevel)
            {
                // 将物体放置在地面上
                Owner.Transform.Position = new Vector3(currentPos.X, GroundLevel, currentPos.Z);
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }

        private void HandleGroundEvents()
        {
            // 触发着地事件
            if (IsGrounded && !wasGrounded)
            {
                OnGrounded?.Invoke();
            }
            // 触发离地事件
            else if (!IsGrounded && wasGrounded)
            {
                OnLeftGround?.Invoke();
            }

            wasGrounded = IsGrounded;
        }
        public void AddUpwardForce(float force)
        {
            VerticalVelocity += force;
            if (IsGrounded && force > 0)
            {
                IsGrounded = false;
            }
        }
        public void SetVerticalVelocity(float velocity)
        {
            VerticalVelocity = velocity;
            if (IsGrounded && velocity > 0)
            {
                IsGrounded = false;
            }
        }
        public void Reset()
        {
            VerticalVelocity = 0f;
            IsGrounded = false;
        }
    }
}
