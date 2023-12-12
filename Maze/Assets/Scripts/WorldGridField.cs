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
    public GridField gridField;                           // �O���b�h�N���X�̃��[���h�O���b�h�t�B�[���h
    void Start()
    {
        gridField = new GridField(100,100,5,5,-1);
        InstanceGridField(gridField);
    }


/// <summary>
/// �O���b�h�t�B�[���h�ɍ��킹�ăZ���I�u�W�F�N�g��z�u����
/// </summary>
/// <param name="gridField">�O���b�h�t�B�[���h</param>
    public void InstanceGridField(GridField gridField)
    {
        // �O���b�h�t�B�[���h�̒��S�̃I�u�W�F�N�g���t�B�[���h�̐^�񒆂ɐe�̃I�u�W�F�N�g
        //gridParent.transform.position = new Vector3((gridField.gridBreadth - 1) / 2 * gridField.cellWidth, 0, (gridField.gridBreadth - 1) / 2 * gridField.cellDepth);
        gridParent.transform.position = gridField.gridMiddle;
        for (int x = 0; x < gridField.gridWidth; x++)
        {
            for (int z = 0; z < gridField.gridDepth; z++)
            {
                if ((x + z) % 2 == 0)       // x��z�̘a�������Ȃ�
                {
                    Instantiate(whiteCellPrefab, gridField.grid[x, z], Quaternion.identity.normalized, gridParent.transform); // �����Z�����C���X�^���X
                }
                else                        // ��Ȃ�
                {
                    Instantiate(grayCellPrefab, gridField.grid[x, z], Quaternion.identity.normalized, gridParent.transform);  // �D�F�̃Z�����C���X�^���X
                }
            }
        }

    }



    void Update()
    {

    }
}
