using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Model;

internal interface IAnimatable
{
    void Start(string name, float speed, float startTime, bool loop);
    void Stop();
    void Update(float time);
    void SetStartTime(float time);
    void Reset(); 
}
