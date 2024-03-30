using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("�p�����[�^�[")]
    [SerializeField] float locoSpeed;                    // �ړ��X�s�[�h
    [SerializeField] float dashSpeed;
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // ���_�X�s�[�h
    private Coord playerCoord;
    private Coord playerPrevious;
    private Stack<SectionTable.Section> _sectionStack1 = new Stack<SectionTable.Section>();
    private Stack<SectionTable.Section> _sectionStack2 = new Stack<SectionTable.Section>();

    [Header("�Q��")]
    [SerializeField] GameObject mainCam;

    [Header("�R���|�[�l���g")]
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] MapManager gameMap;
    private GameObject _triggerObj;
    private FPS fps;

    private void Start()
    {
        fps = new FPS(gameMap.map);
        transform.position = gameMap.StartPos;
    }

    void Update()
    {
        playerCoord = gameMap.gridField.GridCoordinate(transform.position);
        playerPrevious = FPS.GetVector3FourDirection(transform.rotation.eulerAngles);


        // FPS���_�ݒ�
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed,dashSpeed);
        fps.ClampMoveRange(transform);

        PlayerAction0();
        PlayerAction1();

    }

    private void FixedUpdate()
    {
        _triggerObj = null;
    }

    private void OnTriggerStay(Collider other)
    {
        _triggerObj = other.gameObject;

        switch (other.tag)
        {
            case "flag":
                uiManager.EnterDisplayGameMessage("�E�N���b�N�I�I",Color.black,10);
                break;

            case "hamburger":
                uiManager.EnterDisplayGameMessage("�E�N���b�N�I�I", Color.black, 10);

                break;

            case "clearFlag":
                uiManager.EnterDisplayGameMessage("�E�N���b�N�I�I", Color.yellow, 10);

                break;
            case "enemy":
                AudioManager.PlaySEStart("GameOver");
                gameManager.clearFlag = false;
                break;


        }
    }

    /// <summary>
    /// ���N���b�N�����Ƃ��̃A�N�V����
    /// </summary>
    private void PlayerAction0()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.hamburgerCount == 0) return;
            InitStack(_sectionStack1);
            InitStack(_sectionStack2);
            if (OpenBranchingSection(playerCoord, playerPrevious, _sectionStack1.Peek()))
            {
                AudioManager.PlayOneShot("Open");
                gameManager.hamburgerCount--;
                _sectionStack1.Pop();
                // �{�[�_�[���X�g�̃u���b�N������������ꂽ��A�ǂɖ߂�
                if(!gameMap.borderBlockList.All(b => b.isSpace))
                {
                    gameMap.map.CreateWallsSurround();
                }
                gameMap.map.ActiveMapWallObject();
            }
            else
            {
                AudioManager.PlayOneShot("NotOpen");
                uiManager.EnterDisplayGameMessage("�����ł͊J���܂���I�I", Color.red, 10);
            }
            
        }
    }


    /// <summary>
    /// �E�N���b�N�����Ƃ��̃A�N�V����
    /// </summary>
    private void PlayerAction1()
    {
        if(Input.GetMouseButtonDown(1))
        {
            gameManager.CheckInObj(_triggerObj);
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
    private bool OpenBranchingSection(Coord branchCoord, Coord dir,SectionTable.Section section,int branchIndx = 3)
    {
        // ��ڂ��I�[�v���ł��邩�`�F�b�N
        if (!CheckSectionPrevious(branchCoord, dir, section)) 
        {
            return false;
        }
        // ��ڃI�[�v��
        OpenSectionPrevious(branchCoord, dir, section);

        // ��ڂ̃Z�N�V�����̃u�����`���W
        Coord[] branchCoords = section.GetDirectionSection(dir);

        // ��ڂ̎w�肳�ꂽ�u�����`���W���烉���_����
        // ��ڂ̃Z�N�V�������I�[�v��
        if( OpenAround(branchCoords[branchIndx] + branchCoord + dir, _sectionStack2.Peek()))
        {
            _sectionStack2.Pop();
            return true;
        }
        else
        {
            gameMap.CloseSection(branchCoord + dir,branchCoords);
            return false;
        }
    }


    /// <summary>
    /// �w�肵�����W���烉���_���ȕ����ɃZ�N�V�������I�[�v�����܂�
    /// </summary>
    /// <param name="branchCoord">�I�[�v������u�����`���W</param>
    /// <param name="section"></param>
    /// <returns>�I�[�v���ł������ǂ���</returns>
    private bool OpenAround(Coord branchCoord, SectionTable.Section section)
    {
        // �����_���ȕ����̃X�^�b�N
        Stack<Coord> randDirStack = FPS.RandomVector3DirectionStack();

        // �S���̕������������߂�while
        while (true)
        {
            Coord confDir = randDirStack.Pop();
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
    private bool OpenAroundContinue(Coord branchCoord, SectionTable.Section section)
    {
        Stack<Coord> randDirStack = FPS.RandomVector3DirectionStack();
        Debug.Log(randDirStack.Count);

        // �I�[�v���ł��邩�m�F���Ă���
        // �ł��Ȃ������ꍇ�͕�����ς��ă��[�v����
        while (true)
        {
            Coord confDir = randDirStack.Pop();
            
            // �I�[�v���ł��邩�m�F
            if(CheckSectionPrevious(branchCoord,confDir,section ))
            {
                SectionTable.Section rand = SectionTable.RandSection;
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
    public void OpenSectionPrevious(Coord branchCoord, Coord dir, SectionTable.Section section)
    {
        Coord[] sectionCoords = section.GetDirectionSection(dir);
        Coord prevCoord = branchCoord + dir;
        gameMap.OpenSection(prevCoord, sectionCoords);
    }


    /// <summary>
    /// �w�肵�������̂ЂƂO�̃O���b�h���W���V�[�h�Ƃ��āA�Ή����������̃Z�N�V�������I�[�v���ł��邩�m�F���܂�
    /// </summary>
    /// <param name="branchCoord">�I�[�v������Z�N�V�����̈���̍��W</param>
    /// <param name="dir">����</param>
    /// <param name="section">�Z�N�V����</param>
    /// <returns>�I�[�v���ł��邩�ǂ��� true�F�ł���</returns>
    public bool CheckSectionPrevious(Coord branchCoord, Coord dir, SectionTable.Section section)
    {
        Coord[] sectionCoords = section.GetDirectionSection(dir);
        Coord prevCoord = branchCoord + dir;
        return gameMap.CheckAbleOpen(prevCoord,sectionCoords);
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
        }
   �@}
}
