using System.Collections.Generic;
using SpiderHelp.ConfigModule;

namespace SpiderDemo.Interfaces
{
    interface ISpider
    {
        /// <summary>
        /// 爬虫启动入口
        /// </summary>
        void Start();

        /// <summary>
        /// 初始化爬虫配置信息
        /// 包括爬虫数据监控信息
        /// </summary>
        void InitSpider();

        /// <summary>
        /// 获取任务类
        /// </summary>
        /// <param name="startNum">起始id</param>
        /// <param name="endNum">结束id</param>
        /// <returns>任务类集合</returns>
        List<TaskUrlConfig> GetTask(int startNum, int endNum);

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="taskUrlConfigs">任务源集合</param>
        void DoTask(List<TaskUrlConfig> taskUrlConfigs);

        /// <summary>
        /// 回收任务
        /// </summary>
        /// <param name="taskUrls">任务源类集合</param>
        /// <param name="addTaskUrls">新增任务集合</param>
        void GcTask(List<TaskUrlConfig> taskUrls, List<TaskUrlConfig> addTaskUrls);

        /// <summary>
        /// 程序错误代码处理
        /// </summary>
        /// <param name="actionEmail">邮件收件人</param>
        /// <param name="spiderTaskName">爬虫任务名</param>
        /// <param name="errorCode">错误代码</param>
        void FixException(string actionEmail, string spiderTaskName, string errorCode);
    }
}
