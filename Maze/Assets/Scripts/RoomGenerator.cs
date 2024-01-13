using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : GameManager
{

    [SerializeField] GameObject roomPrefab;
    private int flagCount = 0;     // �����̂��߂̃J�E���g

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

            // �����A�O���b�h�̒[����Ȃ����(�����̐����Ŕz��O�ɂȂ�\�������邽��)
            if (randomCoord.x >= 3 &&
                randomCoord.z >= 3 &&
                randomCoord.x <= map.gridField.gridDepth - 4 &&
                randomCoord.z <= map.gridField.gridWidth - 4)
            {
                // �����_���Ȉʒu�ɕ����̒��S�𐶐�
                //Instantiate(flagPrefab, map.gridField.grid[randomCoord.x, randomCoord.z], Quaternion.identity);
                
                // ���S����}2�̃O���b�h���W�𕔉��G�������g�ɂ���
                //for (int x = -1; x <= 1; x++)
                //{
                //    for (int z = -1; z <= 1; z++)
                //    {
                //        map.mapElements[randomCoord.x + x, randomCoord.z + z] = Elements.eElementType.Room_Element;
                //    }
                //}

                /*�f�o�b�O*/
                Instantiate(roomPrefab, map.gridField.grid[55, 55], Quaternion.identity);
                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        map.mapElements[55 + x, 55 + z] = Elements.eElementType.Room_Element;
                    }
                }

                // �����_���Ȉʒu�Ő����\�������̂Ń��[�v�𔲂���
                return;
            }
        }
    }
    void Start()
    {


    }



    void Update()
    {
        // �t���O�̃J�E���g���N���A�ɕK�v�ȃt���O�̐��ɂȂ�܂Ń��[�v
        if (flagCount < clearFlagNum)
        {
            InstanceFlagRoom();
            flagCount++;
        }
    }
}
