using OpenGLStudy.Components;
using OpenGLStudy.Components.Cameras;
using OpenGLStudy.Components.Light;
using OpenGLStudy.Enums;
using OpenGLStudy.Model;
using OpenTK.Mathematics;

namespace OpenGLStudy;

internal partial class Game
{
    private partial void LoadTextures()
    {
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
    private partial void LoadModels()
    {
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
        modelManager.AddModel(new FbxModel("Assets\\虚拟人物 二次元美女 动漫美女 可爱女孩_爱给网_aigei_com\\modelNew.fbx",
            new Transform() { Rotation = new(-(float)Math.PI / 2, 0, (float)Math.PI) }), ModelType.Girl);
        modelManager.AddModel(new ObjModel("Assets\\bugatti\\bugatti.obj"), ModelType.Car);
        modelManager.AddModel(new ObjModel("Assets\\cottage\\cottage_obj.obj", false), ModelType.House);
        modelManager.AddModel(new ObjModel("Assets\\IronMan\\IronMan.obj"), ModelType.IronMan);
        modelManager.AddModel(new ObjModel("Assets\\3pl90nmkl3sw-building_04_all\\building_04.obj"), ModelType.Building);
        modelManager.AddModel(new FbxModel("Assets\\shaonv1\\shaonv1_a_2011.FBX",
            new() { Rotation = new(-(float)Math.PI / 2, 0, (float)Math.PI), Scale = new(0.01f) }, false), ModelType.Fairy);
    }
    private partial void InitializeGameObjects()
    {
        var triangle = new GameObject("Triangle", new(0, 10, 0), new(10, 10, 10), new(1, 1, 1))
        {
            Model = modelManager.GetModel(ModelType.Triangle),
        };
        triangle.AddComponent<TriangleSpinKeyboardController>();
        var cube = new GameObject("Cube", new(0, 0, -2))
        {
            Model = modelManager.GetModel(ModelType.Cube),

        };
        cube.AddComponent(new TriangleSpinKeyboardController() { Speed = 1000f });
        var objTest = new GameObject("Girl")
        {
            Model = modelManager.GetModel(ModelType.Girl)
        };
        //cube.AddComponent(new BiggerAndBigger(1.1f, 1f));
        var player = new GameObject("Player", new(1, 2, 1))
        {
            Model = modelManager.GetModel(ModelType.Cube),
        };

        player.AddComponent(
            new PlayerMove() { Speed = 8 },
            new CameraDirectionController(),
            new PlayerDirectionController(),
            new Camera(new(0, 1.4f, 0), width / height, true) { FarPlane = 1000 },
            new PointLight(new(0, 0, 0), 1.0f, 0.35f, 0.44f, new(0.1f, 0.1f, 0.1f), new(1, 1, 1), new(1, 1, 1)),
            new CameraPerspectiveSwitcher(),
            new ThirdPersonCameraDistanceController()
        );
        player.AddChild(cube);
        player.AddChild(objTest);
        var sun = new GameObject("Sun");
        sun.AddComponent(new DirectLight(new(-0.2f, -1.0f, -0.3f), new(0.1f, 0.1f, 0.1f), new(0.6f, 0.6f, 0.6f), new(2f, 2f, 2f)));
        var materialCube = new GameObject("MaterialCube", new(4, 4, 4))
        {
            Model = modelManager.GetModel(ModelType.GoldenCube)
        };
        var earth = new GameObject("Earth", new(0, 0, 0), new(5, 5, 5))
        {
            Model = modelManager.GetModel(ModelType.Ball)
        };
        earth.AddComponent(new TriangleSpinKeyboardController() { Speed = 10 });
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
        var house = new GameObject("House", new(-20, 0, -20), new(1, 1, 1))
        {
            Model = modelManager.GetModel(ModelType.House)
        };
        var houseMaid = new GameObject("HouseMaid", new(10, 0, 0), new(4, 4, 4))
        {
            Model = modelManager.GetModel(ModelType.Fairy)
        };
        house.AddChild(houseMaid);
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
