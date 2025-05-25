using OpenGLStudy.Shaders;
using OpenGLStudy.Shaders.Enums;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model.Light;

internal class DirectLigthModel : IRenderable
{
    public Vector3 Direction { get; set; }
    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
    public int Index { get; set; }
    public DirectLigthModel(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular, int lightIndex)
    {
        Direction = direction;
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
        Index = lightIndex;
    }
    public DirectLigthModel()
    {
        
    }
    private Vector3 viewDirection;
    public void Render()
    {
        var shader = Shader.Instance;
        shader?.SetMode(ShaderMode.MaterialLight);
        viewDirection = viewDirection = (new Vector4(Direction, 0) * Shader.Instance!.View).Xyz;
        shader?.SetDirLightMaterial(DirLightTarget.Direction, Index, viewDirection);
        shader?.SetDirLightMaterial(DirLightTarget.Ambient, Index, Ambient);
        shader?.SetDirLightMaterial(DirLightTarget.Diffuse, Index, Diffuse);
        shader?.SetDirLightMaterial(DirLightTarget.Specular, Index, Specular);
    }
}
