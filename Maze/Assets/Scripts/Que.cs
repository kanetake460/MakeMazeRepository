using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    /*=====�A���S���Y���֐��ł�=====*/
    // ��Ɏ��Ƃō�����A���S���Y�����Q�l�ɍ쐬
    // 
    public class Que : MonoBehaviour
    {
        public int[] data;
        public int tail = 0;�@// �Ō��
        public int head = 0;   // �擪
        public bool isEmptyQue = true;


        public int size
        {
            
            get
            {
                return tail - head;
            }
        }


        /// <summary>
        /// Que�̏�����
        /// </summary>
        public Que()
        {
             
            tail = 0;
            head = 0;
            isEmptyQue = true;
        }

        /// <summary>
        /// �G���L���[
        /// </summary>
        /// <param name="que">�L���[</param>
        /// <param name="data">���ꂽ���f�[�^</param>
        /// <param name="value">�����l</param>
        public void Enque(int value)
        {
            data[tail] = value;
            tail++;
            if (tail == head)
            {
                Debug.LogError("�f�[�^���I�[�o�[�t���[���܂���");
                Debug.Break();
            }
        }

        /// <summary>
        /// �f�L���[
        /// </summary>
        /// <param name="que">�L���[</param>
        /// <returns>�o�Ă����f�[�^</returns>
        public int Deque()
        {
            int ret = 0;
            if (tail <= head)
            {
                isEmptyQue = true;
                Debug.LogWarning("�f�[�^������܂���");
            }
            else
            {
                isEmptyQue = false;
                ret = data[head];
                head++;
            }
            return ret;
        }

        /// <summary>
        /// �Ō���̒l�������܂�
        /// </summary>
        public void Remove()
        {
            tail--;
        }
    }
}