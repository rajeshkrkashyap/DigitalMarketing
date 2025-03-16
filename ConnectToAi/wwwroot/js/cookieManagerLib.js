// Cookie Management Library
const CookieManager = {
    // Set a cookie
    setCookie: function (name, value, days) {
        let expires = "";
        if (days) {
            const date = new Date();
            date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    },

    // Get a cookie by name
    getCookie: function (name) {
        const nameEQ = name + "=";
        const cookiesArray = document.cookie.split(";");

        for (let i = 0; i < cookiesArray.length; i++) {
            let cookie = cookiesArray[i].trim();
            if (cookie.indexOf(nameEQ) === 0) {
                return cookie.substring(nameEQ.length, cookie.length);
            }
        }
        return null;
    },

    // Delete a cookie by name
    deleteCookie: function (name) {
        document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    },

    // Check if a cookie exists
    hasCookie: function (name) {
        return this.getCookie(name) !== null;
    }
};

//// Example usage
//// Set a cookie
//CookieManager.setCookie("username", "JohnDoe", 7); // Cookie expires in 7 days

//// Get a cookie
//const username = CookieManager.getCookie("username");
//console.log("Username: ", username);

//// Delete a cookie
//CookieManager.deleteCookie("username");

//// Check if a cookie exists
//if (CookieManager.hasCookie("username")) {
//    console.log("Cookie exists");
//} else {
//    console.log("Cookie does not exist");
//}
