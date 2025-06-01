using OpenGLStudy.Enums;
using OpenGLStudy.Model.Base;
using OpenGLStudy.Model.Debug;
using OpenGLStudy.Shaders;
using OpenTK.Mathematics;

namespace OpenGLStudy;

internal class GameObject
{
    public string Name { get; set; }
    public Transform Transform { get; set; } = new();
    public IRenderable? Model { get; set; }
    /// <summary>
    /// 用于绘制调试的线段
    /// </summary>
    public LineRenderer LineRenderer => lineRenderer ??= new();
    private LineRenderer? lineRenderer;
    public GameScene Scene
    {
        get;
        set
        {
            field = value;
            children.ForEach(e => e.Scene = value);
            components.ForEach(e => e.Scene = value);
        }
    } = null!;
    /// <summary>
    /// 是否进入游戏循环 (逻辑上)
    /// </summary>
    public bool IsEnable { get; set; } = true;
    /// <summary>
    /// 模型是否可见
    /// </summary>
    public bool Visible { get; set; } = true;
    /// <summary>
    /// 是否绘制用来调试的线
    /// </summary>
    public bool EnableRenderLine { get; set; } = false;
    private List<Component> components = [];
    private List<GameObject> children = [];
    private GameObject? parent;
    public GameObject(string name) => Name = name;
    public GameObject(string name, Vector3 position) : this(name) => Transform.Position = position;
    public GameObject(string name, Vector3 position, Vector3 scale) : this(name, position) => Transform.Scale = scale;
    public GameObject(string name, Vector3 position, Vector3 scale, Quaternion rotation) : this(name, position, scale) => Transform.Rotation = rotation;
    /// <summary>
    /// 在游戏对象上获取组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件</returns>
    /// <exception cref="InvalidOperationException">没有找到组件</exception>
    public T GetComponent<T>() where T : Component
    {
        if (TryGetComponent<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in {Name}");
    }

    /// <summary>
    /// 尝试在游戏对象上获取组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component">返回组件</param>
    /// <returns>找到时为<see langword="true"/>否则为<see langword="false"/></returns>
    public bool TryGetComponent<T>(out T? component) where T : Component
    {
        component = (T?)components.Find(e => e is T);
        return component is not null;
    }
    /// <summary>
    /// 在游戏对象上获取组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件</returns>
    public List<T> GetComponents<T>() where T : Component => components.OfType<T>().ToList();
    public bool HasComponent<T>() where T : Component => TryGetComponent<T>(out _);
    /// <summary>
    /// 尝试在所有直系子对象上寻找组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="component">返回组件</param>
    /// <returns>找到时为<see langword="true"/>否则为<see langword="false"/></returns>
    public bool TryGetComponentInChildren<T>(out T? component) where T : Component
    {
        component = children.Find(e => e.HasComponent<T>())?.GetComponent<T>();
        return component is not null;
    }
    /// <summary>
    /// 在所有直系子对象上获取组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件</returns>
    /// <exception cref="InvalidOperationException">没有找到组件</exception>
    public T GetComponentInChildren<T>() where T : Component
    {
        if (TryGetComponentInChildren<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in {Name}'s children");
    }
    /// <summary>
    /// 在所有直系子对象上获取所有组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件列表</returns>
    public List<T> GetComponentsInChildren<T>() where T : Component
    {
        return children.FindAll(e => e.HasComponent<T>())
            .SelectMany(e => e.GetComponents<T>())
            .ToList();
    }
    /// <summary>
    /// 尝试在后代对象中获取组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="component">返回组件</param>
    /// <returns>找到时为<see langword="true"/>否则为<see langword="false"/></returns>
    public bool TryGetComponentInDescendants<T>(out T? component) where T : Component
    {
        var stack = new Stack<GameObject>(children);
        while (stack.Count > 0)
        {
            var obj = stack.Pop();
            if (obj.TryGetComponent<T>(out component))
                return true;
            obj.children.ForEach(e => stack.Push(e));
        }
        component = null;
        return false;
    }
    /// <summary>
    /// 在后代对象中获取组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件</returns>
    /// <exception cref="InvalidOperationException">没有找到组件</exception>
    public T GetComponentInDescendants<T>() where T : Component
    {
        if (TryGetComponentInDescendants<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in {Name}'s Descendants");
    }
    /// <summary>
    /// 在后代对象中获取所有组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件列表</returns>
    public List<T> GetComponentsInDescendants<T>() where T : Component
    {
        List<T> result = [];
        var stack = new Stack<GameObject>(children);
        while (stack.Count > 0)
        {
            var obj = stack.Pop();
            result.AddRange(obj.GetComponents<T>());
            obj.children.ForEach(e => stack.Push(e));
        }
        return result;
    }
    public bool TryGetComponentInParent<T>(out T? component) where T : Component
    {
        if (parent is null)
        {
            component = null;
            return false;
        }
        return parent.TryGetComponent<T>(out component);
    }
    public T GetComponentInParent<T>() where T : Component
    {
        if (TryGetComponentInParent<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in {Name}'s parent");
    }
    public List<T> GetComponentsInParent<T>() where T : Component => parent?.GetComponents<T>() ?? [];
    public bool TryGetComponentInAncestors<T>(out T? component) where T : Component
    {
        var parent = this.parent;
        while (parent is not null)
        {
            if (parent.TryGetComponent<T>(out component))
                return true;
            parent = parent.parent;
        }
        component = null;
        return false;
    }
    public T GetComponentInAncestors<T>() where T : Component
    {
        if (TryGetComponentInAncestors<T>(out var component))
            return component!;
        throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in {Name}'s Ancestors");
    }
    public List<T> GetComponentsInAncestors<T>() where T : Component
    {
        List<T> result = [];
        var parent = this.parent;
        while (parent is not null)
        {
            result.AddRange(parent.GetComponents<T>());
            parent = parent.parent;
        }
        return result;
    }
    /// <summary>
    /// 寻找直系子对象
    /// </summary>
    /// <param name="name">对象的 <see cref="Name"></see> 属性值</param>
    /// <returns>游戏对象</returns>
    public GameObject? GetChild(string name) => children.Find(e => e.Name == name);
    /// <summary>
    /// 根据名字寻找所有直系子对象
    /// </summary>
    /// <param name="name">对象的 <see cref="Name"></see> 属性值</param>
    /// <returns>游戏对象列表</returns>
    public List<GameObject> GetChildren(string name) => children.FindAll(e => e.Name == name);
    /// <summary>
    /// 添加子对象 (可一次性添加多个)
    /// </summary>
    /// <param name="children">子对象</param>
    public void AddChild(params List<GameObject> children)
    {
        this.children.AddRange(children);
        children.ForEach(e => { e.parent = this; e.Scene = Scene; });
    }
    /// <summary>
    /// 添加组件 (可一次性添加多个)
    /// </summary>
    /// <param name="components">组件</param>
    public void AddComponent(params List<Component> components)
    {
        components.ForEach(e => { e.Owner = this; e.Scene = Scene; });
        this.components.AddRange(components);
    }
    /// <summary>
    /// 如果组件有无参构造函数，可以调用此方法添加组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    public void AddComponent<T>() where T : Component, new()
    {
        var component = Activator.CreateInstance<T>();
        component.Owner = this;
        component.Scene = Scene;
        components.Add(component);
    }
    /// <summary>
    /// 永久移除游戏对象中所有符合类型的组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    public void RemoveComponents<T>() where T : Component
    {
        components = components.FindAll(e => e is not T);
    }
    /// <summary>
    /// 永久移除符合类型的组件
    /// </summary>
    /// <param name="components"></param>
    public void RemoveComponents(params List<Component> components)
    {
        components.ForEach(e => this.components.Remove(e));
    }
    /// <summary>
    /// 禁用组件 (禁止符合类型的组件进入游戏循环)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void DisableComponents<T>() where T : Component
    {
        components.FindAll(e => e is T).ForEach(e => e.IsEnable = false);
    }
    /// <summary>
    /// 启用所有符合类型的组件 (除非主动禁用过组件，否则组件在添加到对象上时已默认启用)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void EnableComponents<T>() where T : Component
    {
        components.FindAll(e => e is T).ForEach(e => e.IsEnable = true);
    }
    public void RemoveFromScene() => Scene.RemoveGameObjects(this);
    public void Start()
    {
        children.ForEach(e => e.Start());
        components.ForEach(e => e.Start());
    }

    public void Update(float deltaTime)
    {
        children.ForEach(e => { if (e.IsEnable) e.Update(deltaTime); });
        components.ForEach(e => { if (e.IsEnable) e.Update(deltaTime); });
    }

    public void Render()
    {
        if (!Visible)
            return;
        children.ForEach(e => e.Render());
        var parent = this.parent;
        var matrix = Transform.GetModelMatrix();
        while (parent is not null)
        {
            matrix *= parent.Transform.GetModelMatrix();
            parent = parent.parent;
        }
        Shader.Instance?.SetMvpMatrix(MVPMatrixTarget.Model, matrix);
        Model?.Render();
        if (EnableRenderLine)
            lineRenderer?.Render();
    }
}
