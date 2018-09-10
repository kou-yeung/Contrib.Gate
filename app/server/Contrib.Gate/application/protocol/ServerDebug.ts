// サーバのデバッグ時に使用します
function ServerDebug(params, context, done) {
    let r = new ServerDebugReceive();

    r.param = JSON.stringify(params);
    r.context = JSON.stringify(context);
    r.message = GUID.Gen();

    done(r.Pack());
}

