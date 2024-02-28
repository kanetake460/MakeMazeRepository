
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /*クラス参照*/
    [SerializeField] MakeMap map;                   // マップ
    [SerializeField] GameManager gameManager;   // ゲームマネージャー
    [SerializeField] UIManager uiManager;
    FPS fps;

    /*パラメータ*/
    [SerializeField] float speed;                            // 移動スピード
    [SerializeField] float dashSpeed;
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // 視点スピード
    private Vector3 _latestPos;

    /*オブジェクト*/
    [SerializeField] GameObject mainCam;            // プレイヤーの視点カメラ

    /*視点*/
    float minX = -90f, maxX = 90f;                  // 角度の制限

    void Start()
    {
        fps = new FPS(map.map);
    }

    void Update()
    {
        // FPS視点設定
        FPS.CameraViewport(mainCam, Xsensityvity, minX, maxX);
        FPS.PlayerViewport(gameObject, Ysensityvity);

        // プレイヤーの移動
        FPS.Locomotion(transform, speed, dashSpeed);
        ClampLocomotionRange();

        // もし、ハンバーガーがあるなら
        if (gameManager.hamburgerCount > 0)
        {
            SpreadMap();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // flagに当たっていたら
        if (other.gameObject.tag == "flag")
        {
            uiManager.EnterDisplayGameMessage("右クリック！",Color.black,10);
            CheckInFlag(other.gameObject);
        }
        // ハンバーガーに当たっていたら
        if (other.gameObject.tag == "hamburger")
        {
            uiManager.EnterDisplayGameMessage("右クリック！", Color.black, 10);

            // ハンバーガーカウントがマックスでなければ
            if (gameManager.hamburgerCount < gameManager.hamburgerNum)
            {
                CheckInHamburger(other.gameObject);
            }
        }
        if (other.gameObject.tag == "clearFlag")
        {
            if (gameManager.clearFlagNum <= gameManager.flags)
            {
                uiManager.EnterDisplayGameMessage("右クリック！", Color.yellow, 10);

                if (Input.GetMouseButton(1))
                {
                    gameManager.clearFlag = true;
                }
            }
        }
        if(other.gameObject.tag == "enemy")
        {
            gameManager.deadCount = 0;
        }
    }


    /// <summary>
    /// プレイヤーのアクションによってマップを広げる
    /// </summary>
    private void SpreadMap()
    {
        // 右クリックしたら
        if (Input.GetMouseButtonDown(0))
        {
            // インスタンス
            if (map.InstanceMapBlock(transform.position, FPS.GetFourDirectionEulerAngles(transform.eulerAngles))
                == true)
            {
                // ハンバーガーを減らす
                gameManager.hamburgerCount--;
            }
        }
    }


    /// <summary>
    /// プライヤーがフィールド外に出れないようにします
    /// </summary>
    private void ClampLocomotionRange()
    {
        // プレイヤーのグリッド座標
        Vector3Int playerGridPos = map.gridField.GetGridCoordinate(transform.position);

        // もし、プレイヤーのポジションがセクションの上でなければ
        if (map.mapElements[playerGridPos.x, playerGridPos.z] == Elements.eElementType.None_Element ||
            map.mapElements[playerGridPos.x, playerGridPos.z] == Elements.eElementType.OutRange_Element)
        {
            transform.position = _latestPos;     // ポジションを一フレーム前の位置に固定
        }
        _latestPos = transform.position;         // ポジションを格納
    }


    /// <summary>
    /// マウスの右クリックでアイテムを回収します
    /// </summary>
    /// <param name="item">回収するアイテム</param>
    private void CheckInFlag(GameObject item)
    {

        if (Input.GetMouseButton(1))
        {
            item.SetActive(false);
            gameManager.flags++;
        }
    }


    /// <summary>
    /// マウスの右クリックでアイテムを回収します
    /// </summary>
    /// <param name="item">回収するアイテム</param>
    private void CheckInHamburger(GameObject item)
    {
        if (Input.GetMouseButton(1))
        {
            item.SetActive(false);
            gameManager.deadCount = 30;
            gameManager.hamburgerCount += 2;
        }
    }


    /// <summary>
    /// マウスの右クリックでアイテムを回収します
    /// </summary>
    /// <param name="item">回収するアイテム</param>
    private void CheckInClearFlag(GameObject item)
    {

    }
}