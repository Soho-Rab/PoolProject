﻿var TableHeight = 0;
var newHeight = 0;
var Ismob = false;
var $tableOne = null;
var $tableTwo = null;
//17U1 L4Kq W1fb ffG2 XJds CN5n Sn2h BsAW NS
$(function () {
    if ($(window).width() < 768) {
        $(".mobbiledelbig").removeClass("input-group-lg");
        Ismob = true;
    }
    var mianheight = 0;
    setTimeout(function () {
        if (!Ismob) {
            mianheight = $(window).height() - $("#home").height() - $("#firstSub").outerHeight() - 10;
            $(".slidermian").height(mianheight).animate({ "opacity": 1 }, 1000);
            TableHeight = mianheight - $("#ShowChart .butSearchDiv").outerHeight() - $("#ShowChart .topmenu").height() - 5;
            $("#JoinText").height(mianheight - $("#JoinTitle").outerHeight() - $("#DownLoadFiles").outerHeight() - 32);
        } else {
            mianheight = $(window).height() - $("#home").height() - $("#firstSub").outerHeight() + 65;
            $(".slidermian").height(mianheight).animate({ "opacity": 1 }, 1000);
            TableHeight = mianheight - $("#ShowChart .butSearchDiv").outerHeight() - $("#ShowChart .topmenu").height() - 5;
            $("#JoinText").height(mianheight - $("#JoinTitle").outerHeight() - $("#DownLoadFiles").outerHeight() - 17);
        }
        $("#ChartList").css("height", TableHeight.toString() + "px");
        newHeight = Ismob ? TableHeight - 13 : TableHeight - 40
        InitMainTable();

        /**/
        var $tableThree = $("#GroupVotes").bootstrapTable({
            url: "/Home/GetGroupVotes?Rnd=" + Math.random() + "",                      //请求后台的URL（*）
            method: 'GET',                      //请求方式（*）
            //toolbar: '#toolbar',              //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页,并记录
            pageSize: 20,                     //每页的记录行数（*）
            pageList: [20, 30, 50, 100],        //可供选择的每页的行数（*）
            search: false,                      //是否显示表格搜索
            strictSearch: true,
            showColumns: false,                  //是否显示所有的列（选择显示的列）
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            height: newHeight,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            //uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            //得到查询的参数
            queryParams: function (params) {
                //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                var temp = {
                    rows: params.limit,                         //页面大小
                    page: (params.offset / params.limit) + 1//,   //页码
                    //sort: params.sort,      //排序列名  
                    //sortOrder: params.order //排位命令（desc，asc） 
                };
                return temp;
            },
            columns: [{
                checkbox: false,
                visible: false                  //是否显示复选框  
            }, {
                field: 'NodeName',
                title: '节点',
                sortable: false,
                formatter: NodeFormatter
            }, {
                field: 'NodeVotes',
                title: Ismob?'票数':'投票币数',
                sortable: true,
                visible:!Ismob
            }, {
                field: 'MakeShareCoins',
                title: '历史分红',
                sortable: true,
                formatter: CoinsFormatter,
                visible:!Ismob
            }, {
                field: 'GetRate',
                title: Ismob ? '收益率' : '收益率‱',
                sortable: true,
                formatter: RateFormatter
            }, {
                field: 'NodeOrderIndex',
                title: Ismob?'排名':'节点排名',
                sortable: true
            }],
            onLoadError: function () {
                showTips("数据加载失败！");
            }
        });
        var $tableFour = $("#All101Nodes").bootstrapTable({
            url: "/Home/Get101Votes?Rnd=" + Math.random() + "",                      //请求后台的URL（*）
            method: 'GET',                      //请求方式（*）
            //toolbar: '#toolbar',              //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页,并记录
            pageSize: 20,                     //每页的记录行数（*）
            pageList: [20, 30, 50, 100],        //可供选择的每页的行数（*）
            search: false,                      //是否显示表格搜索
            strictSearch: true,
            showColumns: false,                  //是否显示所有的列（选择显示的列）
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            height: newHeight,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            //uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            //得到查询的参数
            queryParams: function (params) {
                //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                var temp = {
                    rows: params.limit,                         //页面大小
                    page: (params.offset / params.limit) + 1//,   //页码
                    //sort: params.sort,      //排序列名  
                    //sortOrder: params.order //排位命令（desc，asc） 
                };
                return temp;
            },
            columns: [{
                checkbox: false,
                visible: false                  //是否显示复选框  
            }, {
                field: 'NodeName',
                title: '节点',
                sortable: false,
                formatter: NodeFormatter
            }, {
                field: 'NodeVotes',
                title: '投票数',
                sortable: true,
                formatter: VotesFormatter
            }, {
                field: 'NodeAddress',
                title: '地址',
                sortable: false,
                formatter: UserFormatter
            }],
            onLoadError: function () {
                showTips("数据加载失败！");
            }
        });
        //rvapi.revshowslide(2);
    }, 3000);
    $("#CheckCoins").click(function () {
        var useraddress = $("#YourAddress").val().trim();
        if (useraddress.length != 34) {
            $("#ShowErrMessage").prop("class", "modal fade").addClass("bottom");
            $("#ShowErrMessage .modal-body p").text("地址格式错误！");
            $("#ShowErrMessage").modal("show");
        } else {
            $("#NowCoinsReports").bootstrapTable("refresh", { url: "/Home/GetNowCoins?useraddress=" + useraddress + "&Rnd=" + Math.random() + "" });
            $("#HisCoinsReports").bootstrapTable("refresh", { url: "/Home/GetHisCoins?useraddress=" + useraddress + "&Rnd=" + Math.random() + "" });
            $.getJSON("/Home/GetView?useraddress=" + useraddress + "&Rnd=" + Math.random() + "", function (data) {
                $("#HisCoins").text("历史收益：" + data.HisSumCoins + "");
                $("#NowCoins").text("当前收益：" + data.NowSumCoins + "");
            });
            //$tableOne.refresh({ url: "/Home/GetNowCoins?useraddress=" + useraddress + "&Rnd=" + Math.random() + "" });
        }
    });
    $("#ChartMenu li.clickli").click(function () {
        $(this).siblings().removeClass("activeone");
        $(this).addClass("activeone");
        var totable = parseInt($(this).attr("data-table"));
        $("#ChartListInDiv .TableOutDiv").hide();
        //alert(totable);
        $("#ChartListInDiv .TableOutDiv").eq(totable).show();
    });


    $("#HowToJoin").click(function () {
        rvapi.revnext();
    });
    $("#ToSendCoins").click(function () {
        rvapi.revnext();
    });
    $("#ReturnToHome").click(function () {
        rvapi.revshowslide(0);
    });

    $(".morebutnothing").click(function () {
        $("#ShowErrMessage").prop("class", "modal fade").addClass("bottom");
        $("#ShowErrMessage .modal-body p").text("栏目正在建设！");
        $("#ShowErrMessage").modal("show");
    });
});

