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

    /*�p�����[�^*/
    [SerializeField] float locoSpeed;                    // �ړ��X�s�[�h
    [SerializeField] float dashSpeed;
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // ���_�X�s�[�h
    private FPS fps;

    private void Start()
    {
        fps = new FPS(map.map);
    }

    void Update()
    {
        playerCoord = map.gridField.GetGridCoordinate(transform.position);
        playerPrevious = FPS.GetVector3FourDirection(transform.rotation.eulerAngles);

        // FPS���_�ݒ�
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed,dashSpeed);
        fps.CursorLock();
        fps.ClampMoveRange(transform);

        PlayerAction1();
    }

    /// <summary>
    /// �E�N���b�N�����Ƃ��̃A�N�V����
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
    /// �w�肵�������̂ЂƂO�̃O���b�h���W���V�[�h�Ƃ��ăI�[�v�����܂�
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
