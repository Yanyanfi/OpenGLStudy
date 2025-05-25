using OpenGLStudy.Model.Light;
using OpenGLStudy.Shaders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Components.Light;

internal class DirectLight : Component
{
    public Vector3 Direction
    {
        get => light.Direction; 
        set => light.Direction = value;
    }
    public Vector3 Ambient
    {
        get => light.Ambient; 
        set => light.Ambient = value;
    }
    public Vector3 Diffuse
    {
        get => light.Diffuse; 
        set => light.Diffuse = value;
    }
    public Vector3 Specular
    {
        get => light.Specular; 
        set => light.Specular = value;
    }
    private DirectLigthModel light;
    public DirectLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
    {
        light = new();
        Direction = direction;
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
    }
    public override void Start()
    {
        Owner.Model = null;
        if (Shader.Instance!.DirLightCount > Shader.MaxDirLightCount)
            return;
        light.Index = Shader.Instance.DirLightCount++;
        Owner.Model = light;
    }
}
