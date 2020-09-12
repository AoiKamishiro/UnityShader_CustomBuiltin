/*
 * Copyright (c) 2020 AoiKamishiro
 * 
 * This code is provided under the MIT license.
 *
 * This program uses the following code, which is provided under the MIT License.
 * https://download.unity3d.com/download_unity/008688490035/builtin_shaders-2018.4.20f1.zip?_ga=2.171325672.957521966.1599549120-262519615.1592172043
 * https://github.com/synqark/Arktoon-Shaders
 * 
 */

using System;
using UnityEngine;
using UnityEditor;

namespace AKSBuiltin
{
    internal class AKSStandardShaderGUI : ShaderGUI
    {
        private enum WorkflowMode
        {
            Specular_,
            Metallic,
            Dielectric
        }
        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
            Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
        }
        public enum SmoothnessMapChannel
        {
            SpecularMetallicAlpha,
            AlbedoAlpha,
            RoughnessMap
        }

        private static class Styles
        {
            public static GUIContent uvSetLabel = EditorGUIUtility.TrTextContent("UV Set");
            public static GUIContent albedoText = EditorGUIUtility.TrTextContent("Albedo", "Albedo (RGB) and Transparency (A)");
            public static GUIContent alphaCutoffText = EditorGUIUtility.TrTextContent("Alpha Cutoff", "Threshold for alpha cutoff");
            public static GUIContent specularMapText = EditorGUIUtility.TrTextContent("Specular", "Specular (RGB) and Smoothness (A)");
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

            public static string primaryMapsText = "Main Maps";
            public static string secondaryMapsText = "Secondary Maps";
            public static string options = "Options";
            public static string forwardText = "Forward Rendering Options";
            public static string renderingMode = "Rendering Mode";
            public static string advancedText = "Advanced Options";
            public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
            public static string mainTitle = "Main";
            public static string emissionTitle = "Emission";
            public static string reflectionTitle = "Reflection";
            public static string scaleOffsetTitle = "Scale Offset";
            public static string detailTitle = "Detail";
            public static string renderingOpTitle = "Rendering Options";
            public static string title = "AK_Standard Shader v1.0 by AoiKamishiro";
        }

        #region Material Property
        private MaterialProperty blendMode = null;
        private MaterialProperty albedoMap = null;
        private MaterialProperty albedoColor = null;
        private MaterialProperty alphaCutoff = null;
        private MaterialProperty specularMap = null;
        private MaterialProperty metallicMap = null;
        private MaterialProperty metallic = null;
        private MaterialProperty smoothness = null;
        private MaterialProperty smoothnessScale = null;
        private MaterialProperty smoothnessMapChannel = null;
        private MaterialProperty highlights = null;
        private MaterialProperty reflections = null;
        private MaterialProperty bumpScale = null;
        private MaterialProperty bumpMap = null;
        private MaterialProperty occlusionStrength = null;
        private MaterialProperty occlusionMap = null;
        private MaterialProperty heigtMapScale = null;
        private MaterialProperty heightMap = null;
        private MaterialProperty emissionColorForRendering = null;
        private MaterialProperty emissionMap = null;
        private MaterialProperty detailMask = null;
        private MaterialProperty detailAlbedoMap = null;
        private MaterialProperty detailNormalMapScale = null;
        private MaterialProperty detailNormalMap = null;
        private MaterialProperty uvSetSecondary = null;
        private MaterialProperty cullMode = null;
        private MaterialProperty roughnessMap = null;
        //MaterialProperty srcFactor = null;
        //MaterialProperty dstFactor = null;
        //MaterialProperty zwrite = null;
        //MaterialProperty ztest = null;
        #endregion
        private MaterialEditor m_MaterialEditor;
        private WorkflowMode m_WorkflowMode = WorkflowMode.Metallic;
        private bool m_FirstTimeApply = true;
        static bool foldMain = true;
        static bool foldSecond = true;
        static bool foldOption = true;

