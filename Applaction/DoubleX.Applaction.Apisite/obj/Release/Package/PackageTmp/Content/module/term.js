/*流量*/
function Term() {

    var self = this;

    self.termQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/term/termquery' }, opt), callback, error);
    }

    self.updateTerm = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/term/updateterm' }, opt), callback, error);
    }

    self.insertTerm = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/term/insertterm' }, opt), callback, error);
    }

    self.deleteTerm = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/term/removeterm' }, opt), callback, error);
    }

    self.statisticsUserIds = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/term/statisticsuserids' }, opt), callback, error);
    }

    self.termDemo = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/term/termdemo' }, opt), callback, error);
    }
    return self;
}