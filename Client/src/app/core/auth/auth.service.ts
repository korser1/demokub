import {Injectable, OnDestroy} from '@angular/core';
import {
  AuthorizationResult,
  AuthorizationState,
  AuthWellKnownEndpoints,
  OidcSecurityService,
  OpenIdConfiguration
} from 'angular-auth-oidc-client';
import {Observable, Subscription, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Router} from '@angular/router';
import {SettingsService} from "../services/settings.service";
import {Settings} from "../settings";

@Injectable()
export class AuthService implements OnDestroy {
    private readonly _settings : Settings;

    isAuthorized = false;

    constructor(
        private oidcSecurityService: OidcSecurityService,
        private http: HttpClient,
        private router: Router,
        private settingsService: SettingsService
    ) {
        this._settings = this.settingsService.settings;
    }

    private isAuthorizedSubscription: Subscription = new Subscription;

    ngOnDestroy(): void {
        if (this.isAuthorizedSubscription) {
            this.isAuthorizedSubscription.unsubscribe();
        }
    }

    public initAuth() {
        const openIdConfiguration: OpenIdConfiguration = {
            stsServer: this._settings.apiServer,
            redirect_url: this._settings.originUrl + '/callback',
            client_id: this._settings.clientid,
            response_type: 'code',
            scope: 'openid profile ' + this._settings.scope,
            post_logout_redirect_uri: this._settings.originUrl,
            forbidden_route: '/forbidden',
            unauthorized_route: '/unauthorized',
            silent_renew: true,
            silent_renew_url: this._settings.originUrl + '/silent-renew.html',
            history_cleanup_off: true,
            auto_userinfo: true,
            log_console_warning_active: true,
            log_console_debug_active: true,
            max_id_token_iat_offset_allowed_in_seconds: 10,
        };

        const authWellKnownEndpoints: AuthWellKnownEndpoints = {
          issuer: this._settings.identityServer,
          jwks_uri: this._settings.identityServer + this._settings.jwksuri,
          authorization_endpoint: this._settings.identityServer + this._settings.authorizationendpoint,
          token_endpoint: this._settings.identityServer + this._settings.tokenendpoint,
          userinfo_endpoint: this._settings.identityServer + this._settings.userinfoendpoint,
          end_session_endpoint: this._settings.identityServer + this._settings.endsessionendpoint,
          check_session_iframe: this._settings.identityServer +  this._settings.checksessioniframe,
          revocation_endpoint: this._settings.identityServer +  this._settings.revocationendpoint,
          introspection_endpoint: this._settings.identityServer +  this._settings.introspectionendpoint,
        };

        this.oidcSecurityService.setupModule(openIdConfiguration, authWellKnownEndpoints);

        if (this.oidcSecurityService.moduleSetup) {
            this.doCallbackLogicIfRequired();
        } else {
            this.oidcSecurityService.onModuleSetup.subscribe(() => {
                this.doCallbackLogicIfRequired();
            });
        }
        this.isAuthorizedSubscription = this.oidcSecurityService.getIsAuthorized().subscribe((isAuthorized => {
            this.isAuthorized = isAuthorized;
        }));

        this.oidcSecurityService.onAuthorizationResult.subscribe(
            (authorizationResult: AuthorizationResult) => {
                this.onAuthorizationResultComplete(authorizationResult);
            });
    }

    private onAuthorizationResultComplete(authorizationResult: AuthorizationResult) {

        console.log('Auth result received AuthorizationState:'
            + authorizationResult.authorizationState
            + ' validationResult:' + authorizationResult.validationResult);

        if (authorizationResult.authorizationState === AuthorizationState.unauthorized) {
            if (window.parent) {
                // sent from the child iframe, for example the silent renew
                this.router.navigate(['/unauthorized']);
            } else {
                window.location.href = '/unauthorized';
            }
        }
    }

    private doCallbackLogicIfRequired() {

        this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
    }

    getIsAuthorized(): Observable<boolean> {
        return this.oidcSecurityService.getIsAuthorized();
    }

    login() {
        console.log('start login');
        this.oidcSecurityService.authorize();
    }

    logout() {
        console.log('start logoff');
        this.oidcSecurityService.logoff();
    }

    get(url: string): Observable<any> {
        return this.http.get(url, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this.oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    put(url: string, data: any): Observable<any> {
        const body = JSON.stringify(data);
        return this.http.put(url, body, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this.oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    delete(url: string): Observable<any> {
        return this.http.delete(url, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this.oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    post(url: string, data: any): Observable<any> {
        const body = JSON.stringify(data);
        return this.http.post(url, body, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this.oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    private getHeaders() {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        return this.appendAuthHeader(headers);
    }

    public getToken() {
      return this.oidcSecurityService.getToken();
    }

    private appendAuthHeader(headers: HttpHeaders) {
        const token = this.oidcSecurityService.getToken();

        if (token === '') { return headers; }

        const tokenValue = 'Bearer ' + token;
        return headers.set('Authorization', tokenValue);
    }
}
