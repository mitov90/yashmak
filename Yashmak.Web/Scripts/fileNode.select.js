$("body").on("click", ".file-row", function (ev) {
    var self = ($(this));
    if (self.hasClass("success")) {
        self.removeClass("success");
        self.prev().addClass("hidden");
    } else {
        self.addClass("success");
        self.prev().removeClass("hidden");
    }
});