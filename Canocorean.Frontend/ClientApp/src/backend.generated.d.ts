declare namespace Backend {
    export interface SampleDataControllerWeatherForecast {
        dateFormatted: string;
        temperatureC: number; // int32
        summary: string;
        readonly temperatureF: number; // int32
    }
    namespace WeatherForecasts {
        namespace Responses {
            export type $200 = Backend.SampleDataControllerWeatherForecast[];
        }
    }
}
