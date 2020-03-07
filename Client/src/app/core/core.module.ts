import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './auth/auth.service';
import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import {SettingsService} from "./services/settings.service";
import {SettingsHttpService} from "./services/settings.http.service";

@NgModule({
  imports: [
    CommonModule,
    AuthModule.forRoot()
  ],
  declarations: [],
  providers: [
    AuthService,
    OidcSecurityService,
    SettingsService,
    SettingsHttpService
  ]
})
export class CoreModule { }
