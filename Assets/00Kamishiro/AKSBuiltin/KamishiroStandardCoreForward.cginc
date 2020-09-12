/*
 * Copyright (c) 2020 AoiKamishiro
 * 
 * This code is provided under the MIT license.
 *
 * This program uses the following code, which is provided under the MIT License.
 * https://download.unity3d.com/download_unity/008688490035/builtin_shaders-2018.4.20f1.zip?_ga=2.171325672.957521966.1599549120-262519615.1592172043
 * 
 */
 
#ifndef UNITY_STANDARD_CORE_FORWARD_INCLUDED
    #define UNITY_STANDARD_CORE_FORWARD_INCLUDED

    #include "UnityStandardConfig.cginc"
    #include "KamishiroStandardCore.cginc"

    VertexOutputForwardBase vertBase(VertexInput v)
    {
        return vertForwardBase(v);
    }
    VertexOutputForwardAdd vertAdd(VertexInput v)
    {
        return vertForwardAdd(v);
    }
    half4 fragBase(VertexOutputForwardBase i): SV_Target
    {
        return fragForwardBaseInternal(i);
    }
    half4 fragAdd(VertexOutputForwardAdd i): SV_Target
    {
        return fragForwardAddInternal(i);
    }

#endif // UNITY_STANDARD_CORE_FORWARD_INCLUDED
