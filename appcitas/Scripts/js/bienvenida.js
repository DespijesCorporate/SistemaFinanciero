
function citas_constructor_prioridades() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_prioridad", ' +
        '"url": "prioridades/getAll/", ' +
        '"val": "PrioridadId", ' +
        '"type": "POST", ' +
        '"data": "", ' +
        '"text": "PrioridadNombre"}');
    LoadComboBox(infojson);
    $('#cod_prioridad option:contains(Cliente BAC)').attr('selected', 'selected');
    $('#cod_prioridad').trigger('change');
}

function getPublicidadTicket() {
    //LoadAnimation("body");
    var path = urljs + 'configuracion/getParametosByIdEncabezado/';
    var posting = $.post(path, { ConfigId: 'PUBTI', ConfigItemId: 'PUBTI1' });
    posting.done(function (data) {
        if (data[0].Accion) {
            $('#PubliTicket').empty().append(data[0].ConfigItemDescripcion);
        }
    });

    posting.fail(function (data, status, xhr) {
        //$('#modalParametros').modal('hide');
        //GenerarErrorAlerta(status, "error");
        //goAlert();
    });

    posting.always(function () {
        //RemoveAnimation("body");
    });
}
moment.locale('es', {
    relativeTime: {
        future: "%s",
        past: "%s",
        s: 'unos pocos segundos',
        ss: '%d segundos',
        m: "un minuto",
        mm: "%d minutos",
        h: "una hora",
        hh: "%d horas",
        d: "un día",
        dd: "%d días",
        M: "un mes",
        MM: "%d meses",
        y: "un año",
        yy: "%d años"
    }
});


function validarObligatorios(selector) {
    $(selector).each(function (index) {
        if ($(this).val() == null || $(this).val() == '' || $(this).val() == '-1') {
            $(this).parent().find('.validation-error p').text($(this).data('message')).addClass('label label-danger inline-block');
        }
        else {
            $(this).parent().find('.validation-error p').text('').removeClass('label label-danger inline-block');
        }
    });
}

