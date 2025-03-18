import React, { useState, useEffect } from "react";
import axios from "axios";

interface WeatherForecast {
  date: string;
  temperatureC: number;
  summary: string;
}

const Weather: React.FC = () => {
  const [forecast, setForecast] = useState<WeatherForecast[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchWeatherData = async () => {
      try {
        const response = await axios.get<WeatherForecast[]>(`/weatherforecast`);
        setForecast(response.data);
      } catch (error) {
        console.error("Error fetching weather data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchWeatherData();
  }, []);

  if (loading) {
    return <p>Laddar weather forecast...</p>;
  }

  return (
    <div>
      <h1>Weather forecast</h1>
      <ul>
        {forecast?.length > 0 &&
          forecast.map((forecastItem) => (
            <li key={forecastItem.date}>
              <strong>{forecastItem.date}</strong>: {forecastItem.temperatureC}
              °C, {forecastItem.summary}
            </li>
          ))}
      </ul>
    </div>
  );
};

export default Weather;
