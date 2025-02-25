using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AvatarPartsChange : MonoBehaviour
{
    [Header("캐릭터베이스")]
    public GameObject obj_root;
    public GameObject OriginHair;
    public GameObject OriginFace;
    public GameObject OriginTop;
    public GameObject OriginBottom;
    public GameObject OriginShoes;
    public GameObject OriginSet;

    public GameObject RootBoneTR;

    public void ChangeAvatarParts(SalinConstants.PartInfo info, SkinnedMeshRenderer newMesh)
    {
        SkinnedMeshRenderer originMeshrenderer = null;
        //SkinnedMeshRenderer newMeshRenderer = null;

        switch (info.type)
        {
            case SalinConstants.PartsType.Hair:
                originMeshrenderer = OriginHair.GetComponentInChildren<SkinnedMeshRenderer>();
                break;
            case SalinConstants.PartsType.Face:
                originMeshrenderer = OriginFace.GetComponentInChildren<SkinnedMeshRenderer>();
                break;
            case SalinConstants.PartsType.Top:
                originMeshrenderer = OriginTop.GetComponentInChildren<SkinnedMeshRenderer>();
                if (OriginSet)
                    OriginSet.SetActive(false);
                OriginTop.SetActive(true);
                OriginBottom.SetActive(true);
                break;
            case SalinConstants.PartsType.Bottom:
                originMeshrenderer = OriginBottom.GetComponentInChildren<SkinnedMeshRenderer>();
                if (OriginSet)
                    OriginSet.SetActive(false);
                OriginTop.SetActive(true);
                OriginBottom.SetActive(true);
                break;
            case SalinConstants.PartsType.Shoes:
                originMeshrenderer = OriginShoes.GetComponentInChildren<SkinnedMeshRenderer>();
                break;
            case SalinConstants.PartsType.Set:
                if(OriginSet)
                    originMeshrenderer = OriginSet.GetComponentInChildren<SkinnedMeshRenderer>();
                else
                {
                    OriginSet = new GameObject("SetMesh");
                    OriginSet.transform.parent = transform;
                    OriginSet.layer = LayerMask.NameToLayer("Player");
                    originMeshrenderer = OriginSet.AddComponent<SkinnedMeshRenderer>();
                }
                OriginSet.SetActive(true);
                OriginTop.SetActive(false);
                OriginBottom.SetActive(false);
                break;

            case SalinConstants.PartsType.Hat:
            case SalinConstants.PartsType.Glasses:
            case SalinConstants.PartsType.EarRing:
            case SalinConstants.PartsType.Piercing:
            case SalinConstants.PartsType.Beard:
            case SalinConstants.PartsType.Necklace:
            case SalinConstants.PartsType.Bag:
            case SalinConstants.PartsType.Bracelet:
            case SalinConstants.PartsType.Glove:
            case SalinConstants.PartsType.Anklet:
                return;
            //break;
            default:
                break;
        }

        originMeshrenderer.sharedMesh = newMesh.sharedMesh;
        originMeshrenderer.sharedMaterials = newMesh.sharedMaterials;

        Transform[] childrens = RootBoneTR.GetComponentsInChildren<Transform>(true);

        // sort bones.
        Transform[] bones = new Transform[newMesh.bones.Length];
        for (int boneOrder = 0; boneOrder < newMesh.bones.Length; boneOrder++)
        {
            bones[boneOrder] = Array.Find(childrens, c => c.name == newMesh.bones[boneOrder].name);
        }
        originMeshrenderer.bones = bones;

        //AddressableManager.Instance.ObjectRelease(newMesh);
        AddressableManager.Instance.GameObjectReleaseInstance(newMesh.gameObject);
        //AddressableManager.Instance.GameObjectRelease(newMesh.gameObject);
    }

    public void ColorChange(SalinConstants.ColorPaletNeedType type, string colorHexCode = null, Material changeMat = null, Action<SalinConstants.ColorPaletNeedType, Material> callback = null)
    {
        Material mat = null;
        switch(type)
        {
            case SalinConstants.ColorPaletNeedType.Skin:
                Color changeColor;
                ColorUtility.TryParseHtmlString("#" + colorHexCode, out changeColor);
                //OriginFace.GetComponent<SkinnedMeshRenderer>().materials[(int)SalinConstants.SubCategoryContentType.Face].color = changeColor;
                mat = Instantiate(Array.Find(OriginFace.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"))); //new Material(OriginFace.GetComponent<SkinnedMeshRenderer>().materials[(int)SalinConstants.SubCategoryContentType.Face]);
                mat.color = changeColor;
                AllBaseBodyMatChange(changeColor);//, mat);
                break;
            case SalinConstants.ColorPaletNeedType.Hair:
                break;
            case SalinConstants.ColorPaletNeedType.Eyebrow:
                break;
            case SalinConstants.ColorPaletNeedType.Lips:
                break;
            case SalinConstants.ColorPaletNeedType.Eye:
                Debug.Log("hithit");
                mat = changeMat;
                OriginFace.GetComponent<SkinnedMeshRenderer>().materials[(int)SalinConstants.FacePartsMaterialType.Pupil].mainTexture = mat.mainTexture;// = mat;                
                break;
        }
        callback.Invoke(type, mat);
    }

    private void AllBaseBodyMatChange(Color color)//, Material changeMat)
    {
        Material[] mat;

        if (OriginHair)
        {
            mat = Array.FindAll(OriginHair.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"));
            if (mat.Length > 0)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    //mat[i] = changeMat;
                    mat[i].color = color;
                }
            }
        }

        if (OriginFace)
        {
            mat = Array.FindAll(OriginFace.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"));
            if (mat.Length > 0)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    //mat[i] = changeMat;
                    mat[i].color = color;
                }
            }
        }

        if (OriginTop)
        {
            mat = Array.FindAll(OriginTop.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"));
            if (mat.Length > 0)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    //mat[i] = changeMat;
                    mat[i].color = color;
                }
            }
        }

        if (OriginBottom)
        {
            mat = Array.FindAll(OriginBottom.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"));
            if (mat.Length > 0)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    //mat[i] = changeMat;
                    mat[i].color = color;
                }
            }
        }

        if (OriginShoes)
        {
            mat = Array.FindAll(OriginShoes.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"));
            if (mat.Length > 0)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    //mat[i] = changeMat;
                    mat[i].color = color;
                }
            }
        }

        if (OriginSet)
        {
            mat = Array.FindAll(OriginSet.GetComponent<SkinnedMeshRenderer>().materials, x => x.name.Contains("basebody"));
            if (mat.Length > 0)
            {
                for (int i = 0; i < mat.Length; i++)
                {
                    //mat[i] = changeMat;
                    mat[i].color = color;
                }
            }
        }
    }

    public void BlendShapesSetting(string[] blendIndex, float[] optionValue)
    {
        int[] blendShapeIndexArray = new int[blendIndex.Length];
        for (int i = 0; i < blendIndex.Length; i++)
            blendShapeIndexArray[i] = int.Parse(blendIndex[i]) - 1;
        float[] valueArray = new float[blendIndex.Length];
        for (int i = 0; i < valueArray.Length; i++)
        {
            valueArray[i] = optionValue[i];
        }

        for (int i = 0; i < blendShapeIndexArray.Length; i++)
        {
            int tmpInt = i;
            OriginFace.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(blendShapeIndexArray[tmpInt], valueArray[tmpInt]);
        }
    }

    public void BlendShapesSetting(SalinConstants.PlayerAvatarPartsData data)
    {
        int blendShapeIndexArrayLength = 0;
        for(int i=0;i<(int)SalinConstants.CustomMorphType.None;i++)
        {
            int tmpInt = i;
            switch(tmpInt)
            {
                case (int)SalinConstants.CustomMorphType.Eyebrow:
                    blendShapeIndexArrayLength += data.Eyebraw.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Eye:
                    blendShapeIndexArrayLength += data.Eye.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Nose:
                    blendShapeIndexArrayLength += data.Nose.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Lips:
                    blendShapeIndexArrayLength += data.Lips.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Jaw:
                    blendShapeIndexArrayLength += data.Jaw.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Ear:
                    blendShapeIndexArrayLength += data.Ear.MorphPoint.Length;
                    break;
            }
        }

        int[] blendShapeIndexArray = new int[blendShapeIndexArrayLength];
        float[] valueArray = new float[blendShapeIndexArrayLength];
        int offset = 0;
        for (int i = 0; i < (int)SalinConstants.CustomMorphType.None; i++)
        {
            int tmpInt = i;
            switch (tmpInt)
            {
                case (int)SalinConstants.CustomMorphType.Eyebrow:
                    for (int j = 0; j < data.Eyebraw.MorphPoint.Length; j++)
                    {
                        blendShapeIndexArray[j + offset] = data.Eyebraw.MorphPoint[j];
                        valueArray[j + offset] = (float)data.Eyebraw.MorphSetting[j];
                    }
                    offset += data.Eyebraw.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Eye:
                    for (int j = 0; j < data.Eye.MorphPoint.Length; j++)
                    {
                        blendShapeIndexArray[j + offset] = data.Eye.MorphPoint[j];
                        valueArray[j + offset] = (float)data.Eye.MorphSetting[j];
                    }
                    offset += data.Eye.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Nose:
                    for (int j = 0; j < data.Nose.MorphPoint.Length; j++)
                    {
                        blendShapeIndexArray[j + offset] = data.Nose.MorphPoint[j];
                        valueArray[j + offset] = (float)data.Nose.MorphSetting[j];
                    }
                    offset += data.Nose.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Lips:
                    for (int j = 0; j < data.Lips.MorphPoint.Length; j++)
                    {
                        blendShapeIndexArray[j + offset] = data.Lips.MorphPoint[j];
                        valueArray[j + offset] = (float)data.Lips.MorphSetting[j];
                    }
                    offset += data.Lips.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Jaw:
                    for (int j = 0; j < data.Jaw.MorphPoint.Length; j++)
                    {
                        blendShapeIndexArray[j + offset] = data.Jaw.MorphPoint[j];
                        valueArray[j + offset] = (float)data.Jaw.MorphSetting[j];
                    }
                    offset += data.Jaw.MorphPoint.Length;
                    break;
                case (int)SalinConstants.CustomMorphType.Ear:
                    for (int j = 0; j < data.Ear.MorphPoint.Length; j++)
                    {
                        blendShapeIndexArray[j + offset] = data.Ear.MorphPoint[j];
                        valueArray[j + offset] = (float)data.Ear.MorphSetting[j];
                    }
                    offset += data.Ear.MorphPoint.Length;
                    break;
            }
        }

        for (int i = 0; i < blendShapeIndexArray.Length; i++)
        {
            int tmpInt = i;
            OriginFace.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(blendShapeIndexArray[tmpInt], valueArray[tmpInt]);
        }
    }

}
