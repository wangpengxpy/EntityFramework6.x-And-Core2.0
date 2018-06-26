(function ($) {
    //分割文件上传成功数量计数从而更新上传进度
    var succeed = 0;
    //上传文件
    function UploadFile(targetFile, id) {
        var fileNamePartToken = ".part_";
        // 创建分割数组
        var fileChunk = [];
        // 取出文件对象（一次仅限上传单个文件）
        var file = targetFile[0];
        // 配置每一次读取文件大小
        var maxFileSizeMB = 3;
        var bufferChunkSize = maxFileSizeMB * (1024 * 1024);
        //文件流读取起始位置
        var fileStreamPos = 0;
        // 设置分割长度
        var endPos = bufferChunkSize;
        //上传文件大小
        var size = file.size;
        // 添加到FileChunk中直到读取到文件结束
        while (fileStreamPos < size) {
            // 分割文件根据the starting position/offset到实际请求长度
            fileChunk.push(file.slice(fileStreamPos, endPos));
            //跳过已写入数量
            fileStreamPos = endPos;
            //设置下一个上传文件长度
            endPos = fileStreamPos + bufferChunkSize;
        }
        // 获取分割上传文件总长度
        var totalParts = fileChunk.length;
        var partCount = 0;
        // 循环遍历上传分割后文件
        while (chunk = fileChunk.shift()) {
            partCount++;
            // 约定文件命名
            var filePartName = file.name + fileNamePartToken + partCount + "." + totalParts;
            // 上传文件
            UploadFileChunk(chunk, filePartName, totalParts, id);
        }
    }
    //上传每一个分割文件
    function UploadFileChunk(chunk, fileName, totalParts, id) {
        var fd = new FormData();
        fd.append('file', chunk, fileName);
        $.ajax({
            type: "POST",
            url: '/book/upload/' + id,
            contentType: false,
            processData: false,
            data: fd,
            success: function (result) {
                succeed++;
                var percentage = ((succeed / totalParts).toFixed(2)) * 100;
                updateProgress(percentage);
            },
            error: function (e) {
                console.log(e);
            }
        });
    }
    //更新上传进度
    function updateProgress(percentage) {
        var $modal = $('.js-loading-bar'),
            $bar = $modal.find('.progress-bar');
        $bar.css('width', percentage + '%');
        $bar.html(parseInt(percentage) + '%');
        if (percentage == 100) {
            setTimeout(function () {
                $bar.removeClass('animate');
                $modal.modal('hide');
            }, 1500);
            $('#uploadFile').val('');
            $('#btnSubmit').removeAttr('disabled');
        }
    }
    $(function () {
        $("#uploadFile").fileinput();
        $('#btnSubmit').click(function () {
            if ($('#uploadFile')[0].files.length <= 0) {
                alert('请上传文件');
                return;
            }
            $(this).prop('disabled', 'disabled');
            var $modal = $('.js-loading-bar'),
                $bar = $modal.find('.progress-bar');
            $modal.modal('show');
            $bar.addClass('animate');
            var id = $('#ID').val();
            UploadFile($('#uploadFile')[0].files, id);
        });
    });
}(jQuery))