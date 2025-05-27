using OpenGLStudy.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenGLStudy.Components.Cameras;

namespace OpenGLStudy.Components;

internal class PlayerMove : Component
{
    public float Speed { get; set; } = 3f;
    private Vector3 Front => Owner.Transform.Front;
    private Vector3 Up=> Owner.Transform.Up;
    private Vector3 Right => Owner.Transform.Right;
    public override void Start()
    {
       
        FrameTimeLimit= Owner.GetComponent<Camera>().FrameTimeLimit;
    }
    protected override void CustomUpdate(float deltaTime)
    {
        ProcessKeyboard(deltaTime);
    }
    private void ProcessKeyboard(float deltaTime)
    {
        float velocity = Speed * deltaTime;

        if (InputState.Keyboard.IsKeyDown(Keys.W))
            Owner.Transform.Position += new Vector3(Front.X, 0, Front.Z).Normalized() * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.S))
            Owner.Transform.Position -= new Vector3(Front.X, 0, Front.Z).Normalized() * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.A))
            Owner.Transform.Position -= Right * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.D))
            Owner.Transform.Position += Right * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.Space))
            Owner.Transform.Position += Up * velocity;
        if (InputState.Keyboard.IsKeyDown(Keys.LeftShift))
            Owner.Transform.Position -= Up * velocity;
    }
}
