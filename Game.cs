using OpenGLStudy.Components.Cameras;
using OpenGLStudy.Inputs;
using OpenGLStudy.Models;
using OpenGLStudy.Shaders;
using OpenGLStudy.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLStudy;

partial class Game : GameWindow
{
    private int width,
        height;

    private Shader shader;
    private ModelManager modelManager;
    public ModelManager ModelManager => modelManager; // 公有只读属性
    private TextureManager textureManager;
    private GameScene scene;

    public Game(int width, int height, Vector2i location, string title)
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
    {
        this.width = width;
        this.height = height;
        Location = location;
        shader = new(
            "Assets\\shaders\\shader.vert",
            "Assets\\shaders\\shader.frag"
        );
        textureManager = new();
        modelManager = new();
        scene = new();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        scene.Update((float)args.Time);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(DepthFunction.Less);
        CursorState = CursorState.Grabbed;
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        shader.Use();
        LoadTextures();
        LoadModels();
        InitializeGameObjects();
        InputState.Keyboard = KeyboardState;
        InputState.Mouse = MouseState;
        scene.Start();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        shader.Dispose();
        textureManager.ClearAll();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        scene.Render();
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        width = e.Width;
        height = e.Height;
        scene.TryGetComponent<Camera>(out var camera);
        camera?.AspectRatio = (float)e.Width / e.Height;

    }
    private partial void LoadTextures();

    private partial void LoadModels();

    private partial void InitializeGameObjects();
}
