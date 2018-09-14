
// 報酬が欲しい！！
function AdsBegin(params, context, done) {

    // 受信データをパースする
    let s = AdsBeginSend.Parse(params);

    GetUser(context, user =>
    {
        let admin = GetAdmin(context);
        new Entities.Ads(admin, user).refresh(ads =>
        {
            ads.create(s.type, s.param, () => {
                // 返信
                let r = new AdsBeginReceive();
                r.id = ads.guid;
                done(r.Pack());
            });
        });
    });
}