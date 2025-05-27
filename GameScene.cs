namespace OpenGLStudy;

internal class GameScene
{
    private List<GameObject> gameObjects = [];
    public void AddGameObject(params List<GameObject> gameObjects)
    {
        gameObjects.ForEach(e => e.Scene = this);
        this.gameObjects.AddRange(gameObjects);
    }
    public void Start() => gameObjects.ForEach(e => e.Start());
    public void Update(float deltaTime) => gameObjects.ForEach(e => e.Update(deltaTime));
    public void Render() => gameObjects.ForEach(e => e.Render());
    public bool TryGetComponent<T>(out T? component) where T : Component
    {
        foreach (var gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent<T>(out component))
                return true;
        }
        component = null;
        return false;
    }
    public Component GetComponent<T>() where T : Component
    {
        if(TryGetComponent<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found.");
    }
}
