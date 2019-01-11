﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DataShow : MonoBehaviour
{
    public AudioSource BGM;
    static bool isbgm;
    static bool isData = true;
    static bool isIntroduce = false;
    Transform Imageleft;
    Transform Imageright;
    Transform Logo;
    Transform Introduce;
    IntroduceEffect effect;
    DetialPosition detialPosition;

    //景观漫游

    public GameObject mainCamera;
    public GameObject cameraManager;
    public GameObject MCDefaultTarget;
    bool isCruise = false;
    float timer = 0;

    //扫光特效
    public ShaderEffect roofLight, wallLight, carPortLight;
    //充电桩动画
    private Animation ChargeAni;
    //空调特效
    public GameObject whiteSmoke, light1, light2, light3;

    //登陆界面
    private GameObject login;
    NetworkHud showConnent;

    // Use this for initialization
    void Start()
    {
        detialPosition = Camera.main.GetComponent<DetialPosition>();
        Imageleft = transform.Find("Imageleft");
        Imageright = transform.Find("Imageright");
        Logo = transform.Find("Logo");
        Introduce = transform.Find("Introduce");
        //ChargeAni = GameObject.Find("MainScence/chongdianzhuang (3)").GetComponent<Animation>();
        login = transform.Find("ButtonManager/LogIn").gameObject;
        showConnent = GameObject.Find("NetworkManager").GetComponent<NetworkHud>();

        effect = GetComponent<IntroduceEffect>();

        Tweener tLeft = Imageleft.DOLocalMoveX(-1130f, 0.5f);
        Tweener tRight = Imageright.DOLocalMoveX(1130f, 0.5f);
        Tweener logo = Logo.DOLocalMoveY(600f, 0.5f);
        logo.SetAutoKill(false);
        tLeft.SetAutoKill(false);
        tRight.SetAutoKill(false);
        tLeft.Pause();
        tRight.Pause();
        logo.Pause();

    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer <= 6)
        {
            return;
        }
        else
        {
            timer = 10;
            OnClickDataShow();
            OnClickIntroduceCtrl();
            OnClickCruiseLandspace();
            OnClickLogin();

        }

    }

    #region 数据主页logo显示

    public void OnClickDataShow()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (isData == false)
            {
                DataLogoOut();
            }
            else
            {
                DataLogoIn();
            }

        }
    }
    public void DataLogoOut()
    {
        if (isData == true) return;
        Imageleft.DOPlayForward();
        Imageright.DOPlayForward();
        Logo.DOPlayForward();
        //特效隐藏
        effect.CharacterOFF();
        isData = true;
    }
    public void DataLogoIn()
    {
        Imageleft.DOPlayBackwards();
        Imageright.DOPlayBackwards();
        Logo.DOPlayBackwards();
        //特效显示
        effect.CharacterON();
        isData = false;
        //隐藏介绍页，地点，漫游
        IntroduceOut();
        detialPosition.ShowPositionOut();
        CruiseLandspaceStop();
    }
    #endregion

    #region 项目介绍
    IEnumerator DefaultPosition()
    {
        yield return new WaitForSeconds(0.75f);
        Introduce.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1940f, 0);
    }
    public void IntroduceIn()
    {
        Introduce.DOLocalMoveX(0, 0.7f);
        isIntroduce = true;
        Introduce.Find("Button").GetComponent<Button>().enabled = true;

        //特效出现
        //effect.TwinkLeON();
        effect.CircleON();

        //隐藏主页，地点，漫游
        DataLogoOut();
        detialPosition.ShowPositionOut();
        CruiseLandspaceStop();
    }
    public void IntroduceOut()
    {
        if (isIntroduce == false) return;
        Introduce.DOLocalMoveX(1940f, 0.7f);
        StartCoroutine("DefaultPosition");
        isIntroduce = false;
        Introduce.Find("Button").GetComponent<Button>().enabled = false;

        //关闭特效
        //effect.TwinkleOFF();
        effect.CircleOFF();
    }
    public void OnClickIntroduceCtrl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isIntroduce == false)
            {
                IntroduceIn();
            }
            else
            {
                IntroduceOut();
            }
        }
    }
    #endregion

    #region 景观漫游
    public void CruiseLandspaceStart()
    {
        isCruise = true;
        mainCamera.GetComponent<Animation>().Play("xunyou");

        //开启所有扫光
        roofLight.enabled = true;
        wallLight.enabled = true;
        carPortLight.enabled = true;
        //开启充电桩动画
        //ChargeAni.Play();
        //ChargeAni.wrapMode = WrapMode.Loop;
        //开启空调特效
        whiteSmoke.SetActive(true);
        light1.GetComponent<AircondictionEffect>().enabled = true;
        light2.GetComponent<AircondictionEffect>().enabled = true;
        light3.GetComponent<AircondictionEffect>().enabled = true;

        //关闭鼠标控制旋转
        mainCamera.GetComponent<CameraRotate>().enabled = false;

        //隐藏主页，地点，介绍页
        IntroduceOut();
        detialPosition.ShowPositionOut();
        DataLogoOut();
    }
    public void CruiseLandspaceStop()
    {
        if (isCruise == false) return;
        mainCamera.GetComponent<Animation>().Stop("xunyou");
        mainCamera.transform.DOMove(MCDefaultTarget.transform.position, 3f);
        mainCamera.transform.DORotate(MCDefaultTarget.transform.eulerAngles, 3f);
      
        //关闭所有扫光
        roofLight.enabled = false;
        wallLight.enabled = false;
        carPortLight.enabled = false;
        //关闭充电桩动画
        //ChargeAni.wrapMode = WrapMode.Default;
        //ChargeAni.Stop();
        //关闭空调特效
        whiteSmoke.SetActive(false);
        light1.GetComponent<AircondictionEffect>().enabled = false;
        light2.GetComponent<AircondictionEffect>().enabled = false;
        light3.GetComponent<AircondictionEffect>().enabled = false;

        //打开鼠标控制旋转
        mainCamera.GetComponent<CameraRotate>().enabled = true;
        isCruise = false;
    }
    void OnClickCruiseLandspace()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (isCruise == false)
            {
                CruiseLandspaceStart();
            }
            else
            {
                CruiseLandspaceStop();
            }
        }
    }
    #endregion

    #region 登陆界面
    bool isShowLogin = false;
    public void ShowLogin()
    {
        login.SetActive(true);
        showConnent.showGUI = true;
    }
    public void HideLogin()
    {
        login.SetActive(false);
        showConnent.showGUI = false;
    }
    void OnClickLogin()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isShowLogin = !isShowLogin;
            if (isShowLogin)
            {
                ShowLogin();     //显示
            }
            else
            {
                HideLogin();   //隐藏
            }

        }
    }
    #endregion

 
}
