using Common;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网络模块
/// </summary>
public class NetManager : ManagerBase, IPhotonPeerListener
{
    public static NetManager Instance = null;

    public static PhotonPeer peer;
    private LoginHandler loginHandler;
    private RegisterHandler registerHandler;
    private PlayerHandler playerHandler;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            //跳转到下一个场景也不再生成
            Destroy(this.gameObject);
            return;
        }
        //通过Listener监听服务器端的响应
        peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
        //192.168.31.166:5055
        //192.168.159.1:5055
        //172.16.75.1:5055
        //www.qq1503414695.xyz:4396
        //106.12.102.146:5055
        //3031s1426h.zicp.vip:36729
        peer.Connect("127.0.0.1:4530", "MergePetServer");
        //print(peer.PeerState);
    }

    /// <summary>
    /// 开始和服务器连接
    /// </summary>
    private void Start()
    {
        //peer.Service();
        loginHandler = new LoginHandler();
        registerHandler = new RegisterHandler();
        playerHandler = new PlayerHandler();
    }

    /// <summary>
    /// photonserver提供的方法 必须通过它启动所有的服务 需要一直调用
    /// </summary>
    private void Update()
    {
        peer.Service();
        //print(peer.PeerState);
    }

    /// <summary>
    /// 物体销毁时 断开和服务器的链接
    /// </summary>
    private void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
            peer.Disconnect();
    }

    #region 完成和服务器的交互

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.LogWarning(message);
    }

    public void OnEvent(EventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode)
        {
            case (byte)OpCode.Login:
                loginHandler.OnOperationResponse(operationResponse);
                break;
            case (byte)OpCode.Register:
                registerHandler.OnOperationResponse(operationResponse);
                break;
            //创建好角色 已经已经创建过角色 都需要刷新信息
            case (byte)OpCode.Create:
            case (byte)OpCode.Refresh:
            case (byte)OpCode.Rank:
                playerHandler.OnOperationResponse(operationResponse);
                break;
            default:
                break;
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.LogWarning(statusCode);
    }
    #endregion

}
