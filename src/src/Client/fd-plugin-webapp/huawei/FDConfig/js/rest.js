var FDManage = {
    //获取eSight配置列表
    getList: function (param, callback) {
        if (!FDManage.checkCefBrowserConnection()) return;

        //调用C#方法获取数据
        ExecuteAnynsMethod(param, "getList", false, (resultStr) => {
            console.log(resultStr);
            var resultJson = JSON.parse(resultStr);
            console.log("getESightList result:");
            console.log(resultJson);
            var listData = [];
            listData = resultJson.data;

            if (typeof callback === "function") {
                var ret = {
                    code: resultJson.code,
                    msg: resultJson.description,
                    data: listData,
                    totalNum: resultJson.totalNum
                }
                callback(ret);
            }
        });
    },
    //编辑eSight配置
    edit: function (param, callback) {
        if (!FDManage.checkCefBrowserConnection()) return;
        var ayncResult;
        if (param.loginPwd == null || param.loginPwd == undefined || param.loginPwd == "undefined") {
            ayncResult = NetBound.execute("editNoCert", param);
        } else {
            ayncResult = NetBound.execute("edit", param);
        }
        ExecuteAnynsMethodOnlyHandlerPromise(ayncResult, (resultStr) => {
            var resultJson = JSON.parse(resultStr);
            if (typeof callback === "function") {
                var ret = {
                    code: resultJson.code,
                    msg: resultJson.description,
                }
                callback(ret);
            }
        });
    },
    //删除eSight配置（单个或者批量）
    delete: function (param, callback) {
        //删除代码逻辑 根据param.ids
        if (!FDManage.checkCefBrowserConnection()) return;

        ExecuteAnynsMethod(param, "delete", false, (resultStr) => {
            var resultJson = JSON.parse(resultStr);

            if (typeof callback === "function") {
                var ret = {
                    code: resultJson.code,
                    msg: resultJson.description,
                }
                callback(ret);
            }
        });
    },
    //测试eSight配置
    test: function (param, callback) {
        if (!FDManage.checkCefBrowserConnection()) return;

        ExecuteAnynsMethod(param, "test", false, (resultStr) => {
            var resultJson = JSON.parse(resultStr);

            if (typeof callback === "function") {
                var ret = {
                    code: resultJson.code,
                    msg: resultJson.description,
                }
                callback(ret);
            }
        });
    },
    //保存eSight配置
    add: function (param, callback) {
        if (!FDManage.checkCefBrowserConnection()) return;
        var ayncResult;
        ayncResult = NetBound.execute("add", param);
        ExecuteAnynsMethodOnlyHandlerPromise(ayncResult, (resultStr) => {
            var resultJson = JSON.parse(resultStr);

            if (typeof callback === "function") {
                var ret = {
                    code: resultJson.code,
                    msg: resultJson.description,
                }
                callback(ret);
            }
        });
    },

    /**
     * 上传证书
     * @param {*} param 
     * @param {*} callback 
     */
    saveCert:function(param, callback){
        if (!FDManage.checkCefBrowserConnection()) return;
        var ayncResult;
        ayncResult = NetBound.execute("saveCert", param);
        ExecuteAnynsMethodOnlyHandlerPromise(ayncResult, (resultStr) => {
            var resultJson = JSON.parse(resultStr);
            if (typeof callback === "function") {
                var ret = {
                    code: resultJson.code,
                    msg: resultJson.description,
                }
                callback(ret);
            }
        });
    },
    
    /**
     * 获取服务器基本信息
     * @param {*} param 
     * @param {*} callback 
     */
    getFDVersion: function (param, callback) {
        var endpoint = "/rich/Appliance/Version";
        var data = {
            "ips": [param.ip],
            "httpMethod": "GET",
            "endpoint": endpoint,
            "data": null
        }
        ajax(data, callback);
    },
    //测试是否连接上CefBrowser
    checkCefBrowserConnection: function () {
        if (window.NetBound == undefined || window.NetBound == null || !window.NetBound) {
            //判断cefBrowser是否已注册JsObject
            alert('window.NetBound does not exist.');
            console.log('window.NetBound does not exist.');
            return false;
        }
        return true;
    }
}