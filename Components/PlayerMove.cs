using OpenGLStudy.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLStudy.Components.Cameras;
namespace OpenGLStudy.Components;

internal class PlayerMove : Component
{
    public float Speed { get; set; } = 3f;
    public float RunSpeedMultiplier { get; set; } = 1.5f; // 奔跑速度倍数
    public float JumpSpeed { get; set; } = 4f;

    private Vector3 Front => Owner.Transform.Front;
    private Vector3 Up => Owner.Transform.Up;
    private Vector3 Right => Owner.Transform.Right;

    private GravityComponent gravityComponent;

    public override void Start()
    {
        FrameTimeLimit = Owner.GetComponent<Camera>().FrameTimeLimit;
        gravityComponent = Owner.GetComponent<GravityComponent>();
    }

    public override void Update(float deltaTime)
    {
        ProcessKeyboard(deltaTime);
    }

    private void ProcessKeyboard(float deltaTime)
    {
        // 检查是否按下 Shift 键来决定移动速度
        bool isRunning = InputState.Keyboard.IsKeyDown(Keys.LeftShift);
        float currentSpeed = isRunning ? Speed * RunSpeedMultiplier : Speed;
        float velocity = currentSpeed * deltaTime;

        // 水平移动
        if (InputState.Keyboard.IsKeyDown(Keys.W))
            Owner.Transform.Position += new Vector3(Front.X, 0, Front.Z).Normalized() * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.S))
            Owner.Transform.Position -= new Vector3(Front.X, 0, Front.Z).Normalized() * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.A))
            Owner.Transform.Position -= Right * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.D))
            Owner.Transform.Position += Right * velocity;

        // 跳跃逻辑
        if (gravityComponent.IsGrounded && InputState.Keyboard.IsKeyDown(Keys.Space))
        {
            gravityComponent.SetVerticalVelocity(JumpSpeed);
        }
    }
}