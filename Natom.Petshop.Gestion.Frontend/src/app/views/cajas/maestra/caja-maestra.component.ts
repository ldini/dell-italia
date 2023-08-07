import { Component, OnInit } from '@angular/core';
import { MovimientoCajaMaestraDTO } from '../../../classes/dto/cajas/movimiento-caja-maestra.dto';
import { ApiResult } from 'src/app/classes/dto/shared/api-result.dto';
import { ApiService } from 'src/app/services/api.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-caja-maestra',
  templateUrl: './caja-maestra.component.html',
  styleUrls: ['./caja-maestra.component.css']
})
export class CajaMaestraComponent implements OnInit {

  movimientosMaestra: MovimientoCajaMaestraDTO[] = [];

  constructor(private apiService: ApiService) { }


  ngOnInit(): void {
    this.obtenerMovimientosMaestra(); // Llamada al método para obtener los movimientos de cierre
  }

  obtenerMovimientosMaestra(): void {
    const emptyHeaders = new HttpHeaders();

    this.apiService.DoGET<ApiResult<MovimientoCajaMaestraDTO[]>>(
      "cajas/cierre/list",
      emptyHeaders,
      (response) => {
        if (response.success) {
          this.movimientosMaestra = response.data;
        } else {
          // Manejar el caso de error si es necesario
        }
      },
      (errorMessage) => {
        // Manejar el error 
      }
    );
  }

  actualizarCierreCaja(id: number) {
    const url = "cajas/cierre/cierre_caja";
    const body = {"Id": id }
    console.log(body);

    this.apiService.DoPOST<ApiResult<MovimientoCajaMaestraDTO>>(
      url,
      body,
      null,
      (response) => {
        if (response.success) {
          console.log("Cierre de caja actualizado correctamente.");
          this.obtenerMovimientosMaestra();
        } else {
          // Manejar el caso de error si es necesario
        }
      },
      (errorMessage) => {
        // Manejar el error si es necesario
      }
    );
  }

}
