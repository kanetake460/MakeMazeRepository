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
            InitStack(_sectionStack1);
            InitStack(_sectionStack2);
            if(OpenBranchingSection(playerCoord,playerPrevious, _sectionStack1.Peek(),idx))
                _sectionStack1.Pop();
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
        // ��ڂ��I�[�v���ł��邩�`�F�b�N
        if (!CheckSectionPrevious(branchCoord, dir, section)) 
        {
            Debug.Log("�����ł͊J���܂���ł���");
            return false;
        }
        // ��ڃI�[�v��
        OpenSectionPrevious(branchCoord, dir, section);
        
        // ��ڂ̃Z�N�V�����̃u�����`���W
        Vector3Int[] branchCoords = section.GetDirectionSection(dir);

        // ��ڂ̎w�肳�ꂽ�u�����`���W���烉���_����
        // ��ڂ̃Z�N�V�������I�[�v��
        if( OpenAround(branchCoords[branchIndx] + branchCoord + dir, _sectionStack2.Peek()))
        {
            _sectionStack2.Pop();
            Debug.Log("�I�[�v���I");
            return true;
        }
        else
        {
            Debug.Log("�����ł͊J���܂���ł���");
            map.CloseSection(branchCoord + dir,branchCoords);
            return false;
        }
    }


    /// <summary>
    /// �w�肵�����W���烉���_���ȕ����ɃZ�N�V�������I�[�v�����܂�
    /// </summary>
    /// <param name="branchCoord">�I�[�v������u�����`���W</param>
    /// <param name="section"></param>
    /// <returns>�I�[�v���ł������ǂ���</returns>
    private bool OpenAround(Vector3Int branchCoord, SectionTable.Section section)
    {
        // �����_���ȕ����̃X�^�b�N
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();

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


    /// <summary>
    /// �I�[�v���ł��Ȃ��Ȃ�܂Ń����_���ȕ����ŃZ�N�V�������I�[�v���������܂�
    /// ���ċN�֐�
    /// </summary>
    /// <param name="branchCoord">��ڂ̃Z�N�V�����̃u�����`���W</param>
    /// <param name="section">��ڂ̃Z�N�V����</param>
    /// <returns>�I�[�v���ł������ǂ���</returns>
    private bool OpenAroundContinue(Vector3Int branchCoord, SectionTable.Section section)
    {
        Stack<Vector3Int> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

        // �I�[�v���ł��邩�m�F���Ă���
        // �ł��Ȃ������ꍇ�͕�����ς��ă��[�v����
        while (true)
        {
            Vector3Int confDir = randDirStack.Pop();
            
            // �I�[�v���ł��邩�m�F
            if(CheckSectionPrevious(branchCoord,confDir,section ))
            {
                SectionTable.Section rand = SectionTable.randSection;
                OpenSectionPrevious(branchCoord,confDir,section);
                OpenAroundContinue(branchCoord + section.GetDirectionSection(confDir)[3], rand);
                return true;
            }

            // ���ׂĂ̕������m�F���āA
            // �ǂ̕����ł��I�[�v���ł��Ȃ������ꍇ��false��Ԃ�
            if(randDirStack.Count <= 0) 
            {
                return false;
            }
        }
    }


    /// <summary>
    /// �w�肵�������̂ЂƂO�̃O���b�h���W���V�[�h�Ƃ��đΉ����������̃Z�N�V�����ŃI�[�v�����܂�
    /// </summary>
    /// <param name="branchCoord">�I�[�v������Z�N�V�����̈���̍��W</param>
    /// <param name="dir">����</param>
    /// <param name="section">�Z�N�V����</param>
    public void OpenSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        map.OpenSection(prevCoord, sectionCoords);
    }


    /// <summary>
    /// �w�肵�������̂ЂƂO�̃O���b�h���W���V�[�h�Ƃ��āA�Ή����������̃Z�N�V�������I�[�v���ł��邩�m�F���܂�
    /// </summary>
    /// <param name="branchCoord">�I�[�v������Z�N�V�����̈���̍��W</param>
    /// <param name="dir">����</param>
    /// <param name="section">�Z�N�V����</param>
    /// <returns>�I�[�v���ł��邩�ǂ��� true�F�ł���</returns>
    public bool CheckSectionPrevious(Vector3Int branchCoord, Vector3Int dir, SectionTable.Section section)
    {
        Vector3Int[] sectionCoords = section.GetDirectionSection(dir);
        Vector3Int prevCoord = branchCoord + dir;
        return map.CheckAbleOpen(prevCoord,sectionCoords);
    }

   �@/// <summary>
   �@/// sectionStack�̒��g�����Ȃ����āA�V���b�t�����܂�
   �@/// </summary>
   �@private void InitStack(Stack<SectionTable.Section> stack)
   �@{
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
            Debug.Log("�v�b�V��");
        }
   �@}
}
