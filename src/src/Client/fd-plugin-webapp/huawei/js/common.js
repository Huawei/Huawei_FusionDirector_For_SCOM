var pluginName = 'webapp';
document.write("<script src='../js/lang.js'></script>");

function langSetting(settingLang) {
    var currentLang = localStorage.getItem('lang');
    if (settingLang == currentLang) {
        console.log("Current lang is correct lang.");
    } else {
        changelang(settingLang);
        console.log("Replace lang using settingLang: " + settingLang + ".");
    }
}
/**
 * 国际化
 **/
function getIn18() {
    var lang = localStorage.getItem('lang');
    if (lang) {
        if (lang == 'en') {
            ELEMENT.locale(ELEMENT.lang.en);
            return i18n_en;
        } else {
            ELEMENT.locale(ELEMENT.lang.zhCN);
            return i18n_zh_CN;

        }
    } else {
        ELEMENT.locale(ELEMENT.lang.zhCN);
        return i18n_zh_CN;
    }
}

/**
 * 改变当前语言
 * @param {string} lang (zhCN,en)
 */
function changelang(lang) {
    if (lang == 'zhCN') {
        ELEMENT.locale(ELEMENT.lang.zhCN);
        localStorage.setItem('lang', 'zhCN');
        this.lang = ELEMENT.lang.zhCN.el.templateManage;
    } else {
        ELEMENT.locale(ELEMENT.lang.en);
        this.lang = ELEMENT.lang.en.el.templateManage;
        localStorage.setItem('lang', 'en');
    }
}
/**
 * 获取禁用启用
 **/
function getForbiddenType() {
    var lang = localStorage.getItem('lang');
    if (lang) {
        if (lang == 'en') {
            return [{
                value: 'Enabled',
                label: 'Enabled'
            }, {
                value: 'Disabled',
                label: 'Disabled'
            }];
        }
    }
    return [{
        value: 'Enabled',
        label: '启用'
    }, {
        value: 'Disabled',
        label: '禁用'
    }];
}
//判断是否空对象 by Jacky on 2017-8-24
function isEmptyObject(obj) {
    if (JSON.stringify(obj) === "{}") {
        return true;
    } else {
        return false;
    }
}

//判断是否空对象并附加默认参数 by Jacky on 2017-8-24
function isEmptyObjectWithDefaultParameter(obj) {
    if (isEmptyObject(obj)) {
        return {
            "defaultParam": ""
        };
    } else {
        return obj;
    }
}

/*
 *功能：异步执行rest.js中跟CefSharp的交互方法（只处理Promise逻辑）
 *参数：ayncResult -> 一个Promise
 *			callback -> 回调函数
 */
function ExecuteAnynsMethodOnlyHandlerPromise(ayncResult, callback) {
    ayncResult.then(function (resultStr) {
        callback(resultStr);
    }).catch((exception) => {
        console.log(exception);
        alert(exception);
    });
}

/*
 *功能：异步执行rest.js中跟CefSharp的交互方法
 *参数：param -> 交互方法的参数
 *			methodName -> 交互方法的名称
 *			isCheckParam -> 是否需要检查是不是空参数
 *			callback -> 回调函数
 */
function ExecuteAnynsMethod(param, methodName, isCheckParam, callback) {
    console.log(param);
    if (isCheckParam) {
        param = isEmptyObjectWithDefaultParameter(param);
    }
    var ayncResult = NetBound.execute(methodName, param);
    ExecuteAnynsMethodOnlyHandlerPromise(ayncResult, callback);
}

/**
 * 根据value值获取下拉列表Label名称
 * @param {string} arry 
 * @param {string} value
 */
function getOptionlabel(arry, value) {
    var optionItem = _.find(arry, function (x) {
        return x.value == value;
    });
    if (optionItem) {
        return optionItem.label;
    }
    return value;
}


/**
 * 获取FD列表
 **/
