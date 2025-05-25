using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLStudy.Inputs;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLStudy;

internal class GameScene
{
    private List<GameObject> gameObjects = [];
    public void AddGameObject(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
    }

    public void Start()
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.Start();
        }
    }
    public void Update(float deltaTime)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Update(deltaTime);
        }
    }

    public void Render()
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.Render();
        }
    }

    public bool TryGetComponent<T>(out T? component)
        where T : Component
    {
        foreach (var gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent<T>(out component))
                return true;
        }
        component = null;
        return false;
    }
}
