using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Map map;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Vector3Int roomSizeMin = new Vector3Int();
    [SerializeField] Vector3Int roomSizeMax = new Vector3Int();
    private int roomCount = 0;     // �����̂��߂̃J�E���g
    [SerializeField] int roomNum = 10;  // �������镔���̐�

    /// <summary>
    /// �t���O�𐶐����镔���𐶐����܂�
    /// </summary>
    public void InstanceFlagRoom()
    {
        // �����A�����ł��Ȃ������ꍇ������x�����_���Ȓl�������邽�߃��[�v�����܂�
        while (true)
        {
            // �����_���ȃO���b�h���W
            Vector3Int randomCoord = map.gridField.randomGridCoord;

            if(CheckInstanceRoom(randomCoord,roomSizeMin,roomSizeMax))
            {
                // �����_���Ȉʒu�ɕ����̒��S�𐶐�
                Instantiate(roomPrefab, map.gridField.grid[randomCoord.x, randomCoord.z], Quaternion.identity);

                // ���S����}2�̃O���b�h���W�𕔉��G�������g�ɂ���
                for (int x = roomSizeMin.x; x <= roomSizeMax.x; x++)
                {
                    for (int z = roomSizeMin.z; z <= roomSizeMax.z; z++)
                    {
                        if (map.mapElements[randomCoord.x + x, randomCoord.z + z] == SetElements.eElementType.None_Element)
                        {
                            map.mapElements[randomCoord.x + x, randomCoord.z + z] = SetElements.eElementType.Room_Element;
                        }
                    }
                }

                // ===�f�o�b�O========================================================================================================================
                //Instantiate(roomPrefab, map.gridField.grid[55, 55], Quaternion.identity);
                //for (int x = -1; x <= 1; x++)
                //{
                //    for (int z = -1; z <= 1; z++)
                //    {
                //        map.mapElements[55 + x, 55 + z] = Elements.eElementType.Room_Element;
                //    }
                //}

                // �����_���Ȉʒu�Ő����\�������̂Ń��[�v�𔲂���
                return;
            }
        }
    }

    private bool CheckInstanceRoom(Vector3Int instanceCoord,Vector3Int roomSizeMin,Vector3Int roomSizeMax)
    {
        // �����A�O���b�h�̒[����Ȃ����(�����̐����Ŕz��O�ɂȂ�\�������邽��)
        if (instanceCoord.x <= 3 ||
            instanceCoord.z <= 3 ||
            instanceCoord.x >= map.gridField.gridDepth - 4 ||
            instanceCoord.z >= map.gridField.gridWidth - 4)
        {
            Debug.Log("�͈͊O�ł�");
            return false;
        }

            int trueCount = 0;
        for (int x = roomSizeMin.x; x <= roomSizeMax.x; x++)
        {
            for (int z = roomSizeMin.z; z <= roomSizeMax.z; z++)
            {
                if (map.mapElements[instanceCoord.x + x, instanceCoord.z + z] == SetElements.eElementType.None_Element)
                {
                    trueCount++;
                }
            }
        }
        return trueCount == (roomSizeMax.x - roomSizeMin.x + 1) * (roomSizeMax.z - roomSizeMin.z + 1);
    }
    void Start()
    {


    }



    void Update()
    {
        // �t���O�̃J�E���g���N���A�ɕK�v�ȃt���O�̐��ɂȂ�܂Ń��[�v
        if (roomCount < roomNum)
        {
            InstanceFlagRoom();
            roomCount++;
        }
    }
}
