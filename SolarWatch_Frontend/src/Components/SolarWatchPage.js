import React, { useState } from 'react';
import { getSunriseFromAPI, getSunsetFromAPI } from "../ApiCalls/solarApi";

const SolarWatchPage = () => {
  const [city, setCity] = useState("");
  const [date, setDate] = useState("");
  const [sunriseTime, setSunriseTime] = useState(null);
  const [sunsetTime, setSunsetTime] = useState(null);

  async function handleGetSunrise() {
    if (!city || !date) return alert ("Please enter both city and date.");
    try {
        const sunrise = await getSunriseFromAPI(city, date);
        setSunriseTime(sunrise); 
    } catch (error) {
        console.error("Error fetching sunrise time:", error);
    }
  }

  async function handleGetSunset() {
    if (!city || !date) return alert("Please enter both city and date.");
    try {
        const sunset = await getSunsetFromAPI(city, date);
        setSunsetTime(sunset);
    } catch (error) {
        console.error("Error fetching sunset time:", error);
    }
}

  return (
    <div>
      <h2>Solar Watch</h2>
      <input
        type='text'
        placeholder='Enter city'
        value={city}
        onChange={(e) => setCity(e.target.value)}
      />
      <input
        type='date'
        placeholder='Enter date'
        value={date}
        onChange={(e) => setDate(e.target.value)}
      />
      <button onClick={handleGetSunrise}>Get Sunrise</button>
      <button onClick={handleGetSunset}>Get Sunset</button>

      {sunriseTime && <p>Sunrise Time: {sunriseTime}</p>}
      {sunsetTime && <p>Sunset Time: {sunsetTime}</p>}
    </div>
  );
};

export default SolarWatchPage;