$(document).ready(function () {
    jQuery.ajaxSetup({ async: false, global: false});
    checkUserAccess('ADCC010');
    var privilegio_combobox = checkUserAccessPrivilegio('ADCC0101');
    var privilegio_acciones = checkUserAccessPrivilegio('ADCC0102');
    if (privilegio_combobox == false) {
        $('#cmb_sucursal').attr('disabled','true');
    }
    else {
        $('#cmb_sucursal').removeAttr('disabled');
    }

    if (privilegio_acciones == false) {
        $('.btn_grid_acciones').attr('disabled');
        $('.btn_grid_acciones').addClass('hidden');
    }
    else {
        $('.btn_grid_acciones').removeAttr('disabled');
    }
    jQuery.ajaxSetup({ async: true, global: true});
    $('#cmb_sucursal').change(function (e) {
        var id = $(this).val();
        if (id != "-1") {
            jQuery.ajaxSetup({ async: false , global: false});
            Get_Citas_Pendientes(id, 1);
            Get_Citas_EnProceso(id, 3);
            Get_Citas_Cliente_en_Agencia(id, 2);
            Get_Clientes_atendidos(id, 5);
            GetPromedioTiempoEspera(id);
            Get_ReporteCentroAtencion();
            jQuery.ajaxSetup({ async: true, global: true });
            Get_Citas_WK(id, 4);
            $('[data-toggle="tooltip"]').tooltip(); 
            $('#div_btn_add_walkin').removeClass('hidden');
        }
        else {
            $('#div_btn_add_walkin').addClass('hidden');
        }
    });
    jQuery.ajaxSetup({ async: false, global: false });
    LoadSucursales();
    LoadSucursales2();
    citas_constructor_prioridades();
    citas_constructor_tramites();
    CheckUserInfo2();
    if (usu_sucursalId > 0)
    {
        $('#cmb_sucursal').val(usu_sucursalId).trigger('change');
    }
    else
    {
        GenerarSuccessAlerta('Su usuario no tiene sucursal asignada en el sistema', "error");
        goAlert();
    }
    jQuery.ajaxSetup({ async: true, global: true });
    function fn_change_sucursal()
    {
        $('#cmb_sucursal').trigger('change');
    }
    
    var t = setInterval(fn_change_sucursal, 120000);
    citas_constructor_segmentos();
    $('#btn_actualizar_vista').click(function () {
        $('#cmb_sucursal').trigger('change');
    })

    var hoy = moment(new Date()).format('YYYY-MM-DD');
    var hoy_time = moment(new Date()).format('YYYY-MM-DD HH:mm:ss.SSS');
    $('.numero').mask('####');
    $('.cuentaFormato').mask('################');
    $('.telefono').mask('########');
    $('.id').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Z0-9]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });



    /* ------------------------ VALIDACIONES INPUTS, SELECTS ------------------------ */
    $("input.modal_requerido[data-requerido='true']").keyup(function (e) {
        if ($(this).val().trim() != "") {
            $(this).removeClass('input-has-error');
        }
        else {
            $(this).addClass('input-has-error');
        }
    });

    $("select.modal_requerido[data-requerido='true']").on('change', function (e) {
        var objeto = $(this);
        if (objeto.val() == null || objeto.val() == '-1' || objeto.val() == '') {

        } else {

        }
    });

    $(".modal_requerido[data-requerido='true']").on('change', function (e) {
        validarObligatorios(".modal_requerido[data-requerido='true']");
    });
    $('select:not(.normal)').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent()
        });
    });
    $('#btn_agregar_wk').click(function () {
        jQuery.ajaxSetup({ async: false, global: false });

        var elementos = ['.limpiar'];
        jQuery.ajaxSetup({ async: false });
        LimpiarInput(elementos, ['']);
        limpiarMensajesValidacion($('#modal_nueva_walkin'));
        jQuery.ajaxSetup({ async: true });
        $('#citaId').val('');
        $('#txt_identificacion').val('');
        $('#txt_nombre').val('');
        $('#txt_email').val('');
        $('#txt_cel').val('');
        $('#txt_tel_oficina').val('');
        $('#txt_tel_casa').val('');
        $('#txt_tarjeta').val('');
        $('#txt_cuenta').val('');
        $("#emisorId").val('');
        $("#txt_marca").val('');
        $("#txt_producto").val('');
        $("#txt_familia").val('');
        $('#cod_sucursal').val('-1').trigger('change');
        $('#cod_tramite').val('-1').trigger('change');
        $('#cod_segmento').val('-1').trigger('change');
        $("#txt_cuenta_formato").trigger('dblclick');
        $("#txt_tarjeta_formato").trigger('dblclick');
        jQuery.ajaxSetup({ async: true, global: true });
        $('#modal_nueva_walkin').modal('show');
    });
    var inputs = ['#txt_identificacion', '#txt_nombre', '#txt_email', '#txt_cuenta', '#txt_cel', '#cod_tramite', '#cod_prioridad','cod_segmento'];
    var mensaje = ['Campo requerido', 'Campo requerido', 'Campo requerido', 'Campo requerido', 'Campo requerido', 'Campo requerido', 'Campo requerido', 'Campo requerido'];
    $('#btn_guardar_walkin').click(function () {
        try {
            var inputs = [];
            var mensaje = [];
            $('#modal_nueva_walkin .modal_requerido').each(function () {
                /*Llenamos los arreglos con la info para la validacion*/
                if ($(this).data('requerido') == true) {
                    inputs.push('#' + $(this).attr('id'));
                    mensaje.push($(this).attr('attr-message'));
                }
            });
            if (Validar(inputs, mensaje)) {
                var path = urljs + 'atencioncitas/SaveData';
                var id = $("#citaId").val();
                var hoy = moment(new Date()).format('YYYY-MM-DD');
                var hoy_time = moment(new Date()).format('YYYY-MM-DD HH:mm:ss.SSS');
                
                var cuentaEncrypted = $('#txt_cuenta').val().toString(); //CryptoJS.AES.encrypt($('#txt_cuenta').val().toString(), 'BACPANAMA');
                var tarjetaEncrypted = $('#txt_tarjeta').val().toString(); //CryptoJS.AES.encrypt($('#txt_tarjeta').val().toString(), 'BACPANAMA');
                if (id == "") {
                    id = -1;
                }
                //JSON data
                var dataType = 'application/json; charset=utf-8';
                var data = {
                    CitaId: id,
                    CitaIdentificacion: $('#txt_identificacion').val(),
                    CitaNombre: $('#txt_nombre').val(),
                    CitaCorreoElectronico: $('#txt_email').val(),
                    CitaCuenta: cuentaEncrypted.toString(),
                    CitaTarjeta: tarjetaEncrypted.toString(),
                    CitaTelefonoCelular: $('#txt_cel').val(),
                    CitaTelefonoCasa: $('#txt_tel_casa').val(),
                    CitaTelefonoOficina: $('#txt_tel_oficina').val(),
                    TramiteId: $('#cod_tramite').val(),
                    CitaFecha: hoy_time,
                    CitaHora: hoy_time,
                    PrioridadId: $('#cod_prioridad').val(),
                    SucursalId: $('#cmb_sucursal').val(),
                    SucursalIdReferido: $('#cod_sucursal').val(),
                    CitaSegmentoId: $('#cod_segmento').val(),
                    EmisorId: $("#emisorId").val()
                }
                var posting = $.post(path, data);
                posting.done(function (data, status, xhr) {
                    if (data.Accion > 1)
                    {
                        jQuery.ajaxSetup({ async: false });
                        $('#modal_nueva_walkin').modal('hide');
                        GenerarSuccessAlerta(data.Mensaje, "successModalTicketWK");
                        goAlert();
                        cita_obtener_ticket(data.Accion);
                        $('#modal_ticket_wk').modal('show');
                        $('#print_ticket_wk').printThis({
                            importCSS: true,
                            loadCSS: "../../Content/Print.css",
                            importStyle: true
                        });
                        $('#cmb_sucursal').trigger('change');
                        jQuery.ajaxSetup({ async: true });
                    }
                    else
                    {
                        GenerarErrorAlerta(data.Mensaje, "successModalTicketWK");
                        goAlert();
                    }
                    
                });
                posting.fail(function (data, status, xhr) {
                });
                //}
            }
            else
            {
                GenerarErrorAlerta("No estan llenos todos los campos requeridos", "successModalTicketWK");
                goAlert();
            }
        }
        catch (e) {
        }
    });

    $('#modal_ticket_wk').on('shown.bs.modal', function (e) {        
        setTimeout(function () {
            $('#modal_ticket_wk').modal('hide');
        }, 5000);
    });

    $('#tbody_citas_wk').on('click', '.btn_print_ticket', function () {
        jQuery.ajaxSetup({ async: false });
        cita_obtener_ticket($(this).data('cita-id'));
        $('#modal_ticket_wk').modal('show');
        $('#print_ticket_wk').printThis({
            importCSS: true,
            loadCSS: "../../Content/Print.css",
            importStyle: true
        });
        jQuery.ajaxSetup({ async: true });
    });

    $('#tbody_citas_wk').on('click', '.btn_editar_cita', function () {
        cita_obtener_citawk($(this).data('cita-id'));
        $('#modal_nueva_walkin').modal('show');
    });
    

    $('#tbody_citas_pendientes').on('click', '.btn_cliente_llego', function () {
        var path = urljs + "atencioncitas/Clientellegocita";
        var posting = $.post(path, { id: Number($(this).data('cita-id')) });
        posting.done(function (data, status, xhr) {
            if (data.Accion = 1) {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
            $('#cmb_sucursal').trigger('change');
        });
        posting.fail(function (data, status, xhr) {
        });
    });
    $('#tbody_citas_cliente_llego').on('click', '.btn_cliente_entro', function () {
        var path = urljs + "atencioncitas/Clienteentro";
        var posting = $.post(path, { id: Number($(this).data('cita-id')) });
        posting.done(function (data, status, xhr) {
            if (data.Accion = 1) {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
            $('#cmb_sucursal').trigger('change');
        });
        posting.fail(function (data, status, xhr) {
        });
    });
    $('#tbody_citas_proceso').on('click', '.btn_cliente_salio', function () {
        var path = urljs + "atencioncitas/Clientesalio";
        var posting = $.post(path, { id: Number($(this).data('cita-id')) });
        posting.done(function (data, status, xhr) {
            if (data.Accion = 1) {
                GenerarSuccessAlerta(data.Mensaje, 'success');
                goAlert();
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
            $('#cmb_sucursal').trigger('change');
        });
        posting.fail(function (data, status, xhr) {
        });
    });

    $('#tbody_citas_pendientes,#tbody_citas_proceso,#tbody_citas_cliente_llego,#tbody_citas_wk').on('click', '.btn_cliente_abandona', function () {
        var mensaje = '<h4>¿Esta seguro que el cliente: <span class="label label-primary">' + $(this).data('cita-nombre') + '</span> ha abandonado la sucursal?</h4>';
        $('#avisoMensaje').html(mensaje);
        $('#hidden_citaId').val($(this).data('cita-id'));
        $('#avisoAbandonoModal').modal('show');
    });

    /*Evento clic de boton de cliente abandona cita*/
    $('#btnComenzarCita').click(function (e) {
        var path = urljs + "atencioncitas/Clienteabandona";
        var posting = $.post(path, { id: Number($('#hidden_citaId').val()), motivo: $('#txt_observacion').val() });
        posting.done(function (data, status, xhr) {
            if (data.Accion = 1) {
                $('#avisoAbandonoModal').modal('hide');
                $('#avisoAbandonoModal').on('hidden.bs.modal', function (e) {
                    GenerarSuccessAlerta(data.Mensaje, 'success');
                    goAlert();
                    $('#txt_observacion').val('');
                });
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
            }
            $('#cmb_sucursal').trigger('change');
        });
        posting.fail(function (data, status, xhr) {
        });
    });


    $('#txt_identificacion').focusout(function () {
        if ($(this).val() != '' && $(this).val() != null)
        {
            GetCitaByClienteId($(this).val());
        }        
    });
    
});


dataCitas = [];
function LoadSucursales() {
    var infojson = jQuery.parseJSON('{"input": "#cmb_sucursal", ' +
        '"url": "/sucursales/getSucursalesByAtencion", ' +
        '"val": "SucursalId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}

function LoadSucursales2() {
    var infojson = jQuery.parseJSON('{"input": "#cod_sucursal", ' +
        '"url": "/sucursales/GetAll", ' +
        '"val": "SucursalId", ' +
        '"type": "POST", ' +
        '"data": "", ' +
        '"text": "SucursalNombre"}');
    LoadComboBox(infojson);
}


function Get_Citas_Pendientes(SucursaId, EstadoCita) {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/citas/GetCitasBySucursalyEstado";
        var posting = $.post(path, { sucursalid: SucursaId, estadocita: EstadoCita });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                LimpiarTabla('#table_citas_pendientes');
                var contador = 1;
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    if (dataCitas[i].CitaId != 0 && dataCitas[i].CitaIdentificacion != null && dataCitas[i].CitaIdentificacion != '') {
                        var newRow = $('#table_citas_pendientes').dataTable().fnAddData([
                            contador,
                            dataCitas[i].CitaIdentificacion,
                            dataCitas[i].CitaNombre,
                            dataCitas[i].tramite,
                            dataCitas[i].CitaTicket,
                            dataCitas[i].CitaHora.toString(),
                            dataCitas[i].posicionDescripcion,
                            '<button class="btn btn-sm btn-primary btn_cliente_llego btn_grid_acciones" data-cita-id="' + dataCitas[i].CitaId + '" data-toggle="tooltip" title="Cliente llegó"><i class="fa fa-get-pocket" aria-hidden="true"></i></button>' +
                            '<button class="btn btn-sm btn-danger btn_cliente_abandona btn_grid_acciones" data-cita-nombre="' + data[i].CitaNombre + '" data-cita-id="' + data[i].CitaId + '" data-toggle="tooltip" title="Cliente no llego"><i class="fa fa-sign-out" aria-hidden="true"></i></button>',
                            dataCitas[i].CitaId
                        ]);

                        var theNode = $('#table_citas_pendientes').dataTable().fnSettings().aoData[newRow[0]].nTr;
                        theNode.setAttribute('id', dataCitas[i].CitaId);
                        $('td', theNode)[8].setAttribute('class', 'CitaId hidden');
                        contador++;
                    }

                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}

function Get_Citas_EnProceso(SucursaId, EstadoCita) {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/citas/GetCitasBySucursalyEstado";
        var posting = $.post(path, { sucursalid: SucursaId, estadocita: EstadoCita });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla('#table_citas_proceso');
            if (data.length > 0) {
                var contador = 1;
                var len = data.length;
                for (var i = 0; i != len; i++) {
                    if (dataCitas[i].CitaId != 0 && dataCitas[i].CitaIdentificacion != null && dataCitas[i].CitaIdentificacion != '') {
                        var newRow = $('#table_citas_proceso').dataTable().fnAddData([
                            contador,
                            data[i].CitaNombre,
                            dataCitas[i].tramite,
                            dataCitas[i].CitaTicket,
                            moment(dataCitas[i].CitaHoraClienteIniciaAtencion).format('hh:mm a').toString(),
                            dataCitas[i].PromedioTiempoEspera + ' minuto(s)',
                            dataCitas[i].duracion + ' minuto(s)',
                            data[i].posicionDescripcion,
                            data[i].CitaId
                        ]);
                        var theNode = $('#table_citas_proceso').dataTable().fnSettings().aoData[newRow[0]].nTr;
                        theNode.setAttribute('id', data[i].CitaId);
                        $('td', theNode)[8].setAttribute('class', 'CitaId hidden');
                        contador++;
                    }                    
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}

function Get_Citas_Cliente_en_Agencia(SucursaId, EstadoCita) {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/citas/GetCitasBySucursalyEstado";
        var posting = $.post(path, { sucursalid: SucursaId, estadocita: EstadoCita });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                LimpiarTabla('#table_citas_cliente_llego');
                var contador = 1;
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    if (dataCitas[i].CitaId != 0 && dataCitas[i].CitaIdentificacion != null && dataCitas[i].CitaIdentificacion != '') {
                        var newRow = $('#table_citas_cliente_llego').dataTable().fnAddData([
                            contador,
                            dataCitas[i].CitaIdentificacion,
                            dataCitas[i].CitaNombre,
                            dataCitas[i].tramite,
                            moment(dataCitas[i].CitaHora, 'HH:mm:ss').format('hh:mm a'),
                            dataCitas[i].PromedioTiempoEspera + ' minuto(s)',
                            dataCitas[i].posicionDescripcion,/*
                            '<button class="btn btn-sm btn-primary btn_cliente_entro btn_grid_acciones" data-cita-id="' + dataCitas[i].CitaId + '" data-toggle="tooltip" title="Cliente es atendido"><i class="fa fa-sign-in" aria-hidden="true"></i></button>' +*/
                            '<button class="btn btn-sm btn-danger btn_cliente_abandona btn_grid_acciones" data-cita-nombre="' + data[i].CitaNombre + '" data-cita-id="' + data[i].CitaId + '" data-toggle="tooltip" title="Cliente Abandona"><i class="fa fa-sign-out" aria-hidden="true"></i></button>',
                            dataCitas[i].CitaId
                        ]);

                        var theNode = $('#table_citas_cliente_llego').dataTable().fnSettings().aoData[newRow[0]].nTr;
                        theNode.setAttribute('id', dataCitas[i].CitaId);
                        $('td', theNode)[8].setAttribute('class', 'CitaId hidden');
                        contador++;
                    }
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}


function cita_obtener_ticket(cita_id) {
    try {
        var path = urljs + "/atencioncitas/GetCitaTicket";
        var posting = $.post(path, { citaid: cita_id});
        posting.done(function (data, status, xhr) {
            $('#SucursalNombre').empty().append(data.SucursalNombre);
            $('#SucursalUbicacion').empty().append(data.SucursalUbicacion);
            $('#CitaTicket').empty().append(data.CitaTicket);
            $('#PrioridadNombre').empty().append(data.PrioridadNombre);
            $('#CitaHora').empty().append(data.CitaHora);
            getPublicidadTicket();
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}


function Get_Citas_WK(SucursaId, EstadoCita) {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/citas/GetCitasBySucursalyEstado";
        var posting = $.post(path, { sucursalid: SucursaId, estadocita: EstadoCita });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla('#table_citas_wk');
            if (data.length > 0) {
                var contador = 1;
                var num_clientes_fila = 0;
                var len = data.length;
                for (var i = 0; i != len; i++) {
                    if (dataCitas[i].CitaId != 0 && dataCitas[i].CitaIdentificacion != null && dataCitas[i].CitaIdentificacion != '') {
                        var newRow = $('#table_citas_wk').dataTable().fnAddData([
                            contador,
                            data[i].CitaIdentificacion,
                            data[i].CitaNombre,
                            data[i].tramite,
                            (data[i].SucursalReferido == '' ? 'N/D' : data[i].SucursalReferido),
                            data[i].CitaTicket,
                            moment(data[i].CitaHora, "HH:mm").format("hh:mm a").toString(),
                            data[i].duracion + ' minuto(s)',
                            '<button class="btn btn-sm btn-primary btn_print_ticket" btn_grid_acciones data-cita-id="' + data[i].CitaId + '" data-toggle="tooltip" data-placement="bottom" title="Imprimir Ticket"><i class="fa fa-print" aria-hidden="true"></i></button>' +
                            '<button class="btn btn-sm btn-warning btn_editar_cita btn_grid_acciones" data-cita-id="' + data[i].CitaId + '" data-toggle="tooltip" data-placement="bottom" title="Editar Cita"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button>' + 
                            '<button class="btn btn-sm btn-danger btn_cliente_abandona btn_grid_acciones" data-cita-nombre="' + data[i].CitaNombre + '" data-cita-id="' + data[i].CitaId + '" data-toggle="tooltip" data-placement="bottom" title="Cliente Abandona"><i class="fa fa-sign-out" aria-hidden="true"></i></button>',
                            data[i].CitaId
                        ]);

                        var theNode = $('#table_citas_wk').dataTable().fnSettings().aoData[newRow[0]].nTr;
                        theNode.setAttribute('id', data[i].CitaId);
                        $('td', theNode)[9].setAttribute('class', 'CitaId hidden');
                        contador++;
                        num_clientes_fila++;
                    }
                }
                $('#num_clientes_fila').empty().append(num_clientes_fila);
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}

function GetCitaByClienteId(Id) {
    try {
        var path = urljs + "/atencioncitas/GetCitaByIdentificacion";
        var posting = $.post(path, { Id: Id });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                $('#txt_nombre').val(data.CitaNombre);
                $('#txt_nombre').trigger('keyup');
                $('#txt_email').val(data.CitaCorreoElectronico);
                $('#txt_cel').val(data.CitaTelefonoCelular);
                $('#txt_tel_oficina').val(data.CitaTelefonoOficina);
                $('#txt_tel_casa').val(data.CitaTelefonoCasa);

            }
            else {
                goAlert();
                $('#txt_nombre').val('');
                $('#txt_email').val('');
                $('#txt_cel').val('');
                $('#txt_tel_oficina').val('');
                $('#txt_tel_casa').val('');
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "successModalTicketWK");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
            validarObligatorios(".modal_requerido[data-requerido='true']");
        });
    }
    catch (e) {
    }
}

function cita_obtener_citawk(Id) {
    try {
        var path = urljs + "atencioncitas/GetCitasById";
        var posting = $.post(path, { CitaId: Id });
        $('#citaId').val(Id);
        posting.done(function (data, status, xhr) {
            jQuery.ajaxSetup({ async: false });
            $('#txt_identificacion').val(data.CitaIdentificacion);
            $('#txt_nombre').val(data.CitaNombre);
            $('#txt_email').val(data.CitaCorreoElectronico);
            $('#txt_cel').val(data.CitaTelefonoCelular);
            $('#txt_tel_oficina').val(data.CitaTelefonoOficina);
            $('#txt_tel_casa').val(data.CitaTelefonoCasa);
            citas_constructor_prioridades();
            citas_constructor_tramites();
            $("#emisorId").val('');
            $("#txt_marca").val('');
            $("#txt_producto").val('');
            $("#txt_familia").val('');
            $('#cod_segmento').val('-1').trigger('change');
            $("#txt_cuenta_formato").trigger('dblclick');
            $("#txt_tarjeta_formato").trigger('dblclick');
            $('#cod_tramite').val(data.TramiteId).trigger('change');
            $('#cod_prioridad').val(data.PrioridadId).trigger('change');
            $('#cod_sucursal').val(data.SucursalIdReferido).trigger('change');
            var cuentaDecrypted = data.CitaCuenta; //CryptoJS.AES.decrypt(data.CitaCuenta, 'BACPANAMA');
            var tarjetaDecrypted = data.CitaTarjeta; //CryptoJS.AES.decrypt(data.CitaTarjeta, 'BACPANAMA');

            $('#txt_cuenta').val(cuentaDecrypted.toString()); //CryptoJS.enc.Utf8));
            $('#txt_tarjeta').val(tarjetaDecrypted.toString()).trigger('blur');//CryptoJS.enc.Utf8)).trigger('blur');
            if (data.CitaCuenta != null && data.CitaCuenta != '')
            {
                buscarEmisorCuenta($('#txt_cuenta').val());
            }            
            jQuery.ajaxSetup({ async: true });
        });
        posting.fail(function (data, status, xhr) {
        });
        posting.always(function (data, status, xhr) {
        });
    }
    catch (e) {
    }
}


function comprobar_segmento_en_sucursal(segmentoId) {
    try {
        var segmento_sucursal = 0;
        var path = urljs + "/sucursales/getSucursalesBySegmento";
        var posting = $.post(path, { segmentoId: segmentoId });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    for (var i = data.length - 1; i >= 0; i--) {
                        if (data[contador].SucursalId == $('#cmb_sucursal').val()) {
                            segmento_sucursal = 1;
                        }
                        contador++;
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "successModalTicketWK");
                    goAlert();
                }
                if (segmento_sucursal == 1) {
                }
                else {
                    GenerarErrorAlerta("Segmento no puede ser atendido en esta sucursal, favor revisar listado", "successModalTicketWK");
                    goAlert();
                }
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
    }
}

function citas_constructor_segmentos_por_sucursal(sucursalId) {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_segmento", ' +
        '"url": "sucursales/GetSegmentosBySucursalId/", ' +
        '"val": "SucSegmentoId", ' +
        '"type": "GET", ' +
        '"data": "' + sucursalId + '", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_tramites() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_tramite", ' +
        '"url": "tramites/GetAllTramitesComboBox/", ' +
        '"val": "TramiteId", ' +
        '"type": "GET", ' +
        '"data": "", ' +
        '"text": "TramiteDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_segmentos() {
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#cod_segmento", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "SEGM", ' +
        '"text": "ConfigItemDescripcion"}');
    LoadComboBox(infojson);
}

function citas_constructor_sucursales_por_segmento(segmentoId) {
    $("#cod_sucursal").empty().append(new Option('No se ha cargado información', '-1'));
    try {
        var path = urljs + "/sucursales/getSucursalesBySegmento";
        var posting = $.post(path, { segmentoId: segmentoId });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    $("#cod_sucursal").empty().append(new Option('Ninguno', '-1'));
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#cod_sucursal").append(new Option(data[contador].SucursalNombre, data[contador].SucursalId));
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
        });
    }
    catch (e) {
    }
}


$("#cod_segmento").on('change', function (e) {
    var cod_segmento = $(this).val();
    if (cod_segmento != '' && cod_segmento != '-1') {
    }
    else {
    }
});


$(".cuentaFormato").on('blur', function (e) {
    var cuenta = $(this).val();
    if ((cuenta.length < 15 || cuenta.length > 16) && cuenta.length > 0) {
        if ($(this).hasClass('cuenta')) {
            jQuery.ajaxSetup({ async: false });
            $('#cod_segmento').val('-1').trigger('change');
            jQuery.ajaxSetup({ async: true });

            $("#emisorId").val('');
            $("#txt_marca").val('');
            $("#txt_producto").val('');
            $("#txt_familia").val('');
        }
        GenerarErrorAlerta("Formato incorrecto!", "successModalTicketWK");
        goAlert();
    }
    else {
        if (cuenta.length > 0)
        {
            if ($(this).hasClass('cuenta')) {
                var emisorCuenta = cuenta.substr(0, 10);
                jQuery.ajaxSetup({ async: false });
                buscarEmisorCuenta(emisorCuenta);
                jQuery.ajaxSetup({ async: true });
            }
            else {
                var longitudTarjeta = $("#txt_tarjeta").val().length;
                var tarjetaFormateada = $("#txt_tarjeta").val().substr(0, 6) + ' **** **** ' + $("#txt_tarjeta").val().substring(longitudTarjeta - 4, longitudTarjeta);

                $("#txt_tarjeta").addClass('hide');
                $("#txt_tarjeta_formato").val(tarjetaFormateada);
                $("#txt_tarjeta_formato").removeClass('hide');
            }
        }        
    }
});

$("#txt_cuenta_formato").on('dblclick', function (e) {
    $("#txt_cuenta_formato").addClass('hide');
    $("#txt_cuenta").removeClass('hide');
    $("#txt_cuenta").focus();
});

$("#txt_tarjeta_formato").on('dblclick', function (e) {
    $("#txt_tarjeta_formato").addClass('hide');
    $("#txt_tarjeta").removeClass('hide');
    $("#txt_tarjeta").focus();
});

function buscarEmisorCuenta(EmisorCuenta) {
    try {
        var path = urljs + "/citas/CheckEmisorCuenta";
        var posting = $.post(path, { EmisorCuenta: EmisorCuenta });
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var longitudCuenta = $("#txt_cuenta").val().length;
                    var cuentaFormateada = $("#txt_cuenta").val().substr(0, 6) + ' **** **** ' + $("#txt_cuenta").val().substring(longitudCuenta - 4, longitudCuenta);
                    $("#txt_cuenta").addClass('hide');
                    $("#txt_cuenta_formato").val(cuentaFormateada);
                    $("#txt_cuenta_formato").removeClass('hide');

                    jQuery.ajaxSetup({ async: false });
                    $('#cod_segmento').val(data[0].CitaSegmentoId);
                    $('#cod_segmento').trigger('change');
                    jQuery.ajaxSetup({ async: true });
                    
                    $("#emisorId").val(data[0].EmisorId);
                    $("#txt_marca").val(data[0].MarcaTarjeta);
                    $("#txt_producto").val(data[0].Producto);
                    $("#txt_familia").val(data[0].Familia);
                }
                else {
                    jQuery.ajaxSetup({ async: false });
                    $('#cod_segmento').val('-1').trigger('change');
                    jQuery.ajaxSetup({ async: true });
                    
                    $("#emisorId").val('');
                    $("#txt_marca").val('');
                    $("#txt_producto").val('');
                    $("#txt_familia").val('');
                    GenerarErrorAlerta(data[0].Mensaje, "successModalTicketWK");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "successModalTicketWK");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            validarObligatorios(".modal_requerido[data-requerido='true']");
        });
    }
    catch (e) {
        console.log(e);
    }
}

