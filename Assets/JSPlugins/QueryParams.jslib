mergeInto(LibraryManager.library, {
    GetQueryParams: function() {
        var params = {};
        var queryString = window.location.search.substring(1);
        var regex = /([^&=]+)=([^&]*)/g;
        var m;

        while (m = regex.exec(queryString)) {
            params[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
        }

        // Convert the params object to a JSON string
        var jsonString = JSON.stringify(params);
        var jsonStringPtr = _malloc(jsonString.length + 1);
        stringToUTF8(jsonString, jsonStringPtr, jsonString.length + 1);
        return jsonStringPtr;
    }
});