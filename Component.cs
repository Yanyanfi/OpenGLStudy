namespace OpenGLStudy;

internal abstract class Component
{
    public GameObject Owner { get; set; } = null!;
    public GameScene Scene { get; set; } = null!;
    public float FrameTimeLimit { get; set; } = 1 / 60f;
    /// <summary>
    /// 是否进入游戏循环
    /// </summary>
    public bool Enabled
    {
        get;
        set
        {
            if (!field && value)
                OnEnable();
            else if (field && !value)
                OnDisable();
            field = value;
        }
    } = true;
    public void RemoveFromOwner() => Owner.RemoveComponents(this);
    public virtual void Start() { }
    public virtual void Update(float deltaTime)
    {
        if (DeltaTimeSum == 0)
            CustomUpdate(customDeltaTime);
        DeltaTimeSum += deltaTime;
    }
    protected virtual void CustomUpdate(float deltaTime) { }
    public virtual void OnDestroy() { }
    public virtual void OnDisable() { }
    public virtual void OnEnable() { }
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
