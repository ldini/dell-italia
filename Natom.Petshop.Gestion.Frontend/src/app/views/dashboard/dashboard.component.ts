import { DataTableDirective } from 'angular-datatables/src/angular-datatables.directive';
import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NotifierService } from "angular-notifier";
import { Chart } from 'chart.js';
import { HttpClient } from '@angular/common/http';
import { ApiService } from "src/app/services/api.service";
import { ApiResult } from "src/app/classes/dto/shared/api-result.dto";
import { ConfirmDialogService } from "../../components/confirm-dialog/confirm-dialog.service";
import { PedidoDTO } from "src/app/classes/dto/pedidos/pedido.dto";
import { CRUDView } from "src/app/classes/views/crud-view.classes";
import { AuthService } from "src/app/services/auth.service";
import { FeatureFlagsService } from "src/app/services/feature-flags.service";
import { DashboardService } from '../../services/dashboard.service';


@Component({
  selector: 'app-dashboard',
  styleUrls: ['./dashboard.component.css'],
  templateUrl: './dashboard.component.html'
})
export class dashboardComponent implements OnInit {
  totalIngresosPorMes: string = "0";
  totalEgresosPorMes: string = "0";
  totalIngresosPorAnio: string = "0";
  totalEgresosPorAnio: string = "0";
  totalVentas: string = "0";
  totalProductos: string = "0";
  crud: CRUDView<PedidoDTO>;
  public chart: any;
  data: any;
  data2: any;



  constructor(private apiService: ApiService,
    private _dashboardServicio: DashboardService,
    private authService: AuthService,
    private routerService: Router,
    private routeService: ActivatedRoute,
    private notifierService: NotifierService,
    private confirmDialogService: ConfirmDialogService,
    private featureFlagsService: FeatureFlagsService){
  }


  ngOnInit() : void
  {
    this.createChart();
  }


  createChart(){

    this.apiService.DoGETWithObservable("cajas/diaria/dashboard", /*headers*/ null).subscribe(
      (response: any) => { // se cambia "response" por "any" para evitar errores de tipado
      if (response.status == true )
      { // se comprueba si la respuesta es válida
        this.totalIngresosPorMes = response.value.totalVentasMensuales;
        this.totalEgresosPorMes = response.value.totalEgresosMensuales;
        this.totalIngresosPorAnio = response.value.totalVentasAnuales;
        this.totalEgresosPorAnio = response.value.totalEgresosAnuales;

        const arrayData: any[] = response.value.ingresosUltimaSemana;
        const labelTemp = arrayData.map((value) => value.fecha);
        const dataTemp = arrayData.map((value) => value.total);
        this.mostrarGrafico(labelTemp, dataTemp)


        const arrayData3: any[] = response.value.cajaGrandeUltimaSemana; // se cambia el nombre del objeto de respuesta para que coincida con el nombre utilizado en la función "mostrarGrafico2"
        const labelTemp3 = arrayData3.map((value) => value.fecha);
        const dataTemp3  = arrayData3.map((value) => value.total);
        this.mostrarGrafico2(labelTemp3, dataTemp3);

        this.mostrarGrafico3(labelTemp, dataTemp)
        this.mostrarGrafico4(labelTemp3, dataTemp3)
      }
      else {
      this.confirmDialogService.showError("No hay datos disponibles para mostrar.");
      }
    }
      );

  }


mostrarGrafico(labelsGrafico:any[],dataGrafico:any[]) {
  const myChart = new Chart('myChart', {
    type: 'bar',
    data: {
      labels: labelsGrafico,
      datasets: [{
        label: 'Ingresos',
        data: dataGrafico,
        backgroundColor: [
          'rgba(54, 162, 235, 0.2)'
        ],
        borderColor: [
          'rgba(54, 162, 235, 1)'
        ],
        borderWidth: 1
      }]
    },
    options: {
      maintainAspectRatio: false,
      responsive: true,
      scales: {
        yAxes: [
          {
            ticks: {
              beginAtZero: true
            }
          }
        ]
      },
      title: {
        display: true,
        text: 'VENTAS SEMANALES',
        fontSize: 30 // aumentar el tamaño de la etiqueta (label)
      }
    },

  });
}
mostrarGrafico2(labelsGrafico:any[],dataGrafico:any[]) {
  const myChart = new Chart('myChart2', {
    type: 'bar',
    data: {
      labels: labelsGrafico,
      datasets: [{
        label: 'Caja',
        data: dataGrafico,
        backgroundColor: [
          'rgba(54, 162, 235, 0.2)'
        ],
        borderColor: [
          'rgba(54, 162, 235, 1)'
        ],
        borderWidth: 1
      }]
    },
    options: {
      maintainAspectRatio: false,
      responsive: true,
      scales: {
        yAxes: [
          {
            ticks: {
              beginAtZero: true
            }
          }
        ]
      },
      title: {
        display: true,
        text: 'CIERRE DE CAJA SEMANAL',
        fontSize: 30 // aumentar el tamaño de la etiqueta (label)
      }
    }
  });
}


mostrarGrafico3(labelsGrafico:any[],dataGrafico:any[]) {
  const myChart = new Chart('myChart3', {
    type: 'bar',
    data: {
      labels: labelsGrafico,
      datasets: [{
        label: 'Ingresos',
        data: dataGrafico,
        backgroundColor: [
          'rgba(54, 162, 235, 0.2)'
        ],
        borderColor: [
          'rgba(54, 162, 235, 1)'
        ],
        borderWidth: 1
      }]
    },
    options: {
      maintainAspectRatio: false,
      responsive: true,
      scales: {
        yAxes: [
          {
            ticks: {
              beginAtZero: true
            }
          }
        ]
      },
      title: {
        display: true,
        text: 'VENTAS SEMANALES',
        fontSize: 30 // aumentar el tamaño de la etiqueta (label)
      }
    }
  });
}
mostrarGrafico4(labelsGrafico:any[],dataGrafico:any[]) {
  const myChart = new Chart('myChart4', {
    type: 'bar',
    data: {
      labels: labelsGrafico,
      datasets: [{
        label: 'Caja',
        data: dataGrafico,
        backgroundColor: [
          'rgba(54, 162, 235, 0.2)'
        ],
        borderColor: [
          'rgba(54, 162, 235, 1)'
        ],
        borderWidth: 1
      }]
    },
    options: {
      maintainAspectRatio: false,
      responsive: true,
      scales: {
        yAxes: [
          {
            ticks: {
              beginAtZero: true
            }
          }
        ]
      },
      title: {
        display: true,
        text: 'CIERRE DE CAJA SEMANAL',
        fontSize: 30 // aumentar el tamaño de la etiqueta (label)
      }
    }
  });
}

}
