
import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NotifierService } from "angular-notifier";
import { fromEvent } from "rxjs";
import { debounceTime, distinctUntilChanged, map, mergeMap } from "rxjs/operators";
import { MovimientoCajaDiariaDTO } from "src/app/classes/dto/cajas/movimiento-caja-diaria.dto";
import { MovimientoCajaFuerteDTO } from "src/app/classes/dto/cajas/movimiento-caja-fuerte.dto";
import { ClienteDTO } from "src/app/classes/dto/clientes/cliente.dto";
import { ApiResult } from "src/app/classes/dto/shared/api-result.dto";
import { AutocompleteResultDTO } from "src/app/classes/dto/shared/autocomplete-result.dto";
import { CRUDView } from "src/app/classes/views/crud-view.classes";
import { ConfirmDialogService } from "src/app/components/confirm-dialog/confirm-dialog.service";
import { ApiService } from "src/app/services/api.service";
import { AuthService } from "src/app/services/auth.service";

@Component({
  selector: 'app-caja-diaria-new-crud',
  styleUrls: ['./caja-diaria-new.component.css'],
  templateUrl: './caja-diaria-new.component.html'
})

export class CajaDiariaNewComponent implements OnInit {
  crud: CRUDView<MovimientoCajaDiariaDTO>;
  clientesSearch: ClienteDTO[];
  general_cliente: string;
  cliente_saldo_deudor: number;

  constructor(private apiService: ApiService,
              private authService: AuthService,
              private routerService: Router,
              private routeService: ActivatedRoute,
              private notifierService: NotifierService,
              private confirmDialogService: ConfirmDialogService) {

    this.crud = new CRUDView<MovimientoCajaDiariaDTO>(routeService);
    this.crud.model = new MovimientoCajaDiariaDTO();
    this.crud.model.tipo = "";
    this.crud.model.medio_de_pago = "Efectivo";
    this.crud.model.usuarioNombre = authService.getCurrentUser().first_name;
    this.cliente_saldo_deudor = null;
  }

  onClienteSearchSelectItem (cliente: ClienteDTO) {
    this.general_cliente = cliente.tipoDocumento + " " + (cliente.numeroDocumento || "") + " /// " + (cliente.esEmpresa ? (cliente.razonSocial || "") : (cliente.nombre || "") + " " + (cliente.apellido || "")) + " /// " + cliente.domicilio + ", " + (cliente.localidad || "");
    this.crud.model.cliente_encrypted_id = cliente.encrypted_id;
    this.closeClienteSearchPopUp();
    this.getSaldoDeudor();
  }

  getSaldoDeudor() {
    this.apiService.DoGET<ApiResult<number>>("clientes/saldos/deudor?encryptedId=" + encodeURIComponent(this.crud.model.cliente_encrypted_id), /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {
          this.cliente_saldo_deudor = response.data;
          if (this.cliente_saldo_deudor <= 0) {
            this.notifierService.notify('success', 'El cliente NO presenta saldo deudor.');
          }
          else {
            this.notifierService.notify('warning', 'El cliente presenta saldo deudor.');
          }
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });
  }

  closeClienteSearchPopUp() {
    setTimeout(() => { this.clientesSearch = undefined; }, 200);
  }

  onClienteSearchKeyUp(event) {
    let observable = fromEvent(event.target, 'keyup')
      .pipe (
        map(value => event.target.value),
        debounceTime(500),
        distinctUntilChanged(),
        mergeMap((search) => {
          return this.apiService.DoGETWithObservable("clientes/search?filter=" + encodeURIComponent(search), /* headers */ null);
        })
      )
    observable.subscribe((data) => {
      var result = <ApiResult<AutocompleteResultDTO<ClienteDTO>>>data;
      if (!result.success) {
        this.confirmDialogService.showError("Se ha producido un error interno.");
      }
      else {
        this.clientesSearch = result.data.results;
      }
    });
  }

  onCancelClick() {
    this.confirmDialogService.showConfirm("¿Descartar cambios?", function() {
      window.history.back();
    });
  }

  onFiltroEstadoChange(newValue: string) {
    this.crud.model.turno = newValue;

  }






