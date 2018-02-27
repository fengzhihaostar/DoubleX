/*组织机构*/
function Organize() {

    var self = this;

    //职员操作
    self.employeeQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/organize/employeequery' }, opt), callback, error);
    }

    self.employeeRemove = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/organize/employeeremove' }, opt), callback, error);
    }

    self.employeeResetPwd = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/organize/employeeresetpwd' }, opt), callback, error);
    }

    self.employeeLogin = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/organize/employeelogin' }, opt), callback, error);
    }


    //操作示例(暂留，用于拷贝)

    //self.demoGet = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demoget' }, opt), callback, error);
    //}

    //self.demoQuery = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demoquery' }, opt), callback, error);
    //}

    //self.demoAdd = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demoadd' }, opt), callback, error);
    //}

    //self.demoModify = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demomodify' }, opt), callback, error);
    //}

    //self.demoResetPwd = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demoresetpwd' }, opt), callback, error);
    //}

    //self.demoRemove = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demoremove' }, opt), callback, error);
    //}

    //self.demoLogin = function (opt, callback, error) {
    //    mu.ajax($.extend({}, { url: '/api/organize/demologin' }, opt), callback, error);
    //}

    return self;
}