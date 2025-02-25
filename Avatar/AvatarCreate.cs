using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class AvatarCreate : MonoBehaviour
{
    [SerializeField] GameObject loadingImage;
    [SerializeField] Transform AvatarCreateTR;

    GameObject UserAvatar;
    AvatarPartsChange avatarParts;
    //NetworkConstants.ResponseDatas.GetAvatarDisplay UserAvatarData;

    SalinConstants.PlayerAvatarPartsData AvatarData;

    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void AvatarPartsChange(GameObject avatar, Action<GameObject> success = null)
    {
        UserAvatar = avatar;
        avatarParts = avatarParts ?? UserAvatar.GetComponent<AvatarPartsChange>();
        avatarParts.obj_root.SetActive(false);
        StartCoroutine(ChangeUserAvatarParts(()=>
        {
            avatarParts.BlendShapesSetting(AvatarData);
            if (loadingImage != null)
            {
                loadingImage.SetActive(false);
            }
            avatarParts.obj_root.SetActive(true);
            success?.Invoke(UserAvatar);
        }));
    }

    public void CreateBaseAvatar(SalinConstants.PlayerAvatarPartsData data, Action<GameObject, Action<GameObject>>action, Action<GameObject> success = null)
    {
        if (loadingImage != null)
        {
            loadingImage.SetActive(true);
        }

        AvatarData = data;

        string objectName = string.Empty;
        if (AvatarData.Gender.Equals((int)SalinConstants.AvatarGenderType.Female))
            objectName = "S_W_BaseBody";
        else
            objectName = "S_M_BaseBody";


        StartCoroutine(AddressableManager.Instance.LoadGameObject(SalinConstants.AssetsLabel.BaseAvatar, objectName, null,
            (obj) =>
            {
                GameObject tmpAvatar = obj;//Instantiate(obj, AvatarCreateTR);
                tmpAvatar.transform.SetParent(AvatarCreateTR);
                tmpAvatar.transform.localPosition = Vector3.zero;
                tmpAvatar.transform.localRotation = Quaternion.identity;
                action?.Invoke(tmpAvatar, success);
            },
            (fail) =>
            {
#if !LIVE
                Debug.Log(fail);
#endif
            }));
    }

    IEnumerator ChangeUserAvatarParts(Action suceess = null)
    {
        bool check = true;
        SalinConstants.PartInfo tmpParts = new SalinConstants.PartInfo();
        for (int i = 0; i < (int)SalinConstants.PartsType.None; ++i)
        {
            check = false;
            int tmpInt = i;
            switch(tmpInt)
            {
                case (int)SalinConstants.PartsType.Hair:
                    tmpParts = AvatarData.Hair;
                    break;
                case (int)SalinConstants.PartsType.Face:
                    tmpParts = AvatarData.Face;
                    break;
                case (int)SalinConstants.PartsType.Top:
                    tmpParts = AvatarData.Top;
                    break;
                case (int)SalinConstants.PartsType.Bottom:
                    tmpParts = AvatarData.Bottom;
                    break;
                case (int)SalinConstants.PartsType.Shoes:
                    tmpParts = AvatarData.Shoes;
                    break;
                case (int)SalinConstants.PartsType.Set:
                    tmpParts = AvatarData.Set;
                    break;
                case (int)SalinConstants.PartsType.Hat:
                    tmpParts = AvatarData.Hat;
                    break;
                case (int)SalinConstants.PartsType.Glasses:
                    tmpParts = AvatarData.Glasses;
                    break;
                case (int)SalinConstants.PartsType.EarRing:
                    tmpParts = AvatarData.EarRing;
                    break;
                case (int)SalinConstants.PartsType.Piercing:
                    tmpParts = AvatarData.Piercing;
                    break;
                case (int)SalinConstants.PartsType.Beard:
                    tmpParts = AvatarData.Beard;
                    break;
                case (int)SalinConstants.PartsType.Necklace:
                    tmpParts = AvatarData.Necklace;
                    break;
                case (int)SalinConstants.PartsType.Bracelet:
                    tmpParts = AvatarData.Bracelet;
                    break;
                case (int)SalinConstants.PartsType.Glove:
                    tmpParts = AvatarData.Glove;
                    break;
                case (int)SalinConstants.PartsType.Bag:
                    tmpParts = AvatarData.Bag;
                    break;
                case (int)SalinConstants.PartsType.Anklet:
                    tmpParts = AvatarData.Anklet;
                    break;
            }
            LoadBaseMeshRenderer(tmpParts, (parts, mesh) =>
            {
                avatarParts.ChangeAvatarParts(parts, mesh);
                check = true;
                //suceess?.Invoke();
            },
            (fail) =>
            {
                check = true;
                Debug.Log(fail);
            });

            yield return new WaitUntil(() => check);
        }
        suceess?.Invoke();
    }

    void LoadBaseMeshRenderer(SalinConstants.PartInfo info, Action<SalinConstants.PartInfo, SkinnedMeshRenderer> success, Action<string> fail = null)
    {
        StartCoroutine(AddressableManager.Instance.LoadGameObject(SalinConstants.AssetsLabel.BaseMesh, info.BaseMeshId, null,
            (obj) =>
            {
                List<string> getMatList = new List<string>(info.materials);
                StartCoroutine(AddressableManager.Instance.LoadAssetList(SalinConstants.AssetsLabel.AvatarMaterials, getMatList, null,
                    (list) =>
                    {
                        //AvatarPartsInfoChange(info);
                        success?.Invoke(info, GetMesh(obj, list));
                    },
                    (text) =>
                    {
                        fail?.Invoke(text);
                    }));
            },
            (text) =>
            {
                fail?.Invoke(text);
            },false));
    }

    SkinnedMeshRenderer GetMesh(GameObject obj, List<object> matObjList)
    {
        Material[] materials = new Material[matObjList.Count];
        for (int i = 0; i < materials.Length; ++i)
        {
            int tmpInt = i;
            materials[tmpInt] = new Material(Instantiate((Material)matObjList[tmpInt]));// (Material)matObjList[i];
            AddressableManager.Instance.ObjectRelease(matObjList[tmpInt]);
            if (materials[tmpInt].name.Contains("basebody"))
            {
                materials[tmpInt].color = new Color
                {
                    r = AvatarData.SkinTone.red / 255f,
                    g = AvatarData.SkinTone.green / 255f,
                    b = AvatarData.SkinTone.blue / 255f,
                    a = 1,
                };
            }
        }
        obj.GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;
        //SkinnedMeshRenderer tmp = obj.GetComponentInChildren<SkinnedMeshRenderer>();
        //tmp.materials = materials;

        return obj.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    #region  일회성 파츠 체인지

    DataManager dataManager;
    Action SubAction;
    /// <summary>
    /// 아리랑 한복 파츠
    /// </summary>
    /// <param name="_num"></param>
    /// <param name="isReset"></param>
    public void SyncPartsChange(int _num, bool isReset = false) 
    {
        object[] tmp = new object[4];
        if (isReset)
        {
            dataManager ??= DataManager.Instance;
            tmp[0] = dataManager.GetSexInt();
            tmp[1] = dataManager.CreatePartInfoJson(dataManager.PlayerData.partsData);
            tmp[2] = dataManager.CreateMakeupInfoJson(dataManager.PlayerData.partsData);
            tmp[3] = dataManager.CreateMorphDataInfoJson(dataManager.PlayerData.partsData);
        }
        pv.RPC("RequestArirangPartsChange", RpcTarget.All, _num, isReset, tmp);
    }

    [PunRPC]
    void RequestArirangPartsChange(int _num, bool isReset, object[] data) 
    {
        if (isReset)
        {
            SalinConstants.PlayerAvatarPartsData partsData = DataManager.Instance.ParsePlayerAvatarPartsData((int)data[0], (string)data[1], (string)data[2], (string)data[3]);
            ResetArirangParts(partsData);
            return;
        }

        string[] _partsStr = new string[2];
        switch (_num)
        {
            case 0:
            case 1:
                _partsStr[0] = $"man_onepiece_0{_num + 3}";
                _partsStr[1] = "man_basebody";
                break;
            case 5:
            case 6:
                _partsStr[0] = $"woman_onepiece_0{_num}";
                _partsStr[1] = "woman_basebody";
                break;
        }

        SalinConstants.PartInfo partInfo = new SalinConstants.PartInfo
        {
            type = _num is 6 ? SalinConstants.PartsType.Bottom : SalinConstants.PartsType.Set,
            BaseMeshId = _partsStr[0],
            materials = new string[2] { _partsStr[0], _partsStr[1] },
        };

        if (_num is 6)
            SubAction = SubArirangPartsChange;

        LoadBaseMeshRenderer(partInfo, (info, mesh) =>
        {
            avatarParts.ChangeAvatarParts(info, mesh);
            SubAction?.Invoke();
            SubAction = null;
        });
    }
    void SubArirangPartsChange()
    {
        SalinConstants.PartInfo partInfo = new SalinConstants.PartInfo
        {
            type = SalinConstants.PartsType.Top,
            BaseMeshId = "woman_onepiece_06_alpha",
            materials = new string[1] { "woman_onepiece_04_alpha" }
        };
        LoadBaseMeshRenderer(partInfo, (info, mesh) => avatarParts.ChangeAvatarParts(info, mesh));
    }
    void ResetArirangParts(SalinConstants.PlayerAvatarPartsData data) 
    {
        SalinConstants.PlayerAvatarPartsData parts = data;
        SalinConstants.PartInfo partInfo = parts.Set;

        if (string.IsNullOrEmpty(partInfo.BaseMeshId)) 
        {
            SubAction = () =>
            {
                partInfo = parts.Top;
                LoadBaseMeshRenderer(partInfo, (info, mesh) => avatarParts.ChangeAvatarParts(info, mesh));
            };
            partInfo = parts.Bottom;
        }

        LoadBaseMeshRenderer(partInfo, (info, mesh) =>
        {
            avatarParts.ChangeAvatarParts(info, mesh);
            SubAction?.Invoke();
            SubAction = null;
        });
    }

    /// <summary>
    /// 우체국 파츠
    /// </summary>
    public void SyncPostPartsChange() 
    {
        pv.RPC("RequestPostPartsChange", RpcTarget.All);
    }
    [PunRPC]
    void RequestPostPartsChange() 
    {
        SalinConstants.PartInfo partInfo;

        if (AvatarData.Gender.Equals((int)SalinConstants.AvatarGenderType.Male))
        {
            partInfo = new SalinConstants.PartInfo
            {
                type = SalinConstants.PartsType.Top,
                BaseMeshId = "man_cloth_upper_02",
                materials = new string[3] {"man_cloth_upper_09", "man_basebody", "man_basebody"}
            };
        }
        else 
        {
            partInfo = new SalinConstants.PartInfo
            {
                type = SalinConstants.PartsType.Top,
                BaseMeshId = "woman_cloth_upper_01",
                materials = new string[2] { "woman_basebody", "woman_cloth_upper_09"}
            };
        }

        LoadBaseMeshRenderer(partInfo, (info, mesh) => avatarParts.ChangeAvatarParts(info, mesh));
    }


    public void SyncTeajoPartsChange() 
    {
        pv.RPC("RequestTeajoPartsChange", RpcTarget.All);
    }
    [PunRPC]
    void RequestTeajoPartsChange()
    {
        string[] _partsStr = new string[2];
        if (AvatarData.Gender.Equals((int)SalinConstants.AvatarGenderType.Male))
        {
            _partsStr[0] = $"man_onepiece_999";
            _partsStr[1] = "man_basebody";

        }
        else
        {
            _partsStr[0] = $"woman_onepiece_999";
            _partsStr[1] = "woman_basebody";
        }


        SalinConstants.PartInfo partInfo = new SalinConstants.PartInfo
        {
            type = SalinConstants.PartsType.Set,
            BaseMeshId = _partsStr[0],
            materials = new string[2] { _partsStr[0], _partsStr[1] },
        };

        LoadBaseMeshRenderer(partInfo, (info, mesh) => avatarParts.ChangeAvatarParts(info, mesh));
    }

    #endregion
    private void OnDestroy()
    {
        if(UserAvatar != null)
            AddressableManager.Instance.GameObjectReleaseInstance(UserAvatar);
    }
}