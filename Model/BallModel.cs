using OpenGLStudy.Model.Base;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal class BallModel : MaterialModel
{
    public BallModel(int latitudeSegments, int longitudeSegments, Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        : base(ambient, diffuse, specular, shininess, latitudeSegments, longitudeSegments) { }

    protected override void GetVerticesAndIndices(params object[] args)
    {
        GenerateVertices((int)args[0], (int)args[1]);
        GenerateIndices((int)args[0], (int)args[1]);
    }
    private void GenerateVertices(int latitudeSegments, int longitudeSegments)
    {
        List<float> vertices = [];
        for (int y = 0; y <= latitudeSegments; y++)
        {
            float v = (float)y / latitudeSegments;
            float theta = v * MathF.PI;

            for (int x = 0; x <= longitudeSegments; x++)
            {
                float u = (float)x / longitudeSegments;
                float phi = u * 2.0f * MathF.PI;

                // 球面坐标系转笛卡尔坐标系
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);
                float sinPhi = MathF.Sin(phi);
                float cosPhi = MathF.Cos(phi);

                float px = sinTheta * cosPhi;
                float py = cosTheta;
                float pz = sinTheta * sinPhi;

                // 单位球中心在原点，顶点位置与法线相同
                vertices.Add(px); // 位置 x
                vertices.Add(py); // 位置 y
                vertices.Add(pz); // 位置 z

                vertices.Add(px); // 法线 x
                vertices.Add(py); // 法线 y
                vertices.Add(pz); // 法线 z
            }
        }
        this.vertices = vertices.ToArray();
    }
    private void GenerateIndices(int latitudeSegments, int longitudeSegments)
    {
        List<uint> indices = new();

        for (int y = 0; y < latitudeSegments; y++)
        {
            for (int x = 0; x < longitudeSegments; x++)
            {
                uint i0 = (uint)(y * (longitudeSegments + 1) + x);
                uint i1 = (uint)((y + 1) * (longitudeSegments + 1) + x);
                uint i2 = (uint)((y + 1) * (longitudeSegments + 1) + x + 1);
                uint i3 = (uint)(y * (longitudeSegments + 1) + x + 1);

                // 每个四边形拆成两个三角形
                indices.Add(i0);
                indices.Add(i1);
                indices.Add(i2);

                indices.Add(i0);
                indices.Add(i2);
                indices.Add(i3);
            }
        }

        this.indices=indices.ToArray();
    }
}
