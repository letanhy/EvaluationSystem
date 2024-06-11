function OpenPopup(url, title, size, h) {
    if (url != '') {
        if (size == 'xs') {
            $(".modal-dialog").css("max-width", "50vh");
        }
        else if (size == 's') {
            $(".modal-dialog").css("max-width", "80vh");
        }
        else if (size == 'm') {
            $(".modal-dialog").css("max-width", "110vh");
        }
        else if (size == 'l') {
            $(".modal-dialog").css("max-width", "140vh");
        }
        else if (size == 'xl') {
            $(".modal-dialog").css("max-width", "170vh");
        }
        else {
            $(".modal-dialog").css("max-width", "192vh");
        }

        //$("#modalCenterTitle").text(title.toUpperCase());

        $("#modalCenter .modal-body .iframe-container").html("<iframe src='" + url + "' style='width:100%; height:100%'></iframe>");
        if (h > 0)
            $("#modalCenter .modal-content").height(h);
        else
            $("#modalCenter .modal-content").height("90vh");


        $('#modalCenter').modal('show');
    }
}


function ClosePopup(bReload) {
    $("#modalCenter .modal-body .iframe-container").html("");
    $('#modalCenter').modal('hide');

    if (bReload)
        location.reload(true);
}

function ClosePopupAndReloadPage() {
    ShowLoading();
    ClosePopup(true);
}
function ClosePopupAndReloadGrid(title, text, type) {
    ClosePopup();
    alertPopup(title, text, type);
    new MvcGrid(document.querySelector('.mvc-grid')).reload();
}
function ClosePopupAndNotifi(title, text, type) {
    ClosePopup();
    alertPopup(title, text, type);
}
function ShowLoading() {
    $(".img-loading-wrap").show();
}