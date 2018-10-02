// ユニット一覧取得
function UnitList(params, context, done) {

    // 受信データをパースする
    let s = UnitListSend.Parse(params);

    GetUser(context, (user) => {
        new Entities.Units(user).refresh(units => {
            // 返信
            let r = new UnitListReceive();
            r.items = units.items;
            done(r.Pack());
        });
    });
}
