/*!

 @Title: DoubleX Framework
 @Description： DoubleX 前面框架
 @Site: www.DoubleX.com
 @Author: carl
 @License：LGPL
 */
//;分号是为了防止这段代码的前一段javascript代码结尾没有分号，避免运行时发生错误
//！叹号会被当成表达式解析，因此也就调用了该函数，和(function(){})()一样
//(window)立即执行并传入window对象
//ref:https://www.v2ex.com/t/131730
; !function (win) {

    "use strict"; //严格模式 ref:http://www.cnblogs.com/jiqing9006/p/5091491.html

    var dx = function () {
        this.version = "1.0.0(DoubleX)";//版本
        this.instance = {};//实例对象
        this.dom = {}; //底层操作
    }

    dx.fn = dx.prototype;

    //当前页面
    var doc = document;
    var isOpera = typeof opera !== 'undefined' && opera.toString() === '[object Opera]';

    //默认全局配置
    dx.fn.cache = {
        dir: "",      //framework js 目录
        host: "",    //站点host
        modules: {}, //记录模块物理路径
        status: {},  //记录模块加载状态
        timeout: 10, //符合规范的模块请求最长等待秒数
        event: {}    //记录模块自定义事件
    };

    //设置全局配置
    dx.fn.config = function (options) {
        options = options || {};
        for (var key in options) {
            dx.fn.cache[key] = options[key];
        }
        return this;
    };
    win.doublex = win.dx = new dx();
}(window)

//工具类 引用于underscore.js ref:http://www.css88.com/doc/underscore1.8.2/*/
; !function (win) {
    var _util = win._ || {};
    _util.str = win._.string || {};

    /* 获取GUID */
    _util.getGuid=function () {
        // rel:https://github.com/tufanbarisyildirim/
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };


    //格式化字符串
    _util.formatString=function () {
        for (var i = 1; i < arguments.length; i++) {
            var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
            arguments[0] = arguments[0].replace(exp, arguments[i]);
        }
        return arguments[0];
    };

    /* 格式化时间显示方式* 用法:format="yyyy-MM-dd hh:mm:ss"; */
    _util.formatDate=function (v, format) {
        if (!v || v === "0001-01-01 00:00:00") return "";
        if (!format) { format = 'yyyy-MM-dd hh:mm:ss'; }
        var d = v;
        if (typeof v === 'string') {
            if (v.indexOf("/Date(") > -1) {
                d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
            }
            else {
                var newStr = v.replace(/-/g, "/").replace("T", " ");
                if (newStr.indexOf(".") > -1) {
                    newStr = newStr.split(".")[0];
                }
                if (newStr.indexOf("+") > -1) {
                    newStr = newStr.split("+")[0];
                }
                d = new Date(Date.parse(newStr));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
            }
        }
        var o = {
            "M+": d.getMonth() + 1,  //month
            "d+": d.getDate(),       //day
            "h+": d.getHours(),      //hour
            "m+": d.getMinutes(),    //minute
            "s+": d.getSeconds(),    //second
            "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
            "S": d.getMilliseconds() //millisecond
        };
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    };

    /* 格式化数字显示方式  formatNumber(12345.999,'#,##0.00');  formatNumber(12345.999,'#,##0.##');  formatNumber(123,'000000'); */
    _util.formatNumber=function (v, pattern) {
        if (v == null)
            return v;
        var strarr = v ? v.toString().split('.') : ['0'];
        var fmtarr = pattern ? pattern.split('.') : [''];
        var retstr = '';
        // 整数部分   
        var str = strarr[0];
        var fmt = fmtarr[0];
        var i = str.length - 1;
        var comma = false;
        for (var f = fmt.length - 1; f >= 0; f--) {
            switch (fmt.substr(f, 1)) {
                case '#':
                    if (i >= 0) retstr = str.substr(i--, 1) + retstr;
                    break;
                case '0':
                    if (i >= 0) retstr = str.substr(i--, 1) + retstr;
                    else retstr = '0' + retstr;
                    break;
                case ',':
                    comma = true;
                    retstr = ',' + retstr;
                    break;
            }
        }
        if (i >= 0) {
            if (comma) {
                var l = str.length;
                for (; i >= 0; i--) {
                    retstr = str.substr(i, 1) + retstr;
                    if (i > 0 && ((l - i) % 3) == 0) retstr = ',' + retstr;
                }
            }
            else retstr = str.substr(0, i + 1) + retstr;
        }
        retstr = retstr + '.';
        // 处理小数部分   
        str = strarr.length > 1 ? strarr[1] : '';
        fmt = fmtarr.length > 1 ? fmtarr[1] : '';
        i = 0;
        for (var f = 0; f < fmt.length; f++) {
            switch (fmt.substr(f, 1)) {
                case '#':
                    if (i < str.length) retstr += str.substr(i++, 1);
                    break;
                case '0':
                    if (i < str.length) retstr += str.substr(i++, 1);
                    else retstr += '0';
                    break;
            }
        }
        return retstr.replace(/^,+/, '').replace(/\.$/, '');
    };

    /* 格式化金额 */
    _util.formatMoney = function (value) {
        var sign = value < 0 ? '-' : '';
        return sign + _util.formatNumber(Math.abs(value), '#,##0.00');
    };

    /* 格式化百分比 */
    _util.formatPercent=function (value) {
        return (Math.round(value * 10000) / 100).toFixed(2) + '%';
    };


    /** 判断空 **/
    _util.isNullOrEmpty = function (obj) {
        if (_util.isNull(obj))
            return true;
        if (_util.isUndefined(obj))
            return true;
        return _util.isEmpty(obj);
    }

    
    /** Url参数 **/
    _util.urlParamter = function (url, keys, values) {
        keys = keys || "";
        vallues = values || "";

        var keyArr = [];
        if (_util.isArray(keys)) {
            keyArr = keys;
        }
        if (_util.isString(keys)) {
            keyArr = keys.split(',');
        }

        if (_util.isEmpty(keyArr))
            return url;

        var queryStr = url.indexOf('?') > -1 ? url.substr(url.indexOf('?') + 1) : "";
        var queryArr = [];
        if (!_util.isEmpty(queryStr)) {
            var items = queryStr.split("&");
            for (var i = 0; i < items.length; i++) {
                var itemArr = items[i].split('=');
                if (itemArr.length > 0) {
                    queryArr.push({ key: itemArr[0], value: itemArr.length > 1 ? unescape(itemArr[1]) : "" });
                }
            }
        }

        var valueArr = [];
        if (_util.isArray(values)) {
            valueArr = values;
        }
        if (_util.isString(values)) {
            valueArr = values.split(',');
        }

        for (var i = 0; i < keyArr.length; i++) {
            var curValue = valueArr.length > i ? valueArr[i] : "";
            var queryItem = null;
            for (var j = 0; j < queryArr.length; j++) {
                if (queryArr[j]["key"].toLowerCase() == keyArr[i].toLowerCase()) {
                    queryArr[j]["value"] = curValue;
                    queryItem = queryArr[j];
                    break;
                }
            }
            if (queryItem == null) {
                queryArr.push({ key: keyArr[i], value: curValue });
            }
        }


        var domainUrl = url.indexOf('?') > -1 ? url.substr(0, url.indexOf('?')) : url;

        var newQueryStr = "";
        for (var i = 0; i < queryArr.length; i++) {
            newQueryStr += queryArr[i]["key"] + "=" + (queryArr[i]["value"] || "") + "&";
        }
        if (newQueryStr.length > 0) {
            return domainUrl + "?" + newQueryStr.substr(0, newQueryStr.length - 1);
        }
        return domainUrl;
    }

    /* html转义 */
    _util.htmlEncode=function (str, reg) {
        return str ? str.replace(reg || /[&<">'](?:(amp|lt|quot|gt|#39|nbsp|#\d+);)?/g, function (a, b) {
            if (b) {
                return a;
            } else {
                return {
                    '<': '&lt;',
                    '&': '&amp;',
                    '"': '&quot;',
                    '>': '&gt;',
                    "'": '&#39;'
                }[a]
            }

        }) : '';
    };

    /* html转义 */
    _util.htmlDecode = function (str) {
        return str ? str.replace(/&((g|l|quo)t|amp|#39|nbsp);/g, function (m) {
            return {
                '&lt;': '<',
                '&amp;': '&',
                '&quot;': '"',
                '&gt;': '>',
                '&#39;': "'",
                '&nbsp;': ' '
            }[m]
        }) : '';
    };

    /** Ajax请求 **/
    _util.ajax = function (opt, success, error) {
        var options = $.extend({}, {
            type: "POST",
            dataType: "JSON",
            async: true,
            contentType: "application/json; charset=utf-8",
            beforeSend: function (data) { },
            complete: function () { },
            success: function (result) { },
            error: function (error) { }
        }, opt);

        var jsonData = options.data;
        if (_util.isFunction(options.data)) {
            jsonData = options.data();
        }
        if (_util.isObject(jsonData)) {
            jsonData = JSON.stringify(jsonData);
        }
        options.data = jsonData;

        if (_util.isFunction(success)) {
            options.success = success;
        }

        if (_util.isFunction(error)) {
            options.error = error;
        }
        $.ajax(options);
    }


    /** 弹出层(该方法指在用于创建/返回Dialog对象) **/
    _util.dialog = function (opt) {
        //如果第一个参数为string类型,返回对象
        if (_util.isString(opt)) {
            return window.BootstrapDialog.dialogs[opt] || window.top.BootstrapDialog.dialogs[opt];
        }
        var options = $.extend({}, {
            title: '提示',
            message: "",
            style: null,
            loading: false,
            onshow: null,
            className: ""
        }, opt);
        //loading.gif
        //显示时回调(增加居中效果)
        var onshowOldCallback = options.onshow;
        options.onshow = function (dialog) {

            //自定义Class
            if (!_util.isEmpty(options["className"])) {
                dialog.getModalDialog().addClass(options.className);
            }

            //设置默认样式
            var style = $.extend({}, { 'width': '500px', 'margin': '120px auto' }, options.style);
            if (!_util.isEmpty(options["width"])) {
                style["width"] = options["width"];
            }
            dialog.getModalDialog().css(style);

            //设置高度
            if (!_util.isEmpty(options["height"])) {
                if (dialog.getModalBody().find(".bootstrap-dialog-message").length > 0) {
                    $(dialog.getModalBody().find(".bootstrap-dialog-message")).css({ height: options["height"] });
                } else if (dialog.getModalBody().find(".bootstrap-dialog-body").length > 0) {
                    dialog.getModalBody().css({ height: options["height"] });
                } else {
                    dialog.getModalBody().css({ height: options["height"] });
                }
            }

            //显示背景图
            if (options.loading) {
                var loadingBg = {
                    //"background-image": "URL(/content/images/loading.gif)",
                    "background-position": "center",
                    "background-repeat": "no-repeat",
                    "background-attachment": "fixed",
                };
                dialog.getModalBody().css(loadingBg);
            }

            //自定义回调执行
            if (_util.isFunction(onshowOldCallback)) {
                onshowOldCallback(dialog);
            }
        }
        return new BootstrapDialog(options);
    }

    /** 消息框 **/
    _util.message = function (opt, callback) {
        if (arguments.length == 1 && _util.isString(opt)) {
            opt = { message: opt };
        }
        var _dialog = window.top.doublex.util.dialog($.extend({}, {
            title: "提示",
            message: "",
            width: "360px",
            buttons: [{
                label: '确定',
                cssClass: 'btn btn-yes',
                action: function (dialog) {
                    if (_util.isFunction(callback)) {
                        callback(dialog);
                    }
                    dialog.close();
                }
            }]
        }, opt));
        _dialog.open();
        return _dialog;
    }

    /** 确认框 **/
    _util.confirm = function (opt, okCallback, cancelCallback) {
        var _dialog = window.top.doublex.util.dialog($.extend({}, {
            title: "提示",
            message: "",
            width: "380px",
            buttons: [{
                label: '确定',
                cssClass: 'btn btn-yes',
                action: function (dialog) {
                    if (_util.isFunction(okCallback)) {
                        okCallback(dialog);
                    } else {
                        dialog.close();
                    }
                }
            }, {
                label: '取消',
                cssClass: 'btn btn-no',
                action: function (dialog) {
                    if (_util.isFunction(cancelCallback)) {
                        cancelCallback(dialog);
                    }
                    else {
                        dialog.close();
                    }
                }
            }]
        }, opt));
        _dialog.open();
        return _dialog;
    }

    /** 框架页 **/
    _util.frame = function (url, opt) {
        var _dialog = _util.dialog($.extend({}, {
            title: "操作",
            message: "",
            width: "850px",
            height: '100%',
            className: "",
            onhide: function (dialog) {
                var $iframe = $(dialog.getModalBody()).find("iframe");
                if ($iframe.length > 0) {
                    $iframe.attr("src", "");
                    $iframe.remove();
                }
            },
            loading: true,
            backdrop: 'static', //空白处不关闭.
            keyboard: false//esc键盘不关闭.
        }, opt));
        url = _util.urlParamter(url, '_dialog', _dialog.options.id);
        var $iframe = $("<iframe  id=\"_dialog_iframe_" + _dialog.options.id + "\"   name=\"_dialog_iframe_" + _dialog.options.id + "\" style=\"width:100%;height:100%; border:none;padding:0px; margin:0px;\" src=\"" + url + "\"></iframe>");
        //$iframe.load(function ()
        //{ 
        //    console.log($(this).find("body"));
        //}); 
        _dialog.setMessage($iframe);
        _dialog.open();
        return _dialog;
    }


    /** 获取窗体里的成员(方法/属性/..)层级往上找至top **/
    _util.getWindowMember = function (name) {
        if (_util.isEmpty(name))
            return null;

        var isTop = false, win = window, memberObj = null;
        do {
            var nameArr = name.split('.');
            if (nameArr.length > 1) {
                var nameStr = "";
                for (var i = 0; i < nameArr.length; i++) {
                    nameStr += "['" + nameArr[i] + "']";
                }
                try {
                    eval("memberObj = win" + nameStr + ";");
                } catch (e) { }
            } else {
                memberObj = win[name];
            }

            if (_util.isFunction(memberObj)) {
                isTop = true;
            }

            if (win == window.top) {
                isTop = true;
            }
            win = win.parent;
        }
        while (!isTop);
        return memberObj;
    }

    /* 倒计时 */
    _util.countdown = function (opt) {

        var self = new function () { };

        self.id = _util.getGuid();

        var options = $.extend({}, {
            times: 60,
            distance: 1,
            start: null,
            interval: null,
            pause: null,
            stop: null,
            over: null
        }, opt);

        var times = options.times;
        var timesId = null;
        var isPause = false;

        self.start = function () {
            if (_util.isFunction(options.start)) {
                options.start(self, times);
            }
            if (!isPause) {
                times = options.times;  //非暂停开始,默认开始时间
            } else {
                isPause = false;        //暂停开始,开始时将状态改为非暂停
            }
            clearInterval();
            interval();
            return self;
        }

        self.pause = function () {
            if (_util.isFunction(options.pause)) {
                options.pause(self, times);
            }
            if (isPause == true) {
                self.start();
            } else {
                clearInterval();
                isPause = true;
            }
            return self;
        }

        self.stop = function () {
            clearInterval();
            times = 0;
            if (_util.isFunction(options.stop)) {
                options.stop(self, times);
            }
            return self;
        }

        self.clear = function () {
            clearInterval();
        }

        function interval() {
            if (times == 0) {
                clearInterval();
                times = 0;
                if (_util.isFunction(options.over)) {
                    options.over(self, times);
                }
                return;
            }
            if (isPause) {
                return;
            }

            if (_util.isFunction(options.interval)) {
                options.interval(self, times);
            }
            timesId = setTimeout(function () {
                times--;
                interval();
            }, 1000 * options.distance);
        }

        function clearInterval() {
            clearTimeout(timesId); //先清除
            timesId = null;        //再置空(注意顺序先置空后再清除的是空对象)
            isPause = false;
        }

        return self;
    }

    win.doublex.util = win.dx.util = _util;
}(window);