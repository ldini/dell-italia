import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { fromEvent } from 'rxjs';
import { map, distinctUntilChanged, debounceTime, mergeMap } from 'rxjs/operators';
import { NotifierService } from "angular-notifier";
import { ProductoListDTO } from "src/app/classes/dto/productos/producto-list.dto";
import { MovimientoStockDTO } from "src/app/classes/dto/stock/movimiento-stock.dto";
import { CRUDView } from "src/app/classes/views/crud-view.classes";
import { ConfirmDialogService } from "src/app/components/confirm-dialog/confirm-dialog.service";
import { ApiService } from "src/app/services/api.service";
import { AuthService } from "src/app/services/auth.service";
import { AutocompleteResultDTO } from "src/app/classes/dto/shared/autocomplete-result.dto";
import { ApiResult } from "src/app/classes/dto/shared/api-result.dto";
import { DepositoDTO } from "src/app/classes/dto/stock/deposito.dto";
import { ProveedorDTO } from "src/app/classes/dto/proveedores/proveedor.dto";

@Component({
  selector: 'app-reportes-listados-suma-costo-productos-stock-real',
  styleUrls: ['./reportes-listados-suma-costo-productos-stock-real.component.css'],
  templateUrl: './reportes-listados-suma-costo-productos-stock-real.component.html'
})

export class ReportesListadosSumaCostoProductosStockRealComponent implements OnInit {
  productoFilterText: string;
  producto_encrypted_id: string;
  productosSearch: ProductoListDTO[];
  proveedoresSearch: ProveedorDTO[];
  proveedor_search: string;
  proveedor_encrypted_id: string;
  filtroFechaDesdeValue: string;
  filtroFechaHastaValue: string;

  constructor(private apiService: ApiService,
              private authService: AuthService,
              private routerService: Router,
              private routeService: ActivatedRoute,
              private notifierService: NotifierService,
              private confirmDialogService: ConfirmDialogService) {
    this.proveedor_encrypted_id = "";
    this.producto_encrypted_id = "";
    this.filtroFechaDesdeValue = "";
    this.filtroFechaHastaValue = "";
  }
  ngOnInit(): void {

  }

  onConsultar() {

    this.apiService.OpenNewTab("reportes/listados/suma-costo-productos-stock-real");
  }

}
