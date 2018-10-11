$(document).ready(function () {
    jQuery.ajaxSetup({ async: false });
    checkUserAccess('CONF050');
    jQuery.ajaxSetup({ async: true });

    GetRazones($('#hidden_tipo_id').val());
    GetTipoRazones();

    $('.identidad').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.rtn').mask('##############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.numero').mask('####', { translation: { '#': { pattern: /[0-9,]/, optional: true } } });
    $('.correo').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[a-z-_.0-9 @]/, optional: true } } });
    $('.telefonoCelular').mask('AAAAAAAA', { translation: { 'A': { pattern: /[0-9]/, optional: true } } });
    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.fecha').mask('##########', { translation: { '#': { pattern: /[0-9-]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû#]/, optional: true } } });


    //LimpiarTabla('#tableRazones');
    $('#btnGuardar').click(function () {
        //if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
        //    existeRazon($('#descripcion_tipo_razon').val(), $('#abreviatura').val());
        //}
        //else {
            GuardarRazon();
        //}
    });
});

var inputsPrestamo = ['#montosolicitado', '#plazo', '#garantia', '#tasainteres', '#destino', '#frecuenciapago', '#observaciones', '#Personas', '#NegocioPropio', '#Salario', '#Finca', '#Otros', '#Renta', '#Servicios', '#Prestamos', '#Transporte',
    '#Alimentacion', '#Vestuario', '#Otros1', '#Observaciones1' ];

