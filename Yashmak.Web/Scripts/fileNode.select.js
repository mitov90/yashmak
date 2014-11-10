// select file row in virtual disk
// and show menu for each row
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


// this script change button attribute fileNodeId
// to know which is the current folder
// to add files or create folder
function OnFolderChange(fileNodeId) {
    $(".dynamic-change-id").attr("href", function (i, a) {
        return a.replace(/(filenodeid=)[0-9]+/ig, '$1' + fileNodeId);
    });
}

