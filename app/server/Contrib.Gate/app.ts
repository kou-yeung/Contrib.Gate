// sample!!
function funcName(params, context) {
    return "Hello! World!!!!!!";
}

// Ping を受信する
function Ping(params, context, done) {
    let res = {};

    let foo = JSON.parse(params.data[0]) as Foo; // parse object

    res["timestamp"] = Date.now().toString();
    done(JSON.stringify(res));
}


// 受信
function Communication(params, context, done) {
    switch (params.command) {
        case "Ping": Ping(params, context, done); break;
    }
}

class Foo {
    name: string;
}