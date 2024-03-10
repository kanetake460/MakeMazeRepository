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

    private Queue<SectionTable.Section> sectionQueue = new Queue<SectionTable.Section>();

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
            //InitStack();
            //OpenSectionPrevious(playerCoord,playerPrevious,sectionQueue.Peek());
            //OpenAroundContinue(new Vector3Int(20,0,20), SectionTable.I);
            OpenBranchingSection(playerCoord,playerPrevious, SectionTable.randSection,idx);
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
        OpenSectionPrevious(branchCoord, dir, section);

        Vector3Int[] branchCoords = section.GetDirectionSection(dir);
        return OpenAround(branchCoords[branchIndx] + branchCoord + dir, SectionTable.randSection);
        
    }

    private bool OpenAround(Vector3Int branchCoord, SectionTable.Section section)
    {
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

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


    // ※再起関数
    private bool OpenAroundContinue(Vector3Int seedCoord, SectionTable.Section section)
    {
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

        // 全部の方向を試すためのwhile
        while (true)
        {
            
            Vector3Int confDir = randDirStack.Pop();
            if(CheckSectionPrevious(seedCoord,confDir,section ))
            {
                SectionTable.Section rand = SectionTable.randSection;
                OpenSectionPrevious(seedCoord,confDir,section);
                OpenAroundContinue(seedCoord + section.GetDirectionSection(confDir)[3], rand);
                return true;
            }
            if(randDirStack.Count <= 0) 
            {

                return false;
            }
        }
    }


    /// <summary>
    /// 指定した向きのひとつ前のグリッド座標をシードとして対応した向きのセクションでオープンします
    /// </summary>
    public void OpenSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        map.OpenSection(prevCoord, sectionCoords);
    }

    public bool CheckSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        return map.CheckAbleOpen(prevCoord,sectionCoords);
    }


    ///// <summary>
    ///// ランダムな向きにオープンします
    ///// </summary>
    ///// <param name="branchCoord"></param>
    ///// <param name="section"></param>
    ///// <returns>false：どこにも置けない</returns>
    //public bool CheckSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    //{
    //    // 全方向のVector3Intの向きが入ったスタックを入れる
    //    Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
    //    while (true)
    //    {
    //        // もし、ポップして開けたらtrue
    //        if (map.CheckAbleOpen(branchCoord, section))
    //        {
    //            return true;
    //        }
    //        // もし、スタックがなくなったらどの方向でも置けないのでブレークする
    //        if (randDirStack.Count == 0)
    //            break;
    //    }
    //    Debug.Log("どこにも置けない");
    //    return false;
    //}
    
    ///// <summary>
    ///// ランダムな向きにオープンします
    ///// </summary>
    ///// <param name="branchCoord"></param>
    ///// <param name="section"></param>
    ///// <returns>false：どこにも置けない</returns>
    //public bool OpenSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    //{
    //    // 全方向のVector3Intの向きが入ったスタックを入れる
    //    Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
    //    while (true)
    //    {
    //        // もし、ポップして開けたらtrue
    //        if (map.CheckAbleOpen(branchCoord, section.GetDirectionSection(randDirStack.Peek())))
    //        {
    //            OpenSectionPrevious(branchCoord,randDirStack.Peek(),section);
    //            return true;
    //        }
    //        randDirStack.Pop();
    //        // もし、スタックがなくなったらどの方向でも置けないのでブレークする
    //        if (randDirStack.Count == 0)
    //            break;
    //    }
    //    Debug.Log("どこにも置けない");
    //    return false;
    //}




    //public bool OpenSectionPreviousContinuous(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section, int countinuousNum)
    //{
    //    int randBranch = Random.Range(1, 5);
    //    Vector3Int seedCoord = branchCoord + dir;
    //    Vector3Int[] sectionCoord = section.GetDirectionSection(dir);

    //    for(int i = 0; i < countinuousNum; i++) 
    //    {
    //        // 置けない場合はfalseを返す
    //        if(!map.CheckAbleOpen(seedCoord,sectionCoord))
    //        {
    //            return false;
    //        }

    //        // セクション、向き、座標を変更
    //        section = sectionList.ToArray()[i];
    //        seedCoord = section.GetDirectionSection(dir)[randBranch];
    //    }
    //}

    ///// <summary>
    ///// sectionStackの中身を入れなおして、シャッフルします
    ///// </summary>
    //private void InitStack(Queue<SectionTable.Section> queue)
    //{
    //    if (queue.Count <= 0)
    //    {
    //        queue.Enqueue(SectionTable.T);
    //        queue.Enqueue(SectionTable.O);
    //        queue.Enqueue(SectionTable.I);
    //        queue.Enqueue(SectionTable.L);
    //        queue.Enqueue(SectionTable.J);
    //        queue.Enqueue(SectionTable.S);
    //        queue.Enqueue(SectionTable.Z);

    //        Algorithm.Shuffle(sectionQueue.ToArray());
    //    }
    //}
}
