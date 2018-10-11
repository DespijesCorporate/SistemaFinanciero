var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {

    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.fecha').mask('##########', { translation: { '#': { pattern: /[0-9-]/, optional: true } } });

});





$("#btnAgregarRegCajero").on('click', function (e) {
    AgregaRegCajero();
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
});

function AgregaRegCajero() {
    try {
        var path = urljs + "/Caja/AgregaRegCajero";
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

$("#btnBuscarRegCajero").on('click', function (e) {
    $("#btnCierreCaja").addClass("hidden");
    $("#btnGuardarDatosArqueo").addClass("hidden");
    if ($("#FechaCajero").val() != '') {
        GetDatosCajero($("#FechaCajero").val());
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
    }
});

function GetDatosCajero(fecha) {
    var path = urljs + "/Caja/GetDatosCajero";
    var posting = $.post(path, {fecha: fecha });
    posting.done(function (data, status, xhr) {
        dataCitas = [];
        dataCitas = data;
        //e.stopPropagation();
        //$('#theHeader').html("Gestión de Solicitudes");
        if (dataCitas.length > 0) {
            for (var i = dataCitas.length - 1; i >= 0; i--) {
                //if (dataCitas[i].CA_Codigo_Cajero == cajero) {
                    $('#InicioDia').val(dataCitas[i].CA_Valor_Inicial_Dia);
                    $('#RecEfectivo').val(dataCitas[i].CA_Valor_Recib);
                    $('#EntrEfectivo').val(dataCitas[i].CA_Valor_Entrega);
                    $('#EntrTesorero').val(dataCitas[i].CA_Valor_Entrega_Supervisor);
                    $('#RecTesorero').val(dataCitas[i].CA_Valor_Recib_Supervisor);
                    $('#SaldoCaja').val(dataCitas[i].SaldoCaja);
                    $('#CA_B_1').val(dataCitas[i].CA_B_1);
                    $('#MontoB1').val(dataCitas[i].CA_B_1*1);
                    $('#CA_B_2').val(dataCitas[i].CA_B_2);
                    $('#MontoB2').val(dataCitas[i].CA_B_2*2);
                    $('#CA_B_5').val(dataCitas[i].CA_B_5);
                    $('#MontoB5').val(dataCitas[i].CA_B_5*5);
                    $('#CA_B_10').val(dataCitas[i].CA_B_10);
                    $('#MontoB10').val(dataCitas[i].CA_B_10*10);
                    $('#CA_B_20').val(dataCitas[i].CA_B_20);
                    $('#MontoB20').val(dataCitas[i].CA_B_20*20);
                    $('#CA_B_50').val(dataCitas[i].CA_B_50);
                    $('#MontoB50').val(dataCitas[i].CA_B_50*50);
                    $('#CA_B_100').val(dataCitas[i].CA_B_100);
                    $('#MontoB100').val(dataCitas[i].CA_B_100*100);
                    $('#CA_B_500').val(dataCitas[i].CA_B_500);
                    $('#MontoB500').val(dataCitas[i].CA_B_500*500);
                    $('#CA_M_1').val(dataCitas[i].CA_M_1);
                    $('#MontoM1').val(dataCitas[i].CA_M_1*0.01);
                    $('#CA_M_2').val(dataCitas[i].CA_M_2);
                    $('#MontoM2').val(dataCitas[i].CA_M_2*0.02);
                    $('#CA_M_5').val(dataCitas[i].CA_M_5);
                    $('#MontoM5').val(dataCitas[i].CA_M_5*0.05);
                    $('#CA_M_10').val(dataCitas[i].CA_M_10);
                    $('#MontoM10').val(dataCitas[i].CA_M_10*0.10);
                    $('#CA_M_20').val(dataCitas[i].CA_M_20);
                    $('#MontoM20').val(dataCitas[i].CA_M_20*0.20);
                    $('#CA_M_50').val(dataCitas[i].CA_M_50);
                    $('#MontoM50').val(dataCitas[i].CA_M_50 * 0.50);
                    $('#TotalMonedas').val(parseFloat($('#MontoM1').val()) +
                    parseFloat($('#MontoM2').val()) + parseFloat($('#MontoM5').val()) +
                    parseFloat($('#MontoM10').val()) + parseFloat($('#MontoM20').val()) +
                    parseFloat($('#MontoM50').val()));
                    $('#TotalBilletes').val(parseInt($('#MontoB1').val()) +
                    parseInt($('#MontoB2').val()) + parseInt($('#MontoB5').val()) +
                    parseInt($('#MontoB10').val()) + parseInt($('#MontoB20').val()) +
                    parseInt($('#MontoB50').val()) + parseInt($('#MontoB100').val()) +
                    parseInt($('#MontoB500').val()));
                    $('#TotalDinero').val(parseInt($('#TotalBilletes').val()) +
                    parseFloat($('#TotalMonedas').val()));
                    $('#CA_Secuencia').val(dataCitas[i].CA_Secuencia);
                    $('#CA_Cajero_Estado').val(dataCitas[i].CA_Cajero_Estado);

                //}
            }
            //activarTab('ver_cita');
        }
        //$('#ver_cita').modal('show');
    });
posting.fail(function (data, status, xhr) {
    GenerarErrorAlerta(xhr, "error");
    goAlert();
});
posting.always(function (data, status, xhr) {
    $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
});
  
}

$("#btnBuscarTransCajero").on('click', function (e) {
    //var cajero = 'jbonilla';
    if ($("#CA_Secuencia").val() != '') {
    GetPlanPago($("#FechaCajero").val());
    }
});

//SE AGREGA NUEVA FUNCIÓN.
function GetPlanPago(fecha) {
    try {
        var path = urljs + "/Caja/GetTransaccionesCajero";
        var posting = $.post(path, { fecha: fecha }); //Aqui se ponen las variables

        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#TransCajero");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        if (data[i].flag_historico == 0) {
                            addRowCitaPendiente(data[i], "#TransCajero", counter);
                        }
                        else {
                            addRowTransCajero(data[i], "#TransCajero", counter);
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
    $('#Transac_Cajero').modal('show');
}

function addRowTransCajero(ArrayData, tableID, counter) //Aqui se cambia y modifica Ver cita.*********************************************************
{
    var newRow = $(tableID).dataTable().fnAddData([

        ArrayData['TC_Numero'],
        ArrayData['TC_Cajero'],
        ArrayData['TC_Fecha'],
        ArrayData['TC_Mto_Entrega'],
        ArrayData['TC_Mto_Recib']

    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('Num', ArrayData['TC_Numero']);
    $('td', theNode)[5];
    //$('td', theNode)[5].setAttribute('class', 'TC_Numero');
}

//TRANSACCIONES DE CAJERO

$("#btnAgregarTransCajero").on('click', function (e) {
    if ($("#CA_Cajero_Estado").val() != 1) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Transacciones de Cajero");
    $('#hidden_id').val("");
    $("#AgregaTransCajero").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#AgregaTransCajero').modal('show');
    $("#btnGeneraTrans").removeClass("disabled");
    }
    else {
        alert("No puede realizar transacciones de cajero cuando la caja ya fue cerrada para este dia.");
    }
});


$("#btnGeneraTrans").on('click', function (e) {
    //var Cajero = ((string)(Session["usuario"]))
    //if ($("#PrestamoId").val() != '') {
    GeneraTransCajero($("#Monto").val(), $("#TipoTransCaja").val());
    $("#btnGeneraTrans").addClass("disabled");
    //$("#btnGeneraRecibo").removeClass("disabled");  
    //}
});

function GeneraTransCajero(Monto, Tipo, Cajero) {
    try {
        var path = urljs + "/Caja/GeneraTransCajero";
        var posting = $.post(path, { Monto: Monto, Tipo:Tipo, Cajero:Cajero });
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

//Iteración de billetes
$('#CA_B_1').change(function (e) {
    var valor = $(this).val();
    $('#MontoB1').val(valor * 1);
});

$('#CA_B_2').change(function (e) {
    var valor = $(this).val();
    $('#MontoB2').val(valor * 2);
});

$('#CA_B_5').change(function (e) {
    var valor = $(this).val();
    $('#MontoB5').val(valor * 5);
});

$('#CA_B_10').change(function (e) {
    var valor = $(this).val();
    $('#MontoB10').val(valor * 10);
});

$('#CA_B_20').change(function (e) {
    var valor = $(this).val();
    $('#MontoB20').val(valor * 20);
});

$('#CA_B_50').change(function (e) {
    var valor = $(this).val();
    $('#MontoB50').val(valor * 50);
});

$('#CA_B_100').change(function (e) {
    var valor = $(this).val();
    $('#MontoB100').val(valor * 100);
});

$('#CA_B_500').change(function (e) {
    var valor = $(this).val();
    $('#MontoB500').val(valor * 500);
});

//Total Billetes
$('input').change(function (e) {
  
    $('#TotalBilletes').val(parseInt($('#MontoB1').val()) +
        parseInt($('#MontoB2').val()) + parseInt($('#MontoB5').val()) +
        parseInt($('#MontoB10').val()) + parseInt($('#MontoB20').val()) +
        parseInt($('#MontoB50').val()) + parseInt($('#MontoB100').val()) +
        parseInt($('#MontoB500').val()));
});

//Iteración de monedas

$('#CA_M_1').change(function (e) {
    var valor = $(this).val();
    $('#MontoM1').val(valor * 0.01);
});

$('#CA_M_2').change(function (e) {
    var valor = $(this).val();
    $('#MontoM2').val(valor * 0.02);
});

$('#CA_M_5').change(function (e) {
    var valor = $(this).val();
    $('#MontoM5').val(valor * 0.05);
});

$('#CA_M_10').change(function (e) {
    var valor = $(this).val();
    $('#MontoM10').val(valor * 0.10);
});

$('#CA_M_20').change(function (e) {
    var valor = $(this).val();
    $('#MontoM20').val(valor * 0.20);
});

$('#CA_M_50').change(function (e) {
    var valor = $(this).val();
    $('#MontoM50').val(valor * 0.50);
});

////Total Moneda
$('input').change(function (e) {

    $('#TotalMonedas').val(parseFloat($('#MontoM1').val()) +
        parseFloat($('#MontoM2').val()) + parseFloat($('#MontoM5').val()) +
        parseFloat($('#MontoM10').val()) + parseFloat($('#MontoM20').val()) +
        parseFloat($('#MontoM50').val()));
});

////Total Dinero
$('input').change(function (e) {

    $('#TotalDinero').val(parseInt($('#TotalBilletes').val()) +
                    parseFloat($('#TotalMonedas').val()));
});

var bandera=0;
$("#btnArqueoCaja").on('click', function (e) {
    //AgregaRegCajero();
    if ($("#CA_Cajero_Estado").val() != 1) {
        if ($("#CA_Secuencia").val() != '') {
        if (bandera==0)
        {
        $("#btnCierreCaja").removeClass("hidden");
        $("#btnGuardarDatosArqueo").removeClass("hidden");
        $(".inputRequired").removeAttr("disabled");
        bandera=1
        }
        else
        {
            $("#btnCierreCaja").addClass("hidden");
            $("#btnGuardarDatosArqueo").addClass("hidden");
            $(".inputRequired").disabled = true;
            //$(".inputRequired").setattr("disabled");
            bandera = 0
        }
            //$("#btnRechazar").addClass("disabled");
        }
            }
            else
            {
                alert("No puede realizar modificaciones en los registros cuando la caja ya fue cerrada para la fecha actual.");
            }   
});

function disableBtn() {
    $(".inputRequired").disabled = false;
}

$("#btnGuardarDatosArqueo").on('click', function (e) {
    if ($("#CA_Secuencia").val() != '') {
    GuardarDatosArqueo($("#CA_Secuencia").val(),
                       $("#CA_B_1").val(),
                       $("#CA_B_2").val(),
                       $("#CA_B_5").val(),
                       $("#CA_B_10").val(),
                       $("#CA_B_20").val(),
                       $("#CA_B_50").val(),
                       $("#CA_B_100").val(),
                       $("#CA_B_500").val(),
                       $("#CA_M_1").val(),
                       $("#CA_M_2").val(),
                       $("#CA_M_5").val(),
                       $("#CA_M_10").val(),
                       $("#CA_M_20").val(),
                       $("#CA_M_50").val());
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
    }
});

function GuardarDatosArqueo(CA_Secuencia, CA_B_1, CA_B_2, CA_B_5, CA_B_10, CA_B_20, CA_B_50, CA_B_100, CA_B_500, CA_M_1, CA_M_2, CA_M_5, CA_M_10, CA_M_20, CA_M_50) {
    try {
        var path = urljs + "/Caja/GuardarDatosArqueo";
        var posting = $.post(path, { CA_Secuencia: CA_Secuencia, CA_B_1: CA_B_1, CA_B_2: CA_B_2, CA_B_5: CA_B_5, CA_B_10: CA_B_10, CA_B_20: CA_B_20, CA_B_50: CA_B_50, CA_B_100: CA_B_100, CA_B_500: CA_B_500, CA_M_1: CA_M_1, CA_M_2: CA_M_2, CA_M_5: CA_M_5, CA_M_10: CA_M_10, CA_M_20: CA_M_20, CA_M_50: CA_M_50 });
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

$("#btnCierreCaja").on('click', function (e) {
    var Diferencia;
    var Mensaje;
    var txt;

    if ($("#CA_Cajero_Estado").val() != 1) {
    if (confirm("Esta seguro en realizar el Cierre de Caja para este dia?")) {
            Diferencia = parseFloat($("#TotalDinero").val()) - parseInt($("#SaldoCaja").val());

            if (Diferencia < 0) {
                alert("Faltante en Caja de" + " " + Diferencia);
                //Mensaje = 'Faltante en Caja';
            }
            else
                if (Diferencia > 0) {
                    alert("Sobrante en Caja" + " " + Diferencia);
                    //Mensaje = 'Sobrante en Caja';
                }
                else
                    if (Diferencia == 0) {
                        alert("Caja Cuadrada");
                        //Mensaje = 'Caja Cuadrada'
                    }
            CierreCaja($("#CA_Secuencia").val(), Diferencia)
    } else {
        txt = "You pressed Cancel!";
        }
    } else {
        alert("El cierre de caja para este dia ya fue realizado, Proceso Cancelado");
    }
    
});

function CierreCaja(Secuencia, Diferencia) {
    try {
        var path = urljs + "/Caja/CierreCaja";
        var posting = $.post(path, { Secuencia: Secuencia, Diferencia: Diferencia });
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
