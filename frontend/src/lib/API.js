function getForecastFromCityName(location) {
    return fetch(`https://localhost:5000/api/forecast/city/${location}`)
    .then(response => response.json());
}

function getForecastFromZipCode(zipCode) {
    return fetch(`https://localhost:5000/api/forecast/zip/${zipCode}`)
    .then(response => response.json());
}

export default {
    getForecastFromCityName,
    getForecastFromZipCode
};