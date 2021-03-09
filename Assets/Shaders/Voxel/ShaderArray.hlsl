//UNITY_SHADER_NO_UPGRADE
#ifndef SHADERSWITCH_INCLUDED
#define SHADERSWITCH_INCLUDED

void ShaderSwitch_float(float4 Grass, float4 Dirt, float1 Index, out float4 Out)
{
    switch (Index)
    {
        case 1:
            Out = Grass;
        break;
        case 2:
            Out = Dirt;
        break;
        default:
            Out = Grass;
        break;
    }
}
#endif