  onSaveClick() {
    //if (this.crud.model.tipo === "")
   // {
    //  this.confirmDialogService.showError("Debes seleccionar el Tipo de movimiento.");
    //  return;
    //}

    if (this.crud.model.importe4 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Registradora 1.");
      return;
    }


    if (this.crud.model.importe1 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Registradora 2.");
      return;
    }


    if (this.crud.model.importe2 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Factura A.");
      return;
    }


    if (this.crud.model.importe3 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Vales.");
      return;
    }


    if (this.crud.model.importe7 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Gastos.");
      return;
    }

    if (this.crud.model.importe6 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Cheques.");
      return;
    }

    if (this.crud.model.importe8 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Tarjeta/M.Pago.");
      return;
    }




    if (this.crud.model.importe11 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Efectivo.");
      return;
    }

    if (this.crud.model.importe12 === undefined)
    {
      this.confirmDialogService.showError("Debes ingresar el monto de Compras.");
      return;
    }


    if (this.crud.model.turno === undefined)
  {
    this.confirmDialogService.showError("Debes ingresar el Turno del cierre.");
    return;
  }

    if (this.crud.model.importe <= 0)
    {
      this.confirmDialogService.showError("Debes ingresar un monto válido.");
      return;
    }

    if (this.crud.model.esCtaCte) {
      if (this.crud.model.cliente_encrypted_id === undefined || this.crud.model.cliente_encrypted_id === null || this.crud.model.cliente_encrypted_id.length === 0)
      {
        this.confirmDialogService.showError("Debes seleccionar el Cliente a cancelar saldo.");
        return;
      }
    }

    //if (this.crud.model.observaciones === undefined || this.crud.model.observaciones === "")
    //{
      //this.confirmDialogService.showError("Debes ingresar una observación.");
      //return;
    //}
    this.crud.model.observaciones = 'Registradora1 // ' + this.crud.model.turno;
    this.crud.model.tipo = 'C';
    this.crud.model.importe = this.crud.model.importe4;
    this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });
      this.crud.model.importe = this.crud.model.importe1;
      this.crud.model.observaciones = 'Registradora2 // ' + this.crud.model.turno;
      this.crud.model.tipo = 'C';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });



      this.crud.model.importe = this.crud.model.importe2;
      this.crud.model.observaciones = 'FacturaA // ' + this.crud.model.turno;
      this.crud.model.tipo = 'C';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });



      this.crud.model.importe = this.crud.model.importe3;
      this.crud.model.observaciones = 'Vales // ' + this.crud.model.turno;
      this.crud.model.tipo = 'D';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });


      this.crud.model.importe = this.crud.model.importe12;
      this.crud.model.observaciones = 'Compras // ' + this.crud.model.turno;
      this.crud.model.tipo = 'D';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });




      this.crud.model.importe = this.crud.model.importe7;
      this.crud.model.observaciones = 'Gastos // ' + this.crud.model.turno;
      this.crud.model.tipo = 'D';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });

      this.crud.model.importe = this.crud.model.importe8;
      this.crud.model.observaciones = 'Tarjeta/M.Pago// ' + this.crud.model.turno;
      this.crud.model.tipo = 'D';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });




      this.crud.model.importe = this.crud.model.importe11;
      this.crud.model.observaciones = 'Efectivo// ' + this.crud.model.turno;
      this.crud.model.tipo = 'D';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });



      this.crud.model.importe = this.crud.model.importe6;
      this.crud.model.observaciones = 'Cheques// ' + this.crud.model.turno;
      this.crud.model.tipo = 'D';
      this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });

      this.crud.model.importe = this.crud.model.importe4 + this.crud.model.importe1 + this.crud.model.importe2 - this.crud.model.importe3 - this.crud.model.importe7 - this.crud.model.importe8 - this.crud.model.importe6 - this.crud.model.importe11;
      this.crud.model.observaciones = 'Diferencias // ' + this.crud.model.turno;
      if (this.crud.model.importe >0)
    {
      this.crud.model.tipo = 'D';
    }else
    { this.crud.model.tipo = 'C';}
    this.apiService.DoPOST<ApiResult<MovimientoCajaDiariaDTO>>("cajas/diaria/save", this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {

          this.routerService.navigate(['/cajas/diaria']);
        }
      },
      (errorMessage) => {
        this.confirmDialogService.showError(errorMessage);
      });




    //--------------------CAJA FUERTE ---------------------------//
    // tslint:disable-next-line:max-line-length
    this.crud.model.importe = this.crud.model.importe4 + this.crud.model.importe1 + this.crud.model.importe2 - this.crud.model.importe;
    if (this.crud.model.importe >0)
    {
      this.crud.model.tipo = 'C';
    }

    this.crud.model.observaciones = 'Cierre General // ' + this.crud.model.turno;

    this.apiService.DoPOST<ApiResult<MovimientoCajaFuerteDTO>>('cajas/fuerte/save', this.crud.model, /*headers*/ null,
      (response) => {
        if (!response.success) {
          this.confirmDialogService.showError(response.message);
        }
        else {
          this.notifierService.notify('success', 'Movimiento guardado correctamente.');
          this.routerService.navigate(['/cajas/fuerte']);
        }
      },
      (errorMessage) => {
       // this.confirmDialogService.showError(errorMessage);
      });
  }

  ngOnInit(): void {

    setTimeout(function() {
      (<any>$("#title-crud").find('[data-toggle="tooltip"]')).tooltip();
    }, 300);

  }

}
