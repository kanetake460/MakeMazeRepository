using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] GameObject mainCam;

    [SerializeField] MapGridField map;

    private Vector3Int playerCoord;
    private Vector3Int playerPrevious;


    private Stack<Vector3Int[]> sectionStack = new Stack<Vector3Int[]>();

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
        if(Input.GetMouseButtonDown(1)) 
        {
            Debug.Log("a");
            //OpenSectionPrevious(playerCoord,playerPrevious,SectionTable.T.Top);
        }
    }


    /// <summary>
    /// 指定した向きのひとつ前のグリッド座標をシードとしてオープンします
    /// </summary>
    public void OpenSectionPrevious(Vector3Int branchCoord,Vector3Int dir, Vector3Int[] sectionCoords )
    {
        Vector3Int prevCoord = branchCoord + dir;
        map.OpenSection(prevCoord,sectionCoords);
    }


    private void InitList()
    {
        //sectionStack.Push();
        //SectionTable.T
    }

}
