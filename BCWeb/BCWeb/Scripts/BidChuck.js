// hack for making sure array filter function is available on all browsers
if (!Array.prototype.filter) {
    Array.prototype.filter = function (fn, context) {
        var i,
            value,
            result = [],
            length;

        if (!this || typeof fn !== 'function' || (fn instanceof RegExp)) {
            throw new TypeError();
        }

        length = this.length;

        for (i = 0; i < length; i++) {
            if (this.hasOwnProperty(i)) {
                value = this[i];
                if (fn.call(context, value, i, this)) {
                    result.push(value);
                }
            }
        }
        return result;
    };
}