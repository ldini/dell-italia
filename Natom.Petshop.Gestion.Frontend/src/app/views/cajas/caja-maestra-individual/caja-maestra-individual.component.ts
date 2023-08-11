import { Component, OnInit } from '@angular/core';
import { MovimientoCajaMaestraDTO } from '../../../classes/dto/cajas/movimiento-caja-maestra.dto';
import { ApiResult } from 'src/app/classes/dto/shared/api-result.dto';
import { ApiService } from 'src/app/services/api.service';
import { HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { MovimientoCajaMaestraIndividualDTO } from 'src/app/classes/dto/cajas/movimiento-caja-maestraIndividual';

@Component({
  selector: 'app-caja-maestra-individual',
  templateUrl: './caja-maestra-individual.component.html',
  styleUrls: ['./caja-maestra-individual.component.css']
})
export class CajaMaestraIndividualComponent implements OnInit {

  movimientosMaestra: MovimientoCajaMaestraIndividualDTO[] = [];

  mes: number | null = null;
  ano: number | null = null; 

  constructor(private apiService: ApiService,private route: ActivatedRoute) { }


  ngOnInit(): void {

    this.route.params.subscribe(params => {
      if ('mes' in params && 'ano' in params) {
        this.mes = +params['mes'];
        this.ano = +params['ano'];
      }
    });

    this.obtenerMovimientosMaestra();
  }

  obtenerMovimientosMaestra(): void {
    const emptyHeaders = new HttpHeaders();
    let url = "cajas/cierre/list/all";

    if (this.ano !== null && this.mes !== null) 
      url += `?mes=${this.mes}&ano=${this.ano}`


    this.apiService.DoGET<ApiResult<MovimientoCajaMaestraIndividualDTO[]>>(
      url,
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
