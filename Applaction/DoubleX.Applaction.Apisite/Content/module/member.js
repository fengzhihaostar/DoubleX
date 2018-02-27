/*会员*/
function Member() {

    var self = this;

    //会员操作

    self.memberGet = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberget' }, opt), callback, error);
    }

    self.memberGetDetail = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/membergetdetail' }, opt), callback, error);
    }

    self.memberQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberquery' }, opt), callback, error);
    }

    self.memberModify = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/membermodify' }, opt), callback, error);
    }

    self.memberRemove = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberremove' }, opt), callback, error);
    }


    self.memberLogin = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberlogin' }, opt), callback, error);
    }

    self.memberRegist = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberregist' }, opt), callback, error);
    }

    self.memberEditPwd = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/membereditpwd' }, opt), callback, error);
    }

    self.memberResetPwd = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberresetpwd' }, opt), callback, error);
    }

    self.memberBindMobile = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberbindmobile' }, opt), callback, error);
    }

    self.memberBindEmail = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberbindemail' }, opt), callback, error);
    }

    self.memberForgetPwd = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/member/memberforgetpwd' }, opt), callback, error);
    }

    return self;
}