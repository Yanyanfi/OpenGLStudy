using OpenGLStudy.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.UselessFile;

internal abstract class TxtModel : IRenderable
{
    protected float[] vertices = null!;
    protected uint[] indices = null!;
    protected int vao;
    protected TxtModel(string verticesFilePath, string IndicesFilePath)
    {
        GetVertices(verticesFilePath);
        GetIndices(IndicesFilePath);
        GenVao();
    }
    public abstract void Render();
    protected abstract void GenVao();
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
