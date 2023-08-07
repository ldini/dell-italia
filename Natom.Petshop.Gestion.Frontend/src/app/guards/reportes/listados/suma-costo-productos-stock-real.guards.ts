import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { ConfirmDialogService } from "src/app/components/confirm-dialog/confirm-dialog.service";
import { AuthService } from "src/app/services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class SumaCostoProductosStockRealGuard implements CanActivate {

    constructor(private _authService: AuthService,
                private confirmDialogService: ConfirmDialogService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> |
        Promise<boolean | UrlTree> | boolean | UrlTree {

        let containsPermission = this._authService.getCurrentPermissions().indexOf("reportes-listados-suma-costo-productos-stock-real") >= 0;

        containsPermission = true;

        return containsPermission;
    }
}
