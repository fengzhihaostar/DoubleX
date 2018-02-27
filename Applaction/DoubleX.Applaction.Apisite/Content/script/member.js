﻿$(document).ready(function () {

    //选择框CheckBox
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-blue-small',
        radioClass: 'iradio_square-blue-small',
    });

    // Add slimscroll to element
    $('.content-main-scroll').slimscroll({
        height: '100%'
    })

    //菜单开关事件
    $("#container-menu").metisMenu();
    $(".menu-switch").click(function () {
        var type = $(this).attr("data-for");
        if ($(".container-body").hasClass(type)) {
            $(".container-body").removeClass(type).removeClass("container-full").addClass("container-full")
        } else {
            $(".container-body").removeClass("container-full").addClass(type)
        }
    })
    
    //列表页全选
    $('input.list-chk-all').on('ifChecked', function (event) {
        var $this = $(this);
        $('input[name="' + $(this).attr("name") + '"]').iCheck('check');
    });
    $("input.list-chk-all").on('ifUnchecked', function (event) {
        var $this = $(this);
        $('input[name="' + $(this).attr("name") + '"]').iCheck('uncheck');
    });

    $(document).on("click", "a.disabled",function () {
        return false;
    })
})

/** 用户中心操作(memberUtil) **/
var mu = au || {};

/** 默认信息 **/
mu.listCheckTd = function (id) {
    return "<td class=\"chk-box\"><input type=\"checkbox\" class=\"i-checks\" name=\"list-chk\" value=\"" + (id || "") + "\" /></td>";
}
mu.listEditTd = function (id) {
    return mu.listButtonTd(id, [{ text: "修改", className: "btn-item-modify" }, { text: "删除", className: "btn-item-remove" }]);
}

mu.listButtonTd = function (id, btns) {
    //按钮信息格式
    //[{text:'',className:''}]
    id = id || "";
    btns = btns || [];
    if (!dx.util.isArray(btns))
        return;

    var btnHtml = "";
    btnHtml += "    <td>";
    $(btns).each(function (index, item) {
        btnHtml += "        <a class=\"btn btn-white btn-xs edit-btn " + item.className + "\" data-id=\"" + id + "\" href=\"javascript:;\">" + item.text + "</a>";
    })
    btnHtml += "    </td>";
    return btnHtml;
}
mu.listFormatDate = function (value) {
    return dx.util.formatDate(value, "yyyy-MM-dd");
}
mu.listFormatDateTime = function (value) {
    return dx.util.formatDate(value, "yyyy-MM-dd hh:mm")
}

function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
function guid() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}