function getFDList(callback) {
    if (window.NetBound == undefined || window.NetBound == null || !window.NetBound) {
        //判断cefBrowser是否已注册JsObject
        alert('window.NetBound does not exist.');
        console.log('window.NetBound does not exist.');
        return;
    }
    var param = {
        "defaultParam": ""
    };
    ExecuteAnynsMethod(param, "loadFDList", false, (resultStr) => {
        var resultJson = JSON.parse(resultStr);
        console.log("loadFDList result: ");
        console.log(resultJson);
        var fdData = resultJson.data;
        if (typeof callback === "function") {
            var ret = {
                code: resultJson.code,
                data: fdData
            }
            if (resultJson.code == "0") {
                localStorage.setItem('FDList', JSON.stringify(fdData));
            }
            callback(ret);
        }
    });
}


/**
 * 根据FDIp获取别名
 * @param {*} ip 
 */
function getFDaliasName(ip) {
    var FDs = localStorage.getItem('FDList');
    if (FDs) {
        var FDList = JSON.parse(FDs);
        for (var i = 0; i < FDList.length; i++) {
            if (FDList[i].hostIp == ip) {
                if (FDList[i].aliasName) {
                    return FDList[i].aliasName;
                }
                return ip;
            }
        }
    }
    return ip;
}

/**
 * 设置当前FD
 **/
function setCurrentFD(FD) {
    localStorage.setItem('FD', FD);
}

/**
 * 获取当前FD
 **/
function getCurrentFD() {
    return localStorage.getItem('FD');
}

/**
 * 弹出提示框
 * @param {String}  msg 消息内容
 * @param {Function} cb 回调函数
 */
function alertMsg(msg, cb) {
    if (app) {
        app.$alert(msg, app.i18ns.common.prompt, {
            confirmButtonText: app.i18ns.common.confirm,
            callback: function () {
                cb && cb();
            }
        });
    } else {
        alert(msg);
    }
}

/**
 * alertMsg('alert msg')
 * alertMsg('alert msg',function(){console.log('alert finish')})
 * @param {*} msg 
 * @param {*} cb 
 */
function alertMsg(msg, cb) {
    if (app) {
        app.$alert(msg, app.i18ns.common.prompt, {
            confirmButtonText: app.i18ns.common.confirm,
            callback: function () {
                cb && cb();
            }
        });
    } else {
        alert(msg);
    }
}

/**
 * confirm('please confirm').then(()=>{console.log('confirm ok')})
 * confirm('please confirm').then(()=>{console.log('confirm ok')},()=>{console.log('cancel')})
 * @param {*} msg 
 */
function confirm(msg) {
    if (app) {
        return app.$confirm(msg, app.i18ns.common.prompt, {
            confirmButtonText: app.i18ns.common.confirm,
            cancelButtonText: app.i18ns.common.cancel,
            closeOnClickModal: false,
            type: 'warning'
        });
    } else {
        console.error('app is undefind');
    }
}

/**
 * notifySuccess("Success")
 * notifySuccess("Success").then(()=>{console.log('callback')})
 */
function notifySuccess(msg, duration) {
    return new Promise(function (reslove) {
        app.$notify({
            message: msg || 'default msg',
            duration: duration || 2000,
            type: 'success'
        });
        setTimeout(function () {
            reslove && reslove();
        }, duration || 2000);
    });
}
/**
 * notifyInfo("Info")
 * notifyInfo("Info").then(()=>{console.log('callback')})
 */
function notifyInfo(msg, duration) {
    return new Promise(function (reslove) {
        app.$notify({
            message: msg || 'default msg',
            duration: duration || 2000
        });
        setTimeout(function () {
            reslove && reslove();
        }, duration || 2000);
    });
}

/**
 * notifyWarn("Warn")
 * notifyWarn("Warn").then(()=>{console.log('callback')})
 */
function notifyWarn(msg, duration) {
    return new Promise(function (reslove) {
        app.$notify({
            message: msg || 'default msg',
            duration: duration || 2000,
            type: 'Warn'
        });
        setTimeout(function () {
            reslove && reslove();
        }, duration || 2000);
    });
}

/**
 * notifyError("error")
 * notifyError("error").then(()=>{console.log('callback')})
 */
function notifyError(msg, duration) {
    return new Promise(function (reslove) {
        app.$notify.error({
            message: msg || 'default msg',
            duration: duration || 2000
        });
        setTimeout(function () {
            reslove && reslove();
        }, duration || 2000);
    })
}

/**
 * dealResult(ret,function(){})
 * dealResult(ret,function(){},function(){})
 */
