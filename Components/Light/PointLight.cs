using OpenGLStudy.Model.Light;
using OpenGLStudy.Shaders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Components.Light;

internal class PointLight : Component
{
    public Vector3 Position
    {
        get => light.PositionOffset; 
        set => light.PositionOffset = value;
    }
    public float Constant 
    {
        get => light.Constant; 
        set => light.Constant = value;
    }
    public float Linear 
    {
        get => light.Linear; 
        set => light.Linear = value;
    }
    public float Quadratic 
    {
        get => light.Quadratic; 
        set => light.Quadratic = value;
    }
    public Vector3 Ambient 
    {
        get => light.Ambient; 
        set => light.Ambient = value;
    }
    public Vector3 Specular 
    {
        get => light.Specular; 
        set => light.Specular = value;
    }
    public Vector3 Diffuse 
    {
        get => light.Diffuse; 
        set => light.Diffuse = value;
    }
    private PointLightModel light;
    public PointLight(Vector3 position, float constant, float linear, float quadratic, Vector3 ambient, Vector3 diffuse, Vector3 specular)
    {
        light = new();
        Position = position;
        Constant = constant;
        Linear = linear;
        Quadratic = quadratic;
        Ambient = ambient;
        Specular = specular;
        Diffuse = diffuse;
    }
    public override void Start()
    {
        Owner.Model = null;
        if (Shader.Instance!.PointLightCount > Shader.MaxPointLightCount)
            return;
        light.Index = Shader.Instance!.PointLightCount++;
        Owner.Model = light;
    }
}