        public void FindProperties(MaterialProperty[] props)
        {
            blendMode = FindProperty("_Mode", props);
            albedoMap = FindProperty("_MainTex", props);
            albedoColor = FindProperty("_Color", props);
            alphaCutoff = FindProperty("_Cutoff", props);
            specularMap = FindProperty("_SpecGlossMap", props, false);
            metallicMap = FindProperty("_MetallicGlossMap", props, false);
            metallic = FindProperty("_Metallic", props, false);
            if (metallicMap != null && metallic != null)
                m_WorkflowMode = WorkflowMode.Metallic;
            else
                m_WorkflowMode = WorkflowMode.Dielectric;
            smoothness = FindProperty("_Glossiness", props);
            smoothnessScale = FindProperty("_GlossMapScale", props, false);
            smoothnessMapChannel = FindProperty("_SmoothnessTextureChannel", props, false);
            highlights = FindProperty("_SpecularHighlights", props, false);
            reflections = FindProperty("_GlossyReflections", props, false);
            bumpScale = FindProperty("_BumpScale", props);
            bumpMap = FindProperty("_BumpMap", props);
            heigtMapScale = FindProperty("_Parallax", props);
            heightMap = FindProperty("_ParallaxMap", props);
            occlusionStrength = FindProperty("_OcclusionStrength", props);
            occlusionMap = FindProperty("_OcclusionMap", props);
            emissionColorForRendering = FindProperty("_EmissionColor", props);
            emissionMap = FindProperty("_EmissionMap", props);
            detailMask = FindProperty("_DetailMask", props);
            detailAlbedoMap = FindProperty("_DetailAlbedoMap", props);
            detailNormalMapScale = FindProperty("_DetailNormalMapScale", props);
            detailNormalMap = FindProperty("_DetailNormalMap", props);
            uvSetSecondary = FindProperty("_UVSec", props);
            cullMode = FindProperty("_Cull", props);
            roughnessMap = FindProperty("_RoughnessMap", props);
            //srcFactor = FindProperty("_SrcBlend", props);
            //dstFactor = FindProperty("_DstBlend", props);
            //zwrite = FindProperty("_ZWrite", props);
            //ztest = FindProperty("_ZTest", props);
        }
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            // Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
            // material to a standard shader.
            // Do this before any GUI code has been issued to prevent layout issues in subsequent GUILayout statements (case 780071)
            if (m_FirstTimeApply)
            {
                MaterialChanged(material, m_WorkflowMode);
                m_FirstTimeApply = false;
            }