function CoinsFormatter(value, row, index) {
    return parseFloat(value).toFixed(6);
}

function RateFormatter(value, row, index) {
    return parseFloat(value).toFixed(2);
}
function HeightFormatter(value, row, index) {
    return "<a href=\"http://explorer.lbtc.io/blockinfo?param=" + value + "\" title=\"单击打开连接\" target=\"_blank\">" + value + "</a>";
}

function DateFormatter(value, row, index) {
    if (Ismob) {
        return new Date(value).format("MM-dd hh:mm:ss");
    } else {
        return new Date(value).format("yyyy-MM-dd hh:mm:ss");
    }
}

function VotesFormatter(value, row, index) {
    if (Ismob) {
        return parseInt(parseFloat(value) / 100000000).toString() + "币";
    } else {
        return value;
    }
}

function NodeFormatter(value, row, index) {
    return "<a href=\"http://explorer.lbtc.io/addrinfo?param=" + row["NodeAddress"] + "\" title=\"单击打开连接\" target=\"_blank\">" + (Ismob ? value.substr(0, 14) : value) + "</a>";
}

function UserFormatter(value, row, index) {
    if (!Ismob) {
        return "<a href=\"http://explorer.lbtc.io/addrinfo?param=" + value+ "\" title=\"单击打开连接\" target=\"_blank\">" + value + "</a>";
    }
    else {
        return "<a href=\"http://explorer.lbtc.io/addrinfo?param=" + value + "\" title=\"单击打开连接\" target=\"_blank\">浏览器</a>";
    } 
}

function HashFormatter(value, row, index) {
    if (!Ismob) {
        return "<a href=\"http://explorer.lbtc.io/transinfo?param=" + value + "\" title=\"单击打开连接\" target=\"_blank\">" + value + "</a>";
    }
    else {
        return "<a href=\"http://explorer.lbtc.io/transinfo?param=" + value + "\" title=\"单击打开连接\" target=\"_blank\">浏览器</a>";
    }
}


