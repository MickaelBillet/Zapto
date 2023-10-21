export function getCurrentPosition(dotNetHelper, enableHighAccuracy, maximumAge) {

    const options = {
        enableHighAccuracy: enableHighAccuracy,
        timeout: 5000,
        maximumAge: maximumAge
    };

    function success(position) {

        const coordinate = {
            latitude: position.coords.latitude,
            longitude: position.coords.longitude,
            accuracy: position.coords.accuracy
        };

        dotNetHelper.invokeMethodAsync('OnSuccessAsync', coordinate);
    }

    function error(error) {

        const errorDetails = {
            errorCode: error.code,
            errorMessage: error.message
        };

        dotNetHelper.invokeMethodAsync('OnErrorAsync', errorDetails);

        console.warn(`ERROR(${error.code}): ${error.message}`);
    }

    navigator.geolocation.getCurrentPosition(success, error, options);
}