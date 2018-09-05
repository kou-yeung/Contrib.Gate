// サーバのデバッグ時に使用します
function ServerDebug(params, context, done) {
    let r = new ServerDebugReceive();

    GetAdmin(context, admin => {
        let bucket = admin.bucketWithName("TEST_DATA").createObject();
        bucket.set("message", "管理者だよ!!");
        bucket.save({
            success: function (theSavedObject) {
                r.param = JSON.stringify(params);
                r.context = JSON.stringify(context);
                done(r.Pack());
            },
            failure: function (theObject, error) {
            }
        });
    });
}