function dealResult(ret, callback) {
    if (app && app.fullscreenLoading) {
        app.fullscreenLoading = false;
    }
    if (app && app.loading) {
        app.loading = false;
    }
    if (app && app.loading1) {
        app.loading1 = false;
    }
    if (app && app.loading2) {
        app.loading2 = false;
    }
    if (app && app.loading3) {
        app.loading3 = false;
    }
    if (ret.code == '0' || ret.code.substr(0, 1) == '2') {
        ret.code = '0';
    }
    callback && callback(ret);
}



/**
 * 字符串扩展函数
 */
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        } else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出

                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

/**
 * 服务器健康状态
 * @param {*} status 
 */
function getServerStatusTxt(serverStatus, healthStatus) {
    var lang = localStorage.getItem('lang');
    if (!serverStatus) {
        return lang == 'zhCN' ? "未知" : "Unknown";
    }
    serverStatus = serverStatus.toLowerCase();
    if (serverStatus == "online") {
        return getHealthStatusTxt(healthStatus);
    } else if (serverStatus == "failed") {
        return lang == 'zhCN' ? "失败" : "Failed";
    } else if (serverStatus == "locked") {
        return lang == 'zhCN' ? "锁定中" : "Locked";
    } else {
        return lang == 'zhCN' ? "离线" : "OffLine";
    }
}

/**
 * 设备健康状态
 * @param {*} status 
 */
function getHealthStatusTxt(status) {
    var lang = localStorage.getItem('lang');
    if (!status) {
        return lang == 'zhCN' ? "--" : "--";
    }
    status = status.toLowerCase();
    if (status == "ok") {
        return lang == 'zhCN' ? "正常" : "Health";
    } else if (status == "warning") {
        return lang == 'zhCN' ? "警告" : "Warning";
    } else if (status == "critical") {
        return lang == 'zhCN' ? "紧急" : "Critical";
    } else {
        return lang == 'zhCN' ? "未知" : "Unknown";
    }
}

/**
 * 设备在位状态
 * @param {*} status 
 */
function getPositionStatusTxt(status) {
    var lang = localStorage.getItem('lang');
    if (!status) {
        return "--";
    }
    status = status.toLowerCase();
    if (status == "absent") {
        return lang == 'zhCN' ? "不在位" : "Absent";
    } else {
        return lang == 'zhCN' ? "在位" : "Present";
    }
}

/**
 * 管理板主备关系
 * @param {*} status 
 */
function getActiveStandbyTxt(status) {
    var lang = localStorage.getItem('lang');
    if (!status) {
        return "--";
    }
    status = status.toLowerCase();
    if (status == "standbyspare") {
        return lang == 'zhCN' ? "备板" : "Standby";
    } else if (status == "absent") {
        return "";
    } else {
        return lang == 'zhCN' ? "主板" : "Active";
    }
}

/**
 * 获取URL参数值
 * @param {string} name 参数名称
 */
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

/**
 * 请求FD接口数据
 * @param {*} data 
 * @param {*} callback 
 */
function ajax(data, callback) {
    if (window.NetBound == undefined || window.NetBound == null || !window.NetBound) {
        //判断cefBrowser是否已注册JsObject
        alert('window.NetBound does not exist.');
        console.log('window.NetBound does not exist.');
        return;
    }
    if (data.ips === undefined || data.ips === null || data.ips.length === 0) {
        var ip = getCurrentFD();
        data.ips = [ip]
    }
    if (data !== null && data != undefined && data !== '' && data.data !== null && data.data != undefined && data.data !== '') {

        data.data = JSON.stringify(data.data);
    }
    var ayncResult = NetBound.execute("requestFusionDirectorAPI", data);
    ExecuteAnynsMethodOnlyHandlerPromise(ayncResult, (resultStr) => {
        var resultJson = JSON.parse(resultStr);
        if (typeof callback === "function") {
            var ret = {
                code: resultJson.code,
                msg: resultJson.description,
                data: resultJson.data,
                headers: resultJson.headers
            }
            if (ret.code == '0') {
                sessionStorage.setItem('serverDate', ret.msg);
            }
            dealResult(ret, callback);
        }
    });
}

/**
 * 获取字符串的字节数值
 * @param string 字符串
 */
