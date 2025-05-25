using OpenGLStudy.Inputs;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLStudy;

internal abstract class Component
{
    public GameObject Owner { get; set; } = default!;
    public float FrameTimeLimit { get; set; } = 1 / 60f;

    public virtual void Start() { }
    public virtual void Update(float deltaTime)
    {
        if (DeltaTimeSum == 0)
            CustomUpdate(customDeltaTime);
        DeltaTimeSum += deltaTime;
    }

    protected virtual void CustomUpdate(float deltaTime) { }


    private float DeltaTimeSum
    {
        get;
        set
        {
            if (value >= FrameTimeLimit)
            {
                field = 0;
                customDeltaTime = value;
            }
            else
            {
                field = value;
            }
        }
    } = 0.0f;
    
    private float customDeltaTime = 0.0f;
}
