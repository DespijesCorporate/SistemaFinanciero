var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {
    ListaTiposTrans();
    //ValidaCajero();

    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9.]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû 0-9.,-]/, optional: true } } });

});

function Otros_Ingresos(e) {
    var path = urljs + "/Caja/ValidaCajero";
    var posting = $.post(path, {});
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
        if ((data.Estado) != 1 && (data.Estado) != 2) {
            e.stopPropagation();
            var id = -1;
            $("#theHeader").html("Otros Ingresos");
            $('#hidden_id').val("");
            $("#Moda_otras_transacciones").find('input[type = "text"]').val("");
            $('#descripcion_tipo_razon').val('');
            $('#tiene_listado_x').val(0);
            $('#tipostatus').val('ACT');
            $('#tiene_listado_x').trigger('change');
            $('#Moda_otras_transacciones').modal('show');
            $("#btnGeneraPago").removeClass("disabled");
            $("#btnGeneraRecibo").addClass("disabled");
        }
        else {
            alert(data.Mensaje);
        }
    });
}

//LISTA DE Tipos de Transacciones
function ListaTiposTrans() {
    $("#ListaTiposTrans").empty().append(new Option('', '-2'));
    try {
        //var path = urljs + "/sucursales/GetAll";
        var path = urljs + "/Caja/ListaTiposTrans";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    //$("#ListaTiposTrans").empty().append(new Option('Todos los Estados', '3'));
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#ListaTiposTrans").append(new Option(data[contador].OTT_Descripcion, data[contador].OTT_Codigo));
                        contador++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {

    }
}

$("#btnGeneraPago").on('click', function (e) {
    var path = urljs + "/Caja/ValidaCajero";
    var posting = $.post(path, {});
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
        //alert(data.Mensaje);
        if ((data.Estado) != 1 && (data.Estado) != 2) {
            GenerarPago($("#Monto").val(), $("#Observacion").val(), $("#ListaTiposTrans").val(),$("#Clave").val());
            $("#btnGeneraPago").addClass("disabled");
            $("#btnGenerarSolicitud").removeClass("disabled");
        }
        else {
            alert(data.Mensaje);
        }
    });
});

function GenerarPago(Monto, Observacion, ListaTiposTrans,Clave) {
    try {
        var path = urljs + "/Caja/OtrosPagos";
        var posting = $.post(path, { Monto: Monto, Observacion: Observacion, ListaTiposTrans: ListaTiposTrans, Clave:Clave });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            $("#NumTrans").val(data.Num);
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
