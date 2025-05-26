using OpenGLStudy.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal class FbxModel : ObjModel
{
    public FbxModel(string path ,bool useDiffuseAsSpecular = false) : base(path, useDiffuseAsSpecular)
    {
    }
}
