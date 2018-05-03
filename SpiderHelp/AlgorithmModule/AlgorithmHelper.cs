#region 版权信息
/* ======================================================================== 
 * 描述信息   
 * 作者：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间：2018/4/28 12:00:03 
 * CLR：4.0.30319.42000 
 * 功能描述：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiderHelp.AlgorithmModule
{
    /// <summary>
    /// 常用算法辅助类
    /// </summary>
    public static class AlgorithmHelper
    {
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="array">需要排序的数组</param>
        public static void BubbleSort(this int[] array)
        {
            for (int i = array.Length - 1; i >= 1; i--)
            {
                var flag = false;
                for (int j = 1; j <= i; j++)
                {
                    if (array[j - 1] > array[j])
                    {
                        var temp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = temp;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="array">需要排序的数组</param>
        /// <param name="low">起始位置</param>
        /// <param name="high">结束位置</param>
        ///<remarks>
        /// 从数列中挑出一个元素（一般都选择第一个），称为 "基准"（pivot），
        /// 重新排序数列，所有元素比基准值小的摆放在基准前面，所有元素比基准值大的摆在基准的后面（相同的数可以到任一边）。
        /// 在这个分区退出之后，该基准就处于数列的中间位置。这个称为分区（partition）操作。
        /// 递归地（recursive）把小于基准值元素的子数列和大于基准值元素的子数列排序。
        /// </remarks>
        public static void QuickSort(this int[] array, int low, int high)
        {
            if (low < high)
            {
                int index = array.Partition(low, high);
                QuickSort(array, low, index - 1);
                QuickSort(array, index + 1, high);
            }
        }

        /// <summary>
        /// 找出快速排序基准位置的索引位置
        /// </summary>
        /// <param name="array">需要排序的数组</param>
        /// <param name="low">起始位置</param>
        /// <param name="high">结束位置</param>
        /// <returns>基准索引位置</returns>
        private static int Partition(this int[] array, int low, int high)
        {
            int i = low;
            int j = high;
            int temp = array[low];

            while (i != j)
            {
                // 先判断右半部分是否有小于temp的数，如果有则交换到array[i]
                while (i < j && temp < array[j])
                {
                    j--;
                }
                if (i < j)
                {
                    array[i++] = array[j];
                }

                // 在判断左半部分是否有大于temp的数，如果有则交换到array[j]
                while (i < j && temp > array[i])
                {
                    i++;
                }
                if (i < j)
                {
                    array[j--] = array[i];
                }
            }
            array[i] = temp;

            return i;
        }

        /// <summary>
        /// 顺序查找
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="key">目标K</param>
        /// <returns>返回目标K在目标数组的索引位置</returns>
        public static int SimpleSearch(this int[] array, int key)
        {
            int result = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == key)
                {
                    result = i + 1;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 二分查找
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="key">目标K</param>
        /// <returns>返回目标K在目标数组的索引位置</returns>
        public static int BinarySearch(this int[] array, int key)
        {
            int low = 0;
            int high = array.Length - 1;

            while (low <= high)
            {
                var mid = (low + high) / 2;
                if (array[mid] > key)
                {
                    high = mid - 1;
                }
                else if (array[mid] < key)
                {
                    low = mid + 1;
                }
                else
                {
                    return mid + 1;
                }
            }
            return -1;
        }
    }
}