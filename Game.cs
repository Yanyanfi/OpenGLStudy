using OpenGLStudy.Components;
using OpenGLStudy.Components.Light;
using OpenGLStudy.Enums;
using OpenGLStudy.Inputs;
using OpenGLStudy.Model;
using OpenGLStudy.Model.Light;
using OpenGLStudy.Models;
using OpenGLStudy.Shaders;
using OpenGLStudy.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLStudy;

class Game : GameWindow
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
        InputState.Keyboard = KeyboardState;
        InputState.Mouse = MouseState;
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
            "E:\\Visual Studio Project\\OpenGLStudy\\Shaders\\shader.vert",
            "E:\\Visual Studio Project\\OpenGLStudy\\Shaders\\shader.frag"
        );
        shader.Use();
        LoadTextures();
        LoadModels();
        InitializeGameObjects();
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
        if (scene.TryGetComponent<Camera>(out var camera))
        {
            camera!.AspectRatio = (float)e.Width / e.Height;
        }
    }

    private void LoadTextures()
    {
        textureManager = new();
        textureManager.AddTexture(
            TextureType.Wall,
            "Assets\\wall.jpg"
        );
        textureManager.AddTexture(
            TextureType.Nana,
            "Assets\\tw_icon_nana_sd.png"
        );
        textureManager.AddTexture(
            TextureType.Wall2,
            "Assets\\images.jfif"
        );
        textureManager.AddTexture(
            TextureType.Bloody,
            "Assets\\44514e8f49b58ea6aca4fb53d257f48d.jpg"
        );
    }

    private void LoadModels()
    {
        modelManager = new();
        modelManager.AddModel(
            new ResxPostexModel("Triangle", textureManager.GetTexture(TextureType.Wall2)),
            ModelType.Triangle
        );
        modelManager.AddModel(
            new ResxPostexModel("Cube", textureManager.GetTexture(TextureType.Wall)),
            ModelType.Cube
        );
        modelManager.AddModel(
            new TxtMaterialModel(
                "Assets\\MaterialCubeVertices.txt",
                "Assets\\MaterialCubeIndices.txt",
                new(1.0f, 0.5f, 0.31f),
                new(1.0f, 0.5f, 0.31f),
                new(0.5f, 0.5f, 0.5f),
                1),
            ModelType.GoldenCube
        );
        modelManager.AddModel(
            new BallModel(
                50,
                50,
                new(1.0f, 0.5f, 0.31f),
                new(1.0f, 0.5f, 0.31f),
                new(0.5f, 0.5f, 0.5f),
                32),
            ModelType.Ball
        );
        modelManager.AddModel(new ObjModel("Assets\\虚拟人物 二次元美女 动漫美女 可爱女孩_爱给网_aigei_com\\modelNew.fbx"), ModelType.Girl);
        modelManager.AddModel(new ObjModel("Assets\\bugatti\\bugatti.obj"), ModelType.Car);
        modelManager.AddModel(new ObjModel("Assets\\cottage\\cottage_obj.obj", true), ModelType.House);
        modelManager.AddModel(new ObjModel("Assets\\IronMan\\IronMan.obj"), ModelType.IronMan);
        modelManager.AddModel(new ObjModel("Assets\\3pl90nmkl3sw-building_04_all\\building_04.obj"), ModelType.Building);
    }

    private void InitializeGameObjects()
    {
        scene = new();
        GameObject triangle = new("Triangle", new(0, 10, 0), new(10, 10, 10), new(1, 1, 1))
        {
            Model = modelManager.GetModel(ModelType.Triangle),
        };
        triangle.AddComponent<TriangleSpinKeyboardController>();
        GameObject cube = new("Cube", new(0, 0, -1))
        {
            Model = modelManager.GetModel(ModelType.Cube),

        };
        cube.AddComponent(new TriangleSpinKeyboardController() { Speed = 1000f });
        var objTest = new GameObject("Girl")
        {
            Model = modelManager.GetModel(ModelType.Girl)
        };
        //cube.AddComponent(new BiggerAndBigger(1.1f, 1f));
        GameObject player = new("Player", new(1, 2, 1), new(1,1,1), Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(90f)))
        {
            Model = modelManager.GetModel(ModelType.Cube),
        };

        player.AddComponent(
            new Camera(new(0, 1f, 0), width / height, true) { FarPlane = 10000f }
        );
        player.AddComponent(new PointLight(new(0, 0, 0), 1.0f, 0.35f, 0.44f, new(0.1f, 0.1f, 0.1f), new(1, 1, 1), new(1, 1, 1)));
        player.AddComponent<CameraPerspectiveSwitcher>();
        player.AddComponent<ThirdPersonCameraDistanceController>();
        player.AddChild(cube);
        player.AddChild(objTest);
        var sun = new GameObject("Sun");
        sun.AddComponent(new DirectLight(new(-0.2f, -1.0f, -0.3f), new(0.1f, 0.1f, 0.1f), new(0.6f, 0.6f, 0.6f), new(2f, 2f, 2f)));
        var materialCube = new GameObject("MaterialCube", new(4, 4, 4))
        {
            Model = modelManager.GetModel(ModelType.GoldenCube)
        };
        var earth = new GameObject("Earth", new(0,0,0), new(5, 5, 5))
        {
            Model = modelManager.GetModel(ModelType.Ball)
        };
        earth.AddComponent(new TriangleSpinKeyboardController() { Speed=10});
        var moon = new GameObject("Moon", new(2, 0, 2), new(0.3f, 0.3f, 0.3f))
        {
            Model = modelManager.GetModel(ModelType.Ball)
        };
        earth.AddChild(moon);
        var car = new GameObject("Car", new(20, 20, 20))
        {
            Model = modelManager.GetModel(ModelType.Car)
        };
        var carLight = new GameObject("carLight", new(0, 5, 0));
        //carLight.AddComponent(new PointLight(new(), 1.0f, 0.045f, 0.0075f, new(0.1f, 0.1f, 0.1f), new(0.8f, 0.8f, 0.8f), new(1, 1, 1)));
        car.AddChild(carLight);
        var house = new GameObject("House", new(-20, 0, -20),new(1,1,1))
        {
            Model = modelManager.GetModel(ModelType.House)
        };
        var houseCube = new GameObject("HouseCube", new(10, 0, 0), new(4, 4, 4))
        {
            Model = modelManager.GetModel(ModelType.Girl)
        };
        house.AddChild(houseCube);
        scene.AddGameObject(sun);
        scene.AddGameObject(materialCube);
        scene.AddGameObject(player);
        scene.AddGameObject(triangle);
        scene.AddGameObject(cube);
        scene.AddGameObject(earth);
        scene.AddGameObject(car);
        scene.AddGameObject(house);
    }
}
