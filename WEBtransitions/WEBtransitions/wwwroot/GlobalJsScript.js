// Module MyScript.js
/*
export function SayHello() {
    console.log("Hello from a module");
    setCookie("user-id");

    alert("Welcome to my application");
}
*/

export { setCookie, generateGuid }

// https://www.w3schools.com/js/js_cookies.asp
function setCookie(cname) {
    let aCookie = getCookie(cname);
    if (!aCookie) {
        let cvalue = generateGuid();
        document.cookie = `${cname}=${cvalue};path=/`;
    }
}

// https://javascript.info/cookie
function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

// see https://javascripts.com/generate-guids-in-javascript/
function generateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}