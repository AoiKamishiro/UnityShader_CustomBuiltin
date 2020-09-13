/*
 * Copyright (c) 2020 AoiKamishiro
 * 
 * This code is provided under the MIT license.
 *
 * This program uses the following code, which is provided under the MIT License.
 * https://download.unity3d.com/download_unity/008688490035/builtin_shaders-2018.4.20f1.zip?_ga=2.171325672.957521966.1599549120-262519615.1592172043
 * 
 */

using UnityEditor;
using UnityEngine;
namespace AKSBuiltin
{
    public class AKSStyles : MonoBehaviour
    {
        public const string localVer = "Local Version: ";
        public const string remoteVer = "Remote Version: ";
        public const string btnUpdate = "Download latest version.";
        public const string btnDescription = "操作説明（日本語）";
        public const string btnReadme = "README.md";
        public const string nameAKStandard = "AK_Standard Shader";
        public const string nameAKUnlit = "AK_Unlit Shader";
        public const string author = "Author: AoiKamishiro / 神城アオイ";
        public const string linkRelease = "https://github.com/AoiKamishiro/UnityShader_CustomBuiltin/releases";
        public const string linkReadme = "https://github.com/AoiKamishiro/UnityShader_CustomBuiltin";
        public const string linkDescriptionAKStandard = "https://github.com/AoiKamishiro/UnityShader_CustomBuiltin/blob/master/AKStandard_Description.md";
        public const string linkDescriptionAKUnlit = "https://github.com/AoiKamishiro/UnityShader_CustomBuiltin/blob/master/AKUnlit_Description.md";


        public static GUIContent uvSetLabel = EditorGUIUtility.TrTextContent("UV Set");
        public static GUIContent albedoText = EditorGUIUtility.TrTextContent("Albedo", "Albedo (RGB) and Transparency (A)");
        public static GUIContent alphaCutoffText = EditorGUIUtility.TrTextContent("Alpha Cutoff", "Threshold for alpha cutoff");
        public static GUIContent metallicMapText = EditorGUIUtility.TrTextContent("Metallic", "Metallic (R) and Smoothness (A)");
        public static GUIContent smoothnessText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness value");
        public static GUIContent smoothnessScaleText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness scale factor");
        public static GUIContent smoothnessMapChannelText = EditorGUIUtility.TrTextContent("Source", "Smoothness texture and channel");
        public static GUIContent highlightsText = EditorGUIUtility.TrTextContent("Specular Highlights", "Specular Highlights");
        public static GUIContent reflectionsText = EditorGUIUtility.TrTextContent("Reflections", "Glossy Reflections");
        public static GUIContent normalMapText = EditorGUIUtility.TrTextContent("Normal Map", "Normal Map");
        public static GUIContent heightMapText = EditorGUIUtility.TrTextContent("Height Map", "Height Map (G)");
        public static GUIContent occlusionText = EditorGUIUtility.TrTextContent("Occlusion", "Occlusion (G)");
        public static GUIContent emissionText = EditorGUIUtility.TrTextContent("Color", "Emission (RGB)");
        public static GUIContent detailMaskText = EditorGUIUtility.TrTextContent("Detail Mask", "Mask for Secondary Maps (A)");
        public static GUIContent detailAlbedoText = EditorGUIUtility.TrTextContent("Detail Albedo x2", "Albedo (RGB) multiplied by 2");
        public static GUIContent detailNormalMapText = EditorGUIUtility.TrTextContent("Normal Map", "Normal Map");
        public static GUIContent smoothMapText = EditorGUIUtility.TrTextContent("Smoothness", "Roughness Map");
        public static GUIContent cullModeText = EditorGUIUtility.TrTextContent("Culling Mode", "Culling Mode");

        public const string primaryMapsText = "Main Maps";
        public const string secondaryMapsText = "Secondary Maps";
        public const string options = "Options";
        public const string forwardText = "Forward Rendering Options";
        public const string renderingMode = "Rendering Mode";
        public const string advancedText = "Advanced Options";
        public const string mainTitle = "Main";
        public const string emissionTitle = "Emission";
        public const string reflectionTitle = "Reflection";
        public const string scaleOffsetTitle = "Scale Offset";
        public const string detailTitle = "Detail";
        public const string renderingOpTitle = "Rendering Options";
    }
}