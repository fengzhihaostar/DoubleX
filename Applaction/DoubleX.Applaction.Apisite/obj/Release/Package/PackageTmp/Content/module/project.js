/*流量*/
function Project() {

    var self = this;

    //流量操作
    self.projectQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/project/projectQuery' }, opt), callback, error);
    }

    self.updateProject = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/project/updateproject' }, opt), callback, error);
    }

    self.insertProject = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/project/insertproject' }, opt), callback, error);
    }
    return self;
}