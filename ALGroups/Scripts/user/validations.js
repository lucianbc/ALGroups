$(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || proxy(value, "dd/MM/yyyy hh:mm", false);
    }
});

var proxy = function (value, format, b) {
    console.log("performing something")
    var res = moment(value, format, b).isValid();
    console.log(res);
    return res
}