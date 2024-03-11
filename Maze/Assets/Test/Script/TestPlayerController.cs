using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.Linq;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] GameObject mainCam;

    [SerializeField] MapGridField map;

    private Vector3Int playerCoord;
    private Vector3Int playerPrevious;

    private Stack<SectionTable.Section> _sectionStack1 = new Stack<SectionTable.Section>();
    private Stack<SectionTable.Section> _sectionStack2 = new Stack<SectionTable.Section>();

    /*パラメータ*/
    [SerializeField] float locoSpeed;                    // 移動スピード
    [SerializeField] float dashSpeed;
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // 視点スピード
    private FPS fps;

    private void Start()
    {
        fps = new FPS(map.map);
    }

    void Update()
    {
        playerCoord = map.gridField.GetGridCoordinate(transform.position);
        playerPrevious = FPS.GetVector3FourDirection(transform.rotation.eulerAngles);

        // FPS視点設定
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed,dashSpeed);
        fps.CursorLock();
        fps.ClampMoveRange(transform);

        PlayerAction1();
    }

    [SerializeField] int idx;
    /// <summary>
    /// 右クリックしたときのアクション
    /// </summary>
    private void PlayerAction1()
    {
        if (Input.GetMouseButtonDown(1))
        {
            InitStack(_sectionStack1);
            InitStack(_sectionStack2);
            if(OpenBranchingSection(playerCoord,playerPrevious, _sectionStack1.Peek(),idx))
                _sectionStack1.Pop();
            map.map.ActiveMapWallObject();
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
    private bool OpenBranchingSection(Vector3Int branchCoord,Vector3Int dir,SectionTable.Section section,int branchIndx)
    {
        // 一つ目がオープンできるかチェック
        if (!CheckSectionPrevious(branchCoord, dir, section)) 
        {
            Debug.Log("そこでは開けませんでした");
            return false;
        }
        // 一つ目オープン
        OpenSectionPrevious(branchCoord, dir, section);
        
        // 一つ目のセクションのブランチ座標
        Vector3Int[] branchCoords = section.GetDirectionSection(dir);

        // 一つ目の指定されたブランチ座標からランダムに
        // 二つ目のセクションをオープン
        if( OpenAround(branchCoords[branchIndx] + branchCoord + dir, _sectionStack2.Peek()))
        {
            _sectionStack2.Pop();
            Debug.Log("オープン！");
            return true;
        }
        else
        {
            Debug.Log("そこでは開けませんでした");
            map.CloseSection(branchCoord + dir,branchCoords);
            return false;
        }
    }


    /// <summary>
    /// 指定した座標からランダムな方向にセクションをオープンします
    /// </summary>
    /// <param name="branchCoord">オープンするブランチ座標</param>
    /// <param name="section"></param>
    /// <returns>オープンできたかどうか</returns>
    private bool OpenAround(Vector3Int branchCoord, SectionTable.Section section)
    {
        // ランダムな方向のスタック
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();

        // 全部の方向を試すためのwhile
        while (true)
        {
            Vector3Int confDir = randDirStack.Pop();
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
    private bool OpenAroundContinue(Vector3Int branchCoord, SectionTable.Section section)
    {
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

        // オープンできるか確認していく
        // できなかった場合は方向を変えてループする
        while (true)
        {
            Vector3Int confDir = randDirStack.Pop();
            
            // オープンできるか確認
            if(CheckSectionPrevious(branchCoord,confDir,section ))
            {
                SectionTable.Section rand = SectionTable.randSection;
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
    public void OpenSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        map.OpenSection(prevCoord, sectionCoords);
    }


    /// <summary>
    /// 指定した向きのひとつ前のグリッド座標をシードとして、対応した向きのセクションがオープンできるか確認します
    /// </summary>
    /// <param name="branchCoord">オープンするセクションの一つ後ろの座標</param>
    /// <param name="dir">向き</param>
    /// <param name="section">セクション</param>
    /// <returns>オープンできるかどうか true：できる</returns>
    public bool CheckSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        return map.CheckAbleOpen(prevCoord,sectionCoords);
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
            Debug.Log("プッシュ");
        }
   　}
}
