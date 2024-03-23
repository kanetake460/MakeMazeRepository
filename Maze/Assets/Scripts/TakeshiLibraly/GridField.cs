using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace TakeshiLibrary
{
    /*=====グリッドフィールドを作成する関数=====*/
    // Vector3のクラスを参考に作成しました
    // C:\Users\kanet\AppData\Local\Temp\MetadataAsSource\b33e6428b1fe4c03a5b0b222eb1e9f0b\DecompilationMetadataAsSourceFileProvider\4496430b4e32462b86d5e9f4984747a4\Vector3.cs

    [Serializable]
    public struct Coord
    {
        [SerializeField]
        private int m_X;
        [SerializeField]
        private int m_Z;

        public int x
        {
            get => m_X;set => m_X = value;
        }

        public int z
        {
            get => m_Z;set => m_Z = value;
        }


        private static readonly Coord s_Zero = new Coord(0, 0);

        private static readonly Coord s_One = new Coord(1, 1);

        private static readonly Coord s_Left = new Coord(-1, 0);

        private static readonly Coord s_Right = new Coord(1, 0);

        private static readonly Coord s_Forward = new Coord(0, 1);

        private static readonly Coord s_Back = new Coord(0, -1);

        public static Coord zero { get { return s_Zero; } }
        public static Coord one { get { return s_One; } }
        public static Coord left { get { return s_Left; } }
        public static Coord right { get { return s_Right; } }
        public static Coord forward { get { return s_Forward; } }
        public static Coord back { get { return s_Back; } }

        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => m_X,
                    1 => m_Z,
                    _ => throw new IndexOutOfRangeException("Invalid Coord index!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m_X = value;
                        break;
                    case 1:
                        m_Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Coord index!");
                }
            }
        }

        public Coord(int x,int z)
        {
            m_X = x;
            m_Z = z;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.x + b.x, a.z + b.z);
        }

        public static Coord operator -(Coord a, Coord b)
        {
            return new Coord(a.x - b.x, a.z - b.z);
        }

        public static Coord operator *(Coord a, Coord b)
        {
            return new Coord(a.x * b.x, a.z * b.z);
        }

        public static Coord operator -(Coord a)
        {
            return new Coord(-a.x, -a.z);
        }

        public static Coord operator *(Coord a, int b)
        {
            return new Coord(a.x * b, a.z * b);
        }

        public static Coord operator *(int a, Coord b)
        {
            return new Coord(a * b.x, a * b.z);
        }

        public static Coord operator /(Coord a, int b)
        {
            return new Coord(a.x / b, a.z / b);
        }

        public static bool operator ==(Coord lhs, Coord rhs)
        {
            return lhs.x == rhs.x && lhs.z == rhs.z;
        }

        public static bool operator !=(Coord lhs, Coord rhs)
        {
            return !(lhs == rhs);
        }

        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x * x + z * z);
            }
        }

        public static float Distance(Coord a,Coord b)
        {
            return (a + b).magnitude;
        }

    }

    public class GridField
    {


        //======変数===========================================================================================================================

        public int gridWidth { get; }               // グリッドの広さ
        public int gridDepth { get; }               //
        public int gridHeight { get; }
        public float cellWidth { get; }
        public float cellDepth { get; }
        public int y { get; }
        public eGridAnchor gridAnchor { get; }              // グリッドのアンカー
        public Vector3[,] grid { get; } = new Vector3[100, 100];     // グリッドのセルの配置Vector3の二次元配列

        public enum eGridAnchor
        {
            center,
            bottomLeft
        }



        //======読み取り専用変数===============================================================================================================

        /// <summary>
        /// グリッドのセルの数を返します(読み取り専用)
        /// </summary>
        public int totalCell
        {
            get
            {
                return gridWidth * gridDepth;
            }
        }

        /// <summary>
        /// グリッドの幅と奥行の最も長い方を返します。
        /// </summary>
        public int gridMaxLength
        {
            get
            {
                return Mathf.Max(gridWidth, gridDepth);
            }
        }
        
        /// <summary>
        /// グリッドの幅と奥行の最も短い方を返します。
        /// </summary>
        public int gridMinLength
        {
            get
            {
                return Mathf.Min(gridWidth, gridDepth);
            }
        }

        /// <summary>
        /// セルの幅と奥行の最も長い方を返します。
        /// </summary>
        public float cellMaxLength
        {
            get
            {
                return Mathf.Max(cellWidth, cellDepth);
            }
        }
        
        
        /// <summary>
        /// セルの幅と奥行の最も長い方を返します。
        /// </summary>
        public float cellMinLength
        {
            get
            {
                return Mathf.Min(cellWidth, cellDepth);
            }
        }



        /*==========グリッドフィールドの角のセルのVector3座標==========*/
        /// <summary>
        ///グリッドの左下のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 bottomLeftCell
        {
            get
            {
                return grid[0, 0];
            }
        }

        /// <summary>
        ///グリッドの右下のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 bottomRightCell
        {
            get
            {
                return grid[gridWidth - 1, 0];
            }
        }

        /// <summary>
        ///グリッドの左上のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 topLeftCell
        {
            get
            {
                return grid[0, gridDepth - 1];
            }
        }

        /// <summary>
        ///グリッドの右上のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 topRightCell
        {
            get
            {
                return grid[gridWidth - 1, gridDepth - 1];
            }
        }



        /*==========グリッドフィールドの角のVector3座標==========*/
        /// <summary>
        /// グリッドの左下の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 bottomLeft
        {
            get
            {
                return grid[0, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// グリッドの右下の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 bottomRight
        {
            get
            {
                return grid[gridWidth - 1, 0] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// グリッドの左上の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 topLeft
        {
            get
            {
                return grid[0, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);
            }
        }

        /// <summary>
        /// グリッドの右上の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 topRight
        {
            get
            {
                return grid[gridWidth - 1, gridDepth - 1] + new Vector3(cellWidth / 2, y, cellDepth / 2);
            }
        }



        /*=========グリッドフィールドの中心Vector3座標===========*/
        /// <summary>
        /// グリッドの真ん中の localPosition を返します(読み取り専用)
        /// </summary>
        public Vector3 middle
        {
            get
            {
                // 横幅奥行がどちらとも偶数
                if (gridWidth % 2 == 0 && gridDepth % 2 == 0)
                {
                    // グリッド座標からセルの半分の数減らした値を返す
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(cellWidth / 2, 0, cellDepth / 2);

                }
                // 横幅が偶数
                else if (gridWidth % 2 == 0)
                {
                    // グリッド座標からからセルの半分の数を減らした値を返す(横幅のみ)
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(cellWidth / 2, 0, 0);
                }
                // 奥行が偶数
                else if (gridDepth % 2 == 0)
                {
                    // グリッド座標からセルの半分の数を減らした値を返す(奥行のみ)
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(0, 0, cellDepth / 2);
                }
                // どちらとも奇数
                else
                {
                    // グリッド座標を返す
                    return grid[gridWidth / 2, gridDepth / 2];
                }
            }
        }


        /*=========グリッドフィールドの中心Vector3座標===========*/
        /// <summary>
        /// グリッドの真ん中の localPosition を返します(読み取り専用)
        /// </summary>
        public Coord middleGrid
        {
            get
            {
                return new Coord(gridWidth / 2, gridDepth / 2);
            }
        }

        /*=========ランダム===========*/
        /// <summary>
        /// グリッド座標のランダムな位置を返します(読み取り専用)
        /// </summary>
        public Vector3 randomGridPos
        {
            get
            {
                int randX = UnityEngine.Random.Range(0, gridWidth);
                int randZ = UnityEngine.Random.Range(0, gridDepth);
                return grid[randX, randZ];
            }
        }

        /// <summary>
        /// ランダムなグリッド座標を返します(読み取り専用)
        /// </summary>
        public Coord randomGridCoord
        {
            get
            {
                int randX = UnityEngine.Random.Range(0, gridWidth);
                int randZ = UnityEngine.Random.Range(0, gridDepth);
                return new Coord(randX, randZ);
            }
        }

        //======コンストラクタ=================================================================================================================

        /// <summary>
        /// GridFieldを初期化します
        /// </summary>
        /// <param name="gridWidth">グリッドの横幅</param>
        /// <param name="gridDepth">グリッドの奥行</param>
        /// <param name="cellWidth">セルの横幅</param>
        /// <param name="cellDepth">セルの奥行</param>
        /// <param name="y">グリッドのy座標</param>
        /// <param name="gridAnchor">グリッドのアンカー位置</param>
        /// <returns>GridFieldの初期化</returns>
        public GridField(int gridWidth = 10, int gridDepth = 10, float cellWidth = 10, float cellDepth = 10, int y = 0, eGridAnchor gridAnchor = eGridAnchor.center)
        {
            // グリッドの横幅代入
            this.gridWidth = gridWidth;

            // グリッドの奥行代入
            this.gridDepth = gridDepth;

            // セルの横幅代入
            this.cellWidth = cellWidth;

            // セルの奥行を代入
            this.cellDepth = cellDepth;

            // グリッドのアンカー位置を代入
            this.gridAnchor = gridAnchor;

            // グリッドの高さを代入
            this.y = y;

            if (gridWidth > 100 || gridDepth > 100)
            {
                Debug.LogError("安全のため広すぎるグリッドは生成できません");
                Debug.Break();
            }

            /*===二重ループでgrid配列のそれぞれにVector3の座標値を代入===*/
            for (int x = 0; x < gridWidth; x += 1)
            {
                for (int z = 0; z < gridDepth; z += 1)
                {
                    if (gridAnchor == eGridAnchor.center)
                    {
                        grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth) - new Vector3((float)(gridWidth - 1) / 2 * cellWidth, 0, (float)(gridDepth - 1) / 2 * cellDepth);    // xとzに10をかけた値を代入
                    }
                    else if (gridAnchor == eGridAnchor.bottomLeft)
                    {
                        grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth);    // xとzにセルの大きさをかけた値を代入
                    }
                }
            }
        }



        //======関数===========================================================================================================================

        ///<summary>
        ///シーンウィンドウにグリッドを表示します
        ///</summary>
        public void DrowGrid()
        {
            // 中の行
            for (int z = 1; z < gridDepth; z++)
            {
                Vector3 gridLineStart = grid[0, z] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridLineEnd = grid[gridWidth - 1, z] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);

                Debug.DrawLine(gridLineStart, gridLineEnd, Color.red);
            }

            // 中の列
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 gridRowStart = grid[x, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridRowEnd = grid[x, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);

                Debug.DrawLine(gridRowStart, gridRowEnd, Color.red);
            }

            // 端のグリッド線表示
            // 最初の列
            Debug.DrawLine(bottomLeft, topLeft, Color.green);

            // 最後の列
            Debug.DrawLine(bottomRight, topRight, Color.green);


            // 最初の行
            Debug.DrawLine(bottomLeft, bottomRight, Color.green);

            // 最後の行
            Debug.DrawLine(topLeft, topRight, Color.green);
        }


        /// <summary>
        /// 引数に与えた Transform がどこのグリッド座標にいるのかを返す
        /// </summary>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのグリッド座標</returns>
        public Coord GetGridCoordinate(Vector3 pos)
        {
            /*===二重ループで現在のセルを調べる===*/
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (pos.x <= grid[x, z].x + cellWidth / 2 &&
                        pos.x >= grid[x, z].x - cellWidth / 2 &&
                        pos.z <= grid[x, z].z + cellDepth / 2 &&
                        pos.z >= grid[x, z].z - cellDepth / 2)     // もしあるセルの上にいるなら
                    {
                        return new Coord(x, z);                      // セルの Vector3を返す
                    }
                }
            }
            Debug.LogError("与えられたポジションはグリッドフィールドの上にいません。");
            return Coord.zero;
        }


        /// <summary>
        /// 引数に与えた Transform がどこの position なのかを調べます
        /// </summary>
        /// <param name="gridField">調べたいグリッド</param>
        /// <returns></returns>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのposition</returns>
        public Vector3 GetGridPosition(Vector3 pos)
        {
            return grid[GetGridCoordinate(pos).x, GetGridCoordinate(pos).z];
        }


        /// <summary>
        /// 引数に与えた Vector3position を グリッド座標に変換します
        /// </summary>
        /// <param name="pos">変換したいポジション</param>
        public void ConvertVector3ToGridCoord(ref Vector3 pos)
        {
            pos = GetGridPosition(pos);
        }


        /// <summary>
        /// 引数に与えた グリッド座標 から Vector3ポジション を返します
        /// </summary>
        /// <param name="gridCoord">グリッド座標</param>
        /// <returns>Vecto3ポジション</returns>
        public Vector3 GetVector3Position(Coord gridCoord)
        {
            return grid[gridCoord.x, gridCoord.z];
        }

        /// <summary>
        /// 与えたpositionから任意の距離のほかのpositionのグリッド座標を調べます
        /// </summary>
        /// <param name="gridField">調べたいグリッド</param>
        /// <param name="pos">調べたい距離の始点のVector3座標</param>
        /// <param name="difference">始点から終点までの差分</param>
        public Coord GetOtherGridCoordinate(Vector3 pos, Coord difference)
        {
            int x = GetGridCoordinate(pos).x;
            int z = GetGridCoordinate(pos).z;

            return new Coord(x + difference.x, z + difference.z);
        }


        /// <summary>
        /// 与えたpositionから任意の距離のほかのpositionのVector3座標を調べます
        /// </summary>
        /// <param name="pos">調べたい距離の始点のVecgtor3座標</param>
        /// <param name="difference">始点から終点までの差分</param>
        public Vector3 GetOtherGridPosition(Vector3 pos, Coord difference)
        {
            int x = GetGridCoordinate(pos).x;
            int z = GetGridCoordinate(pos).z;

            return grid[x + difference.x, z + difference.z];
        }

        /// <summary>
        /// 向きに対応するひとつ前のグリッド座標を返します
        /// </summary>
        /// <param name="fourDirection">向き</param>
        /// <returns>向いている方向の一つ前のグリッド座標</returns>
        public Coord GetPreviousCoordinate(FPS.eFourDirection fourDirection)
        {
            switch (fourDirection)
            {
                case FPS.eFourDirection.top:
                    return Coord.forward;

                case FPS.eFourDirection.bottom:
                    return Coord.back;

                case FPS.eFourDirection.left:
                    return Coord.left;

                case FPS.eFourDirection.right:
                    return Coord.right;
            }
            return Coord.zero;
        }

        /// <summary>
        /// 与えた posistion がグリッドの上にいるかどうか調べます
        /// </summary>
        /// <param name="pos">調べたいポジション</pragma>
        public bool CheckOnGridPos(Vector3 pos)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (GetGridCoordinate(pos) == GetGridCoordinate(grid[x, z]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 与えたグリッド座標がグリッドの上にいるかどうか調べます
        /// </summary>
        /// <param name="pos">調べたいポジション</pragma>
        public bool CheckOnGridCoord(Vector3Int Coord)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (Coord == new Vector3Int(x,y,z))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            // 中の行
            for (int z = 1; z < gridDepth; z++)
            {
                Vector3 gridLineStart = grid[0, z] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridLineEnd = grid[gridWidth - 1, z] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);

                Gizmos.DrawLine(gridLineStart, gridLineEnd);
            }

            // 中の列
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 gridRowStart = grid[x, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridRowEnd = grid[x, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);

                Gizmos.DrawLine(gridRowStart, gridRowEnd);
            }

            // 端のグリッド線表示
            // 最初の列
            Gizmos.DrawLine(bottomLeft, topLeft);

            // 最後の列
            Gizmos.DrawLine(bottomRight, topRight);


            // 最初の行
            Gizmos.DrawLine(bottomLeft, bottomRight);

            // 最後の行
            Gizmos.DrawLine(topLeft, topRight);

        }
    }
}