function GetPromedioTiempoEspera(SucursalId) {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/atencioncitas/GetPromedioTiempoEspera";
        var posting = $.post(path, { id: SucursalId});
        posting.done(function (data, status, xhr) {
            $('#tiempo_espera_prom').empty().append(data.PromedioTiempoEspera)
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}

function Get_Clientes_atendidos(SucursaId, EstadoCita) {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/citas/GetCitasBySucursalyEstado";
        var posting = $.post(path, { sucursalid: SucursaId, estadocita: EstadoCita });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla('#table_clientes_atendidos');
            if (data.length > 0) {
                var contador = 1;
                var len = data.length;
                for (var i = 0; i != len; i++) {
                    if (dataCitas[i].CitaId != 0 && dataCitas[i].CitaIdentificacion != null && dataCitas[i].CitaIdentificacion != '') {
                        var newRow = $('#table_clientes_atendidos').dataTable().fnAddData([
                            contador,
                            data[i].CitaIdentificacion,
                            data[i].CitaNombre,
                            data[i].tramite,
                            data[i].CitaTicket,
                            data[i].posicionDescripcion,
                            moment(dataCitas[i].CitaHoraClienteIniciaAtencion).format('hh:mm a').toString(),
                            moment(dataCitas[i].CitaHoraClienteSaleAtencion).format('hh:mm a').toString(),
                            data[i].duracion + " minuto(s)",
                            data[i].PromedioTiempoEspera + " minuto(s)"
                        ]);

                        var theNode = $('#table_clientes_atendidos').dataTable().fnSettings().aoData[newRow[0]].nTr;
                        theNode.setAttribute('id', data[i].CitaId);
                        contador++;
                    }
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}

function Get_ReporteCentroAtencion() {
    try {
        jQuery.ajaxSetup({ async: false });
        var path = urljs + "/atencioncitas/GetReporteCentroAtencion";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla('#table_centros_fidelizacion');
            if (data.length > 0) {
                var contador = 1;
                var len = data.length;
                for (var i = 0; i != len; i++) {
                        var newRow = $('#table_centros_fidelizacion').dataTable().fnAddData([
                            contador,
                            data[i].SucursalNombre,
                            data[i].PromedioTiempoEspera + " minuto(s)",
                            data[i].NumeroClientes,
                        ]);

                        var theNode = $('#table_centros_fidelizacion').dataTable().fnSettings().aoData[newRow[0]].nTr;
                        contador++;
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
    }
    catch (e) {
    }
}

$("input.email").on('blur', function (e) {
    if ($(this).val().trim() != "") {
        if (emailValido($(this).val()) == true) {
            $(this).removeClass('input-has-error');
            $(this).parent().find('.validation-error-mail').addClass('hide');
        }
        else {
            $(this).addClass('input-has-error');
            $(this).parent().find('.validation-error-mail').removeClass('hide');
        }
    }
    else {
        $(this).addClass('input-has-error');
        $(this).parent().find('.validation-error-mail').addClass('hide');
    }
});