            if (material.globalIlluminationFlags != MaterialGlobalIlluminationFlags.RealtimeEmissive && material.globalIlluminationFlags != MaterialGlobalIlluminationFlags.BakedEmissive)
            {
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            ShaderPropertiesGUI(material);
        }
        public void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;
            UIHelper.ShurikenHeader(Styles.title);
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            {
                BlendModePopup();
                foldMain = UIHelper.ShurikenFoldout(Styles.primaryMapsText, foldMain);
                if (foldMain)
                {
                    GUILayout.Label(Styles.mainTitle, EditorStyles.boldLabel);
                    DoAlbedoArea(material);
                    DoNormalArea();
                    m_MaterialEditor.TexturePropertySingleLine(Styles.heightMapText, heightMap, heightMap.textureValue != null ? heigtMapScale : null);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.occlusionText, occlusionMap, occlusionMap.textureValue != null ? occlusionStrength : null);
                    GUILayout.Label(Styles.emissionTitle, EditorStyles.boldLabel);
                    DoEmissionArea(material);
                    GUILayout.Label(Styles.reflectionTitle, EditorStyles.boldLabel);
                    DoSpecularMetallicArea();
                    GUILayout.Label(Styles.scaleOffsetTitle, EditorStyles.boldLabel);
                    EditorGUI.indentLevel += 2;
                    EditorGUI.BeginChangeCheck();
                    {
                        m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        emissionMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset;
                    }
                    EditorGUI.indentLevel -= 2;
                }
                foldSecond = UIHelper.ShurikenFoldout(Styles.secondaryMapsText, foldSecond);
                if (foldSecond)
                {
                    GUILayout.Label(Styles.detailTitle, EditorStyles.boldLabel);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.detailMaskText, detailMask);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.detailAlbedoText, detailAlbedoMap);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.detailNormalMapText, detailNormalMap, detailNormalMapScale);
                    EditorGUI.indentLevel +=3;
                    m_MaterialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
                    EditorGUI.indentLevel--;
                    m_MaterialEditor.ShaderProperty(uvSetSecondary, Styles.uvSetLabel.text);
                    EditorGUI.indentLevel -= 2;
                }
                foldOption = UIHelper.ShurikenFoldout(Styles.options, foldOption);
                if (foldOption)
                {
                    GUILayout.Label(Styles.forwardText, EditorStyles.boldLabel);
                    EditorGUI.indentLevel += 1;
                    if (highlights != null)
                    {
                        m_MaterialEditor.ShaderProperty(highlights, Styles.highlightsText);
                    }
                    if (reflections != null)
                    {
                        m_MaterialEditor.ShaderProperty(reflections, Styles.reflectionsText);
                    }
                    EditorGUI.indentLevel -= 1;
                    GUILayout.Label(Styles.renderingOpTitle, EditorStyles.boldLabel);
                    EditorGUI.indentLevel += 1;
                    m_MaterialEditor.ShaderProperty(cullMode, Styles.cullModeText);
                    m_MaterialEditor.RenderQueueField();
                    //m_MaterialEditor.ShaderProperty(ztest, "Z Test");
                    //m_MaterialEditor.ShaderProperty(zwrite, "Z Write");
                    //m_MaterialEditor.ShaderProperty(srcFactor, "Src Factor");
                    //m_MaterialEditor.ShaderProperty(dstFactor, "Dst Factor");
                    EditorGUI.indentLevel -= 1;
                    GUILayout.Label(Styles.advancedText, EditorStyles.boldLabel);
                    EditorGUI.indentLevel += 1;
                    m_MaterialEditor.EnableInstancingField();
                    m_MaterialEditor.DoubleSidedGIField();
                    EditorGUI.indentLevel -= 1;
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                foreach (UnityEngine.Object obj in blendMode.targets)
                {
                    //MaterialChanged((Material)obj, m_WorkflowMode);
                }
            }
        }
        internal void DetermineWorkflow(MaterialProperty[] props)
        {
            if (FindProperty("_MetallicGlossMap", props, false) != null && FindProperty("_Metallic", props, false) != null)
                m_WorkflowMode = WorkflowMode.Metallic;
            else
                m_WorkflowMode = WorkflowMode.Dielectric;
        }
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));
                return;
            }

            BlendMode blendMode = BlendMode.Opaque;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                blendMode = BlendMode.Cutout;
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                blendMode = BlendMode.Fade;
            }
            material.SetFloat("_Mode", (float)blendMode);

            DetermineWorkflow(MaterialEditor.GetMaterialProperties(new Material[] { material }));
            MaterialChanged(material, m_WorkflowMode);
        }
        private void BlendModePopup()
        {
            EditorGUI.showMixedValue = blendMode.hasMixedValue;
            BlendMode mode = (BlendMode)blendMode.floatValue;

            EditorGUI.BeginChangeCheck();
            mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                blendMode.floatValue = (float)mode;
                foreach (UnityEngine.Object obj in blendMode.targets)
                {
                    MaterialChanged((Material)obj,m_WorkflowMode);
                }
            }

            EditorGUI.showMixedValue = false;
        }
        private void DoNormalArea()
        {
            m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
            if (bumpScale.floatValue != 1 && UnityEditorInternal.InternalEditorUtility.IsMobilePlatform(EditorUserBuildSettings.activeBuildTarget))
                if (m_MaterialEditor.HelpBoxWithButton(
                    EditorGUIUtility.TrTextContent("Bump scale is not supported on mobile platforms"),
                    EditorGUIUtility.TrTextContent("Fix Now")))
                {
                    bumpScale.floatValue = 1;
                }
        }
        private void DoAlbedoArea(Material material)
        {
            m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
            if (((BlendMode)material.GetFloat("_Mode") == BlendMode.Cutout))
            {
                m_MaterialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
            }
        }
        private void DoEmissionArea(Material material)
        {
            bool hadEmissionTexture = emissionMap.textureValue != null;

            // Texture and HDR color controls
            m_MaterialEditor.TexturePropertyWithHDRColor(Styles.emissionText, emissionMap, emissionColorForRendering, false);

            // If texture was assigned and color was black set color to white
            float brightness = emissionColorForRendering.colorValue.maxColorComponent;
            if (emissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
                emissionColorForRendering.colorValue = Color.white;

            // change the GI flag and fix it up with emissive as black if necessary
            m_MaterialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
        }
        private void DoSpecularMetallicArea()
        {
            bool hasGlossMap = false;
            bool hasRoughnessMap = false;
            if (m_WorkflowMode == WorkflowMode.Metallic)
            {
                hasGlossMap = metallicMap.textureValue != null;
                hasRoughnessMap = roughnessMap.textureValue != null;
                m_MaterialEditor.TexturePropertySingleLine(Styles.metallicMapText, metallicMap, hasGlossMap ? null : metallic);
            }

            bool showSmoothnessScale = hasGlossMap || hasRoughnessMap;
            if (smoothnessMapChannel != null)
            {
                int smoothnessChannel = (int)smoothnessMapChannel.floatValue;
                if (smoothnessChannel == (int)SmoothnessMapChannel.AlbedoAlpha)
                {
                    showSmoothnessScale = true;
                }
            }

            m_MaterialEditor.TexturePropertySingleLine(Styles.smoothMapText, roughnessMap, (showSmoothnessScale ? smoothnessScale : smoothness));
            //int indentation = 2; // align with labels of texture properties
            //m_MaterialEditor.ShaderProperty(showSmoothnessScale ? smoothnessScale : smoothness, showSmoothnessScale ? Styles.smoothnessScaleText : Styles.smoothnessText, indentation);

            if (smoothnessMapChannel != null)
            {
                m_MaterialEditor.ShaderProperty(smoothnessMapChannel, Styles.smoothnessMapChannelText, 2);
            }
        }
        public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", "");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    break;
                case BlendMode.Fade:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
                case BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
            }
        }
        private static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
        {
            int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
            if (ch == (int)SmoothnessMapChannel.AlbedoAlpha)
            {
                return SmoothnessMapChannel.AlbedoAlpha;
            }
            else if (ch == (int)SmoothnessMapChannel.RoughnessMap)
            {
                return SmoothnessMapChannel.RoughnessMap;
            }
            else
            {
                return SmoothnessMapChannel.SpecularMetallicAlpha;
            }
        }
        private static void SetMaterialKeywords(Material material, WorkflowMode workflowMode)
        {
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));
            if (workflowMode == WorkflowMode.Metallic)
                SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
            SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
            SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));

            // A material's GI flag internally keeps track of whether emission is enabled at all, it's enabled but has no effect
            // or is enabled and may be modified at runtime. This state depends on the values of the current flag and emissive color.
            // The fixup routine makes sure that the material is in the correct state if/when changes are made to the mode or color.
            MaterialEditor.FixupEmissiveFlag(material);
            bool shouldEmissionBeEnabled = material.globalIlluminationFlags != MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

            if (material.HasProperty("_SmoothnessTextureChannel"))
            {
                SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha);
                SetKeyword(material, "_SMOOTHNESS_TEXTURE_ROUGHNESS_CHANNEL_G", GetSmoothnessMapChannel(material) == SmoothnessMapChannel.RoughnessMap);
            }
        }
        private static void MaterialChanged(Material material, WorkflowMode workflowMode)
        {
            SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));

            SetMaterialKeywords(material, workflowMode);
        }
        private static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);
        }
    }
}