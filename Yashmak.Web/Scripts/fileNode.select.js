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


//Modal dialog for delete in browse mode
$(function () {
    // Initialize numeric spinner input boxes
    //$(".numeric-spinner").spinedit();
    // Initialize modal dialog
    // attach modal-container bootstrap attributes to links with .modal-link class.
    // when a link is clicked with these attributes, bootstrap will display the href content in a modal dialog.
    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });
    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });
    //clear modal cache, so that new content can be loaded
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
    $('#CancelModal').on('click', function () {
        return false;
    });
});





if ($("#access-id").val() != 1) {
    $("#custom-permission").hide();
}
$("#access-id").change(function(e) {
    if ($("#access-id").val() == 1) {
        $("#custom-permission").show();
    } else {
        $("#custom-permission").hide();
    }
});
