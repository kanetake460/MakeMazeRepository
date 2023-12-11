using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;

public class WorldGridField : MonoBehaviour
{

    /*�v���n�u*/
    [SerializeField] GameObject whiteCellPrefab;        // ���̃Z���̃v���n�u
    [SerializeField] GameObject grayCellPrefab;         // �D�F�̃Z���̃v���n�u
    [SerializeField] GameObject gridParent;             // �O���b�h�̒��S

    /*�N���X�Q��*/
    private GridField worldGridField;                           // �O���b�h�N���X�̃��[���h�O���b�h�t�B�[���h
    void Start()
    {
        // �O���b�h�t�B�[���h�̒��S�̃I�u�W�F�N�g���t�B�[���h�̐^�񒆂ɐe�̃I�u�W�F�N�g
        gridParent.transform.position = new Vector3((GridField.gridBreadth - 1) / 2 * 10, 0, (GridField.gridBreadth - 1) / 2 * 10);
        for (int x = 0; x < GridField.gridBreadth; x++)
        {
            for (int z = 0; z < GridField.gridBreadth; z++)
            {
                worldGridField = GridField.AssignValue(0);
                InstanceGridField(x, z);
            }
        }

    }

    /*=====�O���b�h�t�B�[���h���C���X�^���X����=====*/
    // ����:x,z���W�C���f�b�N�X
    public void InstanceGridField(int x, int z)
    {
        if ((x + z) % 2 == 0)       // x��z�̘a�������Ȃ�
        {
            Instantiate(whiteCellPrefab, worldGridField.m_grid[x, z], Quaternion.identity.normalized, gridParent.transform); // �����Z�����C���X�^���X
        }
        else                        // ��Ȃ�
        {
            Instantiate(grayCellPrefab, worldGridField.m_grid[x, z], Quaternion.identity.normalized, gridParent.transform);  // �D�F�̃Z�����C���X�^���X
        }
    }



    void Update()
    {

    }
}
