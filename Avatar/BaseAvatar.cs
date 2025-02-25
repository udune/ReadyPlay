using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Salin.AvatarModule;
using Debug = UnityEngine.Debug;

public class BaseAvatar : MonoBehaviour, IPunInstantiateMagicCallback
{
    public PhotonView pv;

    public enum AvatarControlMode
    {
        Animation,
        VRIK
    }

    [HideInInspector] public SalinConstants.AnimationType prevType = SalinConstants.AnimationType.Idle;
    [HideInInspector] public int controlMode;

    [Header("Avatar Settings")] public Transform AvatarPos;
    public Transform VideoPos;
    public Transform AvatarUI;
    public Camera SelfieCam;

    [SerializeField] public Avatar_Controller AvatarController;
    [SerializeField] public Avatar_Camera AvatarCamera;
    [SerializeField] public Avatar_Sync AvatarSync;

    public Avatar_Info AvatarInfo;
    public AvatarCreate AvatarCreate;

    FollowCamera followCamera;

    public Chair Chair;
    private int avatarId;

    public int AVATAR_ID
    {
        get { return avatarId; }
        private set { avatarId = value; }
    }

    private string avatarName;

    public string AVATAR_NAME
    {
        get { return avatarName; }
        private set { avatarName = value; }
    }

    private int chatType;

    public int ChatType
    {
        get { return chatType; }
        private set { chatType = value; }
    }

    [HideInInspector] public Vector3 childAdjust = new Vector3(0f, 0.1f, 0.1f);

    [Header("Border Settings")] [SerializeField]
    Transform borderGroup;

    [SerializeField] GameObject nameBorder, roleBorder, videoChatBorder;
    [SerializeField] Animation voiceAnim;
    [SerializeField] public Transform HeadObject;
    [SerializeField] GameObject loadingObject;
    [SerializeField] bool initableAngle = true;

    [Header("MiniMap")] [SerializeField] private GameObject myMinimapIcon;
    [SerializeField] private GameObject otherMinimapIcon;
    private Coroutine vrCameraFixCoroutine;
    public GameObject MyMinimapIcon => myMinimapIcon;
    public GameObject OtherMinimapIcon => otherMinimapIcon;

    private void Awake()
    {
        SelfieCam.enabled = false;

        transform.SetParent(ObjectManager.Standard, true);

        //  pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            //   GetComponent<CharacterController>().enabled = true;

            myMinimapIcon.SetActive(true);
            otherMinimapIcon.SetActive(false);

            // SelfieCam.tag = "MainCamera";
        }
        else
        {
            myMinimapIcon.SetActive(false);
            otherMinimapIcon.SetActive(true);
        }

