(function ($) {
    $.fn.bootstrapValidator.i18n.wordsLength = $.extend($.fn.bootstrapValidator.i18n.wordsLength || {}, {
        'default': 'Please enter a value with valid length',
        less: 'Please enter less than %s words',
        more: 'Please enter more than %s words',
        between: 'Please enter value between %s and %s words long'
    });

    $.fn.bootstrapValidator.validators.wordsLength = {
        html5Attributes: {
            message: 'message',
            min: 'min',
            max: 'max',
            trim: 'trim',
            langue: 'zh-cn'
        },

        enableByHtml5: function ($field) {
            var options = {},
                maxLength = $field.attr('maxlength'),
                minLength = $field.attr('minlength'),
            langueValue = $field.attr("langue");

            if (maxLength) {
                options.max = parseInt(maxLength, 10);
            }
            if (minLength) {
                options.min = parseInt(minLength, 10);
            }
            if (langueValue) {
                options.langue = langueValue;
            }
            return $.isEmptyObject(options) ? false : options;
        },

        /**
         * 字数校验
         *(langue=='en-us' 英文环境判断单词(空格分割 去重复)个数)
         *(langue!='en-us' 中文环境判断字个数(英文1,中文2))
         */
        validate: function (validator, $field, options) {
            var value = $field.val();
            if (options.trim === true || options.trim === 'true') {
                value = $.trim(value);
                if (value === '') {
                    return true;
                }
            }
            else {
                if ($.trim(value)==='') {
                    return true;
                }
            }
            var min = $.isNumeric(options.min) ? options.min : validator.getDynamicOption($field, options.min),
                max = $.isNumeric(options.max) ? options.max : validator.getDynamicOption($field, options.max),
                length = 0,
                isValid = true,
                message = options.message || $.fn.bootstrapValidator.i18n.wordsLength['default'];

            var str = value;
            if (options.langue == "en-us") {
                str = str || "";
                str = str.replace(/(^\s*)|(\s*$)/g, ""); //去前后空格
                while (str.length > 0 && str.indexOf('  ') > -1) {
                    str = str.replace(new RegExp(/  /g), ' ');
                }
                length = str.split(' ').length;
            } else {
                //var s = str.length;
                //for (var i = str.length - 1; i >= 0; i--) {
                //    var code = str.charCodeAt(i);
                //    if (code > 0x7f && code <= 0x7ff) {
                //        s++;
                //    } else if (code > 0x7ff && code <= 0xffff) {
                //        s += 2;
                //    }
                //    if (code >= 0xDC00 && code <= 0xDFFF) {
                //        i--;
                //    }
                //}
                length = str.length;
            }

            if ((min && length < parseInt(min, 10)) || (max && length > parseInt(max, 10))) {
                isValid = false;
            }

            switch (true) {
                case (!!min && !!max):
                    message = $.fn.bootstrapValidator.helpers.format(options.message || $.fn.bootstrapValidator.i18n.wordsLength.between, [parseInt(min, 10), parseInt(max, 10)]);
                    break;

                case (!!min):
                    message = $.fn.bootstrapValidator.helpers.format(options.message || $.fn.bootstrapValidator.i18n.wordsLength.more, parseInt(min, 10));
                    break;

                case (!!max):
                    message = $.fn.bootstrapValidator.helpers.format(options.message || $.fn.bootstrapValidator.i18n.wordsLength.less, parseInt(max, 10));
                    break;

                default:
                    break;
            }

            return { valid: isValid, message: message };
        }
    };
}(window.jQuery));
(function ($) {
    $.fn.bootstrapValidator.i18n.remoteNoSync = $.extend($.fn.bootstrapValidator.i18n.remoteNoSync || {}, {
        'default': 'Please enter a valid value'
    });

    $.fn.bootstrapValidator.validators.remoteNoSync = {
        html5Attributes: {
            message: 'message',
            name: 'name',
            type: 'type',
            url: 'url',
            data: 'data',
            delay: 'delay'
        },

        /**
         * Destroy the timer when destroying the bootstrapValidator (using validator.destroy() method)
         */
        destroy: function (validator, $field, options) {
            if ($field.data('bv.remoteNoSync.timer')) {
                clearTimeout($field.data('bv.remoteNoSync.timer'));
                $field.removeData('bv.remoteNoSync.timer');
            }
        },

        /**
         * Request a remote server to check the input value
         *
         * @param {BootstrapValidator} validator Plugin instance
         * @param {jQuery} $field Field element
         * @param {Object} options Can consist of the following keys:
         * - url {String|Function}
         * - type {String} [optional] Can be GET or POST (default)
         * - data {Object|Function} [optional]: By default, it will take the value
         *  {
         *      <fieldName>: <fieldValue>
         *  }
         * - delay
         * - name {String} [optional]: Override the field name for the request.
         * - message: The invalid message
         * - headers: Additional headers
         * @returns {Deferred}
         */
        validate: function (validator, $field, options) {
            var value = $field.val(),
                dfd = new $.Deferred();
            if (value === '') {
                dfd.resolve($field, 'remoteNoSync', { valid: true });
                return dfd;
            }

            var name = $field.attr('data-bv-field'),
                data = options.data || {},
                url = options.url,
                type = options.type || 'POST',
                headers = options.headers || {};

            // Support dynamic data
            if ('function' === typeof data) {
                data = data.call(this, validator);
            }

            // Parse string data from HTML5 attribute
            if ('string' === typeof data) {
                data = JSON.parse(data);
            }

            // Support dynamic url
            if ('function' === typeof url) {
                url = url.call(this, validator);
            }

            data[options.name || name] = value;
            function runCallback() {
                var xhr = $.ajax({
                    type: type,
                    headers: headers,
                    url: url,
                    dataType: 'json',
                    data: data,
                    async: false
                });
                xhr.then(function (response) {
                    response.valid = response.valid === true || response.valid === 'true';
                    dfd.resolve($field, 'remoteNoSync', response);
                });

                dfd.fail(function () {
                    xhr.abort();
                });

                return dfd;
            }

            if (options.delay) {
                // Since the form might have multiple fields with the same name
                // I have to attach the timer to the field element
                if ($field.data('bv.remoteNoSync.timer')) {
                    clearTimeout($field.data('bv.remoteNoSync.timer'));
                }

                $field.data('bv.remoteNoSync.timer', setTimeout(runCallback, options.delay));
                return dfd;
            } else {
                return runCallback();
            }
        }
    };
}(window.jQuery));