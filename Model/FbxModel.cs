using Assimp;
using OpenGLStudy.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal class FbxModel : ObjModel, IAnimatable
{
    public FbxModel(string path, bool useDiffuseAsSpecular = false) : base(path, useDiffuseAsSpecular) { }
    public FbxModel(string path, Transform offset, bool useDiffuseAsSpecular = false) : base(path, offset, useDiffuseAsSpecular) { }

    protected override void GenVao()
    {
        base.GenVao();
    }
    protected override void GetVerticesAndIndices(params object[] args)
    {
        base.GetVerticesAndIndices(args);
        var fbxPath = args[0].ToString()!;
        GetAnimations(fbxPath);
    }
    private void GetAnimations(string fbxPath)
    {
        var scene = new AssimpContext().ImportFile(fbxPath);
        foreach(var mesh in scene.Meshes)
        {
            var hasBones = mesh.HasBones;
        }
    }
    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void SetStartTime(float time)
    {
        throw new NotImplementedException();
    }

    public void Start(string name, float speed, float startTime, bool loop)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Update(float time)
    {
        throw new NotImplementedException();
    }
}