this.getBytes = function (string) {
    if (string.match(/[^\x00-\xFF]/g)) {
        return string.match(/[^\x00-\xFF]/g).length * 3 + string.length - string.match(/[^\x00-\xFF]/g).length
    } else {
        return string.length
    }
};

/**
 * 计算时间
 * @param d1 开始时间
 * @param d2 结束时间
 */
function timeFn(d1, d2) { //di作为一个变量传进来
    //如果时间格式是正确的，那下面这一步转化时间格式就可以不用了
    var dateBegin = new Date(d1); //将-转化为/，使用new Date
    var dateEnd
    if (d2) {
        dateEnd = new Date(d2);
    } else {
        dateEnd = new Date();
    }
    var dateDiff = dateEnd.getTime() - dateBegin.getTime(); //时间差的毫秒数
    var dayDiff = Math.floor(dateDiff / (24 * 3600 * 1000)); //计算出相差天数
    var leave1 = dateDiff % (24 * 3600 * 1000) //计算天数后剩余的毫秒数
    var hours = Math.floor(leave1 / (3600 * 1000)) //计算出小时数
    //计算相差分钟数
    var leave2 = leave1 % (3600 * 1000) //计算小时数后剩余的毫秒数
    var minutes = Math.floor(leave2 / (60 * 1000)) //计算相差分钟数
    //计算相差秒数
    var leave3 = leave2 % (60 * 1000) //计算分钟数后剩余的毫秒数
    var seconds = Math.round(leave3 / 1000)
    if (dayDiff != 0) {
        return dayDiff + "d" + hours + "h" + minutes + "min" + seconds + "s"
    } else if (hours != 0) {
        return hours + "h" + minutes + "min" + seconds + "s";
    } else if (minutes != 0) {
        return minutes + "min" + seconds + "s";
    } else {
        return seconds + "s";
    }
}
/**
 * 转化时间
 * @param {} time 
 * @param {*} cFormat 
 */
function compatibleTime(time) {
    if (time == null)
        return false;
    time = time.replace(/-/g, "/");
    time = time.replace(/T/g, " ");
    time = time.substr(0, 19);
    return time;
}

function parseTime(time, cFormat) {
    if (arguments.length === 0) {
        return null;
    }
    var format = cFormat || '{y}-{m}-{d} {h}:{i}:{s}';
    var date;
    if (typeof time == 'object') {
        date = time;
    } else {
        if (('' + time).length === 10) time = parseInt(time) * 1000;
        time = compatibleTime(time);
        date = new Date(time);
    }
    var formatObj = {
        y: date.getFullYear(),
        m: date.getMonth() + 1,
        d: date.getDate(),
        h: date.getHours(),
        i: date.getMinutes(),
        s: date.getSeconds(),
        a: date.getDay()
    };
    var time_str = format.replace(/{(y|m|d|h|i|s|a)+}/g, function (result, key) {
        var value = formatObj[key];
        if (key === 'a') return ['一', '二', '三', '四', '五', '六', '日'][value - 1];
        if (result.length > 0 && value < 10) {
            value = '0' + value;
        }
        return value || 0;
    });
    return time_str;
}

var com_huawei_fdvcenterpluginui = {
    webContextPath: 'http://192.168.8.105:8082'
}

function sec_to_time(s) {
    var t = '';
    if (s > -1) {
        var hour = Math.floor(s / 3600);
        var min = Math.floor(s / 60) % 60;
        var sec = s % 60;
        if (hour > 0) {
            t = hour + "h";
        }
        if (min > 0) {
            t += min + "min";
        }
        t += sec + 's';
    }
    return t;
}

/**
 * 转化单位
 * @param {} srcData 
 */
function unitFun(Size) {
    if (parseInt(Size) < 1024) {
        return Size + ' ' + 'B'
    } else if (parseInt(Size) >= 1024 && parseInt(Size) < (1024 * 1024)) {
        return (parseInt(Size) / 1024).toFixed(0) + ' ' + 'KB'
    } else if (parseInt(Size) >= (1024 * 1024) && parseInt(Size) < (1024 * 1024 * 1024)) {
        return (parseInt(Size) / 1024 / 1024).toFixed(0) + ' ' + 'MB'
    } else {
        return (parseInt(Size) / 1024 / 1024 / 1024).toFixed(0) + ' ' + 'GB'
    }
};