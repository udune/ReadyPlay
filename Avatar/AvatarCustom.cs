using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Salin.AvatarModule;
using UnityEngine.Android;

//public class SalinAvatarData
//{
//    public string code;
//    public byte[] photoData;

//    public int avatarIdx;
//    public int topIdx;
//    public int botIdx;
//    public int shoesIdx;

//}

public class AvatarCustom : SceneState 
{
   
    [SerializeField] public Camera AvatarCamera,MainCamera;

   
    protected override void Awake()
    {
        base.Awake();
        SetAvatarCamera();
#if (Mobile)
        //if (!Screen.orientation.Equals(ScreenOrientation.Portrait))
            Screen.orientation = ScreenOrientation.Portrait;
#endif
    }


    private void Update()
    {

        // 모바일 카메라 , 갤러리 권한
#if !UNITY_EDITOR && Mobile  

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }

#endif

        // 아바타를 만들면 itseez 클라우드에 계속 쌓이기 때문에 필요없는 아바타는 삭제
        // 개발 할 때 커스텀 테스트 후 삭제 코드

    }

    protected override void StartScene()
    {
        Debug.Log("[Avatar Custom] InitSecne");

      
        //ItSeezDemoManager.Instance.GenerateAvatar();

        // 로컬 아바타 커스텀 데이터가 있는지 체크
        //if (DataManager.Instance.IsExistAvatar())
        //{

        //    ItSeezDemoManager.Instance.CustomStatus = Constants.CustomStatus.Modity;
        //    ItSeezDemoManager.Instance.UIAvatarSelect.OpenDisplay((int)Constants.Display.None);
        //    ItSeezDemoManager.Instance.CreateAvatarUsingPlayer(DataManager.Instance.PlayerData);

        //}
        //else
        //{
          
        //    ItSeezDemoManager.Instance.CustomStatus = Constants.CustomStatus.New;
        //    ItSeezDemoManager.Instance.UIAvatarSelect.OpenDisplay((int)Constants.Display.Sdk);
        //    ItSeezDemoManager.Instance.SetFailedData();
        //}
        
      
    }


 
    #region ETC

    /// <summary>
    /// 아바타 커스텀 씬에 맞는 카메라 세팅
    /// </summary>
    void SetAvatarCamera()
    {
        if (true)//(!UIManager.Instance.WorldSpace)
        {
            AvatarCamera = GameObject.Find("AvatarCamera").GetComponent<Camera>();
            MainCamera = Camera.main;

            // 아바타 프로필 캡처를 위한 세팅
            RenderTexture rt = CreateRenderTexture();
            //ItSeezDemoManager.Instance.SetAvatarTexture(rt);
            AvatarCamera.targetTexture = rt;
          
            if (MainCamera == null)
            {

                AvatarCamera.enabled = true;

            }
            else
            {
                MainCamera.tag = SalinConstants.Tag.Untagged.ToString();  //UnityEditorInternal.InternalEditorUtility.tags[0];
                MainCamera.enabled = false;

                AvatarCamera.tag = SalinConstants.Tag.MainCamera.ToString();
                AvatarCamera.enabled = true;

            }
        }
     }

     RenderTexture CreateRenderTexture()
    {
        RenderTexture RT = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        RT.antiAliasing = 8;

        return RT;
    }

    #endregion
}
