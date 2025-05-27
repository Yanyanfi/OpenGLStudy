using OpenTK.Mathematics;

namespace OpenGLStudy;

internal class Transform
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    public Vector3 Scale { get; set; } = Vector3.One;
    public Vector3 Front => (new Vector4(Vector3.UnitZ, 0) * Matrix4.CreateFromQuaternion(Rotation)).Xyz.Normalized();
    public Vector3 Up => (new Vector4(Vector3.UnitY, 0) * Matrix4.CreateFromQuaternion(Rotation)).Xyz.Normalized();
    public Vector3 Right => (new Vector4(-Vector3.UnitX, 0) * Matrix4.CreateFromQuaternion(Rotation)).Xyz.Normalized();
    public Matrix4 GetModelMatrix()
    {
        return Matrix4.CreateScale(Scale)
            * Matrix4.CreateFromQuaternion(Rotation)
            * Matrix4.CreateTranslation(Position);
    }

}
