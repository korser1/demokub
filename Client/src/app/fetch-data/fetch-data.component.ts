import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/core/auth/auth.service';
import {SettingsService} from "../core/services/settings.service";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(private authService: AuthService, http: HttpClient, settingsService: SettingsService) {

    this.authService.get(settingsService.settings.apiServer + '/api/WeatherForecasts').subscribe(result => {
      this.forecasts = result as WeatherForecast[];
    }, (error) => {
      console.error(error);
    });
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
