$(document).ready(function () {
    //select2 样式风格
    $.fn.select2.defaults.set("theme", "bootstrap");
    $.fn.select2.defaults.set("language", "zh-CN");
    $.fn.select2.defaults.set("ajax--cache", false);

    //选择框CheckBox
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });

    //tip
    $('.tooltip-box').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    });
})

/** 站点操作(apisiteUtil) **/
var au = {};

/** 查询结果数量 **/
au.queryCount = function (result) {
    if (dx.util.isEmpty(result))
        return 0;
    if (dx.util.isEmpty(result["Obj"]))
        return 0;
    if (dx.util.isNullOrEmpty(result["Obj"]["Items"]))
        return 0;
    return result["Obj"]["Items"].length || 0;
}

/** 分页查询参数**/
au.queryPagingData = function (data) {
    if (dx.util.isEmpty(data)) {
        data = {};
    }
    if (!dx.util.isNumber(data["PageIndex"])) {
        data["PageIndex"] = 0;
    }
    if (data["PageIndex"] < 0) {
        data["PageIndex"] = 0;
    }
    if (!dx.util.isNumber(data["PageSize"])) {
        data["PageSize"] = 10;   //0:所有
    }
    return data;
}

/** 分页查询结果**/
au.queryPagingSet = function ($list, len, opt, queryCallback, successCallback) {
    $list.html("<tr><td colspan=\"" + len + "\" class=\"t-l\">正在查询......</td></tr>");
    queryCallback(opt, successCallback);
}
au.queryPagingHtml = function ($list, len, paging, result, callback) {
    if (au.queryCount(result) == 0) {
        $list.html("<tr><td colspan=\"" + len + "\" class=\"t-l\">暂无数据......</td></tr>");
        return;
    }
    callback(result["Obj"]["Items"]);
    au.iCheckRender($list.find(".i-checks"));
    au.pagination($.find(".pagination-box"), result, paging)
}

/** 分页控件设置 **/
au.pagination = function (obj, result, opt) {
    if (dx.util.isEmpty(obj) || dx.util.isEmpty(result))
        return;

    if (dx.util.isEmpty(result["Obj"]) || !dx.util.isObject(result["Obj"]))
        return;

    opt = opt || {};
    var count = (result["Obj"]["Total"] || 0), size = (result["Obj"]["PageSize"] || 0), cur = (result["Obj"]["PageIndex"] || 0);

    var option = {
        pageSize: size,
        curPageIndex: cur,
        num_display_entries: 3,                 //连续分页主体部分显示的分页条目数
        pageItemNum: 2,
        prevText: "上一页",
        nextText: "下一页",
        ellipseText: "...",
        show_if_single_page: true,   //只有一页是否显示
        load_first_page: false
    }
    $(obj).pagination(count, $.extend({}, option, opt));
}

/** Ajax请求操作 **/
au.ajax = function (opt, success, error) {
    dx.util.ajax(opt, function (result, status) {
        var isOk = au.ajaxIsOk(result, error);
        if (!isOk) {
            return false;
        }
        if (dx.util.isFunction(success)) {
            success(result, status);
        }
    }, function (error) {
        console.log(error);
        //dx.util.message(error);
    });
}
au.ajaxIsOk = function (result, error) {
    var msg = "";
    if (!dx.util.isEmpty(result) && result["Code"] != 0) {
        msg = result["Message"] || "";
    }
    if (dx.util.isEmpty(msg)) {;
        return true; //ajax 成功
    }
    if (result["Code"] == 60000006 && !dx.util.isEmpty(result["Redirect"])) {
        window.top.location.href = result["Redirect"];
        return;
    }
    if (!dx.util.isEmpty(msg)) {
        if (dx.util.isFunction(error)) {
            error(result, msg);
        }
        else {
            dx.util.message({ message: msg });
        }
    }
    return false;   //ajax 错误
}

/** 选择框 选中/选值 **/
au.iCheckList = function (obj) {
    var arr = [];
    $(obj).each(function () {
        if (true == $(this).is(':checked')) {
            arr.push($(this));
        }
    });
    return arr;
}
au.iCheckValues = function (obj) {
    var arr = [];
    $(au.iCheckList(obj)).each(function () {
        arr.push($(this).attr("value"));
    })
    return arr;
}
au.iCheckRender = function (obj) {
    $(obj).iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
}