$('#btnEliminar').click(function () {
    var id = $('#hidden_id').val();
    var path = urljs + 'Razones/delete';
    var posting = $.post(path, { id: Number(id) });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetRazones($('#hidden_tipo_id').val());
            $('#modalEliminarRazon').modal('hide');
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

var inputs = ['#descripcion_tipo_razon', '#abreviatura', '#cod_tipo_razon'];
var client = 0;
function GetRazones(ClienteId, NomPrestamo) {
    try {
        client = ClienteId;
        var path = urljs + "/Prestamos/GetAll";
        var posting = $.post(path, { ClienteId: Number(ClienteId) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableRazones");
            if (data.length) {
                if (data[0].Accion >= 0) {
                    $('.titulo').empty().append('Listado de préstamos del cliente: ' + data[0].NomPrestamo);
                    client = data[0].ClienteId;
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tableRazones");
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

function GetRazonBy(id, tipo_id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Razones/GetOne";
        var posting = $.post(path, { id: Number(id), tipo_id: Number(tipo_id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#descripcion_tipo_razon').val(data.RazonDescripcion);
                $('#abreviatura').val(data.RazonAbreviatura);
                $('#cod_tipo_razon').val(data.TipoId).trigger('change');
                $('#cod_tipo_razon').attr('disabled', 'disabled');
                $('#razongroup').val(data.RazonGroup).trigger('change');
                $('#razonstatus').val(data.RazonStatus);
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

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['FecSolicitud'],
        ArrayData['MontoSolicitado'],
        ArrayData['MontoAprobado'],
        ArrayData['PlazoMeses'],
        ArrayData['Interes'],
        ArrayData['Saldo'],
        ArrayData['Estado'],
        ArrayData['Garantia'],
        "<button data-id='" + ArrayData['PrestamoId'] + "' title='Ver Préstamo' data-toggle='tooltip' onClick='GetDatosPrestamos(" + ArrayData['PrestamoId'] + ")' id='btnGenerarPago" + ArrayData['PrestamoId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Contrato' data-toggle='tooltip' onClick='GenerarContrato(" + ArrayData['PrestamoId'] + ")' id='btngenerarcontrato" + ArrayData['PrestamoId'] + "' class='btn btn-success text-center btn-sm'><i class='glyphicon glyphicon-thumbs-up'></i></button>" +
        "<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Solicitud' data-toggle='tooltip' onClick='GenerarSolicitud(" + ArrayData['PrestamoId'] + ")' id='btngenerarcontrato" + ArrayData['PrestamoId'] + "' class='btn btn-info text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        "<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Préstamo' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>" ,
        //'<a class="btn btn-success" href="@Url.Action(" exportReport","Prestamos")" > Descargar</a >',
        
        ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ClienteId']);
    $('td', theNode)[9].setAttribute('class', 'RazonId hidden');
}


function GenerarSolicitud(PrestamoId) {

    var path = urljs + "/Prestamos/SolicitudCredito";
    var posting = $.post(path, { PrestamoId: PrestamoId });
}

function GenerarContrato(PrestamoId) {
   
    var path = urljs + "/Prestamos/exportReport";
    var posting = $.post(path, { PrestamoId: PrestamoId });
}

function EditarRazon(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var tipo_id = $(e.currentTarget).attr('data-tipo_id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar | " + desc);
    $('#moda_razones').modal('show');
    $('#hidden_id').val(id);
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#razongroup", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "GRPR", ' +
        '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    $('#cod_tipo_razon').val($('#hidden_tipo_id').val()).trigger('change');
    LoadComboBox(infojson);
    GetRazonBy(id, tipo_id);
    jQuery.ajaxSetup({ async: true });
}

function EliminarRazon(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Razon | " + desc);
    $('#modalEliminarRazon').modal('show');
    $('#modalmessage').html('¿Desea eliminar la razon: <b>' + desc + '</b>?');
}

function GetTipoRazones() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTipoRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('#cod_tipo_razon').empty();
                    var contador = 0;
                    for (var i = data.length - 1; i >= 0; i--) {
                        $('#cod_tipo_razon').append(new Option(data[contador].TipoAbreviatura, data[contador].TipoId));
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

function NuevaRazon(e) {
    e.stopPropagation();
    var id = -1;
    $(".inputRequired").removeAttr("readonly");
    $('select').removeAttr("disabled");
    $("#theHeader").html("Nuevo Préstamo");
    $('#hidden_id').val("");
    $("#moda_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    var infojson = jQuery.parseJSON('{"input": "#razongroup", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "GRPR", ' +
        '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    $('#cod_tipo_razon').val($('#hidden_tipo_id').val()).trigger('change');
    LoadComboBox(infojson);
    $('#razonstatus').val('ACT');
    $('#moda_razones').modal('show');
}

function GuardarRazon() {
    try {
        var inputs = [];
        var mensaje = [];

        /*Recorremos el contenedor para obtener los valores*/
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            inputs.push('#' + $(this).attr('id'));
            mensaje.push($(this).attr('attr-message'));
        });
        
        //if (Validar(inputs, mensaje)) {

            var path = urljs + 'Prestamos/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {

                // DATOS PRESTAMO
                ClienteId: client,
                MontoSolicitado: $('#montosolicitado').val(),
                PlazoMeses: $('#plazo').val(),
                Garantia: $('#garantia option:selected').val(),
                Interes: $('#tasainteres').val(),
                Destino: $('#destino option:selected').val(),
                FrecPago: $('#frecuenciapago option:selected').val(),
                Observaciones: $('#observaciones').val(),

                // DATOS ESTDIO SOCIOECONOMICO 
                Personas: $('#Personas').val(),
                NegocioPropio: $('#NegocioPropio').val(),
                Salario: $('#Salario').val(),
                Finca: $('#Finca').val(),
                Otros: $('#Otros').val(),
                Renta: $('#Renta').val(),
                ServicioPublico: $('#Servicios').val(),
                Prestamo: $('#Prestamos').val(),
                Transporte: $('#Transporte').val(),
                Alimentacion: $('#Alimentacion').val(),
                Vestuario: $('#Vestuario').val(),
                Otros1: $('#Otros1').val(),
                Observaciones1: $('#Observaciones1').val(),
            }

            var posting = $.post(path, data);

            posting.done(function (data, status, xhr) {
                console.log(data);
                $('#moda_razones').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetRazones($('#hidden_tipo_id').val());
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });


        //}
    } catch (e) {
        console.log(e);
    }
}

//TOTAL INGRESOS ESTUDIO SOCIOECONOMICO
$('input').change(function (e) {

    var NegocioPropio = parseInt($('#NegocioPropio').val());
    var Salario = parseInt($('#Salario').val());
    var Finca = parseInt($('#Finca').val());
    var OtrosIngresos = parseInt($('#Otros').val());
    var TotalIngresos = NegocioPropio + Salario + Finca + OtrosIngresos;
    
    $('#TotalIngreso').val(TotalIngresos); 

});

//TOTAL EGRESOS ESTUDIO SOCIOECONOMICO
$('input').change(function (e) {

    var Renta = parseInt($('#Renta').val());
    var Servicios = parseInt($('#Servicios').val());
    var Prestamos = parseInt($('#Prestamos').val());
    var Transporte = parseInt($('#Transporte').val());
    var Alimentacion = parseInt($('#Alimentacion').val());
    var Vestuario = parseInt($('#Vestuario').val());
    var OtrosEgresos = parseInt($('#Otros1').val());
    var TotalEgresos = Renta + Servicios + Prestamos + Transporte + Alimentacion + Vestuario + OtrosEgresos;

    $('#Egresos').val(TotalEgresos);

});



//CAPACIDAD DE PAGO ESTUDIO SOCIOECONOMICO
$('input').change(function (e) {

    var TotalIngreso = parseInt($('#TotalIngreso').val());
    var TotalEgreso = parseInt($('#Egresos').val());
    var CapacidadPago = TotalIngreso - TotalEgreso;

    $('#CapPago').val(CapacidadPago);

});

function existeRazon(descripcion, abreviatura) {
    var resultado = false;
    try {
        var path = urljs + "/Razones/CheckOne";
        var posting = $.post(path, { descripcion: descripcion, abreviatura: abreviatura });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#moda_razones').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
                console.log('Res1: ' + resultado);
            }
            else {
                GuardarRazon();
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

//ACCION DE LA TABLA VER DATOS DEL PRESTAMO

function GetDatosPrestamos(id) {
    //$(".inputRequired").setAttribute("readonly", false);
    //$('select').setAttribute("disabled", false);
    //$("#btnGeneraReciboPagoPrestamo").addClass("disabled");

    $('#theHeader').html("Ver Préstamo");
    try {
        
        LimpiarInput(inputsPrestamo, ['']);
        var path = urljs + "/Prestamos/GetDatosPrestamos";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {

                    //DATOS DE PRESTAMO
                    $('#Codigo').val(dataCitas[i].Codigo);
                    $('#montosolicitado').val(dataCitas[i].MontoSolicitado);
                    $('#plazo').val(dataCitas[i].PlazoMeses);
                    $('#garantia').val(dataCitas[i].Garantia);
                    //$('#garantia option:selected').val(dataCitas[i].Garantia);
                    $('#tasainteres').val(dataCitas[i].Interes);
                    $('#destino').val(dataCitas[i].Destino);
                    $('#frecuenciapago').val(dataCitas[i].FrecPago);
                    $('#observaciones').val(dataCitas[i].Observaciones);

                    //DATOS DEL ESTUDIO SOCIOECONOMICO
                    $('#Personas').val(dataCitas[i].Personas);
                    $('#NegocioPropio').val(dataCitas[i].NegocioPropio);
                    $('#Salario').val(dataCitas[i].Salario);
                    $('#Finca').val(dataCitas[i].Finca);
                    $('#Otros').val(dataCitas[i].Otros);
                    $('#Renta').val(dataCitas[i].Renta);
                    $('#Servicios').val(dataCitas[i].ServicioPublico);
                    $('#Prestamos').val(dataCitas[i].Prestamo);
                    $('#Transporte').val(dataCitas[i].Transporte);
                    $('#Alimentacion').val(dataCitas[i].Alimentacion);
                    $('#Vestuario').val(dataCitas[i].Vestuario);
                    $('#Otros1').val(dataCitas[i].Otros1);
                    $('#Observaciones1').val(dataCitas[i].Observaciones1);

                    var Renta = parseInt($('#Renta').val());
                    var Servicios = parseInt($('#Servicios').val());
                    var Prestamos = parseInt($('#Prestamos').val());
                    var Transporte = parseInt($('#Transporte').val());
                    var Alimentacion = parseInt($('#Alimentacion').val());
                    var Vestuario = parseInt($('#Vestuario').val());
                    var OtrosEgresos = parseInt($('#Otros1').val());
                    var TotalEgresos = Renta + Servicios + Prestamos + Transporte + Alimentacion + Vestuario + OtrosEgresos;

                    $('#Egresos').val(TotalEgresos);

                    var NegocioPropio = parseInt($('#NegocioPropio').val());
                    var Salario = parseInt($('#Salario').val());
                    var Finca = parseInt($('#Finca').val());
                    var OtrosIngresos = parseInt($('#Otros').val());
                    var TotalIngresos = NegocioPropio + Salario + Finca + OtrosIngresos;

                    $('#TotalIngreso').val(TotalIngresos);

                    var TotalIngreso = parseInt($('#TotalIngreso').val());
                    var TotalEgreso = parseInt($('#Egresos').val());
                    var CapacidadPago = TotalIngreso - TotalEgreso;

                    $('#CapPago').val(CapacidadPago);

                   
                }
            }
            $('#moda_razones').modal('show');

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
        //RemoveAnimation("body");

    }
}

$("#btnPlanPago").on('click', function (e) {
    var CodPrest = $('#Codigo').val();
    var monto = 100000;
    var tasa = 19.0;
    var plazo = 12;
    var frecuency = 'Q';
    var fecha = '2018-07-22';
    var tipopres = "cd";
    GetPlanPago(CodPrest);

});

//Plan de pago.
function GetPlanPago(CodPrest) {
    try {
        var path = urljs + "/citas/GetPlanPagos";
        var posting = $.post(path, { CodPrest: CodPrest }); //Aqui se ponen las variables

        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#tablePlanPago");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        if (data[i].flag_historico == 0) {
                            addRowCitaPendiente(data[i], "#tablePlanPago", counter);
                        }
                        else {
                            addRowPlanPago(data[i], "#tablePlanPago", counter);
                        }
                        counter++;
                    }
                    //$("#texto_busqueda_encontrado").text(Id);
                    //activarTab('resultado_busqueda_encontrado');
                }
                else {
                    //$("#texto_busqueda").text(Id);
                    //activarTab('resultado_busqueda_ninguno');
                    /*GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
            }
            else {
                //$("#texto_busqueda").text(Id);
                //activarTab('resultado_busqueda_ninguno');
            }
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
    $('#plan_pago').modal('show');
}

function addRowPlanPago(ArrayData, tableID, counter) //Aqui se cambia y modifica Ver cita.*********************************************************
{
    var newRow = $(tableID).dataTable().fnAddData([

        ArrayData['Num'],
        ArrayData['Fecha'],
        ArrayData['Capital'],
        ArrayData['Interes'],
        ArrayData['Total'],
        ArrayData['Saldo'],
        ArrayData['Codigo']

    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['Codigo']);
    $('td', theNode)[6].setAttribute('class', 'Codigo hidden');
}

//TRANSACCIONES DE CREDITOS
$("#btnTransPrestamos").on('click', function (e) {
    var codigo = $('#Codigo').val();
    //if ($("#txt_prestamo").val() != '') {
    GetTransaccionesPrestamos(codigo);
    // GetTransaccionesPrestamos($("#txt_prestamo").val());
    //}
});

function GetTransaccionesPrestamos(codigo) {
    try {
        var path = urljs + "/Clientes/GetTransaccionesPrestamos";
        var posting = $.post(path, { codigo: codigo });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#tableCitas");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = 0; i < data.length; i++) {
                        //for (var i = data.length - 1; i >= 0; i--) {
                        addRowTransaccionesPrestamo(data[i], "#tableCitas", counter);
                        counter++;
                    }
                    //$("#texto_busqueda_encontrado").text(Id);
                    //activarTab('resultado_busqueda_encontrado');
                }
                else {
                    //$("#texto_busqueda").text(Id);
                    //activarTab('resultado_busqueda_ninguno');
                    /*GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
            }
            else {
                //$("#texto_busqueda").text(Id);
                //activarTab('resultado_busqueda_ninguno');
            }
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
    $('#Transacciones_Prestamo').modal('show');
}

//LLENA LA LISTA DE TRANSACCIONES DE PRESTAMOS
function addRowTransaccionesPrestamo(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['Fecha'],
        ArrayData['TRP_Otorgado'],
        ArrayData['TRP_Capital'],
        ArrayData['TRP_Interes'],
        ArrayData['TRP_Mora'],
        ArrayData['TRP_Agrego'],
        ArrayData['Saldo'],
        //estadosArray[parseInt(ArrayData['PRES_Estado'])],
        //accionesHTML,
        ArrayData['TRP_Codigo_Prestamo']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['TRP_Codigo_Prestamo']);
    $('td', theNode)[8].setAttribute('class', 'TRP_Codigo_Prestamo hidden');
}
