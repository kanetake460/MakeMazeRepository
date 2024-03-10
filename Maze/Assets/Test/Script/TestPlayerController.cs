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

    [SerializeField] int idx;
    /// <summary>
    /// �E�N���b�N�����Ƃ��̃A�N�V����
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
    /// �v���C���[�̑O�ɃZ�N�V�������I�[�v�����A���̃Z�N�V������
    /// �w�肵���u�����`����A�����_���ȕ����ɃZ�N�V�����𐶐����܂��B
    /// </summary>
    /// <param name="branchCoord">�v���C���[�̍��W</param>
    /// <param name="dir">�v���C���[�̌���</param>
    /// <param name="section">�I�[�v������Z�N�V����</param>
    /// <param name="branchIndx">��ڂ��I�[�v������u�����`�̃C���f�b�N�X</param>
    /// <returns>�I�[�v���ł������ǂ���</returns>
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

        // �S���̕������������߂�while
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


    // ���ċN�֐�
    private bool OpenAroundContinue(Vector3Int seedCoord, SectionTable.Section section)
    {
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

        // �S���̕������������߂�while
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
    /// �w�肵�������̂ЂƂO�̃O���b�h���W���V�[�h�Ƃ��đΉ����������̃Z�N�V�����ŃI�[�v�����܂�
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
    ///// �����_���Ȍ����ɃI�[�v�����܂�
    ///// </summary>
    ///// <param name="branchCoord"></param>
    ///// <param name="section"></param>
    ///// <returns>false�F�ǂ��ɂ��u���Ȃ�</returns>
    //public bool CheckSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    //{
    //    // �S������Vector3Int�̌������������X�^�b�N������
    //    Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
    //    while (true)
    //    {
    //        // �����A�|�b�v���ĊJ������true
    //        if (map.CheckAbleOpen(branchCoord, section))
    //        {
    //            return true;
    //        }
    //        // �����A�X�^�b�N���Ȃ��Ȃ�����ǂ̕����ł��u���Ȃ��̂Ńu���[�N����
    //        if (randDirStack.Count == 0)
    //            break;
    //    }
    //    Debug.Log("�ǂ��ɂ��u���Ȃ�");
    //    return false;
    //}
    
    ///// <summary>
    ///// �����_���Ȍ����ɃI�[�v�����܂�
    ///// </summary>
    ///// <param name="branchCoord"></param>
    ///// <param name="section"></param>
    ///// <returns>false�F�ǂ��ɂ��u���Ȃ�</returns>
    //public bool OpenSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    //{
    //    // �S������Vector3Int�̌������������X�^�b�N������
    //    Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
    //    while (true)
    //    {
    //        // �����A�|�b�v���ĊJ������true
    //        if (map.CheckAbleOpen(branchCoord, section.GetDirectionSection(randDirStack.Peek())))
    //        {
    //            OpenSectionPrevious(branchCoord,randDirStack.Peek(),section);
    //            return true;
    //        }
    //        randDirStack.Pop();
    //        // �����A�X�^�b�N���Ȃ��Ȃ�����ǂ̕����ł��u���Ȃ��̂Ńu���[�N����
    //        if (randDirStack.Count == 0)
    //            break;
    //    }
    //    Debug.Log("�ǂ��ɂ��u���Ȃ�");
    //    return false;
    //}




    //public bool OpenSectionPreviousContinuous(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section, int countinuousNum)
    //{
    //    int randBranch = Random.Range(1, 5);
    //    Vector3Int seedCoord = branchCoord + dir;
    //    Vector3Int[] sectionCoord = section.GetDirectionSection(dir);

    //    for(int i = 0; i < countinuousNum; i++) 
    //    {
    //        // �u���Ȃ��ꍇ��false��Ԃ�
    //        if(!map.CheckAbleOpen(seedCoord,sectionCoord))
    //        {
    //            return false;
    //        }

    //        // �Z�N�V�����A�����A���W��ύX
    //        section = sectionList.ToArray()[i];
    //        seedCoord = section.GetDirectionSection(dir)[randBranch];
    //    }
    //}

    ///// <summary>
    ///// sectionStack�̒��g�����Ȃ����āA�V���b�t�����܂�
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
