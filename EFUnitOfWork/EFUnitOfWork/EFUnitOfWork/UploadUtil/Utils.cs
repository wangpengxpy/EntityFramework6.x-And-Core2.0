using EF.Core.Exceptions;
using EF.Core.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EFUnitOfWork.UploadUtil
{
    public class Utils
    {
        private const string PARTTOKEN = ".part_";
        public Utils()
        {
            FileParts = new List<string>();
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 临时文件夹
        /// </summary>
        public List<string> FileParts { get; set; }

        /// <summary>
        /// 原始文件切割约定 + ".part_N.X" (N = file part number, X = total files)
        /// 枚举所有匹配模式文件合并
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool MergeFile(string fileName, out bool result, out string storeFileName)
        {
            result = false;
            storeFileName = string.Empty;

            // 根据约定匹配模式来获取文件
            var fileNamePartToken = fileName.IndexOf(PARTTOKEN);
            var baseFileName = fileName.Substring(0, fileNamePartToken);
            var trailingTokens = fileName.Substring(fileNamePartToken + PARTTOKEN.Length);

            var fileIndex = 0;
            var fileCount = 0;
            int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out fileIndex);
            int.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out fileCount);

            //获取文件夹中所有匹配模式文件
            var searchPattern = Path.GetFileName(baseFileName) + PARTTOKEN + "*";
            var filesList = FileHelper.GetFiles(Path.GetDirectoryName(fileName), searchPattern);

            //文件未进行安全校验
            if (filesList.Count() == fileCount)
            {
                
                var extensioName = FileHelper.GetExtensionName(baseFileName);
                storeFileName = FileHelper.GetFileNameWithoutExtension(baseFileName) + extensioName;

                // 使用单例模式确保
                if (!MergeFileSingleton.Instance.InUse(baseFileName))
                {
                    MergeFileSingleton.Instance.AddFile(baseFileName);

                    if (FileHelper.Exist(baseFileName))
                    {
                        FileHelper.Delete(baseFileName);
                    }

                    var mergeList = new List<SortedFile>();
                    foreach (var file in filesList)
                    {
                        var sortedFile = new SortedFile
                        {
                            FileName = file
                        };
                        baseFileName = file.Substring(0, file.IndexOf(PARTTOKEN));
                        trailingTokens = file.Substring(file.IndexOf(PARTTOKEN) + PARTTOKEN.Length);
                        int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out fileIndex);
                        sortedFile.FileOrder = fileIndex;
                        mergeList.Add(sortedFile);
                    }

                    // 按照文件命名顺序进行排序开始合并
                    var mergeOrder = mergeList.OrderBy(s => s.FileOrder).ToList();

                    using (var fileStream = new FileStream(baseFileName, FileMode.Create))
                    {
                        try
                        {
                            foreach (var chunk in mergeOrder)
                            {
                                PollyHelper.WaitAndRetry<IOException>(() =>
                                {
                                    using (var fileChunk = new FileStream(chunk.FileName, FileMode.Open))
                                    {
                                        fileChunk.CopyTo(fileStream);
                                    }
                                });
                            }
                        }
                        catch (IOException ex)
                        {
                            //记录异常日志
                            result = false;
                            throw ex;
                        }
                    }
                    result = true;
                    //在单例模式中未锁住文件
                    MergeFileSingleton.Instance.RemoveFile(baseFileName);

                    Parallel.ForEach(mergeList, (d) =>
                    {
                        FileHelper.Delete(d.FileName);
                    });
                }
            }
            return result;
        }
    }
}