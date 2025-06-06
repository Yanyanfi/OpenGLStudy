namespace OpenGLStudy;

internal class GameScene
{
    private List<GameObject> gameObjects = [];
    public List<GameObject> GetAllGameObjects()
    {
        return new List<GameObject>(gameObjects);
    }
    public void AddGameObject(params List<GameObject> gameObjects)
    {
        gameObjects.ForEach(e => e.Scene = this);
        this.gameObjects.AddRange(gameObjects);
    }
    public bool TryGetGameObject(string name, out GameObject? gameObject)
    {
        gameObject = gameObjects.Find(e => e.Name == name);
        return gameObject is not null;
    }
    public GameObject GetGameObject(string name)
    {
        if (TryGetGameObject(name, out var gameObject))
            return gameObject!;
        throw new InvalidOperationException($"GameObject {name} not found in scene");
    }
    public List<GameObject> GetGameObjects(string name)
    {
        return gameObjects.FindAll(e => e.Name == name);
    }
    public bool TryGetGameObject<T>(out GameObject? gameObject) where T : Component
    {
        gameObject = gameObjects.Find(e => e.HasComponent<T>());
        return gameObject is not null;
    }
    public GameObject GetGameObject<T>() where T : Component
    {
        if (TryGetGameObject<T>(out var obj))
            return obj!;
        throw new InvalidOperationException($"GameObject contains {typeof(T).Name} not found in scene");
    }
    public List<GameObject> GetGameObjects<T>() where T : Component
    {
        return gameObjects.FindAll(e => e.HasComponent<T>());
    }
    public void RemoveGameObject(string name)
    {
        var obj = gameObjects.FirstOrDefault(e => e.Name == name);
        obj?.RemoveFromScene();
    }
    public void RemoveGameObjects(string name)
    {
        gameObjects.FindAll(e => e.Name != name).ForEach(e => e.RemoveFromScene());
    }
    public void RemoveGameObject<T>() where T : Component
    {
        var obj = gameObjects.FirstOrDefault(e => e.HasComponent<T>());
        obj?.RemoveFromScene();
    }
    public void RemoveGameObjects<T>() where T : Component
    {
        gameObjects.FindAll(e => e.HasComponent<T>()).ForEach(e => e.RemoveFromScene());
    }
    public void RemoveGameObjects(params List<GameObject> gameObjects)
    {
        gameObjects.ForEach(e => e.RemoveFromScene());
    }
    public void DisableGameObject(string name)
    {
        gameObjects.FirstOrDefault(e => e.Name == name)?.SetActive(false);
    }
    public void DisableGameObjects(string name)
    {
        gameObjects.FindAll(e => e.Name == name).ForEach(e => e.SetActive(false));
    }
    public void DisableGameObject<T>() where T : Component
    {
        gameObjects.FirstOrDefault(e => e.HasComponent<T>())?.SetActive(false);
    }
    public void DisableGameObjects<T>() where T : Component
    {
        gameObjects.FindAll(e => e.HasComponent<T>()).ForEach(e => e.SetActive(false));
    }
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
        if (TryGetComponent<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found.");
    }
    public void Start() => new List<GameObject>(gameObjects).ForEach(e => e.Start());
    public void Update(float deltaTime)
    {
        var snapshot = new List<GameObject>(gameObjects);
        foreach (var gameObject in snapshot)
        {
            if (!gameObject.Started)
                gameObject.Start();
            gameObject.Update(deltaTime);
        }
    }
    public void Render() => gameObjects.ForEach(e => e.Render());
}
