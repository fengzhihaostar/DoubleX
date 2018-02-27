/*交易，支付，费用*/

function Trade() {

    var self = this;

    //充值操作
    self.rechargeRecordAdd = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/trade/rechargerecordadd' }, opt), callback, error);
    }

    self.rechargeRecordQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/trade/rechargerecordquery' }, opt), callback, error);
    }

    //费用操作
    self.costRecordQuery = function (opt, callback, error) {
        mu.ajax($.extend({}, { url: '/api/trade/costrecordquery' }, opt), callback, error);
    }

    //支付操作
    self.paymentGo = function (id) {
        dx.util.dialog({ message: $("#recharge-tip").html(), width: "360px", backdrop: 'static', keyboard: false }).open();
        window.open("/payment/toplatform?id=" + id);
    }

    return self;
}