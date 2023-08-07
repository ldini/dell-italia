import { Component, OnInit } from '@angular/core';
import { MovimientoCajaMaestraDTO } from '../../../classes/dto/cajas/movimiento-caja-maestra.dto';
import { ApiResult } from 'src/app/classes/dto/shared/api-result.dto';
import { ApiService } from 'src/app/services/api.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-caja-maestra-individual',
  templateUrl: './caja-maestra-individual.component.html',
  styleUrls: ['./caja-maestra-individual.component.css']
})
export class CajaMaestraIndividualComponent implements OnInit {

  movimientosMaestra: MovimientoCajaMaestraDTO[] = [];

  

  constructor(private apiService: ApiService) { }


  ngOnInit(): void {
    this.obtenerMovimientosMaestra(); // Llamada al método para obtener los movimientos de cierre
  }

  obtenerMovimientosMaestra(): void {
    const emptyHeaders = new HttpHeaders();

    this.apiService.DoGET<ApiResult<MovimientoCajaMaestraDTO[]>>(
      "cajas/cierre/list/all",
      emptyHeaders,
      (response) => {
        if (response.success) {
          this.movimientosMaestra = response.data;
          this.movimientosMaestra.reverse();
        } else {
          // Manejar el caso de error si es necesario
        }
      },
      (errorMessage) => {
        // Manejar el error 
      }
    );
  }

}
