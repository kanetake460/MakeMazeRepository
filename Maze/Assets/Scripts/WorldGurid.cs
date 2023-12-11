using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGurid : MonoBehaviour
{
    [SerializeField] float gridBreadth = 30;            // �O���b�h�̍L��
    public Vector3[,] grid = new Vector3[100,100];      // �O���b�h�̃Z���̔z�uVector3�̓񎟌��z��
    [SerializeField] GameObject whiteCellPrefab;        // ���̃Z���̃v���n�u
    [SerializeField] GameObject grayCellPrefab;         // �D�F�̃Z���̃v���n�u
    [SerializeField] GameObject gridParent;             // �O���b�h�̒��S
    void Start()
    {
        gridParent.transform.position = new Vector3((gridBreadth - 1) / 2 * 10, 0, (gridBreadth - 1) / 2 * 10);�@// �O���b�h�t�B�[���h�̒��S�̃I�u�W�F�N�g���t�B�[���h�̐^�񒆂�

        /*===��d���[�v��grid�z��̂��ꂼ���Vector3�̍��W�l����===*/
        for (int x = 0; x < gridBreadth; x++)
        {
            for (int z = 0; z < gridBreadth; z++)
            {
                grid[x, z] = new Vector3(x * 10, 0, z * 10);    // x��z��10���������l����
                InstanceGridField(x, z);                        // �O���b�h�t�B�[���h���C���X�^���X����֐�
            }
        }
    }
    
    /*=====�O���b�h�t�B�[���h���C���X�^���X����=====*/
    // ����:x,z���W�C���f�b�N�X
    public void InstanceGridField(int x, int z)
    {
        if ((x + z) % 2 == 0)       // x��z�̘a�������Ȃ�
        {
            Instantiate(whiteCellPrefab, grid[x, z], Quaternion.identity.normalized, gridParent.transform); // �����Z�����C���X�^���X
        }
        else                        // ��Ȃ�
        {
            Instantiate(grayCellPrefab, grid[x, z], Quaternion.identity.normalized, gridParent.transform);  // �D�F�̃Z�����C���X�^���X
        }
    }

    /*=====�����ɗ^���� Transform ���ǂ��̃Z���ɂ���̂���Ԃ�=====*/
    // ����:Transform
    // �߂�l:������transform�̂���Z���� Vector3
    public Vector3 Grid(Transform pos)
    {
        /*===��d���[�v�Ō��݂̃Z���𒲂ׂ�===*/
        for (int x = 0; x < gridBreadth; x++)
        {
            for (int z = 0; z < gridBreadth; z++)
            {
                if (pos.position.x <= grid[x, z].x + 5 &&
                    pos.position.x >= grid[x, z].x - 5 &&
                    pos.position.z <= grid[x, z].z + 5 &&
                    pos.position.z <= grid[x, z].z + 5)     // ��������Z���̏�ɂ���Ȃ�
                {
                    Debug.Log(grid[x,z]) ;
                    return grid[x, z];                      // �Z���� Vector3��Ԃ�
                }
            }
        }
        Debug.LogError("�^����ꂽ�|�W�V�����̓O���b�h�t�B�[���h�̏�ɂ��܂���B");
        return Vector3.zero;
    }

    void Update()
    {

    }
}
