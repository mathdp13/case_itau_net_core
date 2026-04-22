import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import 'zone.js';
import { initTelemetry } from './app/core/telemetry/telemetry';

initTelemetry();

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
