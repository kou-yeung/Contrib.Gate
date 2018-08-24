// sample!!
function funcName(params, context) {
    return "Hello! World!!!!!!";
}

// Ping を受信する
function Ping(params, context) {
    let res = {};
    res["timestamp"] = Date.now().toString();
    return JSON.stringify(res);
}