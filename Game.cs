using OpenGLStudy.Components;
using OpenGLStudy.Components.Cameras;
using OpenGLStudy.Components.Light;
using OpenGLStudy.Enums;
using OpenGLStudy.Inputs;
using OpenGLStudy.Model;
using OpenGLStudy.Models;
using OpenGLStudy.Shaders;
using OpenGLStudy.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Xml.Schema;

namespace OpenGLStudy;

partial class Game : GameWindow
{
    private int width,
        height;

    private Shader shader = null!;
    private ModelManager modelManager = null!;
    private TextureManager textureManager = null!;
    private GameScene scene = null!;

    public Game(int width, int height, Vector2i location, string title)
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (width, height), Title = title }
        )
    {
        this.width = width;
        this.height = height;
        Location = location;
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
        shader = new(
            "Assets\\shaders\\shader.vert",
            "Assets\\shaders\\shader.frag"
        );
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
        //GL.ClearColor(
        //    (float)MathHelper.Abs(MathHelper.Cos(GLFW.GetTime())),
        //    (float)MathHelper.Abs(MathHelper.Sin(GLFW.GetTime())),
        //    (float)MathHelper.Abs(MathHelper.Sin(GLFW.GetTime())),
        //    1f
        //);
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
