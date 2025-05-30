using OpenTK.Graphics.ES11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.UselessFile;

internal class Vertex : IEnumerable<float>
{
    public Vertex(float x, float y, float z, float texCoordX, float texCoordY)
    {
        Position = new(x, y, z);
        TexCoord = new(texCoordX, texCoordY);
    }
    public Vector3 Position { get; set; }
    public Vector2 TexCoord { get; set; }
    
    public static int Size => sizeof(float) * 5;

    public IEnumerator<float> GetEnumerator()
    {
        yield return Position.X;
        yield return Position.Y;
        yield return Position.Z;
        yield return TexCoord.X;
        yield return TexCoord.Y;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
