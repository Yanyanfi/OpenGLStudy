#version 330 core
struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};
//This is the directional light struct, where we only need the directions
struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
#define NR_DIR_LIGHTS 4
uniform int dirLightCount=0;
uniform DirLight dirLights[NR_DIR_LIGHTS];
//This is our pointlight where we need the position aswell as the constants defining the attenuation of the light.
struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
//We have a total of 4 point lights now, so we define a preprossecor directive to tell the gpu the size of our point light array
#define NR_POINT_LIGHTS 8
uniform int pointLightCount=0;
uniform PointLight pointLights[NR_POINT_LIGHTS];
//This is our spotlight where we need the position, attenuation along with the cutoff and the outer cutoff. Plus the direction of the light
struct SpotLight{
    vec3  position;
    vec3  direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
#define NR_SPOT_LIGHTS 2
uniform int spotLightCount=0;
uniform SpotLight spotLights[NR_SPOT_LIGHTS];

out vec4 outputColor;
in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;
in vec3 tangent;
in vec3 biTangent;
in vec3 color;    //for debug
uniform mat3 fnormal_mat;
uniform Material material;
uniform sampler2D texture0;//Color 
uniform sampler2D texture1;//Ambient
uniform sampler2D texture2;//Diffuse
uniform sampler2D texture3;//Specular
uniform sampler2D texture4;//Normal
uniform int fmode = 0;
uniform bool enColorTex = false;
uniform bool enAmbientTex = false;
uniform bool enDiffuseTex = false;
uniform bool enSpecularTex = false;
uniform bool enNormalTex = false;

vec3 _ambient;
vec3 _diffuse;
vec3 _specular;
vec3 _normal;

void textureUnlit();
void phoneLit();
void debugLine();
vec3 calcDirLight(DirLight light);
vec3 calcPointLight(PointLight light);
vec3 calcSpotLight(SpotLight light);

void main()
{
	if(fmode == 1)
		textureUnlit();
    else if(fmode == 2)
        phoneLit();
    else if(fmode == 3)
        debugLine();
}
void textureUnlit()
{
    outputColor = texture(texture0,texCoord);
}
void phoneLit()
{    
    _diffuse = enDiffuseTex ? vec3(texture(texture2,texCoord)) : material.diffuse;
    _ambient = enAmbientTex ? vec3(texture(texture1,texCoord)) : material.ambient;
    _specular = enSpecularTex? vec3(texture(texture3,texCoord)) : material.specular;
    if(enNormalTex)
    {
        mat3 TBN = mat3(tangent, biTangent, normal);
        vec3 texNormal = texture(texture4,texCoord).rgb*2.0-1.0;
        _normal = normalize(TBN*texNormal);
//        outputColor= vec4(vec3(material.shininess),1.0);
//        return;
    }
    else
    {
        _normal = normal;
    }
//    _diffuse = vec3(texture(texture2,dTexCoord));
//    _diffuse = material.diffuse;
//    _diffuse = vec3(dTexCoord,0);
    vec3 result = vec3(0.0);
    for(int i=0;i<dirLightCount;i++)
    {
        result+=calcDirLight(dirLights[i]);
    }
    for(int i=0;i<pointLightCount;i++)
    {
        result+=calcPointLight(pointLights[i]);
    }
    for(int i=0;i<spotLightCount;i++)
    {
        result+=calcSpotLight(spotLights[i]);
    }
    outputColor = vec4(result,1.0);
    
}

vec3 calcDirLight(DirLight light)
{
    vec3 ambient=light.ambient*_ambient;
    vec3 lightDir=normalize(-light.direction);
    float diff=max(dot(_normal,lightDir),0.0);
    vec3 diffuse=light.diffuse*diff*_diffuse;
    vec3 reflectDir = reflect(-lightDir, _normal);
    float spec = pow(max(dot(normalize(-fragPos), reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * _specular;
    return ambient+diffuse+specular;
}

vec3 calcPointLight(PointLight light)
{
    vec3 ambient = light.ambient * _ambient;

    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(_normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * _diffuse;

    vec3 viewDir = normalize(-fragPos);
    vec3 reflectDir = reflect(-lightDir, _normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * _specular;

    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);

    return (ambient + diffuse + specular) * attenuation;
}

vec3 calcSpotLight(SpotLight light)
{
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(_normal, lightDir), 0.0);

    vec3 viewDir = normalize(-fragPos);
    vec3 reflectDir = reflect(-lightDir, _normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    vec3 ambient = light.ambient * _ambient;
    vec3 diffuse = light.diffuse * diff * _diffuse;
    vec3 specular = light.specular * spec * _specular;

    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);

    return (ambient + diffuse + specular ) * attenuation * intensity;
}

void debugLine()
{
    outputColor=vec4(color,1.0);
}