import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { ConfirmDialogService } from "src/app/components/confirm-dialog/confirm-dialog.service";
import { AuthService } from "src/app/services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class VentasPorProductoVendedorGuard implements CanActivate {

    constructor(private _authService: AuthService,
                private confirmDialogService: ConfirmDialogService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> |
        Promise<boolean | UrlTree> | boolean | UrlTree {

        let containsPermission = this._authService.getCurrentPermissions().indexOf("reportes_listado_ventas_por_producto") >= 0;

        if (!containsPermission)
            this.confirmDialogService.showError("¡Ups! No tienes permisos");

        return containsPermission;
    }
}
