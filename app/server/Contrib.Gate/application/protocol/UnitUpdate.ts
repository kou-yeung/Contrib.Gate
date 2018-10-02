// ユニット一覧を更新する
function UnitUpdate(params, context, done) {

    // 受信データをパースする
    let s = UnitUpdateSend.Parse(params);

    // 何かが設定されてるかのチェック。
    // すべてが未設定の場合、無効になります
    let some = s.items.some(item => {
        return item.uniqids.some(id => id != "");
    });
    if (!some) {
        done(ApiError.Create(ErrorCode.Common, "ユニットにペットが設定されていません").Pack());
        return;
    }

    GetUser(context, (user) => {
        new Entities.Units(user).refresh(units => {
            units.items = s.items;
            units.bucket.save(() => {
                // 返信
                let r = new UnitUpdateReceive();
                done(r.Pack());
            });
        });
    });
}