//初始化bootstrap-table的内容
function InitMainTable() {
    //记录页面bootstrap-table全局变量$table，方便应用
    $tableOne = $("#NowCoinsReports").bootstrapTable({
        url: "/Home/GetNowCoins?Rnd=" + Math.random() + "",                      //请求后台的URL（*）
        method: 'GET',                      //请求方式（*）
        //toolbar: '#toolbar',              //工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                   //是否显示分页（*）
        sortable: false,                     //是否启用排序
        sortOrder: "asc",                   //排序方式
        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                      //初始化加载第一页，默认第一页,并记录
        pageSize: 20,                     //每页的记录行数（*）
        pageList: [20, 30, 50, 100],        //可供选择的每页的行数（*）
        search: false,                      //是否显示表格搜索
        strictSearch: true,
        showColumns: false,                  //是否显示所有的列（选择显示的列）
        showRefresh: false,                  //是否显示刷新按钮
        minimumCountColumns: 2,             //最少允许的列数
        clickToSelect: false,                //是否启用点击选中行
        height: newHeight,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        //uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
        //showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
        cardView: false,                    //是否显示详细视图
        detailView: false,                  //是否显示父子表
        silent: true,
        //得到查询的参数
        queryParams: function (params) {
            //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            var temp = {
                rows: params.limit,                         //页面大小
                page: (params.offset / params.limit) + 1//,   //页码
                //sort: params.sort,      //排序列名  
                //sortOrder: params.order //排位命令（desc，asc） 
            };
            return temp;
        },
        columns: [{
            checkbox: false,
            visible: false                  //是否显示复选框  
        }, {
            field: 'NodeName',
            title: '节点',
            sortable: false,
            formatter: NodeFormatter
        }, {
            field: 'BlockTime',
            title: '出块时间',
            sortable: false,
            formatter: DateFormatter
        }, {
            field: 'GetCoins',
            title: '出块奖励',
            sortable: false,
            formatter: CoinsFormatter
        }, {
            field: 'BlockHeight',
            title: '块高',
            sortable: false,
            visible: !Ismob,
            formatter: HeightFormatter
        }],
        onLoadError: function () {
            showTips("数据加载失败！");
        }
    });
    //alert(TableHeight);
    $tableTwo = $("#HisCoinsReports").bootstrapTable({
        url: "/Home/GetHisCoins?Rnd=" + Math.random() + "",                      //请求后台的URL（*）
        method: 'GET',                      //请求方式（*）
        //toolbar: '#toolbar',              //工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                   //是否显示分页（*）
        sortable: false,                     //是否启用排序
        sortOrder: "asc",                   //排序方式
        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                      //初始化加载第一页，默认第一页,并记录
        pageSize: 20,                     //每页的记录行数（*）
        pageList: [20, 30, 50, 100],        //可供选择的每页的行数（*）
        search: false,                      //是否显示表格搜索
        strictSearch: true,
        showColumns: false,                  //是否显示所有的列（选择显示的列）
        showRefresh: false,                  //是否显示刷新按钮
        minimumCountColumns: 2,             //最少允许的列数
        clickToSelect: false,                //是否启用点击选中行
        height: newHeight,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        //uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
        //showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
        cardView: false,                    //是否显示详细视图
        detailView: false,                  //是否显示父子表
        //得到查询的参数
        queryParams: function (params) {
            //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            var temp = {
                rows: params.limit,                         //页面大小
                page: (params.offset / params.limit) + 1//,   //页码
                //sort: params.sort,      //排序列名  
                //sortOrder: params.order //排位命令（desc，asc） 
            };
            return temp;
        },
        columns: [{
            checkbox: false,
            visible: false                  //是否显示复选框  
        }, {
            field: 'UserAddress',
            title: '地址',
            sortable: false,
            formatter: UserFormatter
        }, {
            field: 'SetTime',
            title: '分红时间',
            sortable: false,
            formatter: DateFormatter
        }, {
            field: 'GetCoins',
            title: '分红数',
            sortable: false,
            formatter: CoinsFormatter
        }, {
            field: 'TransactionHash',
                title: Ismob ? '交易' : '交易Hash',
            sortable: false,
            visible: !Ismob,
            formatter: HashFormatter
        }],
        onLoadError: function () {
            showTips("数据加载失败！");
        }
    });
};

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
} 