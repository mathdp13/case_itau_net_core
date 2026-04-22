import { trace } from '@opentelemetry/api';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { XMLHttpRequestInstrumentation } from '@opentelemetry/instrumentation-xml-http-request';
import { resourceFromAttributes } from '@opentelemetry/resources';
import { BatchSpanProcessor } from '@opentelemetry/sdk-trace-base';
import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { environment } from '../../../environments/environment';

export function initTelemetry(): void {
  const provider = new WebTracerProvider({
    resource: resourceFromAttributes({
      'service.name': 'case-itau-front',
      'service.version': '1.0.0',
      'deployment.environment': environment.production ? 'production' : 'development',
    }),
    spanProcessors: [
      new BatchSpanProcessor(
        new OTLPTraceExporter({ url: `${environment.otlpUrl}/v1/traces` })
      ),
    ],
  });

  provider.register();

  registerInstrumentations({
    instrumentations: [
      new XMLHttpRequestInstrumentation({
        propagateTraceHeaderCorsUrls: [/\/api/],
        ignoreUrls: [/\/otlp/],
      }),
      new FetchInstrumentation({
        propagateTraceHeaderCorsUrls: [/\/api/],
        ignoreUrls: [/\/otlp/],
      }),
    ],
  });
}

export const getTracer = () => trace.getTracer('case-itau-front');