/** 默认日期 操作 **/
au.datetimeDefault = function (obj, op) {
    if (!dx.util.isEmpty(obj)) {
        var option = {
            language: 'zh-CN',
            format: "yyyy-mm-dd",
            autoclose: 1,
            minView: "month",
            todayBtn: true,
            pickTime: true,
            showMeridian: true
        }
        $(obj).datetimepicker(option);
    }
}
/** 区间日期 操作 **/
au.datetimeBetween = function (startObj, endObj) {

    if (util.isEmpty(startObj) || util.isEmpty(endObj))
        return;
    var defaultOpt = {
        format: "yyyy-mm-dd",
        language: "zh-CN",
        autoclose: true,
        minView: "2",
        Number: "2",
        //todayBtn: true,
        todayHighlight: true,
        clearBtn: true,
        linkFormat: "yyyy-mm-dd",
        linkField: null //值反射域文本框的Id
    };
    var $start = $("#" + startObj["id"]), $end = $("#" + endObj["id"]);
    var startOpt = $.extend({}, defaultOpt, (startObj["opt"] || {})), endOpt = $.extend({}, defaultOpt, (endObj["opt"] || {}));
    var $startInput = $start, $endInput = $end;

    if (!util.isEmpty(startOpt["linkField"])) {
        $startInput = $("#" + startOpt["linkField"]);
        $startInput.attr("readonly", "readonly");
        $startInput.css("background", "#ffffff");
    }
    if (!util.isEmpty(endOpt["linkField"])) {
        $endInput = $("#" + endOpt["linkField"]);
        $endInput.attr("readonly", "readonly");
        $endInput.css("background", "#ffffff");
    }

    $start.datetimepicker(startOpt).on("changeDate", function (ev) {
        var startDtStr = $startInput.val() || "",
            endDtStr = $endInput.val() || "";

        var startDt = $start.data("datetimepicker").getUTCDate(),
            endDt = $end.data("datetimepicker").getUTCDate();

        $end.datetimepicker("setStartDate", startDt);

        if (util.isEmpty(endDtStr) || startDt > endDt) {
            var startFormatValue = $start.data("datetimepicker").getFormattedDate();
            $endInput.val(startFormatValue);
            $end.datetimepicker("setUTCDate", startDt);
            $end.datetimepicker("update");
            return;
        }
    }).on("hide", function (ev) {
        var startDt = $start.data("datetimepicker").getUTCDate();
        var endDt = $end.data("datetimepicker").getUTCDate();
        var endDtStr = $endInput.val() || "";
        console.log(endDtStr);
        //结束时间-开始时间 的毫秒数值  如果小于等于1天的毫秒数的负值(少)
        if (!util.isEmpty(endDtStr) && (endDt - startDt) <= (-(1000 * 60 * 60 * 24))) {
            alert("开始时间不能大于结束时间")
            return;
        }
    })

    $end.datetimepicker(endOpt).on("changeDate", function (ev) {
        var startDtStr = $startInput.val() || "",
            endDtStr = $endInput.val() || "";
        var startDt = $start.data("datetimepicker").getUTCDate(),
            endDt = $end.data("datetimepicker").getUTCDate();

        if (util.isEmpty(startDtStr)) {
            var endFormatValue = $end.data("datetimepicker").getFormattedDate();
            $startInput.val(endFormatValue).attr("value", endFormatValue);
            $start.datetimepicker("update");
            $end.datetimepicker("setStartDate", endFormatValue);
            return;
        }
    }).on("hide", function (ev) {
        var startDtStr = $startInput.val() || "",
        endDtStr = $endInput.val() || "";
        console.log("change(hidden):" + $endInput.val());
        console.log("change(hidden):" + endDtStr);

        var startDt = $start.data("datetimepicker").getUTCDate();
        var endDt = $end.data("datetimepicker").getUTCDate();

        //结束时间-开始时间 的毫秒数值  如果小于等于1天的毫秒数的负值(少)
        if (util.isEmpty(endDt) || util.isEmpty(endDtStr)) {
            return;
        }
        if ((endDt - startDt) <= (-(1000 * 60 * 60 * 24))) {
            alert("结束时间不能小于开始时间")
            return;
        }
    })

}


/** 验证正则 **/
au.regMobile = /^1[0|1|2|3|4|5|6|7|8|9]\d{9}$/;
au.regEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
au.validatorRegStr1 = /^[0-9a-zA-Z\_]+$/;
au.validatorRegStr2 = /^[0-9a-zA-Z]+$/;
au.validatorRegStr3 = /^[0-9a-zA-Z\_\@\.]+$/;


/** 表单默认验证 选项 **/
au.validatorOption = function (fields) {
    return {
        message: '这是一个无效的值',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: fields
    };
}
/** 表单默认验证 操作 **/
au.validatorSubmit = function (obj) {
    var formBootstrap = $(obj).data("bootstrapValidator");
    if (formBootstrap) {
        formBootstrap.validate();
        return formBootstrap.isValid();
    }
    return true;
}
/** 表单字段验证 操作 **/
au.validatorField = function (obj, name, rules) {
    rules = rules || [];
    var valdator = $(obj).data("bootstrapValidator");
    var $field = valdator.getFieldElements(name);

    var hasError = false;
    for (var i = 0; i < rules.length; i++) {
        if ($field.data('bv.result.' + rules[i]) != "VALID") {
            hasError = true;
            valdator.updateStatus(name, 'NOT_VALIDATED', null).validateField(name);
            return false;
        }
    }
    return hasError ? false : true;
}
/** 表单默认重置 操作 **/
au.validatorResetForm = function (obj) {
    var formBootstrap = $(obj).data("bootstrapValidator");
    if (formBootstrap) {
        formBootstrap.resetForm();
    }
}
/** 表单字段重置 操作 **/
au.validatorResetField = function (obj,name) {
    var formBootstrap = $(obj).data("bootstrapValidator");
    if (formBootstrap) {
        formBootstrap.resetField(name);
    }
}
