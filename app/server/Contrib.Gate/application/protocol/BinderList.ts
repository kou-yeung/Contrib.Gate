// 図鑑一覧を取得

function BinderList(params, context, done) {

    // 受信データをパースする
    let s = BinderListSend.Parse(params);

    GetUser(context, (user) => {
        new Entities.Binder(user).refresh(binder => {
            // 返信
            let r = new BinderListReceive();
            r.ids = [];

            let obj = binder.bucket.first;
            r.ids = obj.allkey;

            done(r.Pack());
        });
    });
}
