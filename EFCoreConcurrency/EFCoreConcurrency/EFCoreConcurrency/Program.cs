using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore.Query;
using EFCore.Model;
using System.Collections;
using System.Collections.Generic;

namespace EFCoreConcurrency
{
    class Program
    {
        static void Main(string[] args)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding("GB2312");

            var ef = new EFCoreContext();
            var blogs = ef.Blogs;
            Blog a = null;
            GetAll(a);
            Console.ReadKey();
        }

        static void GetAll(Blog b)
        {
            Check.NotNull(b, nameof(b));
        }
    }

    public class Questionare
    {
        /// <summary>
        /// 问卷Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 问卷标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 问卷描述
        /// </summary>
        public int Description { get; set; }

        /// <summary>
        /// 问卷图片缩略图
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 问卷开始时间-结束时间（过期作废）
        /// </summary>
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

    }

    public class Question
    {
        /// <summary>
        /// 问卷题目Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 问卷题干描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 问卷题目内容
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 问卷题目是否必填
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 问卷题目类型最大长度（针对文本框）
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 问卷题目验证（主要针对文本框：比如：数字，手机号码、身份证号码、出生日期等）
        /// </summary>
        public byte Validate { get; set; }

        /// <summary>
        /// 问卷题目选项类型（0：文本框，1：单选框，2：复选框，3：下拉框）
        /// </summary>
        public byte AnswerType { get; set; }

    }

    public class Answer
    {
        public int Id { get; set; }
        public int QuestionareId { get; set; }
        public int QuestionId { get; set; }
    }

    public class AnserResult
    {
        /// <summary>
        /// 用户问卷答案结果Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 问卷Id
        /// </summary>
        public int QuestionareId { get; set; }

        /// <summary>
        /// 问卷题目Id
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// 答案Id
        /// </summary>
        public int AnswerId { get; set; }

        /// <summary>
        /// 答题内容
        /// </summary>
        public string Content { get; set; }
    }
}