using OpenGLStudy.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLStudy.Components.Cameras;
namespace OpenGLStudy.Components;

internal class PlayerMove : Component
{
    public float Speed { get; set; } = 3f;
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
        float velocity = Speed * deltaTime;

        // 水平移动
        if (InputState.Keyboard.IsKeyDown(Keys.W))
            Owner.Transform.Position += new Vector3(Front.X, 0, Front.Z).Normalized() * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.S))
            Owner.Transform.Position -= new Vector3(Front.X, 0, Front.Z).Normalized() * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.A))
            Owner.Transform.Position -= Right * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.D))
            Owner.Transform.Position += Right * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.LeftShift))
            Owner.Transform.Position -= Up * velocity;

        // 跳跃逻辑
        if (gravityComponent.IsGrounded && InputState.Keyboard.IsKeyDown(Keys.Space))
        {
            gravityComponent.SetVerticalVelocity(JumpSpeed);
        }
    }

}