        //videoChatBorder.GetComponentInChildren<Button>().onClick.AddListener(OnClickVideoChat);
        videoChatBorder.GetComponent<Button>().onClick.AddListener(OnClickVideoChat);
        videoChatBorder.SetActive(false);
    }

    /// <summary>
    /// PhotonNetwork.Instantiate 으로 네트워크 객체 생성 완료시 호출되는 콜백
    /// </summary>
    /// <param name="info"></param>
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SetInitial(info.photonView.InstantiationData);
    }

    public bool IsMine()
    {
        return pv.IsMine;
    }

    public void RequestRandomSpawn(bool _isNav = true)
    {
        if (pv.IsMine)
        {
            if (RoomMultiManager.Instance.IsReturn)
            {
                StartCoroutine(AvatarController.ReturnTeleport());
                return;
            }

            AgentSwitch(false);

            if (RoomMultiManager.Instance.roomBase == null)
            {
                Debug.LogError("roombase is null");
                return;
            }

            SetPostiton(AvatarPos);

            AgentSwitch(_isNav);
        }
    }

    public void ReceiveSitSpawn(Chair ch, bool isSitable)
    {
        //int userId = DataManager.Instance.UserData.userId;
        //// 좌석 요청 큐에 추가
        //AvatarSpawnHandler.Instance.EnqueueReqQ(new AvatarSpawnHandlerInfo(userId, this));
        SetSit(ch, isSitable);

        if (controlMode == 1)
        {
            return;
        }

        SetSitPostiton(ch.transform);
    }


    private void SetInitial(object[] param)
    {
        AVATAR_ID = (int)param[0];
        AVATAR_NAME = gameObject.name = param[1].ToString();

        bool isMaxAvatar = (bool)param[2];
        bool isHost = (bool)param[3];

        SalinConstants.PlayerAvatarPartsData partsData = DataManager.Instance.ParsePlayerAvatarPartsData((int)param[4], (string)param[5], (string)param[6], (string)param[7]);

        PlayerManager.AddAvatar(AVATAR_ID.ToString(), this);

        AvatarController.Avatar = this;

        if (!isMaxAvatar)
        {
            SetNickname(AVATAR_NAME);

            AvatarCreate.CreateBaseAvatar(partsData, (obj, objs) =>
            {
                AvatarCreate.AvatarPartsChange(obj, objs);

                PhotonNetwork.IsMessageQueueRunning = true;
                SetAvatarInfo(obj.GetComponent<Avatar_Info>());
                AvatarSync.Avatar = this;
                AvatarSync.enabled = true;
                SetAvatarAnim();
                SetFollowObjects();

                SetRole(isHost);

                if (IsMine())
                {
                    PlayerManager.MyAvatar = this;

                    if (RoomMultiManager.Instance.IsReturn)
                        StartCoroutine(AvatarController.ReturnTeleport());

                    AgentSwitch(RoomMultiManager.Instance.roomBase.isNavAvatar);
                    InitAngle(transform.eulerAngles.y);
                    AvatarCamera.enabled = true;

                    AvatarController.isMovable = true;
                    AvatarController.enabled = true;
                }
                else
                {
                    gameObject.tag = "MultiAvatar";
                    if (TryGetComponent<CapsuleCollider>(out CapsuleCollider collier))
                        collier.enabled = true;
                    Destroy(GetComponent<BoxCollider>());
                    //Destroy(GetComponent<Rigidbody>());

                    AvatarController.enabled = false;
                }
            }, (avatar) =>
            {
                loadingObject.SetActive(false);
                if (IsMine())
                    AvatarController.ExcuteInputControl(RoomMultiManager.Instance.roomBase.isNavAvatar);
            });
        }
        else
        {
            borderGroup.gameObject.SetActive(false);
            loadingObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (pv.IsMine)
            PlayerManager.MyAvatar = null;

        PlayerManager.Dict_Avatar.Remove(AVATAR_ID.ToString());
    }

    void SetFollowObjects()
    {
        borderGroup.GetComponent<FollowTarget>().ExcuteFollow(AvatarInfo.headBone.transform);

        followCamera = borderGroup.GetComponent<FollowCamera>();
        followCamera.StartFollow(Camera.main);
    }

    public void SetSelfiCamera(bool isSelfi)
    {
        if (isSelfi)
        {
            followCamera.ChangeTargetCamera(SelfieCam);
        }
        else
        {
            followCamera.ChangeTargetCamera(Camera.main);
        }
    }

    /// <summary>
    /// 플레이중 해당 공간에서의 아바타 닉네임 변경
    /// </summary>
    /// <param name="_nick"></param>
    public void SetAvatarNick(string _nick)
    {
        pv.RPC("SetNickname", RpcTarget.AllBuffered, _nick);
    }

    [PunRPC]
    void SetNickname(string nickname)
    {
        if (nickname == null)
        {
            borderGroup.gameObject.SetActive(false);
        }
        else
        {
            nameBorder.GetComponentInChildren<Text>().text = nickname;
        }
    }

    public void SyncPosition()
    {
        pv.RPC("SetSyncPos", RpcTarget.Others, transform.position, transform.rotation);
    }

    [PunRPC]
    void SetSyncPos(Vector3 _pos, Quaternion _rot)
    {
        StartCoroutine(SyncPosCor(_pos, _rot));
    }

    IEnumerator SyncPosCor(Vector3 _pos, Quaternion _rot)
    {
        yield return new WaitUntil(() => AvatarInfo != null);
        transform.SetPositionAndRotation(_pos, _rot);
    }


    public void SetRole(bool isHost)
    {
        if (roleBorder != null)
        {
            roleBorder.SetActive(isHost);
        }
    }

    public void SetVideoChat(bool isMine)
    {
        if (videoChatBorder != null)
        {
            // 임시 비활성화
            videoChatBorder.SetActive(isMine);
        }
    }

    public bool IsVideoChat()
    {
        return videoChatBorder.activeSelf;
    }

    public void SetChatType(int chatType)
    {
        this.ChatType = chatType;
    }

    void OnClickVideoChat()
    {
        UIManager.Instance.OpenPopup(TableManager.Instance.GetLanguage(10566), RequestVideoChat, null); //"화상채팅을 시작 하시겠어요?", RequestVideoChat, null);
    }

    void RequestVideoChat()
    {
        AgoraChatManager agora_m = AgoraChatManager.Instance;

        if (agora_m.GetVideoChatData() == null)
        {
            agora_m.RequestVideoChat((uint)avatarId, ChatType);
        }
        else
        {
            uint uid_a = agora_m.GetVideoChatData().uid_a;
            uint uid_b = agora_m.GetVideoChatData().uid_b;
            int chatType = agora_m.GetVideoChatData().chatType;

            if (agora_m.GetVideoChatData().chatMode == (int)SalinConstants.ChatMode.All)
            {
                agora_m.RequestVideoChat(uid_a, chatType);
            }
            else
            {
                agora_m.RequestVideoChatToTarget(uid_a, uid_b, chatType);
            }
        }

        SetVideoChat(false);
    }

    public void SetAvatarInfo(Avatar_Info info)
    {
        AvatarInfo = info;
    }


    void SetAvatarAnim()
    {
        AvatarInfo.enabled = true;

        AvatarInfo.DestroyVRIK();
        AvatarInfo.UseAnimation();
    }

    public void SetChatMode(bool isEnable)
    {
        if (controlMode == (int)AvatarControlMode.Animation)
        {
            AvatarController.isChatting = isEnable;
        }
    }

    public void SetCtrMovable(bool isMovable)
    {
        AvatarController.isMovable = isMovable;
    }

    public void StopMove(bool isStop)
    {
        AvatarController.isStop = isStop;
    }

    public void SetSit(Chair chair, bool isSitable)
    {
        Chair = chair;

        isGoundChair = (Chair == null ? false : Chair.isGroundSit);

        SetCtrMovable(isSitable);
        SetCtrSit(isSitable);

#if (Mobile)
        UIManager.Instance.GetUI<UIAvatarController>().SetInteract(isSitable, isSitable);
        UIManager.Instance.uiChat?.SetEmotionInter(isSitable);
#elif PC
        UIManager.Instance.uiChat?.SetEmotionInter(isSitable);
#endif
        if (controlMode == 1)
            return;

        if (isGoundChair)
            ExcuteBoolAnimation(SalinConstants.AnimationType.SitGround, !isSitable);
        else
        {
            ExcuteBoolAnimation(SalinConstants.AnimationType.Sit, !isSitable, true);
            if (chair != null && chair.isTyping)
                ExcuteBoolAnimation(SalinConstants.AnimationType.Typing, !isSitable, true);
        }
    }


    public void SetCtrSit(bool isSit)
    {
        AvatarController.isSitable = isSit;
    }

    bool isGoundChair = false;

    public void SetSitPostiton(Transform chair_tr) // (Vector3 pos, Quaternion rot)
    {
        Vector3 pos;
        if (isGoundChair)
            pos = chair_tr.position;
        else
            pos = chair_tr.position + (chair_tr.forward * .5f);
        Quaternion rot = chair_tr.rotation;
        transform.SetPositionAndRotation(pos, rot);
        InitAngle(transform.eulerAngles.y);
    }

    public void SetInitableAngle(bool initable)
    {
        initableAngle = initable;
    }

    public void InitAngle(float angle)
    {
        if (initableAngle)
        {
            SetAngle(angle);
            SetInitableAngle(false);
        }
    }

    private void SetAngle(float angle)
    {
        AvatarCamera.SetH = angle;
    }


    public void SetPostiton(Transform tr) // (Vector3 pos, Quaternion rot)
    {
        SetCtrMovable(false);
        AvatarController.isLerpMove = "N";

        Vector3 pos = tr.position;
        Quaternion rot = tr.rotation;

        pos.y = RoomMultiManager.Instance.roomBase.GroundPoint.position.y; //-1.8f; //GetHeight();

        transform.SetPositionAndRotation(pos, rot);

        InitAngle(transform.eulerAngles.y);
        AvatarController.isLerpMove = "Y";
        SetCtrMovable(true);
    }

    // IEnumerator SetPostiton(Transform tr)
    // {
    //     while (true)
    //     {
    //         yield return null;
    //         AgentSwitch(false);
    //         yield return new WaitForSeconds(0.1f);
    //         SetCtrMovable(false);
    //         SetSitColision(false);
    //         AvatarController.isLerpMove = false;
    //         yield return null;
    //         Vector3 pos = tr.position;
    //         Quaternion rot = tr.rotation;
    //
    //         pos.y = RoomMultiManager.Instance.roomBase.GroundPoint.position.y; //-1.8f; //GetHeight();
    //     
    //         transform.position = pos;
    //         transform.rotation = rot; //Quaternion.AngleAxis(rot.y, Vector3.up) ;
    //         yield return null;
    //         Debug.LogFormat ("[BaseAvatar] SetPosition pos: {0}, rot {1} " , pos,rot );
    //
    //         AvatarController.isLerpMove = true;
    //         SetSitColision(true);
    //         SetCtrMovable(true);
    //         yield return new WaitForSeconds(0.1f);
    //         AgentSwitch(true);
    //         yield break;
    //     }
    // }


    /// <summary>
    /// 아이, 성인 아바타 같이 있었을 때 쓰던거
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="pos"></param>
    /// <param name="modelType"></param>
    /// <returns></returns>
    private Vector3 AdjustSitPos(Transform tr, Vector3 pos, int modelType)
    {
        SalinConstants.RoomTemplatType currentType = RoomMultiManager.Instance.roomBase.GetRoomType();

        if (currentType == SalinConstants.RoomTemplatType.Lobby)
        {
            switch (modelType)
            {
                case (int)AvatarConstants.ModelType.Child:
                    pos.y += childAdjust.y;
                    pos = pos + (tr.forward * 0.1f);
                    return pos;
            }
        }
        else if (currentType == SalinConstants.RoomTemplatType.Classroom_ES)
        {
            switch (modelType)
            {
                case (int)AvatarConstants.ModelType.Child:
                    pos.y += childAdjust.y;
                    pos = pos + (tr.forward * 0.01f);
                    return pos;
            }
        }
        else if (currentType == SalinConstants.RoomTemplatType.SemiPlaza)
        {
            switch (modelType)
            {
                case (int)AvatarConstants.ModelType.Child:
                    pos.y += childAdjust.y;
                    pos = pos + (tr.forward * 0.15f);
                    return pos;
            }
        }
        else if (currentType == SalinConstants.RoomTemplatType.Classroom_HS)
        {
            switch (modelType)
            {
                case (int)AvatarConstants.ModelType.Child:
                    pos.y += childAdjust.y;
                    pos = pos + (tr.forward * 0.05f);
                    return pos;
            }
        }

        return pos;
    }

    private void SetSitCollider(bool isEnable)
    {
        GetComponent<BoxCollider>().enabled = isEnable;
    }

    public void LeaveChair()
    {
        if (pv.IsMine && Chair != null)
        {
            AgentSwitch(false);

            int senderId = DataManager.Instance.UserData.userId;
            RoomMultiManager.Instance.RequestSitAvatar(true, senderId, Chair.ID);
            Debug.Log("Request Spawn !! : " + senderId);
            Chair = null;
        }
    }


    public void ExcuteFloatAnimation(SalinConstants.AnimationType type, float value)
    {
        AvatarInfo?.PlayFloatAnimation(type.ToString(), value);

        if (pv.IsMine && !type.Equals(SalinConstants.AnimationType.Move))
        {
            prevType = type;
            AvatarSync.SetFloatAnimation((int)type, value);
        }
    }

    public void ExcuteTriggerAnimation(SalinConstants.AnimationType type)
    {
        AvatarInfo?.PlayTriggerAnimation(type.ToString());

        if (pv.IsMine)
        {
            prevType = type;
            AvatarSync.SetTriggerAnimation((int)type);
        }
    }

    public void ExcuteBoolAnimation(SalinConstants.AnimationType type, bool isPlay, bool isBuffered = false)
    {
        AvatarInfo?.PlayBoolAnimation(type.ToString(), isPlay);


        if (pv.IsMine)
        {
            prevType = type;
            AvatarSync.SeBoolAnimation((int)type, isPlay, isBuffered);
            if (type == SalinConstants.AnimationType.Selfie && isPlay == false)
                prevType = SalinConstants.AnimationType.None;
        }
    }

    public void SetVoiceSignAnimation()
    {
        voiceAnim.Play("Blink");
        //  Debug.Log("[BaseAvatar] : Play Voice Anim");
    }

    public void SetInteraction(string AnimName, Action callback = null, Transform Target = null)
    {
        StartCoroutine(Interact(AnimName, callback, Target));
    }

    IEnumerator Interact(string AnimName, Action callback = null, Transform Target = null)
    {
        if (Target != null)
            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
        AvatarController.enabled = false;
        AvatarInfo.PlayTriggerAnimation(AnimName);

        float WaitTime = AvatarInfo.GetCurAnimationTime() / 3;

        WaitForSeconds waitForSeconds = new WaitForSeconds(WaitTime);
        yield return waitForSeconds;
        callback?.Invoke();
        yield return waitForSeconds;
        AvatarController.enabled = true;
    }

    public void AgentSwitch(bool change)
    {
        if (pv.IsMine)
        {
            this.AvatarController.agent.enabled = change;
        }
    }

    public void AvatarAnimatorChange(RuntimeAnimatorController animatorController)
    {
        AvatarInfo.GetAvatarAni().runtimeAnimatorController = animatorController;
    }
}