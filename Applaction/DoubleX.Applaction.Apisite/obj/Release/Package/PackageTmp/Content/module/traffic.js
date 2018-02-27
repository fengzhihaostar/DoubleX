/*流量*/
function Traffic() {

    var self = this;

    //流量操作
    self.useRecordQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/traffic/useRecordQuery' }, opt), callback, error);
    }
    return self;
}