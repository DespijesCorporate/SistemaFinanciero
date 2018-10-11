var hoy = moment(new Date()).format('YYYY-MM-DD');

function ListaRecibos(e) {
    e.stopPropagation();
    var id = -1;
    GetRecibos();
    $("#theHeader").html("Gestión de Recibos");
    $('#hidden_id').val("");
    $("#ListaRecibos").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#ModalListaRecibos').modal('show');
}

$("#btnGenerarSolicitud").on('click', function (e) {
    var recibo = $("#NumTrans").val()
    GenerarSolicitud(recibo);
});

function GenerarSolicitud(recibo) {

    var path = urljs + "/Prestamos/ReciboOtros";
    var posting = $.post(path, { recibo: recibo });

    //var path = urljs + "/Prestamos/ReciboOtros";
    //var posting = $.post(path, { NumRecibo = $("#NumTrans").val() });
}
  
function GetRecibos() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Caja/GetRecibos";
        var posting = $.post(path, {});
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#ListaRecibos");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('.titulo').empty().append('Listado de préstamos del cliente: ' + data[0].NomPrestamo);
                    //client = data[0].ClienteId;
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#ListaRecibos");
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
}

function addRow(ArrayData, tableID) {

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['RE_Numero'],
        //ArrayData['MontoSolicitado'],
        ArrayData['RE_Fecha'],
        //ArrayData['PlazoMeses'],
        //ArrayData['Interes'],
        ArrayData['RE_Observacion'],
        ArrayData['RE_Total_Rec'],
        ArrayData['RE_agrego'],
        //"<button data-id='" + ArrayData['RE_Documento'] + "' data-name='" + ArrayData['RE_Documento'] + "' title='Ver Prestamos' data-toggle='tooltip' onClick='AnularRecibo(" + ArrayData['RE_Documento'] + ")" + ArrayData['RE_Documento'] + "\"' id='btnVerRazones" + ArrayData['RE_Documento'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-times-circle'></i></button>" +
        //"<button data-id='" + ArrayData['RE_Documento'] + "' data-name='" + ArrayData['RE_Documento'] + "' title='Editar Préstamos' data-toggle='tooltip' onClick='EditarTipoRazon(" + ArrayData['RE_Documento'] + ")' id='btnEditarTipoRazon" + ArrayData['RE_Documento'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>",
        "<button data-id='" + ArrayData['RE_Documento'] + "' title='Anular Recibo' data-toggle='tooltip' onClick='AnularRecibo(" + ArrayData['RE_Documento'] + ")' id='AnularReciboCaja" + ArrayData['RE_Documento'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-times-circle'></i></button>"
        //"<button data-id='" + ArrayData['RE_Documento'] + "' title='Anular Recibo' data-toggle='tooltip' onClick='AnularRecibo(" + ArrayData['RE_Documento'] + ")' id='AnularReciboCaja" + ArrayData['RE_Documento'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-times-circle'></i></button>"
        //+ "<button data-id='" + ArrayData['RE_Numero'] + "' title='Anular Recibo' data-toggle='tooltip' onClick='GetDatosPagoPrestamo(" + ArrayData['RE_Numero'] + ")' id='btnGenerarPag" + ArrayData['RE_Numero'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>"
        
        //"<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Pago' data-toggle='tooltip' onClick='EditarRazon(event)' id='btnEditarRazon" + ArrayData['RazonId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>", 
        //+ "<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Préstamo' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        //ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['RE_Documento']);
    $('td', theNode)[6]//.setAttribute('class', 'RazonId hidden');
}

function AnularRecibo(Documento) {
    try {
        var path = urljs + "/Caja/AnularRecibo";
        var posting = $.post(path, { Documento: Documento });
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
