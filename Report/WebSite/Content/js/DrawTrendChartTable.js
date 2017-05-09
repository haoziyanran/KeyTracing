//Get Data
function GetDetailChartTable() {
    var startTime = $("#startTime").val();
    var endTime = $("#endTime").val();
    if(startTime > endTime)
    {
        alert("Time Range Error!");
        return;
    }
    var data = {
        "tfsServerName":$("#tfsServerName").val(),
        "collectionName":$("#collectionName").val(),
        "projectName": $("#projectName").val(),
        "startTime": startTime,
        "endTime": endTime
    }
    $.ajax({
        //Why the url function without Controller Name
        url: "GetProjectDataForChartTable",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (jsonObject) {
            DrawTrendChartTable(jsonObject);
        }
    });
}

//第一部分：显示Project的Administrator数量的趋势图
function DrawTrendChartTable(projectList){
    $("#trendChart").empty();
    $("#table").empty();

    var RowLables = new Array();
    var ColData = new Array();
    for (var i = 0; i < projectList.length; i++) {
        RowLables[i] = projectList[i].time;
        ColData[i] = projectList[i].userNumber;
    }

    var detailChart = document.getElementById("trendChart");
    var chart = document.createElement("canvas");
    detailChart.appendChild(chart);
    var myChart = new Chart(chart, {
    type: 'line',
    data: {
        labels: RowLables,
        datasets: [{
                      label: "Administrator User Change Chart",
                      data: ColData,
                      pointBorderColor: "rgba(153, 102, 255, 255)",
                      pointBorderWidth:5,
                      borderColor: "rgba(153, 102, 255, 1)",
                      borderWidth: 2,
                      tension:0,
                      backgroundColor : 'rgba(153, 102, 255, 0)'
                  }]
          },
 options: {
        scales: {
              xAxes:[
                        {
                        ticks: {
                                  autoSkip: false,
                               }
                        }
                    ],
              yAxes:[
                        {
                        ticks: {   beginAtZero: true,
                                   max:GetMaxNumber(projectList),
                                   min: 0,
                                   stepSize:1
                               },
                        }
                    ]
                },
        bezierCurve :false,
        lineArc:false
          }//options
    });

    //第二部分：显示该Project趋势图的具体Administrator列表
    //Create Table and head
    var table = document.getElementById("table");

    var head = document.createElement("h3");
    head.setAttribute("class","text-center");
    var txt = document.createTextNode("The Detail Information:");
    head.appendChild(txt);
    table.appendChild(head);

    var detailTable = document.createElement("table");
    detailTable.setAttribute("Id","detailTable");
    detailTable.setAttribute("class","table table-bordered table-responsive");
    table.appendChild(detailTable);

    var tbody = document.createElement("tbody");
    detailTable.appendChild(tbody);
    var head = document.createElement("tr");

    var col1 = document.createElement("th");
    var txt1 = document.createTextNode("DayTime");
    col1.appendChild(txt1);
    var col2 = document.createElement("th");
    var txt2 = document.createTextNode("AdministratorUsers");
    col2.appendChild(txt2);
    var col3 = document.createElement("th");
    var txt3 = document.createTextNode("Number");
    col3.appendChild(txt3);


    head.appendChild(col1);
    head.appendChild(col2);
    head.appendChild(col3);

    tbody.appendChild(head);

    //Add Table Line
    for (var i = projectList.length-1; i >= 0; i--) {
        var daytime = projectList[i].time;
        var adminisratorUsers = projectList[i].administratorUserList;
        var number = projectList[i].userNumber;

        var line = document.createElement("tr");

        var col1 = document.createElement("td");
        var txt1 = document.createTextNode(daytime);
        col1.appendChild(txt1);
        var col2 = document.createElement("td");
        var txt2 = document.createTextNode(adminisratorUsers);
        col2.appendChild(txt2);
        var col3 = document.createElement("td");
        var txt3 = document.createTextNode(number);
        col3.appendChild(txt3);

        line.appendChild(col1);
        line.appendChild(col2);
        line.appendChild(col3);

        tbody.appendChild(line);
    }
}

function GetMaxNumber(projects) {
    var maxNumber = 0;
    for (var object in projects) {
        var object = projects[object];
        var num = object["userNumber"];
        if (maxNumber < num)
            maxNumber = num;
    }
    if (maxNumber < 5)
        return 5;
    else
        return maxNumber + 1;
}
