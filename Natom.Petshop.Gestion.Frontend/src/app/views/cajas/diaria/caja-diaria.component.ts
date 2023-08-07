import { HttpClient } from "@angular/common/http";
import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { NotifierService } from "angular-notifier";
import { MarcaDTO } from "src/app/classes/dto/marca.dto";
import { MovimientoCajaDiariaDTO } from "src/app/classes/dto/cajas/movimiento-caja-diaria.dto";
import { DataTableDTO } from "src/app/classes/data-table-dto";
import { ConfirmDialogService } from "../../../components/confirm-dialog/confirm-dialog.service";
import { ApiService } from "src/app/services/api.service";
import { DataTableDirective } from "angular-datatables/src/angular-datatables.directive";
import { ApiResult } from "src/app/classes/dto/shared/api-result.dto";
import { NgbDateStruct, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-caja-diaria',
  styleUrls: ['./caja-diaria.component.css'],
  templateUrl: './caja-diaria.component.html'
})






export class CajaDiariaComponent implements OnInit {
  @ViewChild(DataTableDirective, { static: false })
  dtElement: DataTableDirective;
  dtInstance: Promise<DataTables.Api>;
  dtIndex: DataTables.Settings = {};
  Movimientos: MovimientoCajaDiariaDTO[];
  saldoActual: number;
  filtroFecha: string;
  filtroFechaFinal: string;
  Noty: any;
  //public selectedTurno = '1'; // valor predeterminado para el turno completo
  //public movimientos: any[] = []; // supongamos que esta es la lista de movimientos que obtienes de algún servicio


  decideClosure(event, datepicker) { const path = event.path.map(p => p.localName); if (!path.includes('ngb-datepicker')) { datepicker.close(); } }

  constructor(private apiService: ApiService,
              private routerService: Router,
              private notifierService: NotifierService,
              private confirmDialogService: ConfirmDialogService,
              private calendar: NgbCalendar) {

  }

  buscar(criterio: string): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.search(criterio).draw();

      const filteredMovimientos = dtInstance.rows({ filter: 'applied' }).data().toArray();
      let saldoFiltrado = 0;
      for (const movimiento of filteredMovimientos) {
       // saldoFiltrado += movimiento.importe;
      }
      //this.saldoFiltrado = saldoFiltrado;
      //this.saldoActual = saldoFiltrado; // Actualiza saldoActual con el valor filtrado
    });
  }





  onFiltroFechaChange(newValue) {
    this.filtroFechaFinal = newValue.year + "/" + newValue.month + "/" +newValue.day;
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.ajax.reload()
    });
  }


  onEnableClick(id: string) {
    let notifier = this.notifierService;
    let confirmDialogService = this.confirmDialogService;
    let apiService = this.apiService;
    let dataTableInstance = this.dtElement.dtInstance;

    this.confirmDialogService.showConfirm("Desea verificar el registro", function () {
      apiService.DoPOST<ApiResult<any>>("cajas/diaria/enable?encryptedId=" + encodeURIComponent(id), {}, /*headers*/ null,
                                            (response) => {
                                              if (!response.success) {
                                                confirmDialogService.showError(response.message);
                                              }
                                              else {
                                                notifier.notify('success', 'Movimiento Verificado con éxito.');
                                                dataTableInstance.then((dtInstance: DataTables.Api) => {
                                                  dtInstance.ajax.reload()
                                                });
                                              }
                                            },
                                            (errorMessage) => {
                                              confirmDialogService.showError(errorMessage);
                                            });
                                          });
  }

  onDisableClick(id: string) {
    let notifier = this.notifierService;
    let confirmDialogService = this.confirmDialogService;
    let apiService = this.apiService;
    let dataTableInstance = this.dtElement.dtInstance;

    this.confirmDialogService.showConfirm("Desea desverificar el registro?", function () {
      apiService.DoDELETE<ApiResult<any>>("cajas/diaria/disable?encryptedId=" + encodeURIComponent(id), /*headers*/ null,
                                            (response) => {
                                              if (!response.success) {
                                                confirmDialogService.showError(response.message);
                                              }
                                              else {
                                                notifier.notify('success', 'Movimiento desverificado con éxito.');
                                                dataTableInstance.then((dtInstance: DataTables.Api) => {
                                                  dtInstance.ajax.reload()
                                                });
                                              }
                                            },
                                            (errorMessage) => {
                                              confirmDialogService.showError(errorMessage);
                                            });
                                          });
  }





  ngOnInit(): void {

    const today = new Date(); // Obtener fecha de hoy
    const day = today.getDate();
    const month = today.getMonth() + 1; // Los meses empiezan en 0, por lo que hay que sumar 1
    const year = today.getFullYear();
    this.filtroFechaFinal = `${year}/${month}/${day}`;

    this.dtIndex = {

      pagingType: 'simple_numbers',
      pageLength: 18,
      serverSide: true,
      processing: true,
      info: true,
      language: {
        emptyTable: '',
        zeroRecords: 'No hay registros',
        lengthMenu: 'Mostrar _MENU_ registros',
        search: 'Buscar:',
        info: 'Mostrando _START_ a _END_ de _TOTAL_ registros',
        infoEmpty: 'De 0 a 0 de 0 registros',
        infoFiltered: '(filtrados de _MAX_ registros totales)',
        paginate: {
          first: 'Primero',
          last: 'Último',
          next: 'Siguiente',
          previous: 'Anterior'
        },
      },
      order: [[0, 'desc']],
      ajax: (dataTablesParameters: any, callback) => {
        this.apiService.DoPOST<ApiResult<DataTableDTO<MovimientoCajaDiariaDTO>>>("cajas/diaria/list" + (this.filtroFechaFinal === undefined ? "" : "?filterDate=" + encodeURIComponent(this.filtroFechaFinal)), dataTablesParameters, /*headers*/ null,
                      (response) => {
                        if (!response.success) {
                          this.confirmDialogService.showError(response.message);
                        }
                        else {
                          callback({
                            recordsTotal: response.data.recordsTotal,
                            recordsFiltered: response.data.recordsFiltered,
                            data: [] //Siempre vacío para delegarle el render a Angular
                          });
                          this.Movimientos = response.data.records;
                          this.saldoActual =response.data.extraData.saldoActual;
                          if (this.Movimientos.length > 0) {
                            $('.dataTables_empty').hide();
                          }
                          else {
                            $('.dataTables_empty').show();
                          }
                          setTimeout(function() {
                            (<any>$("tbody tr").find('[data-toggle="tooltip"]')).tooltip();
                          }, 300);
                        }
                      },
                      (errorMessage) => {
                        this.confirmDialogService.showError(errorMessage);
                      });
      },
      columns: [
        { data: 'fechaHora' },
      //  { data: 'usuario' },
        { data: 'tipo' },
        { data: 'importe' },
        //{ data: 'cheque' },
        { data: 'observaciones', orderable: false },
        { data: 'verificado' }
      ],
      //public filtroMovimientos(movimiento: any): boolean {
       // if (this.selectedTurno === '1') {
          // Si se selecciona el turno completo, mostrar todos los movimientos
      //    return true;
       // } else if (this.selectedTurno === '2') {
          // Si se selecciona el turno mañana, mostrar solo los movimientos anteriores a las 15:00 hs
        //  return new Date(movimiento.fechaHora).getHours() < 15;
        //} else if (this.selectedTurno === '3') {
          // Si se selecciona el turno tarde, mostrar solo los movimientos posteriores a las 15:00 hs
        //  return new Date(movimiento.fechaHora).getHours() >= 15;
        //} else {
         // return false;
       // }




    };



  }
}

