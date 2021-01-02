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
using UnityEditor;
using UnityEngine;

namespace Kamishiro.UnityShader.CustomBuiltin
{
    internal class AKSUnlitShaderGUI : ShaderGUI
    {
        public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
        public enum BlendMode
        {
            Opaque,
            Cutout,
            Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
        }
        private MaterialProperty blendMode = null;
        private MaterialProperty albedoMap = null;
        private MaterialProperty albedoColor = null;
        private MaterialProperty alphaCutoff = null;
        private MaterialProperty cullMode = null;
        private MaterialEditor m_MaterialEditor;
        private bool m_FirstTimeApply = true;

        private void FindProperties(MaterialProperty[] props)
        {
            blendMode = FindProperty("_ModeU", props);
            albedoMap = FindProperty("_MainTex", props);
            albedoColor = FindProperty("_Color", props);
            alphaCutoff = FindProperty("_Cutoff", props);
            cullMode = FindProperty("_Cull", props);
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
                MaterialChanged(material);
                m_FirstTimeApply = false;
            }
            ShaderPropertiesGUI(material);
        }
        private void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            {
                BlendModePopup();
                AKSUIHelper.ShurikenHeader(AKSStyles.primaryMapsText);
                GUILayout.Label(AKSStyles.mainTitle, EditorStyles.boldLabel);
                m_MaterialEditor.TexturePropertySingleLine(AKSStyles.albedoText, albedoMap, albedoColor);
                if (((BlendMode)material.GetFloat("_ModeU") == BlendMode.Cutout))
                {
                    m_MaterialEditor.ShaderProperty(alphaCutoff, AKSStyles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
                }
                EditorGUI.indentLevel += 2;
                m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);
                EditorGUI.indentLevel -= 2;
                AKSUIHelper.ShurikenHeader("Options");
                GUILayout.Label(AKSStyles.renderingOpTitle, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                m_MaterialEditor.ShaderProperty(cullMode, AKSStyles.cullModeText);
                m_MaterialEditor.RenderQueueField();
                EditorGUI.indentLevel--;
            }
            if (EditorGUI.EndChangeCheck())
            {
                foreach (UnityEngine.Object obj in blendMode.targets)
                {
                    MaterialChanged((Material)obj);
                }
            }
            AKSUIHelper.ShurikenHeader(AKSStyles.nameAKUnlit);
            EditorGUILayout.LabelField(AKSStyles.author);
            AKSManager.DisplayVersion();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(AKSStyles.btnReadme)) { AKSUIHelper.OpenLink(AKSStyles.linkReadme); }
            if (GUILayout.Button(AKSStyles.btnDescription)) { AKSUIHelper.OpenLink(AKSStyles.linkDescriptionAKUnlit); }
            EditorGUILayout.EndHorizontal();
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
                SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_ModeU"));
                return;
            }

            BlendMode blendMode = BlendMode.Opaque;
            if (oldShader.name.Contains("/Cutout/"))
            {
                blendMode = BlendMode.Cutout;
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                blendMode = BlendMode.Transparent;
            }
            material.SetFloat("_ModeU", (float)blendMode);

            MaterialChanged(material);
        }
        private void BlendModePopup()
        {
            EditorGUI.showMixedValue = blendMode.hasMixedValue;
            BlendMode mode = (BlendMode)blendMode.floatValue;

            EditorGUI.BeginChangeCheck();
            mode = (BlendMode)EditorGUILayout.Popup("Rendering Mode", (int)mode, blendNames);
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                blendMode.floatValue = (float)mode;
                foreach (UnityEngine.Object obj in blendMode.targets)
                {
                    MaterialChanged((Material)obj);
                }
            }

            EditorGUI.showMixedValue = false;
        }
        private static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", "");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    break;
                case BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
            }
        }
        private static void MaterialChanged(Material material)
        {
            SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_ModeU"));
        }
    }
}
