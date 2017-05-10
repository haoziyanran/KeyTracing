    var xhr;
    function CreateWord() {
        var fd = new FormData();
        var fields = fieldList();
        fd.append("fields", fields);
        fd.append("fileToUpload", document.getElementById('fileToUpload').files[0]);

        if (xhr && xhr.readState != 4) {
            xhr.abort();
        }
        xhr = $.ajax({
            url: "/Export/CreateWord",
            type: "POST",
            data: fd,
            contentType: false,
            processData: false,
            beforeSend: function () {

            },
            success: function (data) {
            
            },
            error: function (data) {

            }
        });
    }
    
    function GenerateDetailList() {
        window.location = "/Export/GenerateDetailList";
    }
            
