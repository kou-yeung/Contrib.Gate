// タマゴの鑑定
function Judgment(params, context, done) {

    // 受信データをパースする
    let s = JudgmentSend.Parse(params);

    let admin = GetAdmin(context);

    GetUser(context, (user) => {
        new Entities.Egg(user, s.guid).refresh(egg => {
            let item = egg.item;

            // 未鑑定かをチェック
            if (item.judgment) {
                done(ApiError.Create(ErrorCode.Common, "鑑定済").Pack());
                return;
            }
            new Entities.Familiar(admin).refresh([egg.result], familiar => {
                item.judgment = true;
                item.race = familiar.race;
                item.rarity = familiar.rarity;

                egg.item = item;
                egg.bucket.save(() => {
                    // 返信
                    let r = new JudgmentReceive();
                    r.egg = item;
                    done(r.Pack());
                });
            });
        });
    });
}
