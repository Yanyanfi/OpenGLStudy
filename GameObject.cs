using OpenGLStudy.Enums;
using OpenGLStudy.Model.Base;
using OpenGLStudy.Shaders;
using OpenTK.Mathematics;
using SixLabors.ImageSharp.Processing;

namespace OpenGLStudy;

internal class GameObject
{
    public string Name { get; set; }
    public Transform Transform { get; set; } = new();
    public IRenderable? Model { get; set; }

    public T GetComponent<T>() where T : Component =>
        (T?)components.Find(e => e is T) ?? throw new Exception($"Component of type {typeof(T).Name} not found in {Name}");
    public bool TryGetComponent<T>(out T? component) where T : Component
    {
        component = (T?)components.Find(e => e is T);
        return component is not null;
    }
    public GameObject? GetChild(string name) => children.Find(e => e.Name == name);
    public List<GameObject> GetChildren(string name) => children.FindAll(e => e.Name == name);
    public void AddChild(GameObject obj)
    {
        children.Add(obj);
        obj.parent = this;
    }
    public void AddComponent(Component component)
    {
        component.Owner = this;
        components.Add(component);
    }
    /// <summary>
    /// 如果组件有无参构造函数，可以调用此方法添加组件
    /// </summary>
    public void AddComponent<T>() where T : Component, new()
    {
        var component = Activator.CreateInstance<T>();
        component.Owner = this;
        components.Add(component);
    }
    private List<Component> components = [];
    private List<GameObject> children = [];
    private GameObject? parent;
    public GameObject(string name) => Name = name;
    public GameObject(string name, Vector3 position) : this(name) => Transform.Position = position;
    public GameObject(string name, Vector3 position, Vector3 scale) : this(name, position) => Transform.Scale = scale;
    public GameObject(string name, Vector3 position, Vector3 scale, Quaternion rotation) : this(name, position, scale) => Transform.Rotation = rotation;
    public void Start()
    {
        children.ForEach(e => e.Start());
        components.ForEach(e => e.Start());
    }

    public void Update(float deltaTime)
    {
        children.ForEach(e => e.Update(deltaTime));
        components.ForEach(e => e.Update(deltaTime));
    }

    public void Render()
    {
        children.ForEach(e => e.Render());
        var parent = this.parent;
        var matrix = Transform.GetModelMatrix();
        while(parent is not null)
        {
            matrix *= parent.Transform.GetModelMatrix();
            parent = parent.parent;
        }
        Shader.Instance?.SetMvpMatrix(MVPMatrixTarget.Model, matrix);
        Model?.Render();
    }
}
