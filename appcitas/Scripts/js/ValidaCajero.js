var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {
    ValidaCajero();
});

function ValidaCajero() {
    try {
        var path = urljs + "/Caja/ValidaCajero";
        var posting = $.post(path, {});
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            alert(data.Mensaje);
                    
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
        });
    }
    catch (e) {
        //RemoveAnimation("body");

    }
    //location.reload();
}


