using OpenGLStudy.Shaders;
using OpenGLStudy.Shaders.Enums;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model.Light;

internal class PointLightModel : IRenderable
{
    public Vector3 PositionOffset { get; set; }
    public float Constant { get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }
    public Vector3 Ambient { get; set; }
    public Vector3 Specular { get; set; }
    public Vector3 Diffuse { get; set; }
    public int Index { get; set; }
    private Vector3 WorldPosition => (new Vector4(PositionOffset, 1) * Shader.Instance!.Model).Xyz;
    public PointLightModel(Vector3 positionOffset, float constant, float linear, float quadratic, Vector3 ambient, Vector3 specular, Vector3 diffuse)
    {
        PositionOffset = positionOffset;
        Constant = constant;
        Linear = linear;
        Quadratic = quadratic;
        Ambient = ambient;
        Specular = specular;
        Diffuse = diffuse;
    }
    public PointLightModel()
    {
        
    }
    public void Render()
    {
        var viewPosition = (new Vector4(WorldPosition, 1) * Shader.Instance!.View).Xyz;
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Position, Index, viewPosition);
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Constant, Index, Constant);
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Linear, Index, Linear);
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Quadratic, Index, Quadratic);
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Ambient, Index, Ambient);
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Specular,Index,Specular);
        Shader.Instance?.SetPointLightMaterial(PointLightTarget.Diffuse, Index, Diffuse);
    }
    

}
