using OpenGLStudy.Model.Base;
using OpenTK.Mathematics;

namespace OpenGLStudy.Model;

internal class TxtMaterialModel : MaterialModel
{
    public TxtMaterialModel(string verticesPath, string indicesPath, Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        : base(ambient, diffuse, specular, shininess, Path.Combine(AppContext.BaseDirectory, verticesPath), Path.Combine(AppContext.BaseDirectory, indicesPath)) { }
    protected override void GetVerticesAndIndices(params object[] paths)
    {
        vertices = File.ReadAllText(paths[0].ToString()!)
            .Split([',', ' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => float.Parse(e.TrimEnd('f')))
            .ToArray();
        indices = File.ReadAllText(paths[1].ToString()!)
            .Split([',', ' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => uint.Parse(e))
            .ToArray();
    }
}
