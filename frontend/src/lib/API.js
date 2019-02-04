async function getForecastFromCityName(location) {
    const response = await fetch(`https://localhost:5000/api/forecast/city/${location}`);
    return await response.json();
}

async function getForecastFromZipCode(zipCode) {
    const response = await fetch(`https://localhost:5000/api/forecast/zip/${zipCode}`);
    return await response.json();
}

export default {
    getForecastFromCityName,
    getForecastFromZipCode
};