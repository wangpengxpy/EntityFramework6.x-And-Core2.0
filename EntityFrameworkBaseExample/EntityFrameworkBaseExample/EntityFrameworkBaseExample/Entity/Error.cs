using System;

namespace EntityFrameworkBaseExample.Entity
{
    public class Error
    {
        /// <summary>
        /// 错误信息主键
        /// </summary>
        public int ErrorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public decimal TotalSeconds { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 内部异常信息
        /// </summary>
        public string InnerException { get; set; }

        /// <summary>
        /// 请求Id
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; }
    }
}