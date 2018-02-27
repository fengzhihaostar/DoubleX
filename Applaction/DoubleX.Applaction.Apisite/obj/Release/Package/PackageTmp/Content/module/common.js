/*公共模块*/
function Common() {

    var self = this;

    //通知消息

    self.sendVerifyCode = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/common/sendverifycode' }, opt), callback, error);
    }
    self.confirmVerifyCode = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/common/confirmverifycode' }, opt), callback, error);
    }

    return self;
}