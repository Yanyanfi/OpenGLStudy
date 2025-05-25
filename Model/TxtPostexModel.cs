using OpenGLStudy.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal class TxtPostexModel : PostexModel
{
    public TxtPostexModel(string verticesPath, string indicesPath, Texture texture) : base(texture, verticesPath, indicesPath) { }

    protected override void GetVerticesAndIndices(params object[] paths)
    {
        GetVertices(paths[0].ToString()!);
        GetIndices(paths[1].ToString()!);
    }
    private void GetVertices(string verticesFilePath)
    {
        vertices = File.ReadAllText(verticesFilePath)
            .Split([',', ' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => float.Parse(e.TrimEnd('f')))
            .ToArray();
    }
    private void GetIndices(string indicesFilePath)
    {
        indices = File.ReadAllText(indicesFilePath)
            .Split([',', ' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => uint.Parse(e))
            .ToArray();
    }
}
