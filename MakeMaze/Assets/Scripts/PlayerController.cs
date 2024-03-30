using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("パラメーター")]
    [SerializeField] float locoSpeed;                    // 移動スピード
    [SerializeField] float dashSpeed;
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // 視点スピード
    private Coord playerCoord;
    private Coord playerPrevious;
    private Stack<SectionTable.Section> _sectionStack1 = new Stack<SectionTable.Section>();
    private Stack<SectionTable.Section> _sectionStack2 = new Stack<SectionTable.Section>();

    [Header("参照")]
    [SerializeField] GameObject mainCam;

    [Header("コンポーネント")]
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] MapManager gameMap;
    private GameObject _triggerObj;
    private FPS fps;

    private void Start()
    {
        fps = new FPS(gameMap.map);
        transform.position = gameMap.StartPos;
    }

    void Update()
    {
        playerCoord = gameMap.gridField.GridCoordinate(transform.position);
        playerPrevious = FPS.GetVector3FourDirection(transform.rotation.eulerAngles);


        // FPS視点設定
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed,dashSpeed);
        fps.ClampMoveRange(transform);

        PlayerAction0();
        PlayerAction1();

    }

    private void FixedUpdate()
    {
        _triggerObj = null;
    }

    private void OnTriggerStay(Collider other)
    {
        _triggerObj = other.gameObject;

        switch (other.tag)
        {
            case "flag":
                uiManager.EnterDisplayGameMessage("右クリック！！",Color.black,10);
                break;

            case "hamburger":
                uiManager.EnterDisplayGameMessage("右クリック！！", Color.black, 10);

                break;

            case "clearFlag":
                uiManager.EnterDisplayGameMessage("右クリック！！", Color.yellow, 10);

                break;
            case "enemy":
                AudioManager.PlaySEStart("GameOver");
                gameManager.clearFlag = false;
                break;


        }
    }

    /// <summary>
    /// 左クリックしたときのアクション
    /// </summary>
    private void PlayerAction0()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.hamburgerCount == 0) return;
            InitStack(_sectionStack1);
            InitStack(_sectionStack2);
            if (OpenBranchingSection(playerCoord, playerPrevious, _sectionStack1.Peek()))
            {
                AudioManager.PlayOneShot("Open");
                gameManager.hamburgerCount--;
                _sectionStack1.Pop();
                // ボーダーリストのブロックが書き換えられたら、壁に戻す
                if(!gameMap.borderBlockList.All(b => b.isSpace))
                {
                    gameMap.map.CreateWallsSurround();
                }
                gameMap.map.ActiveMapWallObject();
            }
            else
            {
                AudioManager.PlayOneShot("NotOpen");
                uiManager.EnterDisplayGameMessage("そこでは開けません！！", Color.red, 10);
            }
            
        }
    }


    /// <summary>
    /// 右クリックしたときのアクション
    /// </summary>
    private void PlayerAction1()
    {
        if(Input.GetMouseButtonDown(1))
        {
            gameManager.CheckInObj(_triggerObj);
        }
    }






    /// <summary>
    /// プレイヤーの前にセクションをオープンし、そのセクションの
    /// 指定したブランチから、ランダムな方向にセクションを生成します。
    /// </summary>
    /// <param name="branchCoord">プレイヤーの座標</param>
    /// <param name="dir">プレイヤーの向き</param>
    /// <param name="section">オープンするセクション</param>
    /// <param name="branchIndx">二つ目をオープンするブランチのインデックス</param>
    /// <returns>オープンできたかどうか</returns>
    private bool OpenBranchingSection(Coord branchCoord, Coord dir,SectionTable.Section section,int branchIndx = 3)
    {
        // 一つ目がオープンできるかチェック
        if (!CheckSectionPrevious(branchCoord, dir, section)) 
        {
            return false;
        }
        // 一つ目オープン
        OpenSectionPrevious(branchCoord, dir, section);

        // 一つ目のセクションのブランチ座標
        Coord[] branchCoords = section.GetDirectionSection(dir);

        // 一つ目の指定されたブランチ座標からランダムに
        // 二つ目のセクションをオープン
        if( OpenAround(branchCoords[branchIndx] + branchCoord + dir, _sectionStack2.Peek()))
        {
            _sectionStack2.Pop();
            return true;
        }
        else
        {
            gameMap.CloseSection(branchCoord + dir,branchCoords);
            return false;
        }
    }


    /// <summary>
    /// 指定した座標からランダムな方向にセクションをオープンします
    /// </summary>
    /// <param name="branchCoord">オープンするブランチ座標</param>
    /// <param name="section"></param>
    /// <returns>オープンできたかどうか</returns>
    private bool OpenAround(Coord branchCoord, SectionTable.Section section)
    {
        // ランダムな方向のスタック
        Stack<Coord> randDirStack = FPS.RandomVector3DirectionStack();

        // 全部の方向を試すためのwhile
        while (true)
        {
            Coord confDir = randDirStack.Pop();
            if (CheckSectionPrevious(branchCoord, confDir, section))
            {
                OpenSectionPrevious(branchCoord, confDir, section);
                return true;
            }
            if (randDirStack.Count <= 0)
            {

                return false;
            }
        }
    }


    /// <summary>
    /// オープンできなくなるまでランダムな方向でセクションをオープンし続けます
    /// ※再起関数
    /// </summary>
    /// <param name="branchCoord">一つ目のセクションのブランチ座標</param>
    /// <param name="section">一つ目のセクション</param>
    /// <returns>オープンできたかどうか</returns>
    private bool OpenAroundContinue(Coord branchCoord, SectionTable.Section section)
    {
        Stack<Coord> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

        // オープンできるか確認していく
        // できなかった場合は方向を変えてループする
        while (true)
        {
            Coord confDir = randDirStack.Pop();
            
            // オープンできるか確認
            if(CheckSectionPrevious(branchCoord,confDir,section ))
            {
                SectionTable.Section rand = SectionTable.RandSection;
                OpenSectionPrevious(branchCoord,confDir,section);
                OpenAroundContinue(branchCoord + section.GetDirectionSection(confDir)[3], rand);
                return true;
            }

            // すべての方向を確認して、
            // どの方向でもオープンできなかった場合はfalseを返す
            if(randDirStack.Count <= 0) 
            {
                return false;
            }
        }
    }


    /// <summary>
    /// 指定した向きのひとつ前のグリッド座標をシードとして対応した向きのセクションでオープンします
    /// </summary>
    /// <param name="branchCoord">オープンするセクションの一つ後ろの座標</param>
    /// <param name="dir">向き</param>
    /// <param name="section">セクション</param>
    public void OpenSectionPrevious(Coord branchCoord, Coord dir, SectionTable.Section section)
    {
        Coord[] sectionCoords = section.GetDirectionSection(dir);
        Coord prevCoord = branchCoord + dir;
        gameMap.OpenSection(prevCoord, sectionCoords);
    }


    /// <summary>
    /// 指定した向きのひとつ前のグリッド座標をシードとして、対応した向きのセクションがオープンできるか確認します
    /// </summary>
    /// <param name="branchCoord">オープンするセクションの一つ後ろの座標</param>
    /// <param name="dir">向き</param>
    /// <param name="section">セクション</param>
    /// <returns>オープンできるかどうか true：できる</returns>
    public bool CheckSectionPrevious(Coord branchCoord, Coord dir, SectionTable.Section section)
    {
        Coord[] sectionCoords = section.GetDirectionSection(dir);
        Coord prevCoord = branchCoord + dir;
        return gameMap.CheckAbleOpen(prevCoord,sectionCoords);
    }

   　/// <summary>
   　/// sectionStackの中身を入れなおして、シャッフルします
   　/// </summary>
   　private void InitStack(Stack<SectionTable.Section> stack)
   　{
        if (stack.Count > 0) return;

        SectionTable.Section[] sections =
        {
            SectionTable.T,
            SectionTable.O,
            SectionTable.I,
            SectionTable.L,
            SectionTable.J,
            SectionTable.S,
            SectionTable.Z
        };
        Algorithm.Shuffle(sections);
        foreach(SectionTable.Section section in sections) 
        {
            stack.Push(section);
        }
   　}
}
