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

    /// <summary>
    /// �E�N���b�N�����Ƃ��̃A�N�V����
    /// </summary>
    private void PlayerAction1()
    {
        if (Input.GetMouseButtonDown(1))
        {
            InitStack();
            Test(playerCoord, playerPrevious);
        }
    }




    public bool Test(Vector3Int playerCoord, Vector3Int plalyerPrevious)
    {
        Stack<Vector3Int> seedCoord = new Stack<Vector3Int>();
        Stack<Vector3Int[]> sectionCoords = new Stack<Vector3Int[]>();
        
        // �v���C���[�̌����̏����v�b�V��
        seedCoord.Push(playerCoord + plalyerPrevious);
        sectionCoords.Push(sectionQueue.Peek().GetDirectionSection(playerPrevious));
        
        // �`�F�b�N����
        if (map.CheckAbleOpen(seedCoord.Peek(), sectionCoords.Peek()))
        {
            // �S�����������_�������ԂŊi�[�����X�^�b�N���쐬
            Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();

            // �X�^�b�N���󂶂�Ȃ��Ȃ�
            while (randDirStack.Count != 0)
            {
                // �����_���Ȍ����̏����v�b�V��
                Vector3Int randDir = randDirStack.Pop();
                seedCoord.Push(playerCoord + sectionCoords.Peek()[1] + randDir);
                sectionCoords.Push(sectionQueue.ToArray()[1].GetDirectionSection(randDir));

                Debug.Log(seedCoord.Peek());
                Debug.Log(sectionCoords.Peek()[1]);
                Debug.Log(seedCoord.Peek() + sectionCoords.Peek()[1]);
                //Debug.Log(sectionCoords.Peek()[1]);
                // �`�F�b�N����
                if (map.CheckAbleOpen(seedCoord.Peek(),sectionCoords.Peek()))
                {
                    // �S�����������_�������ԂŊi�[�����X�^�b�N��������
                    randDirStack = FPS.RandomVector3DirectionStack();

                    // �X�^�b�N���󂶂�Ȃ��Ȃ�
                    while (randDirStack.Count != 0)
                    {
                        // �����_���Ȍ����̏����v�b�V��
                        randDir = randDirStack.Pop();
                        seedCoord.Push(playerCoord + sectionCoords.Peek()[3] + randDir);
                        sectionCoords.Push(sectionQueue.ToArray()[2].GetDirectionSection(randDir));

                        // �`�F�b�N�O���
                        if(map.CheckAbleOpen(seedCoord.Peek(),sectionCoords.Peek()))
                        {
                            // ���ׂăN���A������A�X�^�b�N���Ă��������_���Ȍ����̏���
                            // �I�[�v�����Ă���
                            map.OpenSection(seedCoord.Pop(),sectionCoords.Pop());
                            map.OpenSection(seedCoord.Pop(),sectionCoords.Pop());
                            map.OpenSection(seedCoord.Pop(),sectionCoords.Pop());
                            return true;
                        }
                        // �O��ڂ̃`�F�b�N�ŃI�[�v���ł��Ȃ�������A�����_���Ȍ����̏����|�b�v
                        seedCoord.Pop();
                        sectionCoords.Pop();
                    }
                }
                // ���ڂ̃`�F�b�N�ŃI�[�v���ł��Ȃ�������A�����_���Ȍ����̏����|�b�v
                seedCoord.Pop();
                sectionCoords.Pop();
            }
        }
        return false;
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


    /// <summary>
    /// �����_���Ȍ����ɃI�[�v�����܂�
    /// </summary>
    /// <param name="branchCoord"></param>
    /// <param name="section"></param>
    /// <returns>false�F�ǂ��ɂ��u���Ȃ�</returns>
    public bool CheckSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    {
        // �S������Vector3Int�̌������������X�^�b�N������
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        while (true)
        {
            // �����A�|�b�v���ĊJ������true
            if (map.CheckAbleOpen(branchCoord, section.GetDirectionSection(randDirStack.Pop())))
            {
                return true;
            }
            // �����A�X�^�b�N���Ȃ��Ȃ�����ǂ̕����ł��u���Ȃ��̂Ńu���[�N����
            if (randDirStack.Count == 0)
                break;
        }
        Debug.Log("�ǂ��ɂ��u���Ȃ�");
        return false;
    }
    
    /// <summary>
    /// �����_���Ȍ����ɃI�[�v�����܂�
    /// </summary>
    /// <param name="branchCoord"></param>
    /// <param name="section"></param>
    /// <returns>false�F�ǂ��ɂ��u���Ȃ�</returns>
    public bool OpenSectionRandmDirection(Vector3Int branchCoord, SectionTable.Section section)
    {
        // �S������Vector3Int�̌������������X�^�b�N������
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        while (true)
        {
            // �����A�|�b�v���ĊJ������true
            if (map.CheckAbleOpen(branchCoord, section.GetDirectionSection(randDirStack.Peek())))
            {
                OpenSectionPrevious(branchCoord,randDirStack.Peek(),section);
                return true;
            }
            randDirStack.Pop();
            // �����A�X�^�b�N���Ȃ��Ȃ�����ǂ̕����ł��u���Ȃ��̂Ńu���[�N����
            if (randDirStack.Count == 0)
                break;
        }
        Debug.Log("�ǂ��ɂ��u���Ȃ�");
        return false;
    }




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

    /// <summary>
    /// sectionStack�̒��g�����Ȃ����āA�V���b�t�����܂�
    /// </summary>
    private void InitStack()
    {
        if (sectionQueue.Count <= 0)
        {
            sectionQueue.Enqueue(SectionTable.T);
            sectionQueue.Enqueue(SectionTable.O);
            sectionQueue.Enqueue(SectionTable.I);
            sectionQueue.Enqueue(SectionTable.L);
            sectionQueue.Enqueue(SectionTable.J);
            sectionQueue.Enqueue(SectionTable.S);
            sectionQueue.Enqueue(SectionTable.Z);

            Algorithm.Shuffle(sectionQueue.ToArray());
        }
    }
}
