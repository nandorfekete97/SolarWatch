const API_BASE_URL = "http://localhost:5002/SolarWatch";

export async function getSunriseFromAPI(city, date) {
    const response = await fetch(
        `${API_BASE_URL}/GetSunrise?city=${city}&date=${date}`,
        {
            method: "GET",
            headers: {
                Authorization: `Bearer ${localStorage.getItem("token")}`
            },
        }
    );

    if (!response.ok){
        throw new Error("Failed to fetch sunrise time");
    }

    return await response.text();
}

export async function getSunsetFromAPI(city, date) {
    const response = await fetch(
        `${API_BASE_URL}/GetSunset?city=${city}&date=${date}`, 
        {
            method: "GET",
            headers: {
                Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
        }
    );
    if (!response.ok) throw new Error("Failed to fetch sunset time");
    return await response.text();
}