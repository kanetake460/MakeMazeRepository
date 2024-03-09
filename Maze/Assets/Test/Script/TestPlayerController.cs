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

    /// <summary>
    /// 右クリックしたときのアクション
    /// </summary>
    private void PlayerAction1()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //InitStack();
            //OpenSectionPrevious(playerCoord,playerPrevious,sectionQueue.Peek());
            //Test(playerCoord, playerPrevious);
            OpenSectionContinue(3, 3);

        }
    }


    public bool OpenSectionContinue(int continueCount , int branchIndx)
    {
        Stack<Vector3Int> seedCoord = new Stack<Vector3Int>();
        Stack<Vector3Int[]> sectionCoords = new Stack<Vector3Int[]>();

        // プレイヤーの向きの情報をプッシュ
        seedCoord.Push(playerCoord + playerPrevious);
        sectionCoords.Push(SectionTable.randSection.GetDirectionSection(playerPrevious));

        for (int i = 0; i < continueCount; i++)
        {
            Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();

            // 全部の方向を試すためのwhile
            while (true)
            {
                // もし、置けなかった場合は
                if (!map.CheckAbleOpen(seedCoord.Peek(), sectionCoords.Peek()))
                {
                    // 調べる方向がない場合はどこにも置けないので探索終了
                    if (randDirStack.Count <= 0)
                    {
                        // すべてをクローズ
                        while (seedCoord.Count > 0)
                        {
                            seedCoord.Pop();
                            sectionCoords.Pop();
                            map.CloseSection(seedCoord.Pop(), sectionCoords.Pop());
                        }
                        return false;
                    }
                    // まだ調べる方向があるならいったんセクションをポップして、
                    // べつの方向をプッシュし、もう一度
                    sectionCoords.Pop();
                    sectionCoords.Push(SectionTable.randSection.GetDirectionSection(randDirStack.Pop()));
                    
                    continue;
                }
                // ひとつ前のセクションからランダムな方向の情報をだして、プッシュ
                seedCoord.Push(playerCoord + sectionCoords.Peek()[branchIndx]);
                sectionCoords.Push(SectionTable.randSection.GetDirectionSection(randDirStack.Pop()));
                break;
            }
        }
        return true;
    }




    public bool Test(Vector3Int playerCoord, Vector3Int plalyerPrevious)
    {
        Stack<Vector3Int> seedCoord = new Stack<Vector3Int>();
        Stack<Vector3Int[]> sectionCoords = new Stack<Vector3Int[]>();
        
        // プレイヤーの向きの情報をプッシュ
        seedCoord.Push(playerCoord + plalyerPrevious);
        sectionCoords.Push(SectionTable.randSection.GetDirectionSection(playerPrevious));
        
        // チェック一回目
        if (map.CheckAbleOpen(seedCoord.Peek(), sectionCoords.Peek()))
        {
            // 全方向をランダム名順番で格納したスタックを作成
            Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();

            // スタックが空じゃないなら
            while (randDirStack.Count != 0)
            {
                // ランダムな向きの情報をプッシュ
                Vector3Int randDir = randDirStack.Pop();
                seedCoord.Push(playerCoord + sectionCoords.Peek()[1] + randDir);
                sectionCoords.Push(SectionTable.randSection.GetDirectionSection(randDir));

                Debug.Log(seedCoord.Peek());
                Debug.Log(sectionCoords.Peek()[1]);
                Debug.Log(seedCoord.Peek() + sectionCoords.Peek()[1]);
                //Debug.Log(sectionCoords.Peek()[1]);
                // チェック二回目
                if (map.CheckAbleOpen(seedCoord.Peek(),sectionCoords.Peek()))
                {
                    // 全方向をランダム名順番で格納したスタックを初期化
                    randDirStack = FPS.RandomVector3DirectionStack();

                    // スタックが空じゃないなら
                    while (randDirStack.Count != 0)
                    {
                        // ランダムな向きの情報をプッシュ
                        randDir = randDirStack.Pop();
                        seedCoord.Push(playerCoord + sectionCoords.Peek()[3] + randDir);
                        sectionCoords.Push(SectionTable.randSection.GetDirectionSection(randDir));

                        // チェック三回目
                        if(map.CheckAbleOpen(seedCoord.Peek(),sectionCoords.Peek()))
                        {
                            // すべてクリアしたら、スタックしてきたランダムな向きの情報で
                            // オープンしていく
                            map.OpenSection(seedCoord.Pop(),sectionCoords.Pop());
                            map.OpenSection(seedCoord.Pop(),sectionCoords.Pop());
                            map.OpenSection(seedCoord.Pop(),sectionCoords.Pop());
                            return true;
                        }
                        // 三回目のチェックでオープンできなかったら、ランダムな向きの情報をポップ
                        seedCoord.Pop();
                        sectionCoords.Pop();
                    }
                }
                // 二回目のチェックでオープンできなかったら、ランダムな向きの情報をポップ
                seedCoord.Pop();
                sectionCoords.Pop();
            }
        }
        return false;
    }

    /// <summary>
    /// 指定した向きのひとつ前のグリッド座標をシードとして対応した向きのセクションでオープンします
    /// </summary>
    public bool OpenSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        if (map.CheckAbleOpen(prevCoord, sectionCoords))
        {
            map.OpenSection(prevCoord, sectionQueue.Dequeue().GetDirectionSection(dir));
            return true;
        }
        return false;
    }


    /// <summary>
    /// ランダムな向きにオープンします
    /// </summary>
    /// <param name="branchCoord"></param>
    /// <param name="section"></param>
    /// <returns>false：どこにも置けない</returns>
    public bool CheckSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    {
        // 全方向のVector3Intの向きが入ったスタックを入れる
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        while (true)
        {
            // もし、ポップして開けたらtrue
            if (map.CheckAbleOpen(branchCoord, section.GetDirectionSection(randDirStack.Pop())))
            {
                return true;
            }
            // もし、スタックがなくなったらどの方向でも置けないのでブレークする
            if (randDirStack.Count == 0)
                break;
        }
        Debug.Log("どこにも置けない");
        return false;
    }
    
    /// <summary>
    /// ランダムな向きにオープンします
    /// </summary>
    /// <param name="branchCoord"></param>
    /// <param name="section"></param>
    /// <returns>false：どこにも置けない</returns>
    public bool OpenSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    {
        // 全方向のVector3Intの向きが入ったスタックを入れる
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        while (true)
        {
            // もし、ポップして開けたらtrue
            if (map.CheckAbleOpen(branchCoord, section.GetDirectionSection(randDirStack.Peek())))
            {
                OpenSectionPrevious(branchCoord,randDirStack.Peek(),section);
                return true;
            }
            randDirStack.Pop();
            // もし、スタックがなくなったらどの方向でも置けないのでブレークする
            if (randDirStack.Count == 0)
                break;
        }
        Debug.Log("どこにも置けない");
        return false;
    }




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
