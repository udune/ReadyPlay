using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarWindow : MonoBehaviour
{

    //public enum ErrorState
    //{
    //    Success,
    //    Server_Error,
    //    Data_Null,
    //    Head_Null,
    //    Photo_Null,
    //    Hair_Null,
    //    Body_Null,
    //    Top_Null,
    //    Bottom_Null,
    //    Shoes_Null,

    //}

    //[HideInInspector]
    //public DataManager.Player myPlayer, customPlayer;

    //[SerializeField] AvatarCustom main;

    //[Header("아바타 바디")]
    //[SerializeField] Button[] Item_bodyType;

    //[Header("아이템 카테고리")]
    //[SerializeField] Button[] Item_Tabs;
    //[Header("아이템 리스트 뷰")]
    //[SerializeField] BaseContent[] contentViews;
    //int content_index;

    //[Header("기타 UI")]
    //[SerializeField] GameObject uploadButton;
    //[SerializeField] Button saveButton;


    //[SerializeField] Text txt_nickName;
    //[SerializeField] Text txt_progress;

    //[SerializeField] GameObject UIBlock;



    //[HideInInspector]
    //public int currentHairCutIdx = 0;

    //[HideInInspector]
    //public int currentTopIdx = 0;
    //[HideInInspector]
    //public int currentBottomIdx = 0;

  
    //object[] Data;
    //bool isFirstCustom = true;



    //#region Common

    //private void Awake()
    //{
    //    Open();

    //}

    //private void Open()
    //{

    //    myPlayer = DataManager.Instance.PlayerData;
    //    customPlayer = new DataManager.Player();

    //    SetNickName(myPlayer.nickname);

    //    OnClickItemTab(0);

    //}

    //private void Close()
    //{
    //    // Next SCENE
    //    SceneLoadManager.Instance.NextScene();
    //}

    //public void OnClose()
    //{
    //    Close();
    //}

    //private void SetNickName(string nickName)
    //{
    //    txt_nickName.text = nickName;
    //}

    //#endregion

    //#region Create & Change

    //public void OnClickCreateHead(int btn_idx)
    //{
    //    // 초기화
    //    main.InitSDK();


    //    // 콜백 세팅
    //    main.GetCurrentAvatar().SetAwaitCallback(OnAwaitedCallback);
    //    main.GetCurrentAvatar().SetSuccessLoadPhotoCallback(OnSuccessLoadCallback);
    //    main.GetCurrentAvatar().SetConvertedCallback(OnConvertedCallback);
    //    main.GetCurrentAvatar().SetCompletedCallback(OnCompletedCallback);


    //    main.GetCurrentAvatar().SetAttachBodyCallback(() =>
    //    {
    //        main.DeleteBody();

    //        //main.GetCurrentAvatar().CreateBody(SalinConstants.Path.SalinAvatarPath, customPlayer.body, main.spawnTarget);
    //        //main.GetCurrentAvatar().GetAvatarBody().GetComponent<Avatar_Info>().DestroyVRIK();

    //        main.InitAvatarTransform();

    //    });

    //    // 생성 요청
    //    switch (btn_idx)
    //    {
    //        // Take Photo
    //        case 0:
    //            main.GetCurrentAvatar().CreateHeadUsingTakePhoto(currentHairCutIdx);
    //            break;

    //        // Upload Photo
    //        case 1:
    //            main.GetCurrentAvatar().CreateHeadUsingFileObj(uploadButton, currentHairCutIdx);
    //            break;

    //    }

   
    //}

    //public void OnClickChangeBody(int avatarIdx)
    //{

    //    if (customPlayer.body == avatarIdx)
    //        return;
    //    else
    //        customPlayer.body = avatarIdx;

    //    OnSeletedBodyType(avatarIdx);
    //    ChangeBody(avatarIdx);

    //}

    //private void ChangeBody(int avatarIdx)
    //{

    //    if (customPlayer.head == string.Empty)
    //    {
    //        //  OnAwaitedCallback(ErrorState.Photo_Null.ToString());
    //        ErrorExcute(ErrorState.Head_Null);
    //        return;
    //    }


    //    // 바디타입이 바뀌면 Hair, Top, Bottom 아이템 초기화
    //    currentHairCutIdx = customPlayer.hair = 0;
    //    currentTopIdx = customPlayer.top = 0;
    //    currentBottomIdx = customPlayer.bottom = 0;
    //    customPlayer.shoes = 0;

    //    // 생성요청
    //    main.InitSDK();
    //    main.SetCustomAvatar(customPlayer, OnAwaitedCallback, OnSuccessLoadCallback, OnCompletedCallback, OnConvertedCallback);
    //}

    //#endregion

    //#region Change HairCut & Parts

    //public void ChangeHair(int hair_idx)
    //{
    //    if (currentHairCutIdx == hair_idx)
    //        return;

    //    if (main.GetCurrentAvatar() == null || main.GetCurrentAvatar().GetAvatarHead() == null)
    //    {
    //        return;
    //    }

    //    SetUiBlock(true);

    //    currentHairCutIdx =  customPlayer.hair = hair_idx;

    //    Data = new object[1];
    //    Data[0] = hair_idx;

    //    main.GetCurrentAvatar().ChangeHair(Data, () =>
    //    {

    //        SetUiBlock(false);

    //    });

    //}

    //public void ChangeParts(int index, int type, Action changedCallback = null)
    //{
    //    //if (!main.IsExistAvatar())
    //    //{
    //    //    return;
    //    //}

    //    //switch (type)
    //    //{
    //    //    case (int)AvatarParts.PartsType.Top:
    //    //        currentTopIdx = customPlayer.top = index;
    //    //        break;

    //    //    case (int)AvatarParts.PartsType.Bottom:
    //    //       currentBottomIdx = customPlayer.bottom = index;
    //    //        break;
    //    //}

    //    //Data = new object[2];

    //    //Data[0] = type; // parts type
    //    //Data[1] = SalinConstants.Path.AvatarPartsPath + "/" + ((AvatarParts.PartsType)Data[0]).ToString()
    //    //    + "/" + main.GetCurrentAvatar().modelType.ToString() + "/" + index.ToString(); //GetCurrentPartsIdx();


    //    //main.GetCurrentAvatar().ChangeParts(Data, changedCallback);

    //}

    //#endregion

    //#region Callback

    //private void OnAwaitedCallback(string msg)
    //{
    //    // 아바타 생성 프로세스 로그
    //    AvatarErrorExcute(msg);

    //}

    //private void OnCompletedCallback()
    //{

    //    string newHead = main.GetCurrentAvatar().GetAvatarCode();
    //    byte[] newPhoto = main.GetCurrentAvatar().GetPhotoData();

    //    // 아바타 코드가 존재하면서
    //    if (customPlayer.head != string.Empty)
    //    {

    //        Debug.Log(string.Format("custom : {0} / origin : {1}", customPlayer.head, myPlayer.head));

    //        // 아바타 코드가 원래 사용하던게 아니라면,
    //        if (customPlayer.head != myPlayer.head && customPlayer.head != newHead)// && myPlayer.head != myPlayer.normalHead) 
    //        {
    //            main.GetCurrentAvatar().Delete(customPlayer.head);
    //        }
    //    }


    //    // 새로 생성된 아바타 코드 할당
    //    if (newHead != string.Empty)
    //        customPlayer.head = newHead;


    //    // 새로 생성된 사진 데이터 할당
    //    //if (newPhoto != null)
    //    //    customPlayer.photo = newPhoto;


    //    // 인터렉션 블락 해제
    //    SetUiBlock(false);
    //    InterAllButton(true);
    //    main.SetLoadingObject(false);
     
    
    //}

    //private void OnConvertedCallback()
    //{

    //    // 헤어리스트 데이터 변환이 완료되면
    //    // 커스템 아이템 리스트 중 헤어 업데이트
    ////    contentViews[0].UpdateItemList();

    //}

    //private void OnSuccessLoadCallback(bool isSuccess)
    //{
    //    // 사진 데이터가 정상적으로 들어왔으면
    //    // 그 때 부터 인터렉션 블락
    //    InterAllButton(!isSuccess);
    //    SetUiBlock(isSuccess);
    //}


    //#endregion

    //#region Server Request


    //Action<SimpleJSON.JSONNode> serverCallback;

    //public void RequestServer(int serverMethod)
    //{

  
    //    Hashtable AvatarCustomTable = new Hashtable();
    //    string URL = AvatarCustom.Custom.URL;

    //    // myPlayer.mid = "AU253841681330089677034755048858636472181";
    //    // AvatarCustomTable.Add(AvatarCustom.Custom.Request.mid, myPlayer.mid);

    //    switch (serverMethod)
    //    {
          
    //        case (int)NetworkConstants.HTTPMethod.GET:
                
    //            if (myPlayer.mid == null)
    //            {
    //                SetFailedData();
    //                return;

    //            }

    //            ServerManager.Instance.Request((NetworkConstants.HTTPMethod)serverMethod, URL + "/" + myPlayer.mid, GetCustomData);
                
    //            return;

    //        case (int)NetworkConstants.HTTPMethod.POST:
    //            AvatarCustomTable.Add(AvatarCustom.Custom.Request.mid, myPlayer.mid);
    //            break;

    //        case (int)NetworkConstants.HTTPMethod.PUT:
    //            URL = URL + "/" + myPlayer.mid;
    //            break;

    //    }

    //    AvatarCustomTable.Add(AvatarCustom.Custom.Request.head, myPlayer.head);
    //  //  AvatarCustomTable.Add(AvatarCustom.Custom.Request.photo, DataManager.Instance.ByteToString(myPlayer.photo));
    //    AvatarCustomTable.Add(AvatarCustom.Custom.Request.hair, myPlayer.hair);
    //    AvatarCustomTable.Add(AvatarCustom.Custom.Request.body, myPlayer.body);
    //    AvatarCustomTable.Add(AvatarCustom.Custom.Request.top, myPlayer.top);
    //    AvatarCustomTable.Add(AvatarCustom.Custom.Request.bottom, myPlayer.bottom);
    //    AvatarCustomTable.Add(AvatarCustom.Custom.Request.shoes, myPlayer.shoes);

    //    serverCallback = PostAndPutCustomData;

    //    ServerManager.Instance.Request((NetworkConstants.HTTPMethod)serverMethod, URL, serverCallback, AvatarCustomTable);

    //    UIManager.Instance.OpenPopup("아바타를 저장 중 입니다.", null);
    //}

    ///// <summary>
    ///// Network Method Get Callback
    ///// </summary>
    ///// <param name="data">respone data</param>
    //void GetCustomData(SimpleJSON.JSONNode data)
    //{
    //    Debug.Log("Start GetCustomData");

    //    if (data == null)
    //    {
    //        ErrorExcute(ErrorState.Server_Error);
    //        Debug.Log("GetCustomData Null");
    //        return;
    //    }
    //    else
    //    {
    //        Debug.Log("GetCustomData Exist");

    //        var CustomData = ServerManager.GetJson<AvatarCustom.Custom.Response>(data);

    //        switch (CustomData.code)
    //        {
    //            // 2. 있다면isExistData 커스텀 데이터 기반으로 아바타 생성
    //            case 2000:
    //                SetSuccessData(CustomData);
    //                break;

    //            // 3. 없다면 기본 데이터로 아바타 생성
    //            case -9999:
    //                SetFailedData();
    //                break;
    //        }

    //        Debug.Log("isFirstCustom : " + isFirstCustom);

    //    }

    //}

    ///// <summary>
    ///// Network Method Post & Put Callback
    ///// </summary>
    ///// <param name="data">respone data</param>
    //void PostAndPutCustomData(SimpleJSON.JSONNode data)
    //{
    //    if (data == null)
    //    {
    //        ErrorExcute(ErrorState.Server_Error);
    //        return;
    //    }
    //    else
    //    {
    //        var CustomData = ServerManager.GetJson<AvatarCustom.Custom.Response>(data);

    //        switch (CustomData.code)
    //        {
    //            // Success
    //            case 2000:
    //                ErrorExcute(ErrorState.Success);
    //                Close(); // Next Scene
    //                break;

    //            case 3101:
    //                ErrorExcute(ErrorState.Head_Null);
    //                break;

    //            case 3102:
    //                ErrorExcute(ErrorState.Photo_Null);
    //                break;

    //            case 3103:
    //                ErrorExcute(ErrorState.Hair_Null);
    //                break;

    //            case 3104:
    //                ErrorExcute(ErrorState.Body_Null);
    //                break;

    //            case 3105:
    //                ErrorExcute(ErrorState.Top_Null);
    //                break;

    //            case 3106:
    //                ErrorExcute(ErrorState.Bottom_Null);
    //                break;

    //            case 3107:
    //                ErrorExcute(ErrorState.Shoes_Null);
    //                break;


    //        }


    //    }

    //}

    ///// <summary>
    ///// 기존 커스텀 데이터 세팅
    ///// </summary>
    ///// <param name="response"></param>
    //void SetSuccessData(AvatarCustom.Custom.Response response)
    //{
    //    Debug.LogError("SetSuccessData");

    //    customPlayer.head = myPlayer.head = response.head;
    ////  customPlayer.photo = myPlayer.photo = DataManager.Instance.StringToByte(response.photo);
    //    currentHairCutIdx = customPlayer.hair = myPlayer.hair = response.hair;
    //    customPlayer.body = myPlayer.body = response.body;

    //    currentTopIdx = customPlayer.top = myPlayer.top = response.top;
    //    currentBottomIdx = customPlayer.bottom = myPlayer.bottom = response.bottom;
    //    customPlayer.shoes = myPlayer.shoes = response.shoes;

    //    Close();

    //}

    ///// <summary>
    ///// 디폴트 커스텀 데이터 세팅
    ///// </summary>
    //void SetFailedData()
    //{
    //    Debug.LogError("SetFailedData");

    //    customPlayer.head = string.Empty;
    //    customPlayer.body = SalinConstants.Const.defaultInt;// = UnityEngine.Random.Range(0, 2);
    // // customPlayer.photo = null;

    //    currentHairCutIdx = customPlayer.hair = 0;

    //    customPlayer.top = 0;
    //    customPlayer.bottom = 0;
    //    customPlayer.shoes = 0;

    //    isFirstCustom = true;

    //}


    ///// <summary>
    ///// 수정된 커스텀 데이터를 player data에 할당
    ///// </summary>
    ///// <param name="player"></param>
    //void ReplaceCustomData(DataManager.Player player)
    //{
    //    DataManager.Instance.PlayerData.head = player.head;
    //   // DataManager.Instance.PlayerData.photo = player.photo;

    //    DataManager.Instance.PlayerData.hair = player.hair;
    //    DataManager.Instance.PlayerData.body = player.body;

    //    DataManager.Instance.PlayerData.top = player.top;
    //    DataManager.Instance.PlayerData.bottom = player.bottom;
    //    DataManager.Instance.PlayerData.shoes = player.shoes;
    //}

    //#endregion

    //#region Error

    //public void ErrorExcute(ErrorState state)
    //{

    //    string Error = string.Empty;
    //    switch (state)
    //    {
    //        case ErrorState.Success:
    //            Error = "Success";
    //            break;

    //        case ErrorState.Server_Error:
    //            Error = "Server is being checked.";
    //            break;

    //        case ErrorState.Data_Null:
    //            Error = "Not Exsit Avatar Custom Data.";
    //            break;

    //        case ErrorState.Head_Null:
    //        case ErrorState.Photo_Null:
    //        case ErrorState.Hair_Null:
    //        case ErrorState.Body_Null:
    //        case ErrorState.Top_Null:
    //        case ErrorState.Bottom_Null:
    //        case ErrorState.Shoes_Null:
    //            Error = string.Format("Custom Data {0}! ", state.ToString());
    //            break;

    //    }

    //    Debug.Log("[ErrorState] : " + Error);

    //}
    //public void AvatarErrorExcute(string msg)
    //{
    //    // 쓰지 않는 이전 아바타 삭제
    //    if (msg.Contains(Constants.AvatarMsg.Deleting.ToString())
    //        // 사진 찍는것을 취소하거나, 앨범에서 선택 하기를 취소 했을 때
    //        || (msg.Contains(Constants.AvatarMsg.Null.ToString())))
    //    {
    //        return;
    //    }
    //    else if (msg.Contains(Constants.AvatarMsg.Failed.ToString())
    //        || msg.Contains(Constants.AvatarMsg.Bad.ToString()))
    //    {
    //        UIManager.Instance.OpenPopup(string.Format("다시 시도해주시기 바랍니다. \r\n{0}",msg),null);
           
    //        InterAllButton(true);
    //        InterButton(saveButton,false);

    //        SetUiBlock(false);
    //    }

    //    PrintProgress(msg);

    //}

    //#endregion

    //#region OnClick Event

    ///// <summary>
    ///// Parts Tab Button : Hair, Top, Bottom
    ///// </summary>
    ///// <param name="index"></param>
    //public void OnClickItemTab(int index)
    //{
    //    for (int i = 0; i < contentViews.Length; i++)
    //    {
    //        // scrRect.content = contentViews[i].GetComponent<RectTransform>();

    //        contentViews[i].gameObject.SetActive(i == index);

    //    }

    //    content_index = index;

    //}


    ///// <summary>
    ///// 선택된 바디타입 표시
    ///// </summary>
    ///// <param name="type"></param>
    //private void OnSeletedBodyType(int type)
    //{
    //    for (int i = 0; i < Item_bodyType.Length; i++)
    //    {
    //        Item_bodyType[i].GetComponent<AvatarItem>().SetSeleted(i == type);
    //    }
    //}

    ///// <summary>
    ///// 사진 가져오는 방법 선택
    ///// </summary>
    //public void OnClickAvatarPhoto()
    //{
    //    if (customPlayer.body == SalinConstants.Const.defaultInt)
    //        UIManager.Instance.OpenPopup("바디 타입을 선택해주세요", null);
    //    else
    //    {
    //        UIManager.Instance.OpenAvatarPhoto(() =>
    //        {
    //            OnClickCreateHead(0);
    //        },
    //        () =>
    //        {
    //            OnClickCreateHead(1);
    //        });
    //    }

    //}

    //public void OnClickSave()
    //{
    //    Debug.Log("OnClick Save Custom Data");

       
       
    //    if (main.GetCurrentAvatar().GetAvatarHead() == null)
    //    {
    //        UIManager.Instance.OpenPopup("아바타가 존재하지않습니다.",null);
    //        //ErrorExcute(ErrorState.Head_Null);
    //        return;
    //    }

    //    // 커스텀한 데이터로 교체
    //    ReplaceCustomData(customPlayer);

    //    PrintCustomData(DataManager.Instance.PlayerData);


    //    if (isFirstCustom)
    //    {

    //        RequestServer(2);
    //    }
    //    else
    //    {
    //        RequestServer(3);
    //    }

    //}

    //public void OnClickCancel()
    //{
    //    Debug.Log("OnClick Cancel Custom Data");

    //    // 새롭게 생성한 유저라면 기본 데이터로 저장
    //    if (myPlayer.head == string.Empty)
    //        myPlayer = customPlayer;

    //    Close();
    //}

    //#endregion

    //#region ETC

    //public DataManager.Player GetCustomData()
    //{
    //    return customPlayer;
    //}



    ///// <summary>
    ///// 커스텀 아이템 블락 여부
    ///// </summary>
    ///// <param name="isEnable"></param>
    //public void SetUiBlock(bool isEnable)
    //{
    //    UIBlock.SetActive(isEnable);
    //}


    ///// <summary>
    ///// 커스텀 관련 버튼 활성화 여부
    ///// </summary>
    ///// <param name="isEnable"></param>
    //void InterAllButton(bool isEnable)
    //{
    //    InterButtons(Item_Tabs, isEnable);
    //    InterButtons(Item_bodyType, isEnable);

    //    InterButton(saveButton, isEnable);
    //    InterButton(uploadButton.GetComponent<Button>(), isEnable);

    //    //uploadButton.GetComponent<Button>().interactable = isEnable; // .SetActive(isEnable);
    //}

    //void InterButtons(Button[] buttons, bool isEnable)
    //{
    //    for (int i = 0; i < buttons.Length; i++)
    //    {
    //        buttons[i].interactable = isEnable;
    //    }


    //}

    //void InterButton(Button button, bool isEnable)
    //{

    //    button.interactable = isEnable;

    //}


    //void PrintCustomData(DataManager.Player player)
    //{
    //    string print = string.Format("head : {0} \r\n hair : {1} \r\n body : {2} \r\n top : {3} \r\n bottom : {4} \r\n shoues : {5} \r\n",
    //        player.head, player.hair, player.body, player.top, player.bottom, player.shoes);

    //    Debug.Log(print);

    //}


    //void PrintProgress(string msg)
    //{
    //    txt_progress.text = msg;
    //}
    //#endregion


}
