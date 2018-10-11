var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {
    GetTipoRazones();

    $('.identidad').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.rtn').mask('##############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.numero').mask('####', { translation: { '#': { pattern: /[0-9,]/, optional: true } } });
    $('.correo').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[a-z-_.0-9 @]/, optional: true } } });
    $('.telefonoCelular').mask('AAAAAAAA', { translation: { 'A': { pattern: /[0-9]/, optional: true } } });
    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9.,]/, optional: true } } });
    $('.fecha').mask('##########', { translation: { '#': { pattern: /[0-9-]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû#]/, optional: true } } });


    //LimpiarTabla('#tableTipoRazones');
    $('#btnGuardar').click(function () {
        var date1 = $('#fecha').datetimepicker("getDate");
        if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
            existeCliente($('#identidad').val(), $('#rtn').val());
        }
        else {
            GuardarTipoRazon();
        }
        console.log('hidden_id: ' + $('#hidden_id').val());
    });
    //$('#fecha').change(function () {
    //    var date = $(this).datetimepicker("getDate");
    //    console.log(date);
    //});
    $('#fecha').datetimepicker({
        
        locale: 'es',
        maxDate: hoy,
        defaultDate: hoy,
        format: 'YYYY-MM-DD',
        ignoreReadonly: true,
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            left: "fa fa-arrow-left",
            right: "fa fa-arrow-right"
        },
        
    });
    $("#fecha1").on("dp.change", function (e) {
        this.data("DateTimePicker");
    });
    //$("#fecha").on("dp.change", function (e) {
    //    this.data("DateTimePicker").minDate(e.date);
    //});

    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'TipoRazones/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetTipoRazones();
                $('#modalEliminarTipoRazon').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "successModalEliminarPlantillas");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            //sendError(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function () {
            /*console.log("Se cargo los proyectos correctamente");*/
            //RemoveAnimation("body");
        });
    });

    
});





var inputs = ['#rtn', '#identidad', '#primernombre', '#segundonombre', '#primerapellido', '#segundoapellido', '#genero', '#telcasa', '#celular', '#correo', '#fecha', '#estado', '#estcivil', '#direccion'];

function GetTipoRazones() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Clientes/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTipoRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tableTipoRazones");
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

function GetTipoRazonBy(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetOne";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#abreviatura').val(data.TipoAbreviatura);
                $('#descripcion_tipo_razon').val(data.TipoDescripcion);
                $('#tiene_listado_x').val(data.TipoTieneListadoExtra);
                $('#etiqueta').val(data.TipoEtiquetaListadoExtra);
                $('#origen').val(data.TipoOrigenListadoExtra);
                $('#codigo_listado').val(data.TipoCodigoListadoExtra);
                $('#tiene_listado_x').trigger('change');
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
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
    //var TipoTieneListadoExtra = "No";
    //if (ArrayData['TipoTieneListadoExtra']) {
    //    TipoTieneListadoExtra = "Si";
    //}

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['Identidad'],
        ArrayData['RTN'],
        ArrayData['Nombre'],
        ArrayData['Genero'],
        ArrayData['Edad'],
        ArrayData['EstadoCli'],
        ArrayData['Direccion'],
        ArrayData['TelCasa'],
        ArrayData['Cel'],
        ArrayData['Correo'],
        "<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['Identidad'] + "' title='Ver Prestamos' data-toggle='tooltip' onClick='location.href=\"../Prestamos/Prestamo/" + ArrayData['ClienteId'] + "\"' id='btnVerRazones" + ArrayData['ClienteId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></button>" +
        /*"<button data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' onClick='VerRazones(event)' id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></button>" +
        "<a data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' href=\" ../Razones/razones/" + ArrayData['TipoId'] + " \" id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></a>" +*/
        "<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Editar Cliente' data-toggle='tooltip' onClick='EditarTipoRazon(event)' id='btnEditarTipoRazon" + ArrayData['ClienteId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>", // +
        //"<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Eliminar Tipo Razon' data-toggle='tooltip' onClick='EliminarTipoRazon(event)' id='btnEliminarTipoRazon" + ArrayData['ClienteId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ClienteId']);
    $('td', theNode)[11].setAttribute('class', 'ClienteId hidden');
}

function EditarTipoRazon(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar | " + desc);
    $('#moda_tipo_razones').modal('show');
    $('#hidden_id').val(id);
    GetTipoRazonBy(id);
}

function EliminarTipoRazon(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Tipo Razon | " + desc);
    $('#modalEliminarTipoRazon').modal('show');
    $('#modalmessage').html('¿Desea eliminar el tipo de razon: <b>' + desc + '</b>?');
}

$('#tiene_listado_x').change(function () {
    if ($('#tiene_listado_x').val() == 1) {
        $('#div_listado_extra').removeClass('hidden');
        $('#etiqueta').data('requerido', true);
        $('#origen').data('requerido', true);
        $('#codigo_listado').data('requerido', true);
    }
    else {
        $('#div_listado_extra').addClass('hidden');
        $('#etiqueta').data('requerido', false).val('');
        $('#origen').data('requerido', false).val('');
        $('#codigo_listado').data('requerido', false).val('');
    }
});


function NuevoTipoRazon(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Tipo Cliente");
    $('#hidden_id').val("");
    $("#moda_tipo_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#moda_tipo_razones').modal('show');
}


$('#moda_tipo_razones').on('hidden.bs.modal', function (e) {
    limpiarMensajesValidacion($('#form'));
})

function GuardarClientes() {
    try {
        var inputs = [];
        var mensaje = [];

        /*Recorremos el contenedor para obtener los valores*/
        $('#form .requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            if ($(this).data('requerido') == true) {
                console.log($(this)[0].tagName);
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            }
            console.log(inputs);
        });

        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {

            var path = urljs + 'Clientes/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {

                //DATOS GENERALES DEL CLIENTE
                ClienteId: id,
                Identidad: $('#identidad').val(),
                RTN: $('#rtn').val(),
                PrimerNombre: $('#primernombre').val(),
                SegundoNombre: $('#segundonombre').val(),
                PrimerApellido: $('#primerapellido').val(),
                SegundoApellido: $('#segundoapellido').val(),
                Genero: $('#genero option:selected').val(),
                Edad: $('#fecha').val(),
                EstadoCli: $('#estado option:selected').val(),
                EstadoCivil: $('#estcivil').val(),
                Direccion: $('#direccion').val(),
                TelCasa: $('#telcasa').val(),
                Cel: $('#celular').val(),
                Correo: $('#correo').val(),

                //PRIMERA REFERENCIA DEL CLIENTE
                RefNom1: $('#NomRef1').val(),
                RefId1: $('#IdRef1').val(),
                RefCel1: $('#CelRef1').val(),
                RefTel1: $('#TelRef1').val(),


                //SEGUNDA REFERENCIA DEL CLIENTE
                RefNom2: $('#NomRef2').val(),
                RefId2: $('#IdRef2').val(),
                RefCel2: $('#CelRef2').val(),
                RefTel2: $('#TelRef2').val(),
            }

            var posting = $.post(path, data);

            posting.done(function (data, status, xhr) {
                $('#moda_tipo_razones').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetTipoRazones();
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });
        }
    } catch (e) {
        console.log(e);
    }
}

function existeCliente(identidad, rtn) {
    var resultado = false;
    try {
        var path = urljs + "/Clientes/CheckOne";
        var posting = $.post(path, { identidad: identidad, rtn: rtn });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#moda_tipo_razones').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
            }
            else {
                GuardarClientes();
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
        });
    }
    catch (e) {
        console.log(e);
    }
}