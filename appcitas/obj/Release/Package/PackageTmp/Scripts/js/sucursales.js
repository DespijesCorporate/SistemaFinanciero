$(document).ready(function ()
{
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF060');
    jQuery.ajaxSetup({ async: true });

    $(".timepicker").timepicker({
        format: 'HH:mm',
        
    });
    GetSucursales();
    //LimpiarTabla('#tableSucursales');

    $('#btnNuevasucursal').click(function () {
        //console.log('Nueva Sucursal');
        $("#theHeader").html("Nueva sucursal");
        $('#hidden_id').val('');
        LimpiarInput(['#txt_nombre', '#txt_abreviatura', '#txt_ubicacion', '#hidden_id'], ['#cmb_tipoatencion_sucursal']);
    });

    $('#btnGuardar').click(function ()
    {
        try
        {
            var inputs = [];
            var mensaje = [];            
            var comboBox = [];
            /*Recorremos el contenedor para obtener los valores*/
            $('#form [type=text]').each(function () {
                /*Llenamos los arreglos con la info para la validacion*/
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            });

            $('#form select').each(function () {
                /*Llenamos los arreglos con la info para la validacion*/
                inputs.push('#' + $(this).attr('id'));
                comboBox.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
            });

            /*Si la validación es correcta ejecuta el ajax*/
            if (Validar(inputs, mensaje))
            {
                var r = 0;
                if($('#hidden_id').val() == ''){
                    var r = CheckSucursal($.trim($('#txt_nombre').val()), $.trim($('#txt_abreviatura').val()));
                }

                if (r >= 1)
                {
                    $('#modalSucursales').modal('hide');
                    LimpiarInput(inputs_sucursal, comboxBox_Sucursal);
                    GenerarErrorAlerta('La sucursal que desea ingresar ya existe!', "error");
                    goAlert();
                }
                else
                {
                    var path = urljs + 'sucursales/SaveData';
                    var canal = false;
                    var centroAtencion = false;
                    var id = $('#hidden_id').val();

                    if (id == "") {
                        id = -1;
                    }

                    if (Number($('#txt_atencion').val()) == 1) {
                        centroAtencion = true;
                    }

                    if (Number($('#txt_canal').val()) == 1) {
                        canal = true;
                    }
                    //JSON data
                    var dataType = 'application/json; charset=utf-8';
                    var data = {
                        SucursalId: id,
                        SucursalNombre: $.trim($('#txt_nombre').val()),
                        SucursalAbreviatura: $.trim($('#txt_abreviatura').val()),
                        SucursalUbicacion: $.trim($('#txt_ubicacion').val()),
                        SucursalEsCanal: canal,
                        SucursalEsCentroAtencion: centroAtencion,
                        SucursalTipoAtencion: $('#cmb_tipoatencion_sucursal').val()
                    }

                    var posting = $.post(path, data);

                    posting.done(function (data, status, xhr) {
                        //console.log(data);
                        $('#modalSucursales').modal('hide');
                        LimpiarInput(inputs, ['']);
                        GetSucursales();
                    });

                    posting.fail(function (data, status, xhr) {
                        console.log(status);
                    });
                }                
            }
        } catch (e)
        {
            console.log(e);
        }
        
    });

    $('#btnGuardarHorario').click(function () {
        try {
            var inputs = [];
            var mensaje = [];
            /*Recorremos el contenedor para obtener los valores*/
            $('#formHorario .requerido').each(function () {
                /*Llenamos los arreglos con la info para la validacion*/
                if($(this).data('requerido') == true){
                    inputs.push('#' + $(this).attr('id'));
                    mensaje.push($(this).attr('attr-message'));
                }
            });

            /*Si la validación es correcta ejecuta el ajax*/
            if (Validar(inputs, mensaje))
            {
                var id = $('#hidden_id').val();
                var horarioid = $('#horario_id').val();
                var r = 0;

                if(horarioid == "")
                {
                    r = CheckHorario(id, $('#cmb_dia').val());
                    if (r >= 1)
                    {
                        //$('#modalSucursalesHorario').modal('hide');
                        LimpiarInput(['#txt_inicio', '#txt_fin'], ['#cmb_dia']);
                        GenerarErrorAlerta('El horario que desea que desea ingresar ya existe!', "successModalHorarios");
                        goAlert();
                    }
                    else
                    {
                        /*Inserta*/
                        saveHorario(id, $('#cmb_dia').val());
                    }
                }
                else
                {
                    /*Actualiza*/
                    saveHorario(id, horarioid);
                }
                
            }
        } catch (e) {
            console.log(e);
        }

    });

    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'sucursales/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetSucursales();
                $('#modalEliminarSucursal').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs_sucursal, comboxBox_Sucursal);
 
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

    $('#btnGuardarCubiculo').click(function () {
        try {
            var inputs = ['#cmb_cubiculo', '#cmb_tipoatencion', '#txt_posicion', '#txt_usuario', '#txt_inicio_descanso', '#txt_fin_descanso'];
            var mensaje = ['Debe seleccionar un cubiculo!', 'Debe seleccionar un tipo de atención', 'Debe especificar una posición', 'Debe especificar un usuario', 'Debe seleccionar una hora de inicio', 'Debe seleccionar una hora de finalización'];

            /*Si la validación es correcta ejecuta el ajax*/
            if (Validar(inputs, mensaje)) {

                var id = $('#hidden_id').val();
                var cubiculo = $('#cubiculo_id').val();
                console.log(cubiculo);
                var r = 0;

                if (cubiculo == "") {
                    r = CheckCubiculos(id, $('#cmb_cubiculo').val());
                    if (r >= 1) {
                        //$('#modalSucursalesCubiculos').modal('hide');
                        LimpiarInput(['#txt_posicion', '#txt_usuario', '#txt_inicio_descanso', '#txt_fin_descanso', '#cubiculo_id'], ['#cmb_cubiculo', '#cmb_tipoatencion']);
                        GenerarErrorAlerta('El Cubiculo que desea que desea ingresar ya existe!', "successModalCubiculos");
                        goAlert();
                    }
                    else {
                        /*Inserta*/
                        saveCubiculos(id, $('#cmb_cubiculo').val());
                    }
                }
                else {
                    /*Actualiza*/
                    saveCubiculos(id, cubiculo);
                }
                //$('#modalSucursalesSegmentos').modal('hide');
            }
        } catch (e) {
            console.log(e);
        }
    });

    $('#btnGuardarSegmento').click(function () {
        try {
            var inputs = ['#cmb_segmento'];
            var mensaje = ['Debe seleccionar un segmento!'];

            /*Recorremos el contenedor para obtener los valores*/
            /*$('#formHorario [type=text]').each(function () {
               
                inputs.push('#' + $(this).attr('id'));
                mensaje.push($(this).attr('attr-message'));
                console.log($(this).attr('attr-message'));
            });*/

            /*Si la validación es correcta ejecuta el ajax*/
            if (Validar(inputs, mensaje)) {


                var id = $('#hidden_id').val();
                var segmento = $('#cmb_segmento').val();
                                
                var r = 0;

                //if (segmento == "-1") {
                r = CheckSegmentos(id, segmento);
                    if (r >= 1) {
                        //$('#modalSucursalesSegmentos').modal('hide');
                        LimpiarInput(['#segmento_id'], ['#cmb_segmento']);
                        GenerarErrorAlerta('El Segmento que desea que desea ingresar ya existe!', "successModalSegmento");
                        goAlert();
                    }
                    else {
                        /*Inserta*/
                        saveSegmentos(id, segmento);
                    }
                //}
                /*else {
                    /*Actualiza*/
                /*saveCubiculos(id, segmento);*/
                /*}*/
                
                //$('#modalSucursalesSegmentos').modal('hide');
            }
        } catch (e) {
            console.log(e);
        }

    });

    $('#btnEliminarSegmento').click(function () {
        var id = $('#hidden_id').val();
        var Segmentoid = $('#segmento_id').val();
        var path = urljs + 'sucursales/deleteSegmento';
        var posting = $.post(path, { SucursalId: Number(id), SucSegmentoId: String(Segmentoid) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetSucursalesSegmentos(id);
                $('#modalEliminarSegmento').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                //LimpiarInput(inputs, ['']);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "successModalEliminarSegmentos");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            //sendError(data);
            GenerarErrorAlerta(xhr, "successModalEliminarSegmentos");
            goAlert();
        });
        posting.always(function () {
            /*console.log("Se cargo los proyectos correctamente");*/
            //RemoveAnimation("body");
        });
    });

    $('#btnEliminarCubiculo').click(function () {
        var id = $('#hidden_id').val();
        var Cubiculoid = $('#cubiculo_id').val();
        var path = urljs + 'sucursales/deleteCubiculo';
        var posting = $.post(path, { SucursalId: Number(id), CubId: String(Cubiculoid) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetSucursalesCubiculos(id);
                $('#modalEliminarCubiculo').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                //LimpiarInput(inputs, ['']);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "successModalEliminarCubiculos");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            //sendError(data);
            GenerarErrorAlerta(xhr, "successModalEliminarCubiculos");
            goAlert();
        });
        posting.always(function () {
            /*console.log("Se cargo los proyectos correctamente");*/
            //RemoveAnimation("body");
        });
    });

    $('#btnEliminarHorario').click(function () {
        var id = $('#hidden_id').val();
        var HorarioId = $('#horario_id').val();
        var path = urljs + 'sucursales/deleteHorario';
        var posting = $.post(path, { SucursalId: Number(id), HorarioId: String(HorarioId) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetSucursalesHorarios(id);
                $('#modalEliminarHorario').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "successModalEliminarHorario");
                goAlert();
                //LimpiarInput(inputs, ['']);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "successModalEliminarHorario");
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

var inputs_sucursal = ['txt_cod_suc','#txt_nombre', '#txt_abreviatura', '#txt_ubicacion', '#txt_tipo_atencion', '#hidden_id'];
var comboxBox_Sucursal = ["#cmb_tipoatencion_sucursal"];

function GetSucursales()
{
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/sucursales/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableSucursales");
            if (data[0].Accion > 0) 
            {
                for (var i = data.length - 1; i >= 0; i--) {
                    addRow(data[i], "#tableSucursales");
                }
            }
            else {
                GenerarErrorAlerta(data[0].Mensaje, "error");
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

        var infojson = jQuery.parseJSON('{"input": "#cmb_tipoatencion_sucursal", ' +
                                      '"url": "configuracion/getParametosByIdEncabezado/", ' +
                                      '"val": "ConfigItemID", ' +
                                      '"type": "GET", ' +
                                      '"data": "TATEN", ' +
                                      '"text": "ConfigItemDescripcion"}');
        //console.log(infojson);
        LoadComboBox(infojson);
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
}

function GetSucursalesHorarios(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "sucursales/GetHorariosBySucursal";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableHorarios");
            if (data[0].Accion > 0) {
                for (var i = data.length - 1; i >= 0; i--) {
                    addRowHorario(data[i], "#tableHorarios");
                }
            }
            else {
                GenerarErrorAlerta(data[0].Mensaje, "successModalHorarios");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "successModalHorarios");
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

function GetSucursalesHorariosByID(SucId, HorId)
{
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        console.log(SucId);
        var path = urljs + "sucursales/GetHorariosByID";
        var posting = $.post(path, { SucId: SucId, HorId: HorId });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0)
            {
                $('#cmb_dia').val(data.SucHorarioId);
                $('#cmb_laboral').val(data.SucHorarioIndLaboral);
                $('#cmb_horario_corrido').val(data.SucHorarioCorrido);
                $('#txt_inicio').val(data.SucHorarioHoraInicio);
                $('#txt_fin').val(data.SucHorarioHoraFinal);
            }
            else
            {
                GenerarErrorAlerta(data.Mensaje, "successModalHorarios");
                goAlert();
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "successModalHorarios");
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

function GetSucursalBy(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/sucursales/GetOne";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0)
            {
                var canal = 0;
                var centroAtencion = 0;
                if (data.SucursalEsCanal)
                {
                    canal = 1;
                }
                if (data.SucursalTipoAtencion) {
                    centroAtencion = 1;
                }

                $('#txt_cod_suc').val(data.SucursalId);
                $('#txt_nombre').val(data.SucursalNombre);
                $('#txt_abreviatura').val(data.SucursalAbreviatura);
                $('#txt_ubicacion').val(data.SucursalUbicacion);
                $('#cmb_tipoatencion_sucursal').val(data.SucursalTipoAtencion).trigger('change');
                $('#txt_canal').val(canal);
                $('#txt_atencion').val(centroAtencion);
            }
            else
            {
                GenerarErrorAlerta(data[0].Mensaje, "error");
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

function addRow(ArrayData, tableID)
{
    var canal = "No";
    var centroAtencion = "No";
    if (ArrayData['SucursalEsCanal'])
    {
        canal = "Si";
    }
    if (ArrayData['SucursalEsCentroAtencion']) {
        centroAtencion = "Si";
    }
    
    var newRow = $(tableID).dataTable().fnAddData([
            ArrayData['SucursalId'],
            ArrayData['SucursalNombre'],
            ArrayData['SucursalAbreviatura'],
            ArrayData['SucursalUbicacion'],
            canal,
            centroAtencion,            
            ArrayData['SucursalTipoAtencion'],
            ArrayData['SucursalTipoAtencion'],

            '<div class="btn-group text-center">' + 
 			'<button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars" aria-hidden="true"></i></button>' +
 			'<ul class="dropdown-menu centrar-menu text-left">' + 				
 				"<li><a class='op1' href='#' data-id='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Editar Sucursal' data-toggle='tooltip' data-placement='left' onClick='EditarSucursal(event)' id='btnEditarSucursal" + ArrayData['SucursalId'] + "'><i class='fa fa-pencil-square-o'></i> Editar</a></li>" +
 				"<li><a href='#' data-id='" + ArrayData['SucursalId'] + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Eliminar Sucursal' data-toggle='tooltip' data-placement='left' onClick='EliminarSucursal(event)' id='btnEliminarSucursal" + ArrayData['SucursalId'] + "'><i class='fa fa-trash-o'></i> Eliminar</a></li>" +
 				'<li role="separator" class="divider"></li>' +
 				"<li><a data-id='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Editar Horario' data-toggle='tooltip' data-placement='left' onClick='EditarHorario(event)' id='btnEditarHorario" + ArrayData['SucursalId'] + "' href='#'><i class='fa fa-clock-o' aria-hidden='true'></i> Horario</a></li>" +
 				"<li><a data-id='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Editar Cubiculos' data-toggle='tooltip' data-placement='left' onClick='EditarCubiculos(event)' id='btnEditarCubiculos" + ArrayData['SucursalId'] + "' href='#'><i class='fa fa-table' aria-hidden='true'></i> Cubiculos</a></li>" +
                "<li><a data-id='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Editar Segmentos' data-toggle='tooltip' data-placement='left' onClick='EditarSegmentos(event)' id='btnEditarSegmentos" + ArrayData['SucursalId'] + "' href='#'><i class='fa fa-sitemap' aria-hidden='true'></i> Segmentos</a></li>" +
 			'</ul>' + 
 		'</div>'
            //"<button data-id='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Editar Sucursal' data-toggle='tooltip' onClick='EditarSucursal(event)' id='btnEditarSucursal" + ArrayData['SucursalId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
            //"<button data-id='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucursalNombre'] + "' title='Eliminar Sucursal' data-toggle='tooltip' onClick='EliminarSucursal(event)' id='btnEliminarSucursal" + ArrayData['SucursalId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
            //,ArrayData['SucursalId']
        ]);

        var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
        theNode.setAttribute('id', ArrayData['SucursalId']);
        $('td', theNode)[7].setAttribute('class', 'SucursalId hidden');
        $('td', theNode)[6].setAttribute('class', 'text-center');
}

function addRowHorario(ArrayData, tableID) {
    var laboral = "No";
    if (ArrayData['SucHorarioIndLaboral']) {
        laboral = "Si";
    }
    var horario_corrido = "No";
    if (ArrayData['SucHorarioCorrido']) {
        horario_corrido = "Si";
    }

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['SucHorarioId'],
        laboral,
        horario_corrido,
        ArrayData['SucHorarioHoraInicio'],
        ArrayData['SucHorarioHoraFinal'],
        "<button data-id='" + ArrayData['SucHorarioId'] + "' data-laboral='" + ArrayData['SucHorarioIndLaboral'] +"' data-corrido='" + ArrayData['SucHorarioCorrido'] + "' data-sucursal='" + ArrayData['SucursalId'] + "' data-inicio='" + ArrayData['SucHorarioHoraInicio'] + "' data-fin='" + ArrayData['SucHorarioHoraFinal'] + "' data-name='" + ArrayData['SucHorarioHoraInicio'] + ' - ' + ArrayData['SucHorarioHoraFinal'] + "'   title='Editar Horario' data-toggle='tooltip' onClick='CargarHorario(event)' id='btnEditarHorario" + ArrayData['SucursalId'] + "' class='btn btn-primary botonVED text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['SucHorarioId'] + "' data-sucursal='" + ArrayData['SucursalId'] + "' data-name='" + ArrayData['SucHorarioId'] + " [" +ArrayData['SucHorarioHoraInicio'] + " - " + ArrayData['SucHorarioHoraFinal'] + "]'  title='Eliminar Horario' data-toggle='tooltip' onClick='EliminarHorario(event)' id='btnEliminarHorario" + ArrayData['SucursalId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>" ,
        ArrayData['SucursalId'],
        ArrayData['Orden']
    ]);
    var table = $(tableID).DataTable();
    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['SucHorarioId']);
    $('td', theNode)[5].setAttribute('class', 'text-center');
    $('td', theNode)[6].setAttribute('class', 'hidden');
    $('td', theNode)[7].setAttribute('class', 'hidden');
    table.order([7, 'asc']).draw();
}

function EditarSucursal(e) {
    e.stopPropagation();
    LimpiarInput(['#txt_nombre', '#txt_abreviatura', '#txt_ubicacion'], ['#cmb_tipoatencion_sucursal']);
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');        
    $("#theHeader").html("Editar | " + desc);
    $('#modalSucursales').modal('show');
    $('#hidden_id').val(id);
    GetSucursalBy(id);
}

function CargarHorario(e) {
    e.stopPropagation();
    //console.log('TEST');
    //LimpiarInput(['#txt_nombre', '#txt_abreviatura', '#txt_ubicacion'], ['#cmb_tipoatencion_sucursal']);
    var id = $(e.currentTarget).attr('data-id');
    var lab = $(e.currentTarget).attr('data-laboral');
    var corrido = $(e.currentTarget).attr('data-corrido');
    var inicio = $(e.currentTarget).attr('data-inicio');
    var fin = $(e.currentTarget).attr('data-fin');
    $('#horario_id').val(id);
    $("#cmb_dia").val(id).trigger('change');
    $("#cmb_laboral").val(lab).trigger('change');
    $("#cmb_horario_corrido").val(corrido).trigger('change');
    $("#txt_inicio").val(inicio);
    $("#txt_fin").val(fin);
}

function EliminarSucursal(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Sucursal | " + desc);
    $('#modalEliminarSucursal').modal('show');
    $('#modalmessage').html('¿Realmente desea eliminar la sucursal: ' + desc + '?');
}

function EditarHorario(e) {
    e.stopPropagation();
    LimpiarInput(['#txt_inicio', '#txt_fin', '#horario_id'], ['#cmb_dia']);
    //$('#horario_id').val("");
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    LoadDias(id);
    GetSucursalesHorarios(id);
    $("#theHeaderHorario").html("Horario para sucursal: " + desc);
    $('#modalSucursalesHorario').modal('show');
    $('#hidden_id').val(id);
    //GetSucursalBy(id);
}

function EditarCubiculos(e) {
    e.stopPropagation();
    var inputs = ['#txt_posicion', '#txt_usuario', '#txt_inicio_descanso', '#txt_fin_descanso', '#cubiculo_id'];
    var comboBox = ['#cmb_cubiculo', '#cmb_tipoatencion'];
    LimpiarInput(inputs, comboBox);
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeaderCubiculos").html("Cubiculos para sucursal: " + desc);
    $('#modalSucursalesCubiculos').modal('show');
    $('#hidden_id').val(id);
    var infojson = jQuery.parseJSON('{"input": "#cmb_cubiculo", ' +
                                        '"url": "configuracion/getParametosByIdEncabezado/", ' +
                                        '"val": "ConfigItemID", ' +
                                        '"type": "GET", ' +
                                        '"data": "POS", ' +
                                        '"text": "ConfigItemDescripcion"}');
    //console.log(infojson);
    LoadComboBox(infojson);

    var infojson1 = jQuery.parseJSON('{"input": "#cmb_tipoatencion", ' +
                                       '"url": "configuracion/getParametosByIdEncabezado/", ' +
                                       '"val": "ConfigItemID", ' +
                                       '"type": "GET", ' +
                                       '"data": "TATEN", ' +
                                       '"text": "ConfigItemDescripcion"}');
    //console.log(infojson);
    LoadComboBox(infojson1);
    GetSucursalesCubiculos(id);
}

function EditarSegmentos(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    $('#segmento_id').val("");
    var desc = $(e.currentTarget).attr('data-name');
    var infojson = jQuery.parseJSON('{"input": "#cmb_segmento", ' +
                                        '"url": "configuracion/getParametosByIdEncabezado/", ' +
                                        '"val": "ConfigItemID", ' +
                                        '"type": "GET", ' +
                                        '"data": "SEGM", ' +
                                        '"text": "ConfigItemDescripcion"}');
    //console.log(infojson);
    LoadComboBox(infojson);

    GetSucursalesSegmentos(id);
    $("#theHeaderSegmentos").html("Segmentos para sucursal: " + desc);
    $('#modalSucursalesSegmentos').modal('show');
    $('#hidden_id').val(id);
    //GetSucursalBy(id);
}

function EliminarSegmento(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#segmento_id').val(id);

    $("#theHeaderEliminarSegmento").html("Eliminar Segmento | " + desc);
    $('#modalEliminarSegmento').modal('show');
    $('#modalmessagesegmento').html('¿Realmente desea eliminar el segmento: ' + desc + '?');
}

function EliminarCubiculo(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#cubiculo_id').val(id);

    $("#theHeaderEliminarCubiculo").html("Eliminar Cubiculo | " + desc);
    $('#modalEliminarCubiculo').modal('show');
    $('#modalmessagecubiculo').html('¿Realmente desea eliminar el cubículo: ' + desc + '?');
}

function EliminarHorario(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#horario_id').val(id);

    $("#theHeaderEliminarHorario").html("Eliminar el horario | " + desc);
    $('#modalEliminarHorario').modal('show');
    $('#modalmessageHorario').html('¿Realmente desea eliminar el horario: ' + desc + '?');
}

function LoadDias(id) {
    var data = '{ "id": "DAYS" }';
    //data = JSON.stringify(data);
    console.log(data);
    var infojson = jQuery.parseJSON('{"input": "#cmb_dia", ' +
		                              '"url": "configuracion/getParametosByIdEncabezado/", ' +
		                              '"val": "ConfigItemID", ' +
		                              '"type": "GET", ' +
		                              '"data": "DAYS", ' +
		                              '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    LoadComboBox(infojson);
}

function GetSucursalesSegmentos(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/sucursales/GetSegmentosBySucursal";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableSegmento");
            if (data.length) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRowSegmento(data[i], "#tableSegmento");
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "successModalSegmento");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "successModalSegmento");
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

function GetSucursalesCubiculos(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "sucursales/GetAllCubiculo";
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableCubiculo");
            if (data.length) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRowCubiculo(data[i], "#tableCubiculo");
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "successModalCubiculos");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "successModalCubiculos");
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

function addRowSegmento(ArrayData, tableID) {

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['ConfigItemDescripcion'],
        "<button data-id='" + ArrayData['SucSegmentoId'] + "' data-name='" + ArrayData['ConfigItemDescripcion'] + "' title='Eliminar Segmento' data-toggle='tooltip' onClick='EliminarSegmento(event)' id='btnEliminarSegmento" + ArrayData['SucursalId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['SucSegmentoId']
    ]);

    
    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['SucHorarioId']);
    $('td', theNode)[0].setAttribute('class', 'text-center');
    $('td', theNode)[2].setAttribute('class', 'hidden');
}

function addRowCubiculo(ArrayData, tableID) {

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['ConfigItemDescripcion'],
        ArrayData['TipoAtencionId'],
        ArrayData['PosicionNombre'],
        ArrayData['UsuarioId'],
        ArrayData['PosicionHoraInicioDesc'],
        ArrayData['PosicionHoraFinalDesc'],
         "<button data-id='" + ArrayData['PosicionId'] + "' data-usr='" + ArrayData['UsuarioId'] + "'  data-cubiculo='" + ArrayData['ConfigId'] + "' data-posicion='" + ArrayData['PosicionNombre'] + "' data-atencion='" + ArrayData['TipoAtencionId_COD'] + "' data-name='" + ArrayData['ConfigItemDescripcion'] + "' data-iniciodesc='" + ArrayData['PosicionHoraInicioDesc'] + "' data-findesc='" + ArrayData['PosicionHoraFinalDesc'] + "' title='Editar Cubiculo' data-toggle='tooltip' onClick='CargarCubiculo(event)' id='btnEditarCubiculo" + ArrayData['SucursalId'] + "' class='btn btn-primary botonVED text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['PosicionId'] + "' data-name='" + ArrayData['ConfigItemDescripcion'] + "' title='Eliminar Cubiculo' data-toggle='tooltip' onClick='EliminarCubiculo(event)' id='btnEliminarCubiculo" + ArrayData['PosicionId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>"
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['PosicionId']);
    $('td', theNode)[5].setAttribute('class', 'text-center');    
}

