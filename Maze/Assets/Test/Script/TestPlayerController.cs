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
            OpenSectionPrevious(playerCoord,playerPrevious,Section.I_Top_Branch);
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

    /// <summary>
    /// 向きに対応するひとつ前のグリッド座標を返します
    /// </summary>
    /// <param name="fourDirection">向き</param>
    /// <returns>向いている方向の一つ前のグリッド座標</returns>
    public static Vector3Int GetPreviousCoordinate(FPS.eFourDirection fourDirection)
    {
        switch (fourDirection)
        {
            case FPS.eFourDirection.top:
                return Vector3Int.forward;

            case FPS.eFourDirection.bottom:
                return Vector3Int.back;

            case FPS.eFourDirection.left:
                return Vector3Int.left;

            case FPS.eFourDirection.right:
                return Vector3Int.right;
        }
        return Vector3Int.zero;
    }

}
