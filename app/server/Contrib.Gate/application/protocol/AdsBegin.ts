
// 報酬が欲しい！！
function AdsBegin(params, context, done) {

    // 受信データをパースする
    let s = AdsBeginSend.Parse(params);

    GetUser(context, user =>
    {
        GetAdmin(context, admin =>
        {
            new Entities.Ads(admin, user).refresh(ads1 =>
            {
                ads1.create(s.type, ads2 => {
                    // 返信
                    let r = new AdsBeginReceive();
                    r.id = ads2.guid;
                    done(r.Pack());
                });
            });
        });
    });
}