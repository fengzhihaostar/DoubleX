//
//Url 操作
//

(function (root) {

    // Baseline setup
    // --------------

    // Establish the root object, `window` in the browser, or `global` on the server.
    var _ = root._ || require('underscore');

    // Helpers
    // -------

    // No reason to create regex more than once
    var plusRegex = /\+/g;
    var spaceRegex = /\%20/g;
    var bracketRegex = /(?:([^\[]+))|(?:\[(.*?)\])/g;

    var urlDecode = function (s) {
        return decodeURIComponent(s.replace(plusRegex, '%20'));
    };
    var urlEncode = function (s) {
        return encodeURIComponent(s).replace(spaceRegex, '+');
    };

    var buildParams = function (prefix, val, top) {
        if (_.isUndefined(top)) top = true;
        if (_.isArray(val)) {
            return _.map(val, function (value, key) {
                return buildParams(top ? key : prefix + '[]', value, false);
            }).join('&');
        } else if (_.isObject(val)) {
            return _.map(val, function (value, key) {
                return buildParams(top ? key : prefix + '[' + key + ']', value, false);
            }).join('&');
        } else {
            return urlEncode(prefix) + '=' + urlEncode(val);
        }
    };

    // Mixing in the string utils
    // ----------------------------

    _.mixin({

        //Url来源页
        UrlReferrer: function () {
            return document.referrer;
        },

        //Url参数操作
        urlParamter: function (url, keys, values) {
            keys = keys || "";
            vallues = values || "";

            var keyArr = [];
            if (_.isArray(keys)) {
                keyArr = keys;
            }
            if (_.isString(keys)) {
                keyArr = keys.split(',');
            }

            if (_.isEmpty(keyArr))
                return url;

            var queryStr = url.indexOf('?') > -1 ? url.substr(url.indexOf('?') + 1) : "";
            var queryArr = [];
            if (!_.isEmpty(queryStr)) {
                var items = queryStr.split("&");
                for (var i = 0; i < items.length; i++) {
                    var itemArr = items[i].split('=');
                    if (itemArr.length > 0) {
                        queryArr.push({ key: itemArr[0], value: itemArr.length > 1 ? unescape(itemArr[1]) : "" });
                    }
                }
            }

            var valueArr = [];
            if (_.isArray(values)) {
                valueArr = values;
            }
            if (_.isString(values)) {
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
        },


        //格式化字符串
        formatString: function () {
            for (var i = 1; i < arguments.length; i++) {
                var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
                arguments[0] = arguments[0].replace(exp, arguments[i]);
            }
            return arguments[0];
        },

        /* 格式化时间显示方式* 用法:format="yyyy-MM-dd hh:mm:ss"; */
        formatDate: function (v, format) {
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
        },

        /* 格式化数字显示方式  formatNumber(12345.999,'#,##0.00');  formatNumber(12345.999,'#,##0.##');  formatNumber(123,'000000'); */
        formatNumber: function (v, pattern) {
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
        },

        /* 格式化金额 */
        formatMoney: function (value) {
            var sign = value < 0 ? '-' : '';
            //return sign + utils.formatNumber(Math.abs(value), '#,##0.00');
        },

        /* 格式化百分比 */
        formatPercent: function (value) {
            return (Math.round(value * 10000) / 100).toFixed(2) + '%';
        },


        /* html转义 */
        htmlEncode: function (str, reg) {
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
        },

        /* html转义 */
        htmlDecode: function (str) {
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
        },


        /* 获取GUID */
        getGuid: function () {
            // rel:https://github.com/tufanbarisyildirim/
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        },

        /* 获取窗体里的成员(方法/属性/..) */
        getWindowMember: function (name) {
            if (_.isEmpty(name))
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

                if (_.isFunction(memberObj)) {
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

    });
})(this);