function CargarCubiculo(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-cubiculo');
    var atencion = $(e.currentTarget).attr('data-atencion');
    var posicion = $(e.currentTarget).attr('data-posicion');
    var usuario = $(e.currentTarget).attr('data-usr');
    var iniciodes = $(e.currentTarget).attr('data-iniciodesc');
    var findes = $(e.currentTarget).attr('data-findesc');

    $('#cubiculo_id').val(id);
    $('#txt_inicio_descanso').val(iniciodes);
    $('#txt_fin_descanso').val(findes);
    $("#cmb_cubiculo").val(id).trigger('change');
    $("#cmb_tipoatencion").val(atencion).trigger('change');
    $("#txt_usuario").val(usuario);
    $("#txt_posicion").val(posicion);
}

function CheckSucursal(Nombre, Abreviatura)
{
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var result = 0;
        var path = urljs + "/sucursales/CheckSucursal";
        $.ajaxSetup({ async: false });
        var posting = $.post(path, { Nombre: Nombre, Abreviatura: Abreviatura });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            result = data.Accion;
            if (data.Accion == -1)
            {
                GenerarErrorAlerta(data[0].Mensaje, "error");
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
        //console.log(result);
        return result;
    }
    catch (e) {        
        console.log(e);
    }
}

function CheckHorario(SucId, Horario) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var result = 0;
        var path = urljs + "/sucursales/CheckHorario";
        $.ajaxSetup({ async: false });
        var posting = $.post(path, { SucId: Number(SucId), Horario: Horario });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            result = data.Accion;
            if (data.Accion == -1) {
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
        //console.log(result);
        return result;
    }
    catch (e) {
        console.log(e);
    }
}

function saveHorario(id, horarioid)
{
    var path = urljs + 'sucursales/SaveDataHorarios';
    var laboral = false;
    if ($('#cmb_laboral').val() == "true") {
        laboral = true;
    }
    var corrido = false;
    if ($('#cmb_horario_corrido').val() == "true") {
        corrido = true;
    }
    if (id == "") {
        id = -1;
    }
    //JSON data
    var dataType = 'application/json; charset=utf-8';
    var data = {
        SucursalId: id,
        SucHorarioId: horarioid,
        SucHorarioIndLaboral: laboral,
        SucHorarioCorrido: corrido,
        SucHorarioHoraInicio: $('#txt_inicio').val(),
        SucHorarioHoraFinal: $('#txt_fin').val()
    }

    var posting = $.post(path, data);

    posting.done(function (data, status, xhr) {
        //console.log(data);
        if (data.Accion) {
            GetSucursalesHorarios(id);
            LimpiarInput(['#txt_inicio', '#txt_fin', '#horario_id'], ['#cmb_dia']);
        }
        else {
            $('#modalSucursalesHorario').modal('hide');
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(data.Mensaje, "error");
        goAlert();
    });
}

function CheckCubiculos(SucId, Cubiculo) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var result = 0;
        var path = urljs + "/sucursales/CheckCubiculo";
        $.ajaxSetup({ async: false });
        var posting = $.post(path, { SucId: Number(SucId), Cubiculo: Cubiculo });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            result = data.Accion;
            if (data.Accion == -1) {
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
        //console.log(result);
        return result;
    }
    catch (e) {
        console.log(e);
    }
}

function saveCubiculos(id, cubiculo)
{
    var path = urljs + 'sucursales/SaveDataCubiculos';
    var laboral = false;

    if (id == "") {
        id = -1;
    }
    //JSON data
    var dataType = 'application/json; charset=utf-8';
    var data = {
        SucursalId: Number(id),
        PosicionId: String(cubiculo),
        PosicionNombre: $('#txt_posicion').val(),
        TipoAtencionId: $('#cmb_tipoatencion').val(),
        UsuarioId: $('#txt_usuario').val(),
        PosicionHoraInicioDesc: $('#txt_inicio_descanso').val(),
        PosicionHoraFinalDesc: $('#txt_fin_descanso').val()
    }

    var posting = $.post(path, data);

    posting.done(function (data, status, xhr) {
        if (data.Accion) {
            GetSucursalesCubiculos(id);
            LimpiarInput(['#txt_posicion', '#txt_usuario', '#txt_inicio_descanso', '#txt_fin_descanso', '#cubiculo_id'], ['#cmb_cubiculo', '#cmb_tipoatencion']);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(data, "error");
        goAlert();
    });
}

function CheckSegmentos(SucId, Segmento) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");
        var result = 0;
        var path = urljs + "/sucursales/CheckSegmento";
        $.ajaxSetup({ async: false });
        var posting = $.post(path, { SucId: Number(SucId), Segmento: Segmento });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            result = data.Accion;
            if (data.Accion == -1) {
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
        //console.log(result);
        return result;
    }
    catch (e) {
        console.log(e);
    }
}

function saveSegmentos(id, segmento)
{
    var path = urljs + 'sucursales/SaveDataSegmentos';
    var laboral = false;

    if (id == "") {
        id = -1;
    }
    //JSON data
    var dataType = 'application/json; charset=utf-8';
    var data = {
        SucursalId: Number(id),
        SucSegmentoId: segmento
    }

    var posting = $.post(path, data);

    posting.done(function (data, status, xhr) {
        if (data.Accion) {
            GetSucursalesSegmentos(id);
            LimpiarInput(["#segmento_id"], ['#cmb_segmento']);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "error");
            goAlert();
        }
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(data, "error");
        goAlert();
    });
}

$('#cmb_laboral').on('change', function(event){
    
    if($(this).val() == 'true'){
        $('#txt_inicio').data('requerido',true);
        $('#txt_fin').data('requerido',true);
    }
    else{
        $('#txt_inicio').data('requerido',false).val('');
        $('#txt_fin').data('requerido',false).val('');
        limpiarMensajesValidacion($('#formHorario'));
    }
});

$('#modalSucursales').on('hidden.bs.modal', function (e) {
  limpiarMensajesValidacion($('#modalSucursales'));
});
