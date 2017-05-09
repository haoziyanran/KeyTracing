function DrawEntireChartTable(projectList) {
    var index = 0;
    var allRowArray = new Array();
    var allColArray = new Array();

    for(var project in projectList)//each Project
    {
        allRowArray[index] = projectList[index].projectName;
        allColArray[index] = projectList[index].userNumbers;
        index++;
    }

    if(index == 0)
    {
        $("#tfs_allProject_today_chart").append("<h3 class='text-center'>Error: 404 Not Found......Today Data is Empty</h3>")
        return;
    }

    $("#GenerateList").css('display', 'block');

    //For chart click arguments in form
    $("#collectionName").val(projectList[0].collectionName);
    $("#selectProject").val(projectList[0].projectName);

    //第一部分：显示TFS服务器上所有Project的Administrator整体柱状图
    var myChart = new Chart($("#todayChart"), {
    type: 'bar',
    data: {
        labels: allRowArray,
        datasets: [{
            label: "Administrator User Number",
            data: allColArray,
            backgroundColor : 'rgba(153, 102, 255, 0.2)',
            strokeColor: 'rgba(153, 102, 255, 1)'
                  }]
          },
  options:{
        scales: {
            yAxes: [
                        {
                        ticks:{
                                    beginAtZero: true,
                                    max:GetMaxNumber(projectList),
                                    min: 0,
                                    stepSize:1,
                                    backdropColor: 'rgba(153, 102, 255, 1)'
                              }
                        }
                   ],
            xAxes: [
                        {
                        ticks:{
                                    maxRotation: 65,
                                    autoSkip: false
                              }
                        }
                   ]
                },//scales:
       elements:{
              rectangle:{
                            borderWidth: 2,
                            borderColor: 'rgba(153, 102, 255, 20)',
                            borderSkipped: 'bottom',
                        }
                }
          }//options
    });

    //第二部分：显示Collection数量，Project数量，Project Administrator数量
    var numberOverview = document.getElementById("numberOverview");

    var divCol = document.createElement("div");
    divCol.setAttribute("class","col-md-2");
    var colDis = document.createElement("h4");
    colDis.setAttribute("class","navbar-right");
    var txt = document.createTextNode("CollectionNumber:");
    colDis.appendChild(txt);
    divCol.appendChild(colDis);
    numberOverview.appendChild(divCol);

    var divCol2 = document.createElement("div");
    divCol2.setAttribute("class","col-md-1");
    var colNum = document.createElement("h4");
    colNum.setAttribute("class","navbar-left");
    var txt = document.createTextNode(GetCollectionNumber(projectList));
    colNum.appendChild(txt);
    divCol2.appendChild(colNum);
    numberOverview.appendChild(divCol2);

    var divPro = document.createElement("div");
    divPro.setAttribute("class","col-md-offset-1 col-md-2");
    var proDis = document.createElement("h4");
    proDis.setAttribute("class","navbar-right");
    var txt = document.createTextNode("ProjectNumber:");
    proDis.appendChild(txt);
    divPro.appendChild(proDis);
    numberOverview.appendChild(divPro);

    var divPro2 = document.createElement("div");
    divPro2.setAttribute("class","col-md-1");
    var proNum = document.createElement("h4");
    proNum.setAttribute("class","navbar-left");
    var txt = document.createTextNode(projectList.length);
    proNum.appendChild(txt);
    divPro2.appendChild(proNum);
    numberOverview.appendChild(divPro2);

    var divAD = document.createElement("div");
    divAD.setAttribute("class","col-md-offset-1 col-md-2");
    var adDis = document.createElement("h4");
    adDis.setAttribute("class","navbar-right");
    var txt = document.createTextNode("AdministratorNumber:");
    adDis.appendChild(txt);
    divAD.appendChild(adDis);
    numberOverview.appendChild(divAD);

    var divAD2 = document.createElement("div");
    divAD2.setAttribute("class","col-md-1");
    var adNum = document.createElement("h4");
    adNum.setAttribute("class","navbar-left");
    var txt = document.createTextNode(GetAdministratorNumber(projectList));
    adNum.appendChild(txt);
    divAD2.appendChild(adNum);
    numberOverview.appendChild(divAD2);

    //第三部分：显示异常(管理员包含组/管理员数量大于2个)Project列表
    var isCreateTableFlag = 1;
    //Add Table Line
    for (var i = 0; i < projectList.length; i++) {
        if(projectList[i].flag)
        {
            if(isCreateTableFlag)
            {
                //Create Table and head
                var table = document.getElementById("exception_table");

                var head = document.createElement("h3");
                head.setAttribute("class","text-center");
                var txt = document.createTextNode("The Exception Tips:");
                head.appendChild(txt);
                table.appendChild(head);

                var detailTable = document.createElement("table");
                detailTable.setAttribute("class","table table-hover table-responsive");
                table.appendChild(detailTable);

                var tbody = document.createElement("tbody");
                detailTable.appendChild(tbody);
                var head = document.createElement("tr");

                var col1 = document.createElement("th");
                var txt1 = document.createTextNode("Collection");
                col1.appendChild(txt1);
                var col2 = document.createElement("th");
                var txt2 = document.createTextNode("Project");
                col2.appendChild(txt2);
                var col3 = document.createElement("th");
                var txt3 = document.createTextNode("Instruction");
                col3.appendChild(txt3);

                head.appendChild(col1);
                head.appendChild(col2);
                head.appendChild(col3);

                tbody.appendChild(head);

                isCreateTableFlag = 0;
            }

            var tfs = $("#tfs_server_select").val();
            var collectionName = projectList[i].collectionName;
            var projectName = projectList[i].projectName;

            var line = document.createElement("tr");
            line.setAttribute("style","cursor: pointer;");
            line.setAttribute("onclick","window.location = " + "\"/TFSServer/ProjectTrendView?tfsServerName="+ tfs + "&collectionName=" + collectionName + "&selectProject=" + projectName +"\"");

            var col1 = document.createElement("td");
            var txt1 = document.createTextNode(collectionName);
            col1.appendChild(txt1);
            var col2 = document.createElement("td");
            var txt2 = document.createTextNode(projectName);
            col2.appendChild(txt2);

            var flag = projectList[i].flag;
            if(flag == 1)
            {
                var col3 = document.createElement("td");
                col3.setAttribute("class","info");
                var txt3 = document.createTextNode("Include Group Adminisrtator User");
                col3.appendChild(txt3);
            }
            else if(flag == 3)
            {
                var col3 = document.createElement("td");
                col3.setAttribute("class","danger");
                var txt3 = document.createTextNode("Include Group AD User And Administrator Number is More than Two");
                col3.appendChild(txt3);
            }
            else if(flag == 2)
            {
                var col3 = document.createElement("td");
                col3.setAttribute("class","warning");
                var txt3 = document.createTextNode("Administrator Number is More than Two");
                col3.appendChild(txt3);
            }

            line.appendChild(col1);
            line.appendChild(col2);
            line.appendChild(col3);

            tbody.appendChild(line);
        }
    }
}

//Set Chart Column Max Value
function GetMaxNumber(projects) {
    var maxNumber = 0;
    for (var object in projects) {
        var object = projects[object];
        var num = object["userNumbers"];
        if (maxNumber < num)
            maxNumber = num;
    }
    if (maxNumber < 5)
        return 5;
    else
        return maxNumber + 1;
}

//Get CollectionNumber
function GetCollectionNumber(projects)
{
    var collectionNumber = 0;
    var colArray = new Array();
    for (var object in projects) {
        var object = projects[object];
        var collectionName = object["collectionName"];

        if($.inArray(collectionName, colArray) == -1)
        {
            colArray[collectionNumber] = collectionName;
            collectionNumber++;
        }
    }
    return collectionNumber;
}

//Get AdministratorNumber
function GetAdministratorNumber(projects)
{
    var adminNumber = 0;
    var adminArray = new Array();

    for(var object in projects)
    {
        var object = projects[object];
        var adminList = object["administratorUserList"];

        if(adminList != null)
        {
            for (var i = 0; i < adminList.length; i++) {
                if($.inArray(adminList[i], adminArray) == -1)
                {
                    adminArray[adminNumber] = adminList[i];
                    adminNumber++;
                }
            }
        }
    }
    return adminNumber;
}
