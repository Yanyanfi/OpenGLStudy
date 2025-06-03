#version 330 core
#define MAX_BONES 10
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;
layout (location = 3) in vec3 aTangent;
layout (location = 4) in vec3 aBiTangent;
layout (location = 5) in vec3 aColor;//for debug
layout (location = 6) in vec4 aBoneIndices;
layout (location = 7) in vec4 aBoneWeights;
uniform int mode=0;
uniform mat4 mvp;
uniform mat4 mv;
uniform mat3 normal_mat;//for viewspace transform
uniform mat4 boneMatrices[MAX_BONES];
out vec2 texCoord;
out vec3 fragPos;
out vec3 normal;
out vec2 dTexCoord;
out vec3 tangent;
out vec3 biTangent;
out vec3 color; //for debug

void textureUnlit();
void phoneLit();
void debugLine();


void main()
{
    fragPos = vec3(mv * vec4(aPosition, 1.0));
    gl_Position = mvp * vec4(aPosition, 1.0);
    if(mode == 1)
    {
        textureUnlit();
    }
    else if(mode == 2)
    {
        phoneLit();
    }
    else if(mode == 3)
    {
        debugLine();
    }
}

void textureUnlit()
{ 
    texCoord = aTexCoord;
}

void phoneLit()
{
    normal = normalize(normal_mat*aNormal);
    tangent = normalize(normal_mat*aTangent);
    biTangent=normalize(normal_mat*aBiTangent);
    texCoord = aTexCoord;
}

void debugLine()
{
    color = aColor